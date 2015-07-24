///=============================================================================
/// System/Module Name: DBOperation 
/// FileName:           this.FileName
/// Author:	            Heye
///
/// Change History:
/// Version	    Date    		By	            Details
/// 0.0.1       2008-01-02      HY              Create
/// 0.0.2       2008-03-11      HY              modify exception and add new contruct function
/// 0.0.3       2009-01-07      HY              modify DBType and add DBService(string dbString)
/// 0.0.4       2012-08-06      ZYR             Refactor
/// 
///============================================================================
/// Function:
/// Database Access and Operation Service 
///============================================================================
///
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using LIB_Common;

namespace LIB_DB
{
    /// <summary>
    /// Database Type: Oracle or SqlServer
    /// </summary>   
    
    public abstract class DBService
    {
        #region class properties
        protected IDbConnection DB_Connection = null;
        protected List<IDbDataParameter> db_Parameters = new List<IDbDataParameter>();
        protected String dbErrCode;
        protected int dbChangeNum;  

        protected IDbDataParameter DB_Parameter = null;

        protected IDbDataAdapter DB_Adapter = null;
        protected IDbCommand DB_Command = null;
        protected IDbTransaction DB_Transaction = null;
        protected Boolean isOpenTransaction = false;

        public Boolean IsOpenTransaction
        {
            get { return isOpenTransaction; }
            set { isOpenTransaction = value; }
        }
        protected DbProviderFactory DB_Factory;
        protected ConnectionStringSettings DB_setting;
        protected String serverType;
               
        /// <summary>
        /// Command Parameter 
        /// </summary>
        public virtual string CPTop
        {
            get
            {
                return ":";                
            }
        }

        public virtual eDBType DBType
        {
            get
            {
                return eDBType.Oracle;
            }
        }

        public virtual string ConnectStr
        {
            get
            {
                return "||";              
            }
        }

        #endregion

        #region Constructor
        ///read "ConnnectionString"  from  App.config 
        ///create a by providerName in app.config 
        ///note: may create own config:className+.dll+.config  to use in your class libaray
        
        public DBService(ConnectionStringSettings dbSetting)
        {
            DB_setting = dbSetting;
            if (dbSetting != null)
            {
                DB_Factory = DbProviderFactories.GetFactory(DB_setting.ProviderName);
            }
        }

        public static void InitWAPException()
        {
            WAPExceptionHelper.RegisterInternalWAPException(10001001, "DBOpenException",
                    "DBService", "DB_Open", "Open Database Connection Failure");
            WAPExceptionHelper.RegisterInternalWAPException(10001002, "DBCloseException",
                "DBService", "DB_Close", "Close Database Connection Failure");

            WAPExceptionHelper.RegisterInternalWAPException(10001011, "StartTransactionException",
                "DBService", "StartTransction", "StartTransaction Failure");
            WAPExceptionHelper.RegisterInternalWAPException(10001012, "CommitTransactionException",
                "DBService", "CommitTransction", "CommitTransaction Failure");
            WAPExceptionHelper.RegisterInternalWAPException(10001013, "DBRollBackTransctionException",
                "DBService", "RollbackTransaction", "RollbackTransaction Failure");

            WAPExceptionHelper.RegisterInternalWAPException(10001021, "DBSelectDataException",
                "DBService", "SelectDataSet", "Select Data Failure");
            WAPExceptionHelper.RegisterInternalWAPException(10001022, "DBSelectScalarException",
                "DBService", "SelectScalar", "Select Scalar Data Failure");
            WAPExceptionHelper.RegisterInternalWAPException(10001023, "DBUpdateDataException",
                "DBService", "UpdataDataBase", "Update Data Failure");
            WAPExceptionHelper.RegisterInternalWAPException(10001024, "DBInsertDataException",
                "DBService", "InsertDatabase", "Insert Data Failure");
            WAPExceptionHelper.RegisterInternalWAPException(10001025, "DBNoDataSelectedException",
                "DBService", "QueryData", "No Data Found Through Input SQL Sentence");

            WAPExceptionHelper.RegisterInternalWAPException(10001031, "DBGetReaderException",
                "DBService", "SelectDataSet", "Select Data Failure");
            WAPExceptionHelper.RegisterInternalWAPException(10001032, "DBGetSchemaException",
                "DBService", "GetSchema", "GetDatabase Schema Failure");

            WAPExceptionHelper.RegisterInternalWAPException(10001041, "DBStoredProcedureException",
                "DBService", "StoredProcedure", "Run StoredProcedure Failure");
            WAPExceptionHelper.RegisterInternalWAPException(10001042, "DBProcedureDataException",
                "DBService", "ProcedureData", "Run StoredProcedure with return Data Failure");            
        }

        #endregion

        #region common Function
        private void BuildParameters(IDbCommand Command)
        {
            Command.Parameters.Clear();
            foreach (IDbDataParameter objParameter in this.db_Parameters)
            {
                objParameter.ParameterName = TranParameterName_Add(objParameter.ParameterName);
                Command.Parameters.Add(objParameter);
            }
            db_Parameters.Clear();
        }
        #endregion

        #region deal with Transaction
        //Start Trancaction 
        public virtual void StartTransaction()
        {
            if (isOpenTransaction) return;
            DB_Open();
            try
            {
                this.DB_Transaction = this.DB_Connection.BeginTransaction();
                isOpenTransaction = true;
            }
            catch (Exception ex)
            {
                this.DB_Transaction = null;
                throw WAPExceptionHelper.GetWAPException(10001011, DB_Connection.ToString(), ex);
            }
            finally
            {
            }
        }

        //Commit Transaction and close the Database
        public virtual void CommitTransaction()
        {
            if (!isOpenTransaction) return;
            try
            {
                this.DB_Transaction.Commit();
                this.DB_Close();
            }
            catch (Exception ex)
            {
                throw WAPExceptionHelper.GetWAPException(10001012, DB_Connection.ToString(), ex);
            }
            finally
            {
                this.DB_Connection = null;
                isOpenTransaction = false;
            }
        }

        //rollback Transaction and Close the database
        public virtual void RollbackTransaction()
        {
            try
            {
                if (isOpenTransaction == true)
                {
                    this.DB_Transaction.Rollback();
                    this.DB_Close();
                }
            }
            catch (Exception ex)
            {
                throw WAPExceptionHelper.GetWAPException(10001013, DB_Connection.ToString(), ex);
            }
            finally
            {
                this.DB_Transaction = null;
                isOpenTransaction = false;
            }
        }
        #endregion

        #region DataBase Command:DB_Open,DB_Close,SelectDataSet,Select Datatable,UpdataDataBase,GetTableSchema,SelectScalar,BatchSQL
        public virtual void DB_Open()
        {
            try
            {
                ///create DB_Connection if is null 
                if (this.DB_Connection == null)
                {
                    this.DB_Connection = DB_Factory.CreateConnection();
                    this.DB_Connection.ConnectionString = DB_setting.ConnectionString;                   
                }
                ///open the connection if is not open yet
                if (this.DB_Connection.State != ConnectionState.Open)
                {
                    DB_Connection.Open();
                }
            }
            catch (Exception ex)
            {
                this.DB_Close();
                throw WAPExceptionHelper.GetWAPException(10001001, DB_setting.ConnectionString.ToString(), ex);
            }
        }

        public virtual  void DB_Close()
        {
            try
            {
                if (this.DB_Connection != null)
                {
                    //have a connection that is open,then close
                    if (this.DB_Connection.State != System.Data.ConnectionState.Closed)
                    {
                        this.DB_Connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw WAPExceptionHelper.GetWAPException(10001002, DB_Connection.ToString(), ex);
            }
        }
        
        ///Create DB_Command to simple the code
        ///        
        private IDbCommand CreateDBCommand(string sql)
        {
            //IDbCommand dbc = DB_Factory.CreateCommand();
            DB_Command = DB_Factory.CreateCommand();
            DB_Command.CommandText = sql;
            DB_Command.Connection = this.DB_Connection;
            DB_Command.Transaction = this.DB_Transaction;
            return DB_Command;// dbc;
        }


        /// <summary>select data with returning dataset
        /// select any data from database through delivering SQL string ,and return dataset
        /// write and read without connection
        /// </summary>
        public virtual DataSet SelectDataSet(string sql, out string errorCode)
        {
            errorCode = "-1";            
            DataSet ds = null;
            DB_Open();
            try
            {
                DB_Command = CreateDBCommand(sql);
                BuildParameters(DB_Command);
                DB_Adapter = DB_Factory.CreateDataAdapter();//create a dataAdapter to fill with dataset
                DB_Adapter.SelectCommand = this.DB_Command;
                ds = new DataSet();
                DB_Adapter.Fill(ds);
                errorCode = "0";                
            }
            catch (Exception ex)
            {
                errorCode = "-1";                
                //throw new WAPGeneralException(ex.StackTrace.Clone().GetType().ToString (), ex.StackTrace.Clone().GetHashCode().ToString (), ex.TargetSite.Attributes.ToString(), "&&", ex.Message, ex.InnerException);
                throw WAPExceptionHelper.GetWAPException(10001021, sql, ex);
            }
            finally
            {
                if (!isOpenTransaction)
                    this.DB_Close();
            }
            return ds;   //return dataset
        }

        public virtual DataSet SelectDataSet(string sql)
        {
            DataSet tempDataSet = this.SelectDataSet(sql, out dbErrCode);
            return tempDataSet;
        }


        /// <summary>select data with returning dataTable
        /// select any data from database through delivering SQL string , return datatable
        /// </summary>
        public virtual DataTable SelectDataTable(string sql, out string errorCode)
        {
            errorCode = "-1";            
            DataSet ds = this.SelectDataSet(sql);
            if (ds != null)
            {
                errorCode = "0";
                return ds.Tables[0]; //return a table
            }
            else
            {
                return null;
            }
        }
        public virtual DataTable SelectDataTable(string sql, out string errorCode, out TimeSpan execTime)
        {
            errorCode = "-1";            
            execTime = new TimeSpan(0);
            DateTime datetime = DateTime.Now;
            DataSet ds = this.SelectDataSet(sql);
            execTime = DateTime.Now - datetime;
            if (ds != null)
            {
                errorCode = "0";                
                return ds.Tables[0];  //return a table
            }
            else
            {
                return null;
            }
        }

        public virtual DataTable SelectDataTable(string sql)
        {
            DataTable tempDataTable = this.SelectDataTable(sql, out dbErrCode);
            return tempDataTable;
        }

        /// <summary>Updata data 
        /// Updata data with SQL string
        /// ExecuteNonQuery:connection ,only write
        /// </summary>    
        public virtual  void UpdataDataBase(string sql, out string errorCode, out int changeNum)
        {
            errorCode = "-1";            
            DB_Open();
            try
            {

                DB_Command = CreateDBCommand(sql);
                changeNum = DB_Command.ExecuteNonQuery();//ExcuteNonQuery include insert ,updata and delete
                errorCode = "0";                
            }
            catch (Exception ex)
            {
                errorCode = "-1";                
                changeNum = 0;
                throw WAPExceptionHelper.GetWAPException(10001023, sql, ex);
            }
            finally
            {
                if (!isOpenTransaction)
                    this.DB_Close();
            }
        }

        public virtual bool UpdataDataBase(string sql)
        {
            this.UpdataDataBase(sql, out dbErrCode, out dbChangeNum);
            return (dbErrCode == "0" && dbChangeNum >= 0) ? true : false;
        }
        
        /// <summary>Acquire TableSchema
        /// Acquire detail information about table structure
        /// </summary>  
        public DataTable GetTableSchema(string sql, out string errorCode)
        {
            errorCode = "-1";            
            IDataReader DB_Reader;
            DB_Open();
            try
            {

                DB_Command = CreateDBCommand(sql);
                DB_Reader = DB_Command.ExecuteReader();//create a data reader
                errorCode = "0";                
                return DB_Reader.GetSchemaTable();
            }
            catch (Exception ex)
            {
                errorCode = "-1";                
                throw WAPExceptionHelper.GetWAPException(10001032, sql, ex);
            }
            finally
            {
                if (!isOpenTransaction)
                    this.DB_Close();
            }
        }
        public DataTable GetTableSchema(string sql)
        {
            DataTable tempDataTable = this.GetTableSchema(sql, out dbErrCode);
            return tempDataTable;
        }


        /// <summary>select data with returning datareader
        /// Acquire datareader 
        /// note:Please close the reader after finish
        /// </summary>  
        public IDataReader GetReader(string sql, out string errorCode)
        {
            errorCode = "-1";            
            //not have connection
            DB_Open();
            try
            {

                DB_Command = CreateDBCommand(sql);
                BuildParameters(DB_Command);
                errorCode = "0";                
                return DB_Command.ExecuteReader();  //return datareader
            }
            catch (Exception ex)
            {
                errorCode = "-1";                
                throw WAPExceptionHelper.GetWAPException(10001031, sql, ex);
            }
            finally
            {
            }

        }
        public IDataReader GetReader(string sql)
        {
            IDataReader tempDataReader = this.GetReader(sql, out dbErrCode);
            return tempDataReader;
        }
        public Hashtable SelectDtToHashTable(string sql)
        {
            Hashtable H1 = new Hashtable();
            try
            {
                IDataReader tempDataReader = this.GetReader(sql, out dbErrCode);
                while (tempDataReader.Read())
                {
                    H1.Add(tempDataReader.GetValue(0).ToString(), tempDataReader.GetValue(1));
                }
            }
            catch (Exception ex)
            {
                dbErrCode = "-1";                
                throw WAPExceptionHelper.GetWAPException(10001031, sql, ex);
            }
            finally
            {
                if (!isOpenTransaction)
                    this.DB_Close();
            }
            return H1;
        }

        /// <summary>fast query with returning a object
        /// fast query with returning a objectm
        /// </summary>  
        public virtual  Object SelectScalar(string sql, out string errorCode)
        {
            errorCode = "-1";            
            object result = null;
            DB_Open();
            try
            {
                DB_Command = CreateDBCommand(sql);
                BuildParameters(DB_Command);
                result = DB_Command.ExecuteScalar();
                errorCode = "0";                
            }
            catch (Exception ex)
            {
                errorCode = "-1";
                throw WAPExceptionHelper.GetWAPException(10001022, sql, ex);
            }
            finally
            {
                if (!isOpenTransaction)
                    this.DB_Close();
            }
            return result;
        }
        public virtual Object SelectScalar(string sql)
        {
            Object tempObject = this.SelectScalar(sql, out dbErrCode);
            return tempObject;
        }


        /// <summary>deal with data by batch
        /// deal with data by batch
        /// </summary>  
        public virtual bool BatchSQL(StringList recStringList, out int errorRow)
        {
            errorRow = 0;
            DB_Open();
            try
            {
                this.StartTransaction();
                for (int i = 0; i < recStringList.Count; i++)
                {
                    errorRow = i;
                    DB_Command = CreateDBCommand(recStringList[i]);
                    BuildParameters(DB_Command);
                    this.dbChangeNum = DB_Command.ExecuteNonQuery();
                    if (this.dbChangeNum == 0)
                    {                        
                        return false;
                    }
                }
                this.CommitTransaction();
            }
            catch (Exception ex)
            {

                this.RollbackTransaction();
                throw WAPExceptionHelper.GetWAPException(10001022,"", ex);
            }
            finally
            {
                if (!isOpenTransaction)
                    this.DB_Close();
            }
            return true;

        }
        #endregion

        #region use Procedure process to deal with data
        //use StoredProcedure with ProcessName
        public bool StoredProcedure(string procedureName)
        {
            DB_Open();
            try
            {
                DB_Command = CreateDBCommand(procedureName);
                DB_Command.CommandType = CommandType.StoredProcedure;
                BuildParameters(DB_Command);
                DB_Command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw WAPExceptionHelper.GetWAPException(10001041, procedureName, ex);
            }
            finally
            {
                if (!isOpenTransaction)
                    this.DB_Close();
            }
        }
        public bool StoredProcedure(string procedureName, out Hashtable Hdbparameter)
        {
            DB_Open();
            try
            {
                DB_Command = CreateDBCommand(procedureName);
                DB_Command.CommandType = CommandType.StoredProcedure;
                BuildParameters(DB_Command);
                DB_Command.ExecuteNonQuery();
                Hdbparameter = new Hashtable();
                foreach (IDbDataParameter objParameter in DB_Command.Parameters)
                {
                    if (objParameter.Direction != ParameterDirection.Input)
                    {
                        Hdbparameter.Add(TranParameterName_Del(objParameter.ParameterName), objParameter.Value);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw WAPExceptionHelper.GetWAPException(10001041, procedureName, ex);
            }
            finally
            {
                if (!isOpenTransaction)
                    this.DB_Close();
            }
        }
        public DataSet ProcedureDataSet(string procedureName)
        {
            DB_Open();
            try
            {
                DB_Command = CreateDBCommand(procedureName);
                DB_Command.CommandType = CommandType.StoredProcedure;
                BuildParameters(DB_Command);
                DB_Adapter = DB_Factory.CreateDataAdapter();//create a dataAdapter to fill with dataset
                DB_Adapter.SelectCommand = this.DB_Command;
                DataSet tempds = new DataSet();
                DB_Adapter.Fill(tempds);
                return tempds;
            }
            catch (Exception ex)
            {
                throw WAPExceptionHelper.GetWAPException(10001042, procedureName, ex);
            }
            finally
            {
                if (!isOpenTransaction)
                    this.DB_Close();
            }

        }

        protected abstract string TranParameterName_Add(string parameterName);
       
        private string TranParameterName_Del(string parameterName)
        {
            string topstr = parameterName.Substring(0, 1);
            string strvalue = parameterName;
            if (topstr == "@")
            {
                strvalue = parameterName.Substring(1, parameterName.Length - 1);
            }
            else if (topstr == ":")
            {
                strvalue = parameterName.Substring(1, parameterName.Length - 2);
            }
            return strvalue;
        }
        /// <summary>
        /// renew AddParameter
        /// </summary>
        /// <param name="ValueName">parameter name</param>
        /// <param name="Value">parameter value</param>
        /// eg:--------------------------------------------------
        ///this.DBtest.DB_Open(out errorcode, out errorMessage);
        ///DBtest.AddParameter ("@PGroupCode","002");
        ///DBtest.AddParameter("@PGroupName", "heye");
        ///DBtest.AddParameter("@PModifyPerson", "heye");
        ///DBtest.AddParameter("@PModifyDate", System.DateTime.Now);
        ///DBtest.AddParameter("@PComments", "hahaha");
        ///DBtest.StoredProcedure("UserGroup_Add");
        ///this.DBtest.DB_Close(out errorcode1);
        ///------------------------------------------------------
        public void AddParameter(string ValueName, object Value)
        {
            this.DB_Parameter = DB_Factory.CreateParameter();
            DB_Parameter.ParameterName = ValueName;
            DB_Parameter.Value = Value;
            db_Parameters.Add(this.DB_Parameter);
            this.DB_Parameter = null;
        }
        /// <summary>
        /// renew AddParameter
        /// </summary>
        /// <param name="ValueName">parameter name</param>
        /// <param name="pType">parameter type</param>
        /// <param name="pSize">parameter size</param>
        /// <param name="Value">parameter value</param>
        public void AddParameter(string ValueName, DbType Type, int Size, object Value)
        {
            this.DB_Parameter = DB_Factory.CreateParameter();
            this.DB_Parameter.ParameterName = ValueName;
            this.DB_Parameter.DbType = Type;
            this.DB_Parameter.Size = Size;
            this.DB_Parameter.Value = Value;
            this.db_Parameters.Add(this.DB_Parameter);
            this.DB_Parameter = null;
        }
        /// <summary>
        /// renew AddParameter
        /// </summary>
        /// <param name="ValueName">parameter name</param>
        /// <param name="pSize">parameter size</param>
        /// <param name="pDir">parameter deliver type</param>
        /// <param name="Value">parameter value</param>
        public void AddParameter(string ValueName, int Size, ParameterDirection pDirection, object Value)
        {
            this.DB_Parameter = DB_Factory.CreateParameter();
            this.DB_Parameter.ParameterName = ValueName;
            this.DB_Parameter.Direction = pDirection;
            this.DB_Parameter.DbType = DbType.String;
            this.DB_Parameter.Size = Size;
            this.DB_Parameter.Value = Value;
            this.db_Parameters.Add(this.DB_Parameter);
            this.DB_Parameter = null;
        }
        /// <summary>
        /// renew AddParameter
        /// </summary>
        /// <param name="ValueName">parameter name</param>
        /// <param name="pType">parameter type</param>
        /// <param name="pSize">parameter size</param>
        /// <param name="pDir">parameter deliver type</param>
        /// <param name="Value">parameter value</param>
        public void AddParameter(string ValueName, DbType pType, int pSize, ParameterDirection pDirection, object Value)
        {
            this.DB_Parameter = DB_Factory.CreateParameter();
            //this.DB_Parameter.ParameterName = this.DB_Parameter;
            this.DB_Parameter.ParameterName = ValueName;
            this.DB_Parameter.DbType = pType;
            this.DB_Parameter.Size = pSize;
            this.DB_Parameter.Direction = pDirection;
            this.DB_Parameter.Value = Value;
            this.db_Parameters.Add(this.DB_Parameter);
            this.DB_Parameter = null;
        }
        #endregion

        #region method from old DBOperation Class
        public DataTable QueryDataRtnDt(string querySQL, string funName)
        {
            try
            {
                DataTable tmpTable = this.SelectDataTable(querySQL);
                if (tmpTable == null || tmpTable.Rows.Count == 0)
                    throw WAPExceptionHelper.GetWAPException(10001025, funName, null); 

                return tmpTable;
            }
            catch (Exception ex)
            {
                throw WAPExceptionHelper.GetWAPException(10001021, funName, ex); 
            }
        }

        public DataTable QueryDataRtnDt(string querySQL, string funName, bool drctRtn)
        {
            try
            {
                DataTable tmpTable = this.SelectDataTable(querySQL);
                if (drctRtn == false)
                {
                    if (tmpTable == null || tmpTable.Rows.Count == 0)
                        throw WAPExceptionHelper.GetWAPException(10001025, funName, null); 
                }
                return tmpTable;
            }
            catch (Exception ex)
            {
                throw WAPExceptionHelper.GetWAPException(10001021, funName, ex); 
            }
        }

        public int QueryDataRtnCount(string querySQL, string funName)
        {
            try
            {
                DataTable tmpTable = this.SelectDataTable(querySQL);

                if (tmpTable == null || tmpTable.Rows.Count == 0)
                    return 0;

                return tmpTable.Rows.Count;
            }
            catch (Exception ex)
            {
                throw WAPExceptionHelper.GetWAPException(10001021, funName, ex); 
            }
        }

        public string QueryDataRtnStr(string querySQL, string funName)
        {
            string strRtn = "";
            try
            {
                DataTable tmpTable = this.SelectDataTable(querySQL);
                if (tmpTable == null)
                    throw WAPExceptionHelper.GetWAPException(10001025, funName, null); 

                if (tmpTable.Rows.Count > 0)
                    strRtn = tmpTable.Rows[0][0].ToString().Trim();

                return strRtn;
            }
            catch (Exception ex)
            {
                throw WAPExceptionHelper.GetWAPException(10001021, funName, ex); 
            }
        }

        public bool UpdataDataRtnBool(string querySQL, string funName)
        {
            try
            {
                if (!this.UpdataDataBase(querySQL))
                {
                    this.RollbackTransaction();
                    throw WAPExceptionHelper.GetWAPException(10001023, funName + " " + querySQL, null); 
                }

                return true;
            }
            catch (Exception ex)
            {
                this.RollbackTransaction();
                throw WAPExceptionHelper.GetWAPException(10001023, funName + " " + querySQL,ex); 
            }
        }
        #endregion
    }
}
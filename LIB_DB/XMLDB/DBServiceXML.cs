using System;
using System.Configuration;
using System.Data;
using System.IO;
using LIB_Common;
using System.Collections;
using System.Collections.Generic;

namespace LIB_DB
{
    public enum eXmlFileType
    {
        XML,
        XSD,
        ODT,
        DLK,
        DAS,
        TLK,
        Other
    }

    public class DBServiceXML : DBService
    {
        public static  Dictionary<string, DataSet> XMLDataSetPool;
        public const int DefaultTimeOut = 30;
        public const int DefaultMaxAccUser = 1;
        private DataSet _XMLData;
        private DBLockControler LockControler;
        private bool _isOpenTransaction = false;
        private string fileFolder = "";
        private int HBTimeOut = LockUserInfo.DefaultHBTimeOut;
        private ConnectionStringSettings DB_setting;
        private int _currentTimeOut = DefaultTimeOut;
    
        public int CurrentTimeOut
        {
            get { return _currentTimeOut; }
            set { _currentTimeOut = value; }
        }

        protected override string TranParameterName_Add(string parameterName)
        {
            return "";
        }

        public DBServiceXML(ConnectionStringSettings dbSetting)
            : base(null)
        {
            DB_setting = dbSetting;
        }
        public override string CPTop
        {
            get
            {
                return ":";
            }
        }

        public override eDBType DBType
        {
            get
            {
                return eDBType.XML;
            }
        }

        public override string ConnectStr
        {
            get
            {
                return "||";
            }
        }

        public void AddTable(DataTable dt)
        {
            if (dt.TableName != "")
            {
                _XMLData.Tables.Add(dt);
                SaveTable(dt.TableName);
            }
            else
            {
                throw new ApplicationException("表名不能为空");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ReLoad"></param>
        public void DB_Open(bool ReLoad)
        {
            if (ReLoad)
            {
                DBServiceXML.XMLDataSetPool.Remove(DB_setting.Name);
                _XMLData = null;
            }
            this.DB_Open();
        }

        #region Open Close
        public override void DB_Open()
        {
            eXmlFileType efileType;
            if (XMLDataSetPool == null)
            {
                XMLDataSetPool = new Dictionary<string, DataSet>();
            }

            if (_XMLData == null)
            {
                if (XMLDataSetPool.ContainsKey(DB_setting.Name))
                {
                    _XMLData = XMLDataSetPool[DB_setting.Name];
                }
                else
                {
                    _XMLData = new DataSet();
                    XMLDataSetPool.Add(DB_setting.Name, _XMLData);
                }

                string[] tmpSplitStrs=DB_setting.ConnectionString.Split(';');
                fileFolder = tmpSplitStrs[0];
                if (tmpSplitStrs.Length > 1)
                    int.TryParse(tmpSplitStrs[1], out HBTimeOut);
                _XMLData.DataSetName = fileFolder;
                fileFolder = fileFolder.TrimEnd('\\');

                LockControler = new DBLockControler(fileFolder);
                if (!LockControler.AllowAccess)
                {
                    throw new ApplicationException("超出用于访问最大数。");
                }
                LockControler.Booking(HBTimeOut);

                string[] strFiles = Directory.GetFiles(fileFolder);
                foreach (string file in strFiles)
                {
                    string[] sptString = new string[1] { @"\" };
                    string[] sfileSplt = file.Split(sptString, StringSplitOptions.None);
                    string sfile = sfileSplt[sfileSplt.Length - 1];
                    efileType = XMLDBFileHelper.GetFileType(sfile);
                    string tableName = sfile.Split('.')[0];
                    if (efileType == eXmlFileType.XML)
                    {
                        if (_XMLData.Tables.Contains(tableName))
                        {
                            _XMLData.Tables.Remove(tableName);
                        }
                        DataTable dt = new DataTable();
                        dt.TableName = tableName;
                        dt.ReadXmlSchema(XMLDBFileHelper.GetFullFileName(fileFolder, eXmlFileType.XSD, sfile));
                        dt.ReadXml(XMLDBFileHelper.GetFullFileName(fileFolder, eXmlFileType.XML, sfile));
                        _XMLData.Tables.Add(dt);
                    }
                }
            }
        }
        public override void DB_Close()
        {
            _XMLData = null;
            if (LockControler != null)
                LockControler.UnBooking();
        }
        #endregion

        #region Select,Update,Insert,Delete
        public override DataTable SelectDataTable(string sql, out string errorCode)
        {
            errorCode = "-1";
            int changeNum;
            DB_Open();
            if (_XMLData != null)
            {
                //errorCode = "0";
                //return _XMLData.Tables[sql].Copy();
                eSqlType type = SQLAnalysiser.GetSqlType(sql);
                ISqlExecAgency iexec = SQLAgencyFactory.CreateSQLAgency(type, sql);
                bool result = iexec.SQLExec(_XMLData, sql, out errorCode, out changeNum);
                if(iexec.Result.Count>0)
                {
                    return (DataTable)iexec.Result[0];
                }
                return null;
            }
            else
            {
                return null;
            }


        }
        public override DataTable SelectDataTable(string sql, out string errorCode, out TimeSpan execTime)
        {
            errorCode = "-1";
            execTime = new TimeSpan(0);
            DateTime datetime = DateTime.Now;
            DataTable dt = this.SelectDataTable(sql, out errorCode);
            execTime = DateTime.Now - datetime;
            if (dt != null)
            {
                errorCode = "0";
                return dt;  //return a table
            }
            else
            {
                return null;
            }
        }

        public override void UpdataDataBase(string sql, out string errorCode, out int changeNum)
        {
            errorCode = "-1";
            changeNum = 0;
            DB_Open();
            try
            {
                this.DB_Open();
                eSqlType type = SQLAnalysiser.GetSqlType(sql);
                ISqlExecAgency iexec = SQLAgencyFactory.CreateSQLAgency(type, sql);
                bool result = iexec.SQLExec(_XMLData, sql, out errorCode,out changeNum);
                SaveTable(iexec.Table);
                errorCode = result ? "0" : "-1";
            }
            catch (Exception ex)
            {
                errorCode = "-1";
                changeNum = 0;
                throw WAPExceptionHelper.GetWAPException(10001023, sql, ex);
            }
            finally
            {

                //this.DB_Close();
            }
        }

        public override bool UpdataDataBase(string sql)
        {
            this.UpdataDataBase(sql, out dbErrCode, out dbChangeNum);
            return (dbErrCode == "0" && dbChangeNum >= 0) ? true : false;
        }

        public bool SaveTable(string dtName)
        {
            try
            {
                string xmlPath=string.Format(@"{0}\{1}.xml", fileFolder, dtName);
                string xsdPath=string.Format(@"{0}\{1}.xsd", fileFolder, dtName);
                if(!_XMLData.Tables.Contains(dtName))
                {
                    if (File.Exists(xmlPath))
                    {
                        File.Delete(xmlPath);
                        File.Delete(xsdPath);
                    }
                    else
                    {
                        throw WAPExceptionHelper.GetWAPException(10001023, dtName, null);
                    }
                }
                else
                {
                    DataTable dt = _XMLData.Tables[dtName];
                    dt.WriteXml(xmlPath);
                    dt.WriteXmlSchema(xsdPath);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw WAPExceptionHelper.GetWAPException(10001023, dtName, ex);
            }
        }
        #endregion

        #region deal with Transaction
        //Start Trancaction 
        public override void StartTransaction()
        {
            this.DB_Open();
            _isOpenTransaction = true;
        }

        //Commit Transaction and close the Database
        public override void CommitTransaction()
        {
            _isOpenTransaction = false;
        }

        //rollback Transaction and Close the database
        public override void RollbackTransaction()
        {
            _isOpenTransaction = false;
        }

        private static string[] GetDirectoryList(string strpath, ref string errmsg)
        {
            //errmsg = "";
            string[] strpaths = new string[0];
            string strfullpath;//= FunBase.GetSysBasePath() + strpath;

            try
            {
                if (strpath.IndexOf(':') > -1)
                {
                    strfullpath = strpath;
                }
                else
                {
                    strfullpath = FunBase.GetSysBasePath() + strpath;
                }

                if (Directory.Exists(strfullpath))
                {
                    strpaths = Directory.GetDirectories(strfullpath);
                }
                else
                {
                    errmsg = "Have not XML Files.";
                }
                return strpaths;
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return new string[0];
            }
        }
        #endregion

    }
}

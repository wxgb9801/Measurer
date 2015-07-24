using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using LIB_Common;

namespace LIB_DB
{
    /// <summary>
    /// 主表数据抽象应用类
    /// </summary>
    public abstract class DataOperation: System.MarshalByRefObject, IDisposable,IDataOpertion
    {
        public string errMessage;
        public string errCode;
        public int changeNum;

        private bool _isForceUpdate = true;

        public bool IsForceUpdate
        {
            get { return _isForceUpdate; }
            set { _isForceUpdate = value; }
        }
        
        protected bool _isGetData;
        protected DBService theDB;

        #region 属性
        /// <summary>
        /// 得到Inert操作的字段串
        /// </summary>
        public virtual string InsertFieldsString
        {
            get
            {
                return "";
            }
        }

        /// <summary>
        /// 得到Inert操作的Values串
        /// </summary>
        public virtual string InsertValuesString
        {
            get
            {
                return "";
            }
        }
        /// <summary>
        /// 得到Where条件串
        /// </summary>
        public virtual string WhereSql
        {
            get
            {
                return "";
            }
        }
        /// <summary>
        /// 得到Update中Set字段的串
        /// </summary>
        public virtual string UpdateFieldsString
        {
            get
            {
                return "";
            }
        }
        /// <summary>
        /// 得到Select语句要显示的字段串
        /// </summary>
        public virtual string SelectFieldsString
        {
            get
            {
               return "";
            }
        }

        /// <summary>
        /// 表中所用操作字段集合
        /// </summary>
        private FieldCollection _fields;

        public FieldCollection Fields
        {
            get { return _fields; }
            set { _fields = value; }
        }
        /// <summary>
        /// 用户自定义的Where条件字段类
        /// </summary>
        public FieldCollection WhereFields;
        /// <summary>
        /// 在数据库中所关联的表
        /// </summary>
        public string TableName;
        /// <summary>
        /// 是否从数据库得到数据
        /// </summary>
        public bool IsGetData
        {
            get
            {
                return _isGetData;
            }
        }
        #endregion
        #region 构造函数
        /// <summary>
        /// 自动读数据库载入数据的构造函数
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <param name="db">操作数据类</param>
        /// <param name="IsLoadDt">是否从数据库载入数据到类中</param>
        /// <param name="fields">Field集合</param>        
        public DataOperation(string tablename, DBService db, bool IsLoadDt, FieldCollection fields)
        {
            this.TableName = tablename;
            if(db == null)
                throw WAPExceptionHelper.GetWAPException(10001052, "A DBService must be inputted into the constructer", null);
            else
                this.theDB = db;
            if (fields == null || fields.Count == 0)
                throw WAPExceptionHelper.GetWAPException(10001051, "A FieldCollection must be inputted into the constructer", null);
            else
                this.Fields = fields;
            if (IsLoadDt)
            {
                this.InitData();
            }
            this.WhereFields = new FieldCollection();
        }

        public static void InitWAPException()
        {
            WAPExceptionHelper.RegisterInternalWAPException(10001051, "NoInputFieldsException",
                "DataOperation", "Constructer", "A FieldCollection must be inputted into the constructer");
            WAPExceptionHelper.RegisterInternalWAPException(10001052, "NoInputDBServiceException",
                "DataOperation", "Constructer", "A DBService must be inputted into the constructer");
            WAPExceptionHelper.RegisterInternalWAPException(10001053, "ResultMoreThanOneRowException",
               "DataOperation", "InitData", "Result return more than one row but expect only one row");
        }        

        #endregion
        #region  Public 方法
        /// <summary>
        /// 在数据库中更新
        /// </summary>
        /// <returns>是否成功</returns>
        public abstract bool Update();
        
        /// <summary>
        /// 在数据库中插入Data中数据(根据WhereSql)
        /// </summary>
        /// <returns>是否成功</returns>
        public abstract bool Insert();

        public abstract bool Insert(out object id);
        
        /// <summary>
        /// 在数据库中删除Data中数据(根据WhereSql)
        /// </summary>
        /// <returns>是否成功</returns>
        public abstract bool Delete();
        
        /// <summary>
        /// 得到字段和值得hashtable
        /// </summary>
        /// <returns></returns>
        public abstract Hashtable ToHashTable();
        
        /// <summary>
        /// 得到数据DataTable
        /// </summary>
        /// <returns>DataTable</returns>
        public abstract DataTable ToDataTable();
        
        /// <summary>
        /// 得到数据DataTableByDesc
        /// </summary>
        /// <returns>DataTable</returns>
        public abstract DataTable ToDataTableByDesc();

        public void Dispose()
        {
            theDB.DB_Close();
        }
        
        #endregion

        #region protected
        /// <summary>
        /// 同步Field的Value和Old_Value;
        /// </summary>
        protected void SyncFields()
        {
            for (int i = 0; i < Fields.Count; i++)
            {
                Fields[i].SyncField();
            }
        }           

        /// <summary>
        /// 得到需要更新的Field(Fields中Value与Old_Value不同的Filed)
        /// </summary>
        /// <returns></returns>
        protected List<Field> GetUpFields()
        {
            List<Field> tmplist = new List<Field>();
            for (int i = 0; i < Fields.Count; i++)
            {
                if (Fields[i].IsUpdate)
                {
                    tmplist.Add(Fields[i]);
                }
            }
            return tmplist;
        }
        /// <summary>
        /// 从数据库得到初始化数据
        /// </summary>
        protected abstract void InitData();

        protected abstract void GetDisplayText();   
        
        #endregion
        /// <summary>
        /// 根据DataSet批量插入数据
        /// </summary>
        /// <param name="Ds"></param>
        /// <returns></returns>
        public abstract bool InsertByDataSet(DataSet Ds);
       
        /// <summary>
        /// 根据DataSet批量更新数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dBTable"></param>
        /// <param name="KeyWord"></param>
        /// <returns></returns>
        public abstract bool UpdateByDataSet(DataTable dt, string dBTable, string KeyWord);

        public abstract bool DeleteByDataSet(string dBTable, string whereKeyName, ArrayList listKeyValue);
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using LIB_Common;
namespace LIB_DB
{
    public class DataOperation_Complex : IDataOpertion
    {
        private Dictionary<string, Dictionary<string, FieldCollection>> _dbConn_fc_Dir;
        private Dictionary<string, DBService> _Dir_DBService =  new Dictionary<string,DBService>();

        public string defaultConnID = string.Empty;
        public string defaultTableID = string.Empty;

        private FieldCollection _fields;
        public FieldCollection Fields
        {
            get
            {
                return _fields;
            }
        }
        private bool isLoadDT;

        private bool _isForceUpdate = true;

        public bool IsForceUpdate
        {
            get { return _isForceUpdate; }
            set { _isForceUpdate = value; }
        }

        #region 构造函数
        /// <summary>
        /// 自动读数据库载入数据的构造函数
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <param name="db">操作数据类</param>
        /// <param name="IsLoadDt">是否从数据库载入数据到类中</param>
        /// <param name="fields">Field集合</param>
        public DataOperation_Complex(string DefConnID, string DefTableID, FieldCollection fc)
        {
            _dbConn_fc_Dir = new Dictionary<string, Dictionary<string, FieldCollection>>();
            _fields = fc.Clone();
            defaultConnID = DefConnID;
            defaultTableID = DefTableID;
            decompose(_fields);

        }
        public DataOperation_Complex( FieldCollection fc)
        {
            if (fc != null)
            {
                _dbConn_fc_Dir = new Dictionary<string, Dictionary<string, FieldCollection>>();
                _fields = fc.Clone();
                decompose(_fields);
            }
        }
        #endregion

        #region Public 方法
        public void AddDbService(string serviceName, DBService DB)
        {
            if (!_Dir_DBService.ContainsKey(serviceName))
            {
                _Dir_DBService.Add(serviceName, DB);
            }
        }
        public bool LoadFieldsBySource()
        {
            return InitData();
        }
        public void Add(FieldCollection fcp)
        {
            FieldCollection tmpfc = fcp.Clone();
            decompose(tmpfc);
            if (isLoadDT)
            {
                _fields = tmpfc;
            }
        }
        #endregion

        #region  数据操作
        /// <summary>
        /// 在数据库中插入Data中数据(根据WhereSql)
        /// </summary>
        /// <returns>是否成功</returns>
        public bool Insert()
        {
            Dictionary<string, DBService> Dir_StartTransction = new Dictionary<string, DBService>();
            if (_dbConn_fc_Dir == null || _dbConn_fc_Dir.Keys == null || _dbConn_fc_Dir.Keys.Count < 0)
                return false;

            foreach (string dbConnid in _dbConn_fc_Dir.Keys)
            {
                DBService db;
                if (_Dir_DBService.ContainsKey(dbConnid))
                {
                    db = _Dir_DBService[dbConnid];
                }
                else
                {
                    db = DataPersistence.GetDBService(dbConnid);
                }
                //if (db == null)
                //{
                //    throw WAPExceptionHelper.GetWAPException(10007009, String.Format("Conntion Name is {0}.", dbConnid), null);
                //}
                try
                {
                    //modified by fyp 2014/3/21
                    if (!db.IsOpenTransaction)
                    {
                        db.StartTransaction();
                        Dir_StartTransction.Add(dbConnid, db);
                    }

                    //db.StartTransaction();
                    //Dir_StartTransction.Add(dbConnid, db);

                    foreach (string tabname in _dbConn_fc_Dir[dbConnid].Keys)
                    {
                        DataOperation op = DataPersistence.GetDataOperation(tabname, db, false, _dbConn_fc_Dir[dbConnid][tabname]);
                        if (!op.Insert())
                        {
                            throw WAPExceptionHelper.GetWAPException(10007000, op.UpdateFieldsString, null);
                        }
                    }
                }
                catch (Exception ex)
                {
                    foreach (string conn in Dir_StartTransction.Keys)
                    {
                        Dir_StartTransction[conn].RollbackTransaction();
                    }
                    throw WAPExceptionHelper.GetWAPException(10007001, ex.ToString(), ex);
                }

            }
            foreach (string conn in Dir_StartTransction.Keys)
            {
                Dir_StartTransction[conn].CommitTransaction();
            }
            return true;
        }
        /// <summary>
        /// 在数据库中更新
        /// </summary>
        /// <returns>是否成功</returns>
        public bool Update()
        {
            Dictionary<string, DBService> Dir_StartTransction = new Dictionary<string, DBService>();
            foreach (string dbConnid in _dbConn_fc_Dir.Keys)
            {
                DBService db;
                if (_Dir_DBService.ContainsKey(dbConnid))
                {
                    db = _Dir_DBService[dbConnid];
                }
                else
                {
                    db = DataPersistence.GetDBService(dbConnid);
                }
                if (db == null)
                {
                    throw WAPExceptionHelper.GetWAPException(10007009, String.Format("Conntion Name is {0}.", dbConnid), null);
                }
                try
                {
                    if (!db.IsOpenTransaction)
                    {
                        db.StartTransaction();
                        Dir_StartTransction.Add(dbConnid, db);
                    }
                    foreach (string tabname in _dbConn_fc_Dir[dbConnid].Keys)
                    {
                        DataOperation op = DataPersistence.GetDataOperation(tabname, db, false, _dbConn_fc_Dir[dbConnid][tabname]);
                        op.IsForceUpdate = _isForceUpdate;
                        if (!op.Update())
                        {
                            throw WAPExceptionHelper.GetWAPException(10007000, op.UpdateFieldsString, null);
                        }
                    }
                }
                catch (Exception ex)
                {
                    foreach (string conn in Dir_StartTransction.Keys)
                    {
                        Dir_StartTransction[conn].RollbackTransaction();
                    }
                    throw WAPExceptionHelper.GetWAPException(10007001, ex.ToString(), null);
                }

            }
            foreach (string conn in Dir_StartTransction.Keys)
            {
                Dir_StartTransction[conn].CommitTransaction();
            }
            return true;
        }
        /// <summary>
        /// 在数据库中删除Data中数据(根据WhereSql)
        /// </summary>
        /// <returns>是否成功</returns>
        public bool Delete()
        {
            Dictionary<string, DBService> Dir_StartTransction = new Dictionary<string, DBService>();
            foreach (string dbConnid in _dbConn_fc_Dir.Keys)
            {
                DBService db;
                if (_Dir_DBService.ContainsKey(dbConnid))
                {
                    db = _Dir_DBService[dbConnid];
                }
                else
                {
                    db = DataPersistence.GetDBService(dbConnid);
                }
                if (db == null)
                {
                    throw WAPExceptionHelper.GetWAPException(10007009, String.Format("Conntion Name is {0}.", dbConnid), null);
                }
                try
                {

                    if (!db.IsOpenTransaction)
                    {
                        db.StartTransaction();
                        Dir_StartTransction.Add(dbConnid, db);
                    }
                    foreach (string tabname in _dbConn_fc_Dir[dbConnid].Keys)
                    {
                        DataOperation op = DataPersistence.GetDataOperation(tabname, db, false, _dbConn_fc_Dir[dbConnid][tabname]);
                        if (!op.Delete())
                        {
                            throw WAPExceptionHelper.GetWAPException(10007000, op.UpdateFieldsString, null);
                        }
                    }
                }
                catch (Exception ex)
                {
                    foreach (string conn in Dir_StartTransction.Keys)
                    {
                        Dir_StartTransction[conn].RollbackTransaction();
                    }
                    throw WAPExceptionHelper.GetWAPException(10007001, ex.ToString(), null);
                }

            }
            foreach (string conn in Dir_StartTransction.Keys)
            {
                Dir_StartTransction[conn].CommitTransaction();
            }
            return true;
        }
        /// <summary>
        /// 得到字段和值得hashtable
        /// </summary>
        /// <returns></returns>
        public Hashtable ToHashTable()
        {
            return Fields.ToHashByValue();
        }
        /// <summary>
        /// 得到数据DataTable
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable ToDataTable()
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < Fields.Count; i++)
            {
                dt.Columns.Add(Fields[i].FieldName, Fields[i].type);
            }
            DataRow dr = dt.NewRow();
            for (int i = 0; i < Fields.Count; i++)
            {
                dr[i] = Fields[i].Value;
            }
            dt.Rows.Add(dr);
            return dt;
        }
        /// <summary>
        /// 得到数据DataTableByDesc
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable ToDataTableByDesc()
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < Fields.Count; i++)
            {
                dt.Columns.Add(Fields[i].Desc == null ? Fields[i].FieldName : Fields[i].Desc, Fields[i].type);
            }
            DataRow dr = dt.NewRow();
            for (int i = 0; i < Fields.Count; i++)
            {
                dr[i] = Fields[i].Value;
            }
            dt.Rows.Add(dr);
            return dt;
        }

        #endregion

        #region protected
        /// <summary>
        /// 从数据库得到初始化数据
        /// </summary>
        protected bool InitData()
        {
            foreach (string dbConnid in _dbConn_fc_Dir.Keys)
            {
                DBService db;
                if (_Dir_DBService.ContainsKey(dbConnid))
                {
                    db = _Dir_DBService[dbConnid];
                }
                else
                {
                    db = DataPersistence.GetDBService(dbConnid);
                }
                if (db == null)
                {
                    throw WAPExceptionHelper.GetWAPException(10007009, String.Format("Conntion Name is {0}.", dbConnid), null);
                }
                StringBuilder sb_fields = new StringBuilder();
                StringBuilder sb_TabName = new StringBuilder();
                StringBuilder sb_Where = new StringBuilder();

                foreach (Field f in _fields)
                {
                    if (sb_fields.Length > 0)
                        sb_fields.Append(",");
                    string bindDataFieldName;
                    string bindDataTabName;

                    if (f.Source.IndexOf('=') >= 0)
                    {
                        string[] sclist = f.Source.Split('=');
                        if (sb_Where.Length > 0)
                            sb_Where.Append(" and ");
                        sb_Where.Append(string.Format("{0}={1}", sclist[0], FunBase.ChkField(sclist[1],true)));
                        bindDataFieldName = getFullDataFieldName(sclist[0]);
                    }
                    else
                    {
                        bindDataFieldName = getFullDataFieldName(f.Source);

                    }
                    bindDataTabName = bindDataFieldName.Split('.')[0];
                    if (sb_TabName.Length > 0)
                    {
                        //modified by FYP
                        
                        int tmpindex = sb_TabName.ToString().IndexOf(bindDataTabName);
                        if (tmpindex < 0)
                        {
                            sb_TabName.Append(',');
                            sb_TabName.Append(bindDataTabName.ToUpper());
                        }
                        //sb_TabName.Append(',');
                        //int tmpindex = sb_TabName.ToString().IndexOf(bindDataTabName);
                        //if (tmpindex >= 0)
                        //{
                        //    sb_TabName.Append(bindDataTabName);
                        //}
                    }
                    else
                    {
                        sb_TabName.Append(bindDataTabName.ToUpper());
                    }
                    sb_fields.Append(bindDataFieldName);
                    if (f.IsKey)
                    {
                        if (sb_Where.Length > 0)
                            sb_Where.Append(" and ");
                        sb_Where.Append(string.Format("{0}={1}", bindDataFieldName,FunBase.ChkField(f.GetValueAsString(),true)));
                    }
                }
                string sql = string.Format("Select {0} From {1} Where {2} ", sb_fields.ToString(), sb_TabName.ToString(), sb_Where.ToString());
                string errCode = "";
                DataTable dt = db.SelectDataSet(sql, out errCode).Tables[0];
                if (dt.Rows.Count == 1)
                {
                    for (int i = 0; i < Fields.Count; i++)
                    {
                        Fields[i].Value = dt.Rows[0][Fields[i].FieldName];
                        Fields[i].SyncField();
                    }
                    return true;
                }
                else if (dt.Rows.Count == 0)
                {
                    return false;
                }
                else
                {
                    throw WAPExceptionHelper.GetWAPException(10001053, sql, null);
                }
            }
            return true;
        }
        #endregion
        /// <summary>
        /// 根据来源字符串得到 表名.字段名
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private string getFullDataFieldName(string source)
        {
            int tmpindex = source.IndexOf('.');
            return source.Substring(tmpindex+1);
        }
        /// <summary>
        /// 拆解符合的FieldCollection。 根据传入的FieldCollection 以连接名和表名为Key存到 _dbConn_fc_Dir 字典中成为多个单纯子FieldCollection
        /// </summary>
        /// <param name="fields"></param>
        private void decompose(FieldCollection fields)
        {
            foreach (Field thefield in fields)
            {
                string ConnID = defaultConnID;
                string TabID = defaultTableID;
                string FieldID = "";
                string[] sourceList = thefield.Source.Split('=');
                for (int i = 0; i < sourceList.Length; i++)
                {
                    string[] sourceItems = sourceList[0].Split('.');
                    if (sourceItems.Length < 1)
                    {
                        throw WAPExceptionHelper.GetWAPException(10001053, "", null);
                    }
                    if (sourceItems.Length == 1)
                    {
                        if (sourceItems[0].Trim().Length < 1)
                        {
                            FieldID = thefield.FieldName;
                        }
                        else
                        {
                            FieldID = sourceItems[0].Trim();
                        }
                    }
                    if (sourceItems.Length == 2)
                    {
                        FieldID = sourceItems[1];
                        TabID = sourceItems[0];
                    }
                    if (sourceItems.Length >= 3)
                    {
                        ConnID = sourceItems[sourceItems.Length - 3];
                        TabID = sourceItems[sourceItems.Length - 2];
                        FieldID = sourceItems[sourceItems.Length - 1];
                    }
                    Dictionary<string, FieldCollection> fc_dir;
                    FieldCollection fc;

                    if (ConnID == String.Empty || TabID == string.Empty)
                        continue;

                    if (!_dbConn_fc_Dir.ContainsKey(ConnID))
                    {
                        fc_dir = new Dictionary<string, FieldCollection>();
                        _dbConn_fc_Dir.Add(ConnID, fc_dir);
                    }
                    else
                    {
                        fc_dir = _dbConn_fc_Dir[ConnID];
                    }
                    if (!fc_dir.ContainsKey(TabID))
                    {
                        fc = new FieldCollection();
                        fc_dir.Add(TabID, fc);
                    }
                    else
                    {
                        fc = fc_dir[TabID];
                    }
                    if (fc[thefield.FieldName] == null)
                    {
                        fc.Add(thefield.Clone());
                    }
                    else
                    {
                        fc[thefield.FieldName] = thefield.Clone();
                    }
                    _fields[thefield.FieldName].Source = string.Format("{0}.{1}.{2}", ConnID, TabID, FieldID);
                }
            }

        }
    }
}

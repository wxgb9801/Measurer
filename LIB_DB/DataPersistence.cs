
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using LIB_Common;

namespace LIB_DB
{
    /// <summary>
    /// 数据永久化服务一小部分功能
    /// </summary>
    public class DataPersistence
    {
        private static Dictionary<string, DataOperation> do_Dict = new Dictionary<string, DataOperation>();
        public static Dictionary<string, string> dbConn_Dict = new Dictionary<string, string>();
        public static Dictionary<string, DBService> DBService_Dict = new Dictionary<string, DBService>();

        public static DBService GetDBServiceByServiceID(string serviceID)
        {
            if (DBService_Dict == null)
                DBService_Dict = new Dictionary<string, DBService>();
            if (DBService_Dict.ContainsKey(serviceID))
            {
                return DBService_Dict[serviceID];
            }
            else
            {
                DBService db = DBServiceFactory.GetDBService();
                DBService_Dict.Add(serviceID, db);
                return db;
            }
        }
        public static void AddDBServiceByServiceID(string serviceID,DBService db)
        {
            if (!DBService_Dict.ContainsKey(serviceID))
            {
                DBService_Dict.Add(serviceID, db);
            }
        }
        public static void RemoveDBServiceByServiceID(string serviceID)
        {
            if (DBService_Dict.ContainsKey(serviceID))
            {
                DBService_Dict.Remove(serviceID);
            }
        }
        public static DBService GetDBService(string connectID)
        {
            DBService tmpdb = null;

            if (dbConn_Dict.ContainsKey(connectID))
            {
                string connstr = dbConn_Dict[connectID];
                try
                {
                    tmpdb = DBServiceFactory.GetDBService(connstr);
                }
                catch
                {
                    tmpdb = null;
                }

            }
            if (tmpdb == null)
                return DBServiceFactory.GetDBService();
            else
                return tmpdb;
        }
        public static DataOperation GetDataOperation(string tabNameID, DBService dbserver, bool isload, FieldCollection fields)
        {
            DataOperation op = DBServiceFactory.GetDataOperation(tabNameID, dbserver, isload, fields);
            return op;
        }
    }
    public enum eDataOPType
    {
        Select,
        OP_Insert,
        OP_Delete,
        OP_Update,
        EXEC_SQL,
        Call
    }
    public enum eTastStatus
    {
        Added,
        Processing,
        Complete,
        Error
    }
    public class DataProcessTaskItem
    {
        public eDataOPType OPType;
        public eTastStatus TastStatus;
        public FieldCollection Fields;
        public Object AssistInfo;
        public Object Result;
        public DataProcessTaskItem(eDataOPType oPType, FieldCollection fields, Object assistInfo)
        {
            OPType = oPType;
            Fields = fields;
            AssistInfo = assistInfo;

        }
    }
    public class DataProcessTask
    {
        private eTastStatus _taskStatus;

        public eTastStatus TaskStatus
        {
            get { return _taskStatus; }
            set { _taskStatus = value; }
        }
        public List<DataProcessTaskItem> Items = new List<DataProcessTaskItem>();
        private Dictionary<string, DBService> dbConn_Dict = new Dictionary<string, DBService>();

        public Dictionary<string, DBService> DbConn_Dict
        {
            get { return dbConn_Dict; }
            set { dbConn_Dict = value; }
        }
        public DataProcessTask()
        {
            _taskStatus = eTastStatus.Added;
        }
        public void AddTaskItem(DataProcessTaskItem item)
        {
            Items.Add(item);
        }
        public void Clear()
        {
            Items.Clear();
        }
    }
    public class DataPersistenceService : IOPFieldObject
    {
        private string _uFOID;
        public string UFOID
        {
            get { return _uFOID; }
            set { _uFOID = value; }
        }

        public DataPersistenceService()
        {
            _uFOID = Guid.NewGuid().ToString();
        }
        public ConcurrentDictionary<string, FieldCollection> Flash_LVL1;
        public ConcurrentDictionary<string, FieldCollection> Flash_LVL2;
        public const UInt16 MaxClearTaskNum = 100;
        private object ObjectLock_Task = new object();
        private Thread ProcessThead;


        private List<DataProcessTask> Tasks = new List<DataProcessTask>();
        private bool _IsAutoExec = false;
        private bool _isClosed = false;

        public bool IsClosed
        {
            get { return _isClosed; }
            set { _isClosed = value; }
        }
        public bool IsAutoExec
        {
            get { return _IsAutoExec; }
            set { _IsAutoExec = value; }
        }
        private bool _isIgnoreUpdate;

        public bool IsIgnoreUpdate
        {
            get { return _isIgnoreUpdate; }
            set { _isIgnoreUpdate = value; }
        }
        public void AddTask(DataProcessTask task)
        {
            lock (ObjectLock_Task)
            {
                Tasks.Add(task);
            }
        }
        public void ExeTask()
        {
            lock (ObjectLock_Task)
            {
                for (int i = 0; i < Tasks.Count; i++)
                {
                    ExeOneTask(Tasks[i]);
                }
                Tasks.Clear();
                //if (Tasks.Count > MaxClearTaskNum)
                //{
                //    for (int i = 0; i < Tasks.Count; i++)
                //    {
                //        if (Tasks[i].TaskStatus == eTastStatus.Complete)
                //        {
                //            Tasks.Remove(Tasks[i]);
                //        }
                //    }
                //}
            }
        }
        public bool ExeOneTask(DataProcessTask task)
        {
            bool result = false;
            if (task.TaskStatus != eTastStatus.Added)
            {
                return result;
            }
            task.TaskStatus = eTastStatus.Processing;
            for (int j = 0; j < task.Items.Count; j++)
            {
                DataProcessTaskItem taskitem = task.Items[j];
                DataOperation_Complex DtOpC = new DataOperation_Complex(taskitem.Fields);
                DtOpC.IsForceUpdate = _isIgnoreUpdate;
                //WXG20131122@ModifyProcessEngine 
                foreach (KeyValuePair<string, DBService> kv in task.DbConn_Dict)
                {
                    DtOpC.AddDbService(kv.Key,kv.Value);
                }

                switch (taskitem.OPType)
                {
                    case eDataOPType.Select:
                        result = DtOpC.LoadFieldsBySource();
                        taskitem.Fields = DtOpC.Fields;
                        break;
                    case eDataOPType.OP_Insert:
                        result = DtOpC.Insert();
                        break;
                    case eDataOPType.OP_Delete:
                        result = DtOpC.Delete();
                        break;
                    case eDataOPType.OP_Update:
                        result = DtOpC.Update();
                        break;
                    case eDataOPType.EXEC_SQL:

                        break;
                    case eDataOPType.Call:
                        break;
                    default:
                        break;
                }
                taskitem.Result = result;
            }
            if (result)
            {
                task.TaskStatus = eTastStatus.Complete;
            }
            else
            {
                task.TaskStatus = eTastStatus.Error;
            }
            return result;
        }
        private void Exec()
        {
            while (_IsAutoExec)
            {
                ExeTask();
                Thread.Sleep(5);
            }
        }

        public void Start()
        {
            ProcessThead = new Thread(new ThreadStart(Exec));
            ProcessThead.Start();
        }
        public void Stop()
        {
            _IsAutoExec = false;
        }

        #region IOPFieldObject_M Members

        public bool CreateData(IFields Fields)
        {
            if (Fields != null)
            {
                FieldCollection fc = Fields.ToFieldCollection();
                DataProcessTask task = new DataProcessTask();
                task.AddTaskItem(new DataProcessTaskItem(eDataOPType.OP_Insert, fc, null));
                return ExeOneTask(task);
            }
            else
                return false;
        }
        public DataProcessTaskItem CreateDataToTask(DataProcessTask task, IFields Fields)
        {
           FieldCollection fc = Fields.ToFieldCollection();
           DataProcessTaskItem item = new DataProcessTaskItem(eDataOPType.OP_Insert, fc, null);
           task.AddTaskItem(item);
           return item;
        }
        public bool RemoveData(IFields Fields)
        {
            FieldCollection fc = Fields.ToFieldCollection();
            DataProcessTask task = new DataProcessTask();
            task.AddTaskItem(new DataProcessTaskItem(eDataOPType.OP_Delete, fc, null));
            return ExeOneTask(task);
        }
        public DataProcessTaskItem RemoveDataToTask(DataProcessTask task, IFields Fields)
        {
            FieldCollection fc = Fields.ToFieldCollection();
            DataProcessTaskItem item = new DataProcessTaskItem(eDataOPType.OP_Delete, fc, null);
            task.AddTaskItem(item);
            return item;
        }
        #endregion

        #region IOPFieldObject_U Members

        public bool LoadData(IFields Fields)
        {
            bool result;
            FieldCollection fc = Fields.ToFieldCollection();
            DataProcessTask task = new DataProcessTask();
            DataProcessTaskItem DPTI = new DataProcessTaskItem(eDataOPType.Select, fc, null);
            task.AddTaskItem(DPTI);
            result = ExeOneTask(task);
            return result && Fields.LoadFieldCollection(DPTI.Fields);

        }
        public DataProcessTaskItem LoadDataToTask(DataProcessTask task, IFields Fields)
        {
            //FieldCollection fc = Fields.ToFieldCollection();
            //DataProcessTaskItem item = new DataProcessTaskItem(eDataOPType.OP_Delete, fc, null);
            //task.AddTaskItem(item);
            //return item;
            throw new ApplicationException("Not Exists");
        }
        public bool SaveData(IFields Fields)
        {
            FieldCollection fc = Fields.ToFieldCollection();
            DataProcessTask task = new DataProcessTask();
            task.AddTaskItem(new DataProcessTaskItem(eDataOPType.OP_Update, fc, null));
            return ExeOneTask(task);
        }
        public DataProcessTaskItem SaveDataToTask(DataProcessTask task, IFields Fields)
        {
            FieldCollection fc = Fields.ToFieldCollection();
            DataProcessTaskItem item = new DataProcessTaskItem(eDataOPType.OP_Update, fc, null);
            task.AddTaskItem(item);
            return item;
        }

        //WXG20140715@BASF.Site
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Fields"></param>
        /// <param name="fc_New"></param>
        /// <returns> 0: Successed, 1:SetValue Fail;2:LoadDataField Fail;3:UpdataToDB Fail;4:Rollback Fail</returns>
        public Int32 UpdateData(IFields Fields, FieldCollection fc_New)
        {
            bool result = false;

            FieldCollection fc_Current = Fields.ToFieldCollection();
            FieldCollection fc_Mark = fc_Current.Clone();
            result = fc_Current.UpdateFieldsValue(fc_New);
            if (!result)
                return 2;
            DataProcessTask task = new DataProcessTask();
            task.AddTaskItem(new DataProcessTaskItem(eDataOPType.OP_Update, fc_Current, null));
            result = Fields.LoadFieldCollection(fc_Current);
            if (!result)
                return 3;
            result = ExeOneTask(task);
            if (!result)
            {
                result = Fields.LoadFieldCollection(fc_Mark);
                if (!result)
                    return 4;
            }
            return 0;
        }
        #endregion

    }
    public class DataPersitenceManger 
    {
        private static Dictionary<string, DataPersistenceService> Dict_Service = new Dictionary<string, DataPersistenceService>();

        public static int AddPersitenceService(string SvcID,DataPersistenceService DPService )
        {
            try
            {
                if (Dict_Service.ContainsKey(SvcID))
                {
                    return -1;
                }
                else
                {
                    Dict_Service.Add(SvcID, DPService);
                    return 0;
                }
            }
            catch(Exception)
            {
                return -1;
            }
        }
        public static int Remove(string SvcID)
        {
            try
            {
                if (Dict_Service.ContainsKey(SvcID))
                {
                    return -1;
                }
                else
                {
                    DataPersistenceService DPService = Dict_Service[SvcID];
                    if (DPService.IsClosed)
                    {
                        Dict_Service.Remove(SvcID);
                        return 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            catch(Exception)
            {
                return -1;
            }
        }
        public static DataPersistenceService GetPersitenceService(string SvcID)
        {
            if (Dict_Service.ContainsKey(SvcID))
            {
                return Dict_Service[SvcID];
            }
            return null;
        }
    }









}
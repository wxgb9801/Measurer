//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using LIB_Common;

//namespace LIB_DB.DataObject
//{
//    public class ProcessMessageDataManager
//    {
//        public static Dictionary<string, ProcessMessage> _dict_ProcessMessage;
//        public static Dictionary<string, ProcessMessage> Dict_ProcessMessage
//        {
//            get
//            {
//                if (_dict_ProcessMessage == null)
//                    _dict_ProcessMessage = new Dictionary<string, ProcessMessage>();
//                return _dict_ProcessMessage;
//            }
//        }
//        public static Dictionary<string, WAPDataObject> Dict_DataObject;

//        private static DataPersistenceService otherService = null;

//        static ProcessMessageDataManager()
//        {


//        }

//        protected static DataPersistenceService DPService
//        {
//            get
//            {
//                if (otherService == null)
//                {
//                    return WAPDataObjectHelper.GetDataDataPersitenceService();
//                }
//                else
//                {
//                    return otherService;
//                }
//            }
//        }
//        protected static DBService TheDB
//        {
//            get
//            {
//                return DataPersistence.GetDBServiceByServiceID(WAPDataObjectHelper.Get_DBServiceID());
//            }
//        }

//        public static ProcessMessage GetMessage(string messageID)
//        {
//            ProcessMessage pm = null;
//            Dict_ProcessMessage.TryGetValue(messageID, out pm);
//            if (pm == null)
//            {
//                pm = LoadMessageFromDB(messageID);
//            }
//            return pm;
//        }
//        public static void LoadMessageFromDB()
//        {

//        }

//        protected static ProcessMessage LoadMessageFromDB(string messageID)
//        {
//            ProcessMessage thePM = null;
//            DM_ProcessMessage dm_pm = new DM_ProcessMessage();
//            dm_pm.MESSAGEID = messageID;
//            DPService.LoadData(dm_pm);
//            WAPDataObjectDataManager wdodm = new WAPDataObjectDataManager();
//            thePM.DataObject = wdodm.LoadWAPDataObjectFromDB(dm_pm.DATAOBJCTID);
//            return thePM;
//        }


//        public static List<ProcessMessage> GetProcessMessageBySourceNotDone(string source)
//        {
//            List<ProcessMessage> result = new List<ProcessMessage>();
//            try
//            {
//                if (Dict_ProcessMessage != null)
//                {
//                    result = Dict_ProcessMessage.Values.Where(e => e.Source == source).ToList<ProcessMessage>();
//                }
//                else
//                {
//                    string sql = string.Format(string.Format("Select * From T_PROCESSMESSAGE Where Source={0} and Status<{1}", FunBase.ChkField(source, true), (int)eProcessMsgStatus.Done));
//                    DataTable dt = TheDB.SelectDataTable(sql);
//                    result = GetProcessListByDataTable(dt);
//                }
//                return result;
//            }
//            catch (Exception ex)
//            {
//                WAPDataObjectHelper.WriteException(ex);
//                return result;
//            }
//        }
//        public static List<ProcessMessage> GetProcessMessageByDestinationNotDone(string destination)
//        {
//            List<ProcessMessage> result = new List<ProcessMessage>();
//            if (Dict_ProcessMessage != null &&  Dict_ProcessMessage.Count>0)
//            {
//                result = Dict_ProcessMessage.Values.Where(e => e.Destination == destination).ToList<ProcessMessage>();
//            }
//            else
//            {
//                string sql = string.Format(string.Format("Select * From T_PROCESSMESSAGE Where Destination={0} and Status<{1}", FunBase.ChkField(destination,true),(int)eProcessMsgStatus.Done));
//                DataTable dt = TheDB.SelectDataTable(sql);
//                result = GetProcessListByDataTable(dt);
//            }
//            return result;
//        }

//        public static  bool SaveMessage(ProcessMessage pm,string saveBy)
//        {
//            //数据库连接  几个长数据连接----固定数量上线的的短连接---
//            //(1)子Field字段字段
//            //(2)子FielcCollection
//            //(3)主Field字段
//            //(4)主FieldCollection
//            //(5)WAPDataObject
//            //(6)ProcessMessage
//            DataProcessTask dptask = new DataProcessTask();
//            try
//            {
//                DM_ProcessMessage dm_pmsg = GetObjectDataModel_ProcessMsg(pm);
//                dm_pmsg.SAVEBY = saveBy;
//                dm_pmsg.SAVEON = System.DateTime.Now;
//                dm_pmsg.LOADTIMES = 1;
//                dm_pmsg.STATUS = 0;
//                WAPDataObjectDataManager wdodm = new WAPDataObjectDataManager();
//                bool ExistsDataOBject = wdodm.ExistsWAPDataObjectBydID(pm.DataObject.GUID);
//                if (!ExistsDataOBject)
//                {
//                    wdodm.SaveWAPDataObjectToDBTask(dptask, pm.DataObject);

//                    if (!Dict_ProcessMessage.ContainsKey(pm.GUID))
//                    {
//                        Dict_ProcessMessage.Add(pm.GUID, pm);
//                    }
//                    else
//                    {
//                        Dict_ProcessMessage[pm.GUID] = pm;
//                    }
//                }
//                dptask.AddTaskItem(new DataProcessTaskItem(eDataOPType.OP_Insert, dm_pmsg.DFields, null));
//                return DPService.ExeOneTask(dptask);
//            }
//            catch(Exception ex)
//            {
//                WAPDataObjectHelper.WriteException(ex);
//                return false;
//            }  
//        }
//        /// <summary>
//        /// 内存中移出，标记状态
//        /// </summary>
//        /// <param name="pm"></param>
//        /// <returns></returns>
//        public static bool RemoveMessage(ProcessMessage pm)
//        {
//            DataProcessTask dptask = new DataProcessTask();
//            try
//            {
//                pm.Status = eProcessMsgStatus.Done;
//                DM_ProcessMessage dm_pmsg = GetObjectDataModel_ProcessMsg(pm);
//                dptask.AddTaskItem(new DataProcessTaskItem(eDataOPType.OP_Update, dm_pmsg.DFields, null));
//                WAPDataObjectDataManager wdodm = new WAPDataObjectDataManager();
//                wdodm.MarkRemoveWAPDataObjectToTask(dptask, pm.DataObject);
//                return DPService.ExeOneTask(dptask);
//            }
//            catch (Exception ex)
//            {
//                WAPDataObjectHelper.WriteException(ex);
//                return false;
//            }
//        }

//        protected static DM_ProcessMessage GetObjectDataModel_ProcessMsg(ProcessMessage pmsg)
//        {
//            DM_ProcessMessage dm_pmsg = new DM_ProcessMessage();
//            dm_pmsg.MESSAGEID = pmsg.GUID;
//            dm_pmsg.PROCCESSID = pmsg.ProcessID;
//            dm_pmsg.PROCESSTYPE = pmsg.ProcessType;
//            dm_pmsg.PROCESSVEXTEXID = pmsg.ProcessVertexID;
//            dm_pmsg.PRI = pmsg.Priority;
//            dm_pmsg.ACTIVITYCONTRACT = pmsg.ActivityContract.UFOID;
//            dm_pmsg.SOURE = pmsg.Source;
//            dm_pmsg.DESTINATION = pmsg.Destination;
//            dm_pmsg.DATAOBJCTID = pmsg.DataObject.GUID;
//            dm_pmsg.STATUS = (int)pmsg.Status;
//            dm_pmsg.LOADTIMES = 1;
//            dm_pmsg.PROCESSREQUESTID = pmsg.ProcessRequestID;
//            return dm_pmsg;
//        }
//        protected static ProcessMessage GetObjectProcessMsg_DataModel(DM_ProcessMessage dm_pmsg)
//        {
//            ProcessMessage  pmsg= new ProcessMessage();
//            pmsg.GUID = dm_pmsg.MESSAGEID;
//            pmsg.ProcessID = dm_pmsg.PROCCESSID;
//            pmsg.ProcessType = dm_pmsg.PROCESSTYPE;
//            pmsg.ProcessVertexID = dm_pmsg.PROCESSVEXTEXID;
//            pmsg.Priority = dm_pmsg.PRI;
//            pmsg.ActivityContractString = dm_pmsg.ACTIVITYCONTRACT;
//            pmsg.Source = dm_pmsg.SOURE;
//            pmsg.Destination = dm_pmsg.DESTINATION;
//            pmsg.Status=(eProcessMsgStatus)dm_pmsg.STATUS;
//            pmsg.ProcessRequestID = dm_pmsg.PROCESSREQUESTID;
//            return pmsg;
//        }
//        private static List<ProcessMessage> GetProcessListByDataTable(DataTable dt)
//        {
//            List<ProcessMessage> result = new List<ProcessMessage>();
//            for (int i = 0; i < dt.Rows.Count; i++)
//            {
//                DM_ProcessMessage dm_pm = new DM_ProcessMessage();
//                dm_pm.DFields.SetFieldsValue(dt.Rows[i]);
//                WAPDataObjectDataManager wdodm = new WAPDataObjectDataManager();
//                WAPDataObject wdo = wdodm.LoadWAPDataObjectFromDB(dm_pm.DATAOBJCTID);
//                ProcessMessage thePM = GetObjectProcessMsg_DataModel(dm_pm);
//                thePM.DataObject = wdo;
//                if (_dict_ProcessMessage == null)
//                    _dict_ProcessMessage = new Dictionary<string, ProcessMessage>();
//                if (!_dict_ProcessMessage.ContainsKey(thePM.GUID))
//                {
//                    _dict_ProcessMessage.Add(thePM.GUID, thePM);
//                }
//                result.Add(thePM);
//            }
//            return result;
//        }
//    }
//}

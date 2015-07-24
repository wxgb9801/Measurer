//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using LIB_Common;

//namespace LIB_DB.DataObject
//{

//    public class WAPDataObjectDataManager
//    {
//        public static  Dictionary<string, WAPDataObject> SysDataObjects;

//        protected static  DBService DB
//        {
//            get
//            {
//                return  DataPersistence.GetDBService(WAPDataObjectHelper.Get_DBServiceID());
//            }
//        }
//        protected static DataPersistenceService DPS
//        {
//            get
//            {
//                return WAPDataObjectHelper.GetDataDataPersitenceService();
//            }
//        }
//        public WAPDataObjectDataManager()
//        {
//        }

//        public  bool ExistsWAPDataObjectBydID(string GUID)
//        {
//            WAPDataObject dataObject = LoadWAPDataObjectFromDB(GUID);
//            return dataObject != null;
//        }
//        public void SaveWAPDataObjectToDBTask(DataProcessTask task, WAPDataObject wdo)
//        {
//            DM_WAPDataObjectInst dm_wdo = GetObjectDataModel_WAPDataObject(wdo);
//            dm_wdo.STATUS = eWAPDataObjectStatus.Created;
//            DataProcessTaskItem item = new DataProcessTaskItem(eDataOPType.OP_Insert, dm_wdo.DFields, null);
//            task.AddTaskItem(item);
//            for (int i = 0; i < wdo.Count; i++)
//            {
//                FieldCollection fc = wdo[i];
//                SaveFieldColletionToDBTask(task, dm_wdo.WDOGUID, String.Empty, wdo[i]);
//            }
//        }
//        protected void SaveFieldColletionToDBTask(DataProcessTask task, string wdoGuid, string pfcID, FieldCollection fc)
//        {
//            DM_FieldCollectionInst dm_fc = GetObjectDataModel_FieldCollection(wdoGuid, pfcID, fc);
//            DataProcessTaskItem item = new DataProcessTaskItem(eDataOPType.OP_Insert, dm_fc.DFields, null);
//            task.AddTaskItem(item);
//            if (fc.ChildTable != null)
//            {
//                for (int i = 0; i < fc.ChildTable.Count; i++)
//                {
//                    SaveFieldColletionToDBTask(task, wdoGuid, fc.GUID, fc.ChildTable[i]);
//                }
//            }
//            for (int i = 0; i < fc.Count; i++)
//            {
//                SaveFieldToDBTask(task,fc.GUID,i, fc[i]);
//            }

//        }
//        protected void SaveFieldToDBTask(DataProcessTask task,string fcID,int index, Field fld)
//        {
//            DM_FieldInst dm_fld = GetObjectDataModel_Field(fcID, index, fld);
//            DataProcessTaskItem item = new DataProcessTaskItem(eDataOPType.OP_Insert, dm_fld.DFields, null);
//            task.AddTaskItem(item);
//       }

//        public  WAPDataObject LoadWAPDataObjectFromDB(string wdoID)
//        {
//            WAPDataObject theDataObject = null;
//            bool result = false;
//            if (SysDataObjects != null)
//            {
//                result = SysDataObjects.TryGetValue(wdoID, out theDataObject);
//            }
//            if (theDataObject == null)
//            {
//                DM_WAPDataObjectInst dm_wdo = new DM_WAPDataObjectInst();
//                dm_wdo.WDOGUID = wdoID;
//                DataPersistenceService dps = WAPDataObjectHelper.GetDataDataPersitenceService();
//                result = dps.LoadData(dm_wdo);
//                if (result)
//                {
//                    theDataObject = GetWAPDataObject_DM(dm_wdo);
//                    List<FieldCollection> childs = LoadFieldCollectionsFromDB(wdoID);
//                    theDataObject.AddRange(childs);
//                }
//                else
//                {
//                    return null;
//                }
//            }
//            return theDataObject;
//        }
//        /// <summary>
//        /// Load FieldCollection List From DB
//        /// </summary>
//        /// <param name="wdoID"></param>
//        /// <sql ID="">Select * From T_FIELDCOLLECTIONINST  where WDOGUID={0} start with  ParentFCID is null  connect by  PRIOR FCGUID=  ParentFCID   Order by level,rownum</sql>
//        /// <returns></returns>
//        private List<FieldCollection> LoadFieldCollectionsFromDB(string wdoID)
//        {
//            List<FieldCollection> fcs = new List<FieldCollection>();
//            DBService theDB = DataPersistence.GetDBService(WAPDataObjectHelper.Get_DBServiceID());
//            string sql = string.Format("Select * From T_FIELDCOLLECTIONINST Where WDOGUID={0} start with ParentFCID is null  connect by  PRIOR FCGUID=ParentFCID  Order by level,rownum", 
//                         FunBase.ChkField(wdoID,true));
//            DataTable dt = theDB.SelectDataTable(sql);
//            for (int i = 0; i < dt.Rows.Count; i++)
//            {
//                DM_FieldCollectionInst dm_fc = new DM_FieldCollectionInst();
//                dm_fc.DFields.SetFieldsValue(dt.Rows[i]);
//                FieldCollection dbFc = GetFieldCollection_DM(dm_fc);
//                FieldCollection schemaFcClone = SysBusPropertySchemaManager.GetBusPropertySchema(dm_fc.FIELDSTRUCTTYPE);
//                if (schemaFcClone == null)
//                {
//                    schemaFcClone = dbFc;
//                    continue;
//                }
//                else
//                {
//                    schemaFcClone.GUID = dbFc.GUID;
//                }
//                LoadFieldsFromDB(schemaFcClone, dm_fc.FCGUID);

//                if (dm_fc.PARENTFCID ==string.Empty)
//                {
//                    fcs.Add(schemaFcClone);
//                }
//                else
//                {
//                    FieldCollection parentFc = fcs.Where(e => e.GUID == dm_fc.PARENTFCID).FirstOrDefault();
//                    if (parentFc.ChildTable == null)
//                    {
//                        parentFc.ChildTable = new FieldTable();
//                        parentFc.ChildTable.ProtoFieldCollection = SysBusPropertySchemaManager.GetBusPropertySchema(dm_fc.FIELDSTRUCTTYPE);
//                    }
//                    parentFc.ChildTable.AddRow(schemaFcClone);
//                }
//            }
//            return fcs;
//        }
//        private void LoadFieldsFromDB(FieldCollection fc,string fcID)
//        {
//            List<Field> flds = new List<Field>();
//            DBService theDB = DataPersistence.GetDBService(WAPDataObjectHelper.Get_DBServiceID());
//            string sql = string.Format("Select * From T_FIELDSINST Where FCGUID={0} Order by FieldIndex", FunBase.ChkField(fcID,true));
//            DataTable dt = theDB.SelectDataTable(sql);
//            if (dt.Rows.Count == fc.Count)
//            {
//                for (int i = 0; i < dt.Rows.Count; i++)
//                {
//                    DM_FieldInst dm_fld = new DM_FieldInst();
//                    dm_fld.DFields.SetFieldsValue(dt.Rows[i]);
//                    LoadField_DM(fc[i], dm_fld);
//                }
//            }
//            else
//            {
//                throw new ApplicationException("MissMatch FieldCollection Struct");
//            }
//        }


//        public void MarkRemoveWAPDataObjectToTask(DataProcessTask task, WAPDataObject wdo)
//        {
//            string wdoID = wdo.GUID;
//            if (SysDataObjects!=null && SysDataObjects.ContainsKey(wdoID))
//            {
//                SysDataObjects.Remove(wdoID);
//            }
//            DM_WAPDataObjectInst dm_wdo = GetObjectDataModel_WAPDataObject(wdo);
//            dm_wdo.STATUS = eWAPDataObjectStatus.Expired;
//            dm_wdo.WDOGUID = wdoID;
//            DPS.SaveDataToTask(task, dm_wdo);
//        }
//        public bool MarkRemoveWAPDataObject(WAPDataObject wdo)
//        {
//            bool result = false;
//            string wdoID = wdo.GUID;
//            if (SysDataObjects.ContainsKey(wdoID))
//            {
//                result = SysDataObjects.Remove(wdoID);
//            }
//            DM_WAPDataObjectInst dm_wdo = GetObjectDataModel_WAPDataObject(wdo);
//            dm_wdo.WDOGUID = wdoID;
//            result = DPS.SaveData(dm_wdo);
//            return result;
//        }

//        protected static DM_WAPDataObjectInst GetObjectDataModel_WAPDataObject(WAPDataObject wdo)
//        {
//            DM_WAPDataObjectInst dm_wdo = new DM_WAPDataObjectInst();
//            dm_wdo.WDOGUID = wdo.GUID;
//            dm_wdo.WDONAME = wdo.GUID;
//            dm_wdo.ITEMLENGTH = wdo.Count;
//            dm_wdo.CREATEON = DateTime.Now;
//            return dm_wdo;
//        }
//        protected static DM_FieldCollectionInst GetObjectDataModel_FieldCollection(string wdoid,string pfcID ,FieldCollection fc)
//        {
//            DM_FieldCollectionInst dm_fc = new DM_FieldCollectionInst();
//            dm_fc.FCGUID = fc.GUID;
//            dm_fc.PARENTFCID = pfcID;
//            dm_fc.WDOGUID = wdoid;
//            dm_fc.CREATEON = DateTime.Now;
//            dm_fc.PARENTTYPE = pfcID == fc.GUID ? 0 : 1;
//            dm_fc.FIELDSTRUCTTYPE = fc.Name;
//            return dm_fc;
//        }
//        protected static DM_FieldInst GetObjectDataModel_Field(string fcid, int index,Field fld)
//        {
//            string fieldString = ((ICustomerSerializable)fld).ToSerializableString(eCustomerSerializableType.NoStruct);
//            DM_FieldInst dm_fld = new DM_FieldInst();
//            dm_fld.FCGUID = fcid;
//            dm_fld.FIELDGUID = fld.GUID;
//            dm_fld.FIELDINDEX = index;
//            dm_fld.FIELDSERIALSTRING1 = fieldString;
//            dm_fld.CREATEON = DateTime.Now;
//            return dm_fld; 
//        }


//        protected static WAPDataObject GetWAPDataObject_DM(DM_WAPDataObjectInst dm_wdo)
//        {
//            return WAPDataObject.CreateWAPDataObjectByGUID(dm_wdo.WDOGUID);
//        }
//        protected static FieldCollection GetFieldCollection_DM(DM_FieldCollectionInst dm_wdo)
//        {
//            FieldCollection fc = FieldCollection.CreateFiledCollectionByGUID(dm_wdo.FCGUID);
//            return fc;
//        }
//        protected static void LoadField_DM(Field fld,DM_FieldInst dm_fld)
//        {
//            fld.GUID = dm_fld.FIELDGUID;
//            string fldstr = string.Format("{0}{1}", dm_fld.FIELDSERIALSTRING1, dm_fld.FIELDSERIALSTRING2);
//            ((ICustomerSerializable)fld).LoadObjectFromSerializableString(eCustomerSerializableType.NoStruct, fldstr);
//        }
//    }
//}

//using System;
//using System.Collections;
//using System.Text;
//using LIB_Common;
//using WAP.LOG;
//namespace LIB_DB.DataObject
//{
//    public class WAPDataObjectHelper
//    {
//        private static DataPersistenceService _defaultDataPersistenceService;

//        public static string Get_DBServiceID()
//        {
//            //return "WAPDataObject";
//            return "ORACLE";
//        }
//        public static string Get_LogName()
//        {
//            return "ProcessEngine";
//        }

//        private static DataPersistenceService CreateDataPersitenceService(string name)
//        {
//            DataPersistenceService theService = new DataPersistenceService();
//            theService.IsAutoExec = false;
//            DataPersitenceManger.AddPersitenceService(name, theService);
//            return theService;
//        }
//        public static DataPersistenceService GetDataDataPersitenceService()
//        {
//            if (_defaultDataPersistenceService == null)
//            {
//                _defaultDataPersistenceService = DataPersitenceManger.GetPersitenceService("WAPDataObject");
//                if (_defaultDataPersistenceService == null)
//                {
//                    _defaultDataPersistenceService = CreateDataPersitenceService("WAPDataObject");
//                }
//            }
//            else
//            {
//                return _defaultDataPersistenceService;
//            }
//            return _defaultDataPersistenceService;
//        }

//        public static void WriteMessage(uint exceptionID)
//        {
//            WriteMessage(Get_LogName(), exceptionID);
//        }
//        public static void WriteMessage(string logName, uint exceptionID)
//        {
//            try
//            {
//                LogService _logsvc = LogManager.GetLogService(logName);
//                FieldCollection fcdta;
//                if (_logsvc != null)
//                {
//                    fcdta = WAPMessageHelper.GetWAPMessage(exceptionID);
//                    _logsvc.WriteLog(Convert.ToUInt32(fcdta["MessageGrade"].Value), fcdta["MessageBody"].GetValueAsString());
//                }

//            }
//            catch (Exception ex)
//            {
//                WriteException(ex);
//            }
//        }
//        public static void WriteMessageAndInfo(uint exceptionID, string param)
//        {
//            WriteMessageAndInfo(Get_LogName(), exceptionID, param);
//        }
//        public static void WriteMessageAndInfo(string logName, uint exceptionID, string param)
//        {
//            try
//            {
//                LogService _logsvc = LogManager.GetLogService(logName);
//                FieldCollection fcdta;
//                if (_logsvc != null)
//                {
//                    fcdta = WAPMessageHelper.GetWAPMessage(exceptionID);
//                    _logsvc.WriteLog(Convert.ToUInt32(fcdta["MessageGrade"].Value), string.Format(fcdta["MessageBody"].GetValueAsString(), param));
//                }

//            }
//            catch (Exception ex)
//            {
//                WriteException(ex);
//            }
//        }
//        public static void WriteLog_Lvl5(string info)
//        {
//            LogService _logsvc = LogManager.GetLogService(Get_LogName());
//            _logsvc.WriteLog(5, info);
//        }
//        public static void WriteLog_Lvl1(string info)
//        {
//            LogService _logsvc = LogManager.GetLogService(Get_LogName());
//            _logsvc.WriteLog(1, info);
//        }
//        public static void WriteException(Exception ex)
//        {
//            LogService _logsvc = LogManager.GetLogService(Get_LogName());
//            {
//                _logsvc.WriteLog(ex.Message);
//            }
//        }
//        public static void WriteException(Exception ex, string extraInfo)
//        {
//            LogService _logsvc = LogManager.GetLogService(Get_LogName());
//            {
//                string msg = string.Format("{0}---{1}", ex.Message, extraInfo);
//                _logsvc.WriteLog(msg);
//            }
//        }
//        public static void WriteException(Exception ex, FieldCollection fc)
//        {
//            string extraInfo = "";
//            if (fc != null)
//                extraInfo = fc.ToString();
//            WriteException(ex, extraInfo);
//        }

//        public static WAPGeneralException GetWAPException(uint exceptionID, ArrayList extraInfo, Exception ex)
//        {
//            FieldCollection fcdta;
//            StringBuilder sb = new StringBuilder();
//            if (extraInfo != null)
//            {
//                for (int i = 0; i < extraInfo.Count; i++)
//                {
//                    object obj = extraInfo[i];
//                    string msg = "";
//                    if (obj is int)
//                    {
//                        fcdta = WAPMessageHelper.GetWAPMessage(uint.Parse(obj.ToString()));
//                        msg = fcdta["MessageBody"].GetValueAsString();
//                        if ((i + 1) < extraInfo.Count)
//                        {
//                            obj = extraInfo[i + 1];
//                            if (obj is string)
//                            {
//                                string pram = ((string)obj);
//                                //if (pram.Substring(0, 1) == "$")
//                                //{
//                                msg = string.Format(msg, pram);
//                                //}
//                            }
//                        }
//                    }
//                    sb.Append(msg);
//                }
//            }
//            return WAPExceptionHelper.GetWAPException(exceptionID, sb.ToString(), ex);
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using Log_LIB;
namespace Measurer
{
    static class Program
    {
        public  static WriteErrorLog WLog = new WriteErrorLog();

        public static void WriteErrorLog(string model,Exception ex)
        {
            Program.WLog.WriteLog(Enum_LogType.LogType_Error, Enum_LogGrade.LogGrade_One, Enum_LogMessageType.LogMsgType_Exception, model,ex.Message,ex.StackTrace);
        }
        public static void WriteEventLog(string model, string Info,string description)
        {
            Program.WLog.WriteLog(Enum_LogType.LogType_Error, Enum_LogGrade.LogGrade_One, Enum_LogMessageType.LogMsgType_Event, model, Info, description);
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Frm_Login());
        }
        private static void InitLog()
        {
            string logType = ConfigurationManager.AppSettings["logType"].ToString().Trim();
            string logGrade = ConfigurationManager.AppSettings["logGrade"].ToString().Trim();
            string logMsgType = ConfigurationManager.AppSettings["LogMsgType"].ToString().Trim();
            string logFunName = ConfigurationManager.AppSettings["LogFunctionName"].ToString().Trim();
            string logMessage = ConfigurationManager.AppSettings["LogMessage"].ToString().Trim();
            string logDesp = ConfigurationManager.AppSettings["LogDescription"].ToString().Trim();
            string seperator = ConfigurationManager.AppSettings["Seperator"].ToString().Trim();

            WLog.SetLogPara(logType, logGrade, logMsgType, logFunName, logMessage, logDesp, seperator);
            WLog.IsBeginProcess = true;
            WLog.SleepTime = 2000;
            WLog.logName = "CommunLog";
            WLog.Start(WLog);
            WLog.WriteLog(Enum_LogType.LogType_StartStop,
                                   Enum_LogGrade.LogGrade_One,
                                   Enum_LogMessageType.LogMsgType_Event,
                                   "StartLog",
                                   "Log Thread Start",
                                   "");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Commu_ClsLibrary.CommuPort;
using System.Data;
using LIB_DB;
using LIB_Common;
using System.Globalization;
using System.ComponentModel;
namespace Measurer
{
    public enum eTaskSts
    {
        Start,
        Complete,
        End
    }

    public class Task
    {
        public const string DateTimeFormat1 = "yyyy-MM-dd HH:mm:ss";
        public const string DateTimeFormat2 = "yyyyMMddHHmmss";
        public const string FromDateTimeFormat3 = "yyyy-MM-dd 00:00:00";
        public const string ToDateTimeFormat3 = "yyyy-MM-dd 24:00:00";

        public string TaskNo;
        public string BatchNo;
        public string ItemNo;
        public string ItemName;
        public string Sn;
        public int PV;
        public int Result;
        public int redundantVaue;
        public DateTime StartTime;
        public DateTime CompleteTime;
        public eTaskSts Status;

        public static DataTable GetTaskStruct()
        {
            DataTable dt = new DataTable("T_Task");
            dt.Columns.Add("TaskNo");
            dt.Columns.Add("BatchNo");
            dt.Columns.Add("ItemNo");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("Sn");
            dt.Columns.Add("PV");
            dt.Columns.Add("Result");
            dt.Columns.Add("redundantVaue");
            dt.Columns.Add("computeRatio");
            dt.Columns.Add("Status");
            dt.Columns.Add("StartTime");
            dt.Columns.Add("CompleteTime");
            dt.Columns.Add("ModifyTime");
            return dt;
        }
        public static DataTable GetRecordStruct()
        {
            DataTable dt = new DataTable("T_Record");
            dt.Columns.Add("TaskNo");
            dt.Columns.Add("BatchNo");
            dt.Columns.Add("ItemNo");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("Sn");
            dt.Columns.Add("PV");
            dt.Columns.Add("Result");
            dt.Columns.Add("redundantVaue");
            dt.Columns.Add("computeRatio");
            dt.Columns.Add("Status");
            dt.Columns.Add("StartTime");
            dt.Columns.Add("CompleteTime");
            dt.Columns.Add("ModifyTime");
            dt.Columns[0].Unique = true;
            return dt;
        }
        public static DataTable GetItemTask()
        {
            DataTable dt = new DataTable("T_Item");
            dt.Columns.Add("ItemNo");
            dt.Columns.Add("ItemName");
            return dt;
        }

        public static Task CreateTaskByTable(DataTable dt)
        {
            Task task = null;
            try
            {
                task = new Task();
                task.TaskNo = (dt.Rows[0]["TaskNo"] == null ? "" : dt.Rows[0]["TaskNo"].ToString());
                task.BatchNo = (dt.Rows[0]["BatchNo"] == null ? "" : dt.Rows[0]["BatchNo"].ToString());
                task.ItemNo = (dt.Rows[0]["ItemNo"] == null ? "" : dt.Rows[0]["ItemNo"].ToString());
                task.ItemName = (dt.Rows[0]["ItemName"] == null ? "" : dt.Rows[0]["ItemName"].ToString());
                task.Sn = (dt.Rows[0]["Sn"] == null ? "" : dt.Rows[0]["Sn"].ToString());
                task.PV = (dt.Rows[0]["PV"] == null ? 0 : int.Parse(dt.Rows[0]["PV"].ToString()));
                task.Result = (dt.Rows[0]["Result"] == null ? 0 : int.Parse(dt.Rows[0]["Result"].ToString()));
                task.redundantVaue = (dt.Rows[0]["RedundantVaue"] == null ? 0 : int.Parse(dt.Rows[0]["RedundantVaue"].ToString()));
                IFormatProvider ifp = new CultureInfo("zh-CN", true);
                if (dt.Rows[0]["StartTime"] != null)
                {
                    DateTime.TryParseExact(dt.Rows[0]["StartTime"].ToString(), DateTimeFormat2, ifp, DateTimeStyles.None, out task.StartTime);
                }
                if (dt.Rows[0]["CompleteTime"] != null)
                {
                    DateTime.TryParseExact(dt.Rows[0]["CompleteTime"].ToString(), DateTimeFormat2, ifp, DateTimeStyles.None, out task.CompleteTime);
                }
                return task;
            }
            catch (Exception ex)
            {
                Program.WriteErrorLog("CreateTaskByTable", ex);
                return task;
            }
        }

    }
}
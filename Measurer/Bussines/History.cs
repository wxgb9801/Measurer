using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using LIB_DB;
using LIB_Common;

namespace Measurer
{
    public class HistoryManger
    {
        public DBService theDB;

        public HistoryManger()
        {
            theDB = DBServiceFactory.GetDBService("MeasurerXML");
        }
        public DataTable GetHistory(string ItemNo,DateTime  FromDate,DateTime ToDate)
        {
            DataTable dt = null;
            try
            {
                dt = new DataTable();
                StringBuilder Sql = new StringBuilder("Select * From T_Record Where 1=1 ");
                Sql.Append(string.Format(" and ItemNo like '%{0}%'", FunBase.ChkField(ItemNo,false)));
                Sql.Append(string.Format(" and CompleteTime>={0}", FunBase.ChkField(FromDate.ToString(Task.FromDateTimeFormat3), true)));
                Sql.Append(string.Format(" and CompleteTime<={0}", FunBase.ChkField(ToDate.ToString(Task.ToDateTimeFormat3), true)));
                dt = theDB.SelectDataTable(Sql.ToString());
                return dt;
            }
            catch (Exception ex)
            {
                return dt;
            }
        }
    }
}

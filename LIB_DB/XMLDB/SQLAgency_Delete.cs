using System;
using System.Collections.Generic;
using System.Data;

namespace LIB_DB
{
    public class SQLAgency_Delete : ISqlExecAgency
    {
        public const eSqlType OPSqlType = eSqlType.DELETE;
        private string _table;
        private DataSet _dS;
        private string[] SplitSpaceStrs;
        private string _sql;

        public string Sql
        {
            get { return _sql; }
            set { _sql = value; }
        }

        private List<Object> _result;

        public List<Object> Result
        {
            get { return _result; }
            set { _result = value; }
        }
        private string _whereString;
        public string Wherestring
        {
            get { return _whereString; }
            set { _whereString = value; }
        }
        #region SqlAgency Members
        public string Table
        {
            get { return _table; }
            set { _table = value; }
        }
        public DataSet DS
        {
            get { return _dS; }
        }
        public bool SQLExec(DataSet DataSource, string OPSQL, out string outMessage, out int chgRecordCount)
        {
            outMessage = "";
            _dS = DataSource;
            _sql = OPSQL;
            SplitSpaceStrs = _sql.Split(' ');
            eSqlType type = SQLAnalysiser.GetSqlType(_sql);
            if (type != OPSqlType)
            {
                throw new ApplicationException(string.Format("无效的[{0}]语句{{1}", OPSqlType.ToString(), _sql));
            }
            _table = GetTable();
            _whereString = SQLAnalysiser.GetWhereString(_sql);
            _result = new List<object>();
             return execDelete(out outMessage, out chgRecordCount);
        }

        #endregion
        private bool execDelete(out string outMessage,out int chgRecordCount)
        {
            outMessage = "";
            chgRecordCount = 0;
            try
            {
                DataTable dt = _dS.Tables[_table];
                DataTable tmpdt = dt.Copy();

                DataView dv = new DataView(dt, _whereString, "", DataViewRowState.CurrentRows);
                foreach (DataRowView drv in dv)
                {
                    dt.Rows.Remove(drv.Row);
                }
                //_dS.Tables.Remove(dt);
                //_dS.Tables.Add(tmpdt);
                chgRecordCount = dv.Count;
                return true;
            }
            catch (Exception ex)
            {
                outMessage = ex.Message;
                chgRecordCount = 0;
                return false;
            }
        }
        private string GetTable()
        {
            string table = SplitSpaceStrs[1];

            if (table.ToUpper() == "TABLE" && SplitSpaceStrs.Length>2)
            {
                table = SplitSpaceStrs[2];
            }
           
            if (_dS.Tables.Contains(table))
            {
                return table;
            }
            else
            {
                throw new ApplicationException(string.Format("数据源[{0}]不存在。", table));
            }
        }
    }
}

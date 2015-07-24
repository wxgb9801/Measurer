using System;
using System.Collections.Generic;
using System.Data;
namespace LIB_DB
{
    public class SQLAgency_Drop : ISqlExecAgency
    {
        public const eSqlType OPSqlType = eSqlType.DROP;
        private string _table = "";
        private string[] SplitSpaceStrs;
        private DataSet _dS;
        #region ISqlExecAgency Members

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

        public string Table
        {
            get { return _table; }
        }
        public DataSet DS
        {
            get { return _dS; }
        }
        public bool SQLExec(DataSet DataSource, string OPSQL, out string outMessage,out int chgRecordCount)
        {
            outMessage = "";
            chgRecordCount = 0;
            try
            {
                _sql = OPSQL;
                _table = GetTable();
                DataSource.Tables.Remove(_table);
                chgRecordCount = 1;
                _result = new List<object>();
                return true;
            }
            catch (Exception ex)
            {
                outMessage = ex.Message;
                return false;
            }

        }
        private string GetTable()
        {
            string table = SplitSpaceStrs[1];

            if (DS.Tables.Contains(table))
            {
                return table;
            }
            else
            {
                throw new ApplicationException(string.Format("数据源[{0}]不存在。", table));
            }
        }
        #endregion
    }
}

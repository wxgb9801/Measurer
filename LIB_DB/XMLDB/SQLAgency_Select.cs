using System;
using System.Collections.Generic;
using System.Data;
namespace LIB_DB
{
    public class SQLAgency_Select : ISqlExecAgency
    {
        private string _table;
        private string _sql;
        private DataSet _dS;

        private List<Object> _result;
        private string[] _selectItems;

        public string[] SelectItems
        {
            get { return _selectItems; }
            set { _selectItems = value; }
        }
        private string _whereString;

        public string WhereString
        {
            get { return _whereString; }
            set { _whereString = value; }
        }
        private string[] SplitSpaceStrs;

        #region ISqlExecAgency Members

        public string Sql
        {
            get { return _sql; }
            set { _sql = value; }
        }

        public List<Object> Result
        {
            get { return _result; }
            set { _result = value; }
        }
        public string Table
        {
            get { return _table; }
            set { _table = value; }
        }

        System.Data.DataSet ISqlExecAgency.DS
        {
            get { return _dS; }
        }

        public bool SQLExec(System.Data.DataSet DataSource, string OPSQL, out string outMessage, out int chgRecordCount)
        {
            outMessage = "";
            chgRecordCount = 0;
            _sql = OPSQL;

            try
            {
                _dS = DataSource;
                SplitSpaceStrs = _sql.Split(' ');
                GetTable();
                _whereString = SQLAnalysiser.GetWhereString(_sql);
                string selecString = GetSelectString();
                _selectItems = selecString.Split(',');

                DataTable dt = _dS.Tables[_table];

                DataView dv = new DataView(dt, _whereString, "", DataViewRowState.CurrentRows);
                 DataTable tmpdt;
                 if (selecString == "*")
                 {
                     tmpdt = dv.ToTable(_table);
                 }
                 else
                 {
                     tmpdt = dv.ToTable(_table, false, _selectItems);
                 }
                _result = new List<object>();
                _result.Add(tmpdt);
                chgRecordCount = tmpdt.Rows.Count;
                return true;
            }
            catch (Exception ex)
            {
                outMessage = ex.Message;
                return false;
            }


        }
        private void GetTable()
        {
            int tempindex = _sql.ToUpper().IndexOf(" FROM ");
            int TableNameStartIndex = tempindex + " FROM ".Length;
            int TableNameEndIndex = _sql.IndexOf(" ", TableNameStartIndex);

            if (TableNameEndIndex == -1)
            {
                TableNameEndIndex = _sql.Length;
            }
            _table = _sql.Substring(TableNameStartIndex, TableNameEndIndex - TableNameStartIndex);

        }
        private string GetSelectString()
        {

            return "*";
        }
        #endregion
    }
}

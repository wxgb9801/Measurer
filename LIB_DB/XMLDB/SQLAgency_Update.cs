using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using LIB_Common;
namespace LIB_DB
{
    public class SQLAgency_Update : ISqlExecAgency
    {
        public const eSqlType OPSqlType = eSqlType.UPDATE;
        private string _table;
        private DataSet _dS;
        private Dictionary<string, string> _setItems;
        private string _whereString;
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
        private int SetIndex;
        private int WhereIndex;

        public DataSet DS
        {
            get { return _dS; }
        }
        public Dictionary<string, string> SetItems
        {
            get { return _setItems; }
        }
        public string WhereString
        {
            get { return _whereString; }
        }

        public SQLAgency_Update()
        {

        }
        #region SqlAgency Members
        public string Table
        {
            get { return _table; }
            set { _table = value; }
        }
        public bool SQLExec(DataSet DataSource, string OPSQL, out string outMessage, out int chgRecordCount)
        {
            outMessage = "";
            _dS = DataSource;
            Sql = OPSQL;
            SplitSpaceStrs = OPSQL.Split(' ');
            eSqlType type = SQLAnalysiser.GetSqlType(OPSQL);
            if (type != OPSqlType)
            {
                throw new ApplicationException(string.Format("无效的[{0}]语句{{1}", OPSqlType.ToString(), OPSQL));
            }
            _table = GetTable();
            _setItems = GetSetItem();
            _whereString = SQLAnalysiser.GetWhereString(_sql);
            return execUpdate(out outMessage, out chgRecordCount);

        }
        #endregion
        private bool execUpdate(out string outMessage, out int chgRecordCount)
        {
            outMessage = "";
            chgRecordCount = 0;
            try
            {
                DataTable dt = _dS.Tables[_table];
                DataTable tmpdt = dt.Copy();
                DataView dv = new DataView(tmpdt, _whereString, "", DataViewRowState.CurrentRows);
                foreach (DataRowView drv in dv)
                {
                    foreach (KeyValuePair<string, string> keyvalue in _setItems)
                    {
                        drv[keyvalue.Key] = keyvalue.Value;
                    }
                }
                _dS.Tables.Remove(dt);
                _dS.Tables.Add(tmpdt);
                chgRecordCount = dv.Count;
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
        private Dictionary<string, string> GetSetItem()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            SetIndex = FunBase.GetCboIndex(true, "SET", SplitSpaceStrs);
            WhereIndex = FunBase.GetCboIndex(true, "Where", SplitSpaceStrs);

            if (WhereIndex < 0) WhereIndex = SplitSpaceStrs.Length;

            StringBuilder sb_set = new StringBuilder();

            for (int i = SetIndex + 1; i < WhereIndex; i++)
            {
                sb_set.Append(SplitSpaceStrs[i]);
            }
            string[] tmpSplit_Set = sb_set.ToString().Split(',');

            for (int i = 0; i < tmpSplit_Set.Length; i++)
            {
                string[] tempsplit_equal = tmpSplit_Set[i].Split('=');
                if (tempsplit_equal.Length > 1)
                {
                    string key = tempsplit_equal[0].ToUpper();
                    string value = SQLAnalysiser.ClearSqlValueChar(tempsplit_equal[1]);
                    if (result.Keys.Contains(key))
                    {
                        throw new ApplicationException("重复列名。");
                    }
                    else
                    {
                        result.Add(key, value);
                    }
                }
                else
                {
                    throw new ApplicationException("表达式不正确。");
                }
            }
            return result;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Data;

namespace LIB_DB
{
    public class SQLAgency_Insert : ISqlExecAgency
    {
        public const eSqlType OPSqlType = eSqlType.INSERT;
        private string _table;
        private DataSet _dS;
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

        private string[] SplitSpaceStrs;
        private int ValuesIndex_fullSql;
        public List<string> FieldItems;
        public List<string> ValueItems;

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
            _sql = OPSQL;
            _dS = DataSource;
            SplitSpaceStrs = _sql.Split(' ');
            eSqlType type = SQLAnalysiser.GetSqlType(_sql);

            _table = GetTable();

            ValuesIndex_fullSql = _sql.IndexOf("Values(");
            FieldItems = GetInsertFields();
            ValueItems = GetInsertValues();
            _result = new List<object>();
            return execInsert(out outMessage, out chgRecordCount);

        }
        #endregion
        private bool execInsert(out string outMessage, out int chgRecordCount)
        {
            outMessage = "";
            chgRecordCount = 0;
            try
            {
                DataTable dt = _dS.Tables[_table];
                DataRow dr = dt.NewRow();
                if (FieldItems.Count == ValueItems.Count)
                {
                    for (int i = 0; i < FieldItems.Count; i++)
                    {
                        dr[FieldItems[i]] = SQLAnalysiser.ClearSqlValueChar(ValueItems[i]);
                    }
                    dt.Rows.Add(dr);
                }
                else
                {
                    throw new ApplicationException("插入项数量不匹配。");
                }
                return true;
            }
            catch (Exception ex)
            {
                outMessage = ex.Message;
                return false;
            }
        }
        private List<string> GetInsertFields()
        {
            List<string> result;
            //int InsertIntoIndex_SplitArray = FunBase.GetCboIndex(true, "Insert Into", SplitSpaceStrs);
            //int ValuesIndex_SplitArray = FunBase.GetCboIndex(true, "Values(", SplitSpaceStrs);
            //int tableIndex_FullSql = Sql.IndexOf(_table);


            string TmpString = _sql.Substring(0, ValuesIndex_fullSql);
            string FieldsString = FindMatchingChar_FristLast(TmpString, "(", ")");
            string[] fieldArray = FieldsString.Split(',');
            result = new List<string>(fieldArray);
            return result;
        }
        private List<string> GetInsertValues()
        {
            List<string> result;

            string TmpString = _sql.Substring(ValuesIndex_fullSql, _sql.Length - ValuesIndex_fullSql);
            string FieldsString = FindMatchingChar_FristLast(TmpString, "(", ")");
            string[] fieldArray = FieldsString.Split(',');
            result = new List<string>(fieldArray);
            return result;
        }
        private string GetTable()
        {
            string table = SplitSpaceStrs[2];

            int index = table.IndexOf("(");
            table = table.Substring(0, index);
            if (_dS.Tables.Contains(table))
            {
                return table;
            }
            else
            {
                throw new ApplicationException(string.Format("数据源[{0}]不存在。", table));
            }
        }
        private string FindMatchingChar_FristLast(string sc, string lStr, string rStr)
        {
            //List<string> tmpList = new List<string>();
            //int stack = 0;
            //string tempstring = sc;
            //int lfristIndex = sc.IndexOf(lStr);
            //int rfristIndex = sc.IndexOf(rStr);
            //int rlastIndex = sc.LastIndexOf(rStr);

            //if (lfristIndex <= 0) lfristIndex = 0;
            //if (rlastIndex <= 0) rlastIndex = sc.Length;

            //tempstring = sc.Substring(lIndex + lStr.Length, rIndex);

            //return tempstring;

            int lfristIndex = sc.IndexOf(lStr);
            int rfristIndex = sc.IndexOf(rStr);
            string tempString = sc.Substring(lfristIndex + 1, rfristIndex - lfristIndex - 1);
            return tempString;

        }
    }
}

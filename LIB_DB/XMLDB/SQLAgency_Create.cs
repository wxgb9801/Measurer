﻿using System;
using System.Collections.Generic;
using System.Data;
namespace LIB_DB
{
    public class SQLAgency_Create : ISqlExecAgency
    {
        public const eSqlType OPSqlType = eSqlType.CREATE;
        private string _table = "";
        private string[] SplitSpaceStrs;
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

        
        #region ISqlExecAgency Members

        public string Table
        {
            get { return _table; }
        }
        public DataSet DS
        {
            get { return _dS; }
        }
        public bool SQLExec(DataSet DataSource, string OPSQL, out string outMessage, out int chgRecordCount)
        {
            outMessage = "";
            _sql = OPSQL;
            chgRecordCount = 0;
            _result = new List<object>();
            throw new ApplicationException("还未支持。");
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
                return "";
            }
        }
        #endregion
    }
}

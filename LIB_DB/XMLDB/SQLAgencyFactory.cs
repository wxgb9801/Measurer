using System;
using System.Collections.Generic;
using System.Data;

namespace LIB_DB
{
    public enum eSqlType
    {
        SELECT,
        UPDATE,
        INSERT,
        DELETE,
        DROP,
        CREATE,
        OTHER
    }
    public class SQLAnalysiser
    {
        public eSqlType type;

        public static eSqlType GetSqlType(string sql)
        {
            eSqlType tmptype = eSqlType.OTHER;
            string[] tempStrs = sql.Trim().Split(' ');
            string keyword = tempStrs[0];
            Enum.TryParse<eSqlType>(keyword.ToUpper(), out tmptype);
            return tmptype;
        }
        public static string GetWhereString(string sql)
        {
            int index = sql.ToUpper().IndexOf(" WHERE ");
            if (index < 0) return "";
            string result = sql.Substring(" WHERE ".Length + index);
            return result;
        }
        public static string ClearSqlValueChar(string ValueChar)
        {
            string tmpstring = ValueChar.Trim();

            tmpstring.TrimStart(new char[] { '\'' });
            tmpstring.TrimEnd(new char[] { '\'' });

            if (tmpstring.Substring(0, 1) == "'")
            {
                tmpstring.TrimEnd(new char[] { '\'' });
                if (tmpstring.Substring(tmpstring.Length - 1, 1) == "'")
                {
                    tmpstring = tmpstring.Substring(1, tmpstring.Length - 2);
                }
                return tmpstring;
            }
            else
            {
                return ValueChar;
            }
        }
    }

    public interface ISqlExecAgency
    {
        string Table{ get; }
        string Sql { get; }
        DataSet DS { get; }
        List<Object> Result {get;}
        bool SQLExec(DataSet DataSource, string OPSQL, out string outMessage, out int chgRecordCount);
    }

    public class SQLAgencyFactory
    {
        public static ISqlExecAgency CreateSQLAgency(eSqlType type, string sql)
        {
            ISqlExecAgency sa = null;
            switch (type)
            {
                case eSqlType.INSERT:
                    sa = new SQLAgency_Insert();
                    break;
                case eSqlType.UPDATE:
                    sa = new SQLAgency_Update();
                    break;
                case eSqlType.DELETE:
                    sa = new SQLAgency_Delete();
                    break;
                case eSqlType.DROP:
                    sa = new SQLAgency_Drop();
                    break;
                case eSqlType.CREATE:
                    sa = new SQLAgency_Create();
                    break;
                case eSqlType.SELECT:
                    sa = new SQLAgency_Select();
                    break;
            }
            return sa;
        }
    }
}

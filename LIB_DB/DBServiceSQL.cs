using System.Configuration;

namespace LIB_DB
{
    class DBServiceSQL:DBService
    {
        public DBServiceSQL(ConnectionStringSettings dbSetting):base(dbSetting)
        {    
        }

        protected override string TranParameterName_Add(string parameterName)
        {
            string topstr = parameterName.Substring(0, 1);
            string strvalue = parameterName;
            if ((topstr == "@") || (topstr == ":"))
            {
                strvalue = parameterName.Substring(1, parameterName.Length - 2);
            }
            topstr = "@";
            return topstr + strvalue;
        }
    }
}

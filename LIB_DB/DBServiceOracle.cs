using System.Configuration;

namespace LIB_DB
{
    class DBServiceOracle:DBService
    {
        public DBServiceOracle(ConnectionStringSettings dbSetting):base(dbSetting)
        {
            
        }

        #region override properties
        public override string CPTop
        {
            get
            {
              return ":";            
            }
        }

        public override eDBType DBType
        {
            get
            {
               return eDBType.Oracle;                   
            }
        }

        public override string ConnectStr
        {
            get
            {               
              return "||";              
            }
        }

        #endregion

        #region implementation of abstract method
        protected override string TranParameterName_Add(string parameterName)
        {
            string topstr = parameterName.Substring(0, 1);
            string strvalue = parameterName;
            if ((topstr == "@") || (topstr == ":"))
            {
                strvalue = parameterName.Substring(1, parameterName.Length - 2);
            }
            topstr = "";
            return topstr + strvalue;
        }
        
        #endregion
    }
}

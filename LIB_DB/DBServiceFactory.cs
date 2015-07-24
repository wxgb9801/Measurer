///*************************************************************************//
/// System/Module Name:DBOperation 
/// FileName:          this.FileName
/// Author:	           ZYR
///
/// Change History: 
/// Version	    Date		        By	        Details
/// 0.0.1       2012-08-23          ZYR         Create
///***************************************************************************//
/// Function: 
/// DBService Factory
///***************************************************************************//
///

using System;
using System.Configuration;
using LIB_Common;

namespace LIB_DB
{
    public enum eDBType
    {
        Oracle,
        SqlServer,
        XML,
        TXT
    }

    public static class DBServiceFactory
    {
        static Boolean _isExceptionRegistered = false;
        
        static DBServiceFactory()
        {
            InitWAPException();
        }
        
        static void InitWAPException()
        {
            if (!_isExceptionRegistered)
            {
               DBService.InitWAPException();
               DataOperation.InitWAPException();
                _isExceptionRegistered = true;
            }

        }

        public static DBService GetDBService(ConnectionStringSettings dbSetting)
        {
            if (dbSetting != null)
            {
                switch (dbSetting.ProviderName.ToUpper())
                {
                    case "SYSTEM.DATA.SQLCLIENT":
                        return new DBServiceSQL(dbSetting);
                    case "SYSTEM.DATA.ORACLECLIENT":
                        return new DBServiceOracle(dbSetting);
                    case "WXG.DB.XML":
                        return new DBServiceXML(dbSetting);
                    default:
                        return new DBServiceOracle(dbSetting);
                }
            }
            else
                return null;
        }

        public static DBService GetDBService(String dbString)
        {
            ConnectionStringSettings dbSetting = ConfigurationManager.ConnectionStrings[dbString];
            return GetDBService(dbSetting);
        }

        public static DBService GetDBService()
        {
            String strConnection = ConfigurationManager.AppSettings["DBConnectionName"];
            ConnectionStringSettings dbSetting = ConfigurationManager.ConnectionStrings[strConnection]; 
            return GetDBService(dbSetting);
        }

        public static DataOperation GetDataOperation(string tablename, DBService db, bool IsLoadDt, FieldCollection fields)
        { 
            if(db == null)
                db = DBServiceFactory.GetDBService();
            switch (db.DBType)
            {
                case eDBType.SqlServer:
                    return null;
                case eDBType.Oracle:
                    return new DataOperationOracle(tablename,db,IsLoadDt,fields);
                case eDBType.XML:
                    return new DataOperation_XML(tablename, db, IsLoadDt, fields);
                default:
                    return new DataOperationOracle(tablename, db, IsLoadDt, fields);
            }
        }
    }















}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LIB_DB;
using LIB_Common;
namespace Measurer
{
    public class Install
    {
        public static bool ReCreateDB()
        {
            try
            {
                DBServiceXML xmldb = (DBServiceXML)DBServiceFactory.GetDBService("MeasurerXML");
                xmldb.DB_Open();
                xmldb.AddTable(Task.GetTaskStruct());
                xmldb.AddTable(Task.GetRecordStruct());
                xmldb.AddTable(Task.GetItemTask());
                xmldb.AddTable(UserManger.GetUserDataTable());
                xmldb.DB_Close();
                return true;
            }
            catch (Exception ex)
            {
                Program.WriteErrorLog("Install.ReCreateTable", ex);
                return false;
            }
        }

        public static bool ReCreateTastStruct()
        {
            try
            {
                DBServiceXML xmldb = (DBServiceXML)DBServiceFactory.GetDBService("MeasurerXML");
                xmldb.DB_Open();
                xmldb.AddTable(Task.GetTaskStruct());
                xmldb.DB_Close();
                return true;
            }
            catch (Exception ex)
            {
                Program.WriteErrorLog("Install.ReCreateTable", ex);
                return false;
            }
        }
        public static bool ReCreateRecordStruct()
        {
            try
            {
                DBServiceXML xmldb = (DBServiceXML)DBServiceFactory.GetDBService("MeasurerXML");
                xmldb.DB_Open();
                xmldb.AddTable(Task.GetRecordStruct());
                xmldb.DB_Close();
                return true;
            }
            catch (Exception ex)
            {
                Program.WriteErrorLog("Install.ReCreateTable", ex);
                return false;
            }
        }
        public static bool ReCreateItemTask()
        {
            try
            {
                DBServiceXML xmldb = (DBServiceXML)DBServiceFactory.GetDBService("MeasurerXML");
                xmldb.DB_Open();
                xmldb.AddTable(Task.GetItemTask());
                xmldb.DB_Close();
                return true;
            }
            catch (Exception ex)
            {
                Program.WriteErrorLog("Install.ReCreateTable", ex);
                return false;
            }
        }
        public static bool ReCreateUserDataTable()
        {
            try
            {
                DBServiceXML xmldb = (DBServiceXML)DBServiceFactory.GetDBService("MeasurerXML");
                xmldb.DB_Open();
                xmldb.AddTable(UserManger.GetUserDataTable());
                xmldb.DB_Close();
                return true;
            }
            catch (Exception ex)
            {
                Program.WriteErrorLog("Install.ReCreateTable", ex);
                return false;
            }
        }

        public static bool InitDefaultUser()
        {
            try
            {
                UserManger um = new UserManger();
                um.CreateUser("admin", "password", "管理员", "admin");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

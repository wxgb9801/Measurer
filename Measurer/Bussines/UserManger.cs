using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using LIB_DB;
using LIB_Common;
using System.Security.Cryptography;

namespace Measurer
{

    public class UserManger
    {
        public DBService theDB;

        public UserManger()
        {
            theDB = DBServiceFactory.GetDBService("MeasurerXML");
        }
        public int CreateUser(string userID,string PWD,string Name,string Group)
        {
            try
            {
                string md5PWD = UserManger.MD5Encryption(PWD);
                string sql = string.Format("Insert into T_User(UserID,PWD,Name,Group) Values({0},{1},{2},{3})",
                                              FunBase.ChkField(userID,true), FunBase.ChkField(md5PWD,true), FunBase.ChkField(Name,true), FunBase.ChkField(Group,true));
               if(theDB.UpdataDataBase(sql))
               {
                   return 0;
               }
               else
               {
                   return -1;
               }
            }
            catch (Exception ex)
            {
                Program.WriteErrorLog(string.Format(this.GetType().FullName),ex);
                return -2;
            }
        }
        public int Modify(string userID, string Name, string Group)
        {
            try
            {
                string sql = string.Format("Update  T_User Set Name={1},Group={2} Where UserID={0}",
                                           FunBase.ChkField(userID,true), FunBase.ChkField(Name,true), FunBase.ChkField(Group,true));
                if (theDB.UpdataDataBase(sql))
                {
                    return 0;
                }
                else
                {
                    return -3;
                }
            }
            catch (Exception ex)
            {
                Program.WriteErrorLog(string.Format(this.GetType().FullName),ex);
                return -4;
            }
        }
        public int ChangePWD(string userID, string oldPWD, string newPWD)
        {
            try
            {
                string md5OldPWD = UserManger.MD5Encryption(oldPWD);
                string md5NewPWD = UserManger.MD5Encryption(newPWD);
                string sql = string.Format("Update  T_User Set PWD={0} Where UserID={1} and PWD={2}", 
                                            FunBase.ChkField(userID,true),
                                            FunBase.ChkField(md5NewPWD,true),
                                            FunBase.ChkField(md5OldPWD,true));
                if (theDB.UpdataDataBase(sql))
                {
                    return 0;
                }
                else
                {
                    return -3;
                }
            }
            catch (Exception ex)
            {
                Program.WriteErrorLog(string.Format(this.GetType().FullName), ex);
                return -4;
            }
        }
        public int Login(string UserID,string PWD,out string Name,out string Group)
        {
            Name = "";
            Group = "";
            try
            {
                string md5PWD = UserManger.MD5Encryption(PWD);
                string sql = string.Format("Select * From T_User Where UserID={0} and PWD={1}",
                                            FunBase.ChkField(UserID,true), FunBase.ChkField(md5PWD,true));
                DataTable dt =theDB.SelectDataTable(sql);
                if (dt != null && dt.Rows.Count==1)
                {
                    Name = dt.Rows[0]["Name"].ToString();
                    Group = dt.Rows[0]["Group"].ToString();
                    return 0;
                }
                else
                {
                    return -3;
                }
            }
            catch (Exception ex)
            {
                Program.WriteErrorLog(string.Format(this.GetType().FullName), ex);
                return -4;
            }
        }

        public static string MD5Encryption(string password)
        {
            string EncodeString = password;
            string encryptionString = "";
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.Unicode.GetBytes(EncodeString));
            for (int i = 0; i < s.Length; i++)
            {
                encryptionString = encryptionString + s[i].ToString("x");
            }
            return encryptionString;
        }
        public static DataTable GetUserDataTable()
        {
            DataTable dt = new DataTable("T_User");
            dt.Columns.Add("UserID");
            dt.Columns.Add("PWD");
            dt.Columns.Add("Name");
            dt.Columns.Add("Group");
            return dt;
        }
    }
}

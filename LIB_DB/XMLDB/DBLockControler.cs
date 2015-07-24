using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
namespace LIB_DB
{
    public class LockUserInfo
    {
        public const int DefaultHBTimeOut = 30;
        public string SessionID;
        public int HBTimeOut;
        public DateTime MarkDT;
        public bool IsLostConn
        {
            get
            {
                TimeSpan sp = DateTime.Now - MarkDT;
                return sp.TotalSeconds > HBTimeOut;
            }
        }
    }
    public class DBLockInfo
    {
        private byte currAccUserNum;
        public byte CurrAccUserNum
        {
            get { return currAccUserNum; }
            set { currAccUserNum = value; }
        }
        private byte maxAccUserNum;
        public byte MaxAccUserNum
        {
            get { return maxAccUserNum; }
            set { maxAccUserNum = value; }
        }

        public DateTime LastOPDT;
        public LockUserInfo CurrentUserAccessInfo;
    }
    public class DBLockControler
    {
        public const char AccNumSplitChar = '/';
        public const char UserInfoSplitChar = '#';
        public const byte DefaultMaxUserNum = 1;
        public const int MaxReadDataBytes = 1024;
        public const string DefaultLockFileName = "DB";
        public string DBLockFilePath = "";
        public string DBBaseFolder = "";
        public DBLockInfo LockInfo;
        private FileStream objFileStream;
        private object lockobject = new object();
        private bool IsLife = true;

        protected DBLockControler()
        {
            InitLockInfo();
        }
        public DBLockControler(string DBBaseFolderPath):this()
        {
            DBBaseFolder = DBBaseFolderPath;
            DBLockFilePath = String.Format(@"{0}\{1}.{2}", DBBaseFolder,DefaultLockFileName,eXmlFileType.DLK.ToString());
            if (File.Exists(DBLockFilePath))
            {
                Load();
            }
            else
            {
                objFileStream = new FileStream(DBLockFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            }
        }
        private void InitLockInfo()
        {
            LockInfo = new DBLockInfo();
            LockInfo.CurrAccUserNum = 0;
            LockInfo.MaxAccUserNum = DefaultMaxUserNum;
        }
        private Thread LifeThread = null;

        public void Load()
        {
            if (objFileStream == null)
            {
                objFileStream = new FileStream(DBLockFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            }
            byte[] tempbuffer = new byte[MaxReadDataBytes];
            int bytes = objFileStream.Read(tempbuffer, 0, tempbuffer.Length);
            byte[] buffer = new byte[bytes];
            Buffer.BlockCopy(tempbuffer, 0, buffer, 0, bytes);
            string rdString = Encoding.ASCII.GetString(buffer);
            objFileStream.Flush();
           
            StringReader sr = new StringReader(rdString);
            string line1 = sr.ReadLine();
            string line2 = sr.ReadLine();
            if (line1 == null || line1 == String.Empty)
            {
                throw new ApplicationException("文件系统损坏，请联系管理员修复。");
            }
            string[] tmps = line1.Split(AccNumSplitChar);
            if (tmps.Length > 1)
            {
                LockInfo.CurrAccUserNum = byte.Parse(tmps[0]);
                LockInfo.MaxAccUserNum = byte.Parse(tmps[1]);
            }
            LockInfo.LastOPDT = DateTime.Parse(line2);
            sr.Close();
        }
        public void Booking(int HBTimeOut)
        {
            lock(lockobject)
            {
                LockInfo.CurrAccUserNum++;
                //if (LockInfo.CurrentUserAccessInfo == null || LockInfo.CurrentUserAccessInfo.SessionID == "")
                //{
                //    LockInfo.CurrentUserAccessInfo = new LockUserInfo();
                //    LockInfo.CurrentUserAccessInfo.SessionID = GetOnlyAccessID();
                //}
                LockInfo.CurrentUserAccessInfo = new LockUserInfo();
                LockInfo.CurrentUserAccessInfo.SessionID = GetOnlyAccessID();
                LockInfo.CurrentUserAccessInfo.MarkDT = DateTime.Now;
                LockInfo.CurrentUserAccessInfo.HBTimeOut = HBTimeOut;
                SaveInfo();
                SaveUserAccessInfo();
                if (LifeThread == null)
                {
                    LifeThread = new Thread(new ThreadStart(LifeHB));
                    LifeThread.Start();
                }
            }
        }
        public void UnBooking()
        {
            lock (lockobject)
            {
                if (LockInfo.CurrentUserAccessInfo != null)
                    RecycleOneSession(LockInfo.CurrentUserAccessInfo.SessionID);
                LockInfo.CurrAccUserNum--;
                SaveInfo();
            }
        }
        private void SaveUserAccessInfo()
        {
            string fullFileName = XMLDBFileHelper.GetFullFileName(DBBaseFolder, eXmlFileType.DAS, LockInfo.CurrentUserAccessInfo.SessionID);
            LockInfo.CurrentUserAccessInfo.MarkDT = DateTime.Now;

            StringWriter sw = new StringWriter();
            sw.WriteLine("{0}", LockInfo.CurrentUserAccessInfo.HBTimeOut);
            sw.WriteLine("{0}", LockInfo.CurrentUserAccessInfo.MarkDT.ToString("yyyy-MM-dd HH:mm:ss"));
            using (FileStream TmpFileStream = new FileStream(fullFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                byte[] bytes = Encoding.ASCII.GetBytes(sw.ToString());
                TmpFileStream.SetLength(0);
                TmpFileStream.Write(bytes, 0, bytes.Length);
                TmpFileStream.Flush();
                TmpFileStream.Close();
            }
            sw.Close();
        }
        public void SaveInfo()
        {
            if (objFileStream == null)
            {
                objFileStream = new FileStream(DBLockFilePath, FileMode.Truncate, FileAccess.Write, FileShare.None);
            }
            LockInfo.LastOPDT = DateTime.Now;
            StringWriter sw = new StringWriter();
            sw.WriteLine("{0}/{1}", LockInfo.CurrAccUserNum, LockInfo.MaxAccUserNum);
            sw.WriteLine("{0}", LockInfo.LastOPDT.ToString("yyyy-MM-dd HH:mm:ss"));
            sw.Close();

            byte[] bytes = Encoding.ASCII.GetBytes(sw.ToString());
            objFileStream.SetLength(0);
            objFileStream.Write(bytes, 0, bytes.Length);
            objFileStream.Flush();
            objFileStream.Close();
            objFileStream = null;
        }
        public bool AllowAccess
        {
            get
            {
                bool result=false;
                try
                {
                    result = LockInfo.CurrAccUserNum < LockInfo.MaxAccUserNum;
                    if (result)
                    {
                        return result;
                    }
                    else
                    {
                        return ScanDBAccessFileAndRecycleSession();
                    }
                }
                catch(Exception)
                {
                    return false;
                }
 
            }
        }
        private bool ScanDBAccessFileAndRecycleSession()
        {
            bool result = false;
            List<LockUserInfo> LostConnAccList = LoadUserAccessFile();

            foreach (LockUserInfo lui in LostConnAccList)
            {
                if (RecycleOneSession(lui.SessionID))
                {
                    LockInfo.CurrAccUserNum--;
                    SaveInfo();
                    result = true;
                    break;
                }
            }
            return result;
        }
        private bool RecycleOneSession(string SessionID)
        {
            //Get FileName
            //Delete File
             //UnBooking
            try
            {
                string fullFileName = XMLDBFileHelper.GetFullFileName(DBBaseFolder, eXmlFileType.DAS, SessionID);
                File.Delete(fullFileName);
                return true;
            }
            catch (Exception)
            {
                //Write Log
                return false;
            }

        }
        private List<LockUserInfo> LoadUserAccessFile()
        {
            List<LockUserInfo> AccList = new List<LockUserInfo>();
            string[] strFiles = Directory.GetFiles(DBBaseFolder);
            eXmlFileType efileType;

            foreach (string file in strFiles)
            {
                string[] sptString = new string[1] { @"\" };
                string[] sfileSplt = file.Split(sptString, StringSplitOptions.None);
                string sfile = sfileSplt[sfileSplt.Length - 1];
                efileType = XMLDBFileHelper.GetFileType(sfile);
                LockUserInfo LUI = new LockUserInfo();
                LUI.SessionID = sfile.Split('.')[0];
                
                if (efileType == eXmlFileType.DAS)
                {
                    string fullFileName = XMLDBFileHelper.GetFullFileName(DBBaseFolder, eXmlFileType.DAS, LUI.SessionID);
                    byte[] tempbuffer = new byte[MaxReadDataBytes];
                    int bytes;
                    byte[] buffer;
                    string rdString;
                    using (FileStream TmpFileStream = new FileStream(fullFileName, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        bytes = TmpFileStream.Read(tempbuffer, 0, tempbuffer.Length);
                        buffer = new byte[bytes];
                        Buffer.BlockCopy(tempbuffer, 0, buffer, 0, bytes);
                        rdString = Encoding.ASCII.GetString(buffer);
                        TmpFileStream.Close();
                    }
                    StringReader sr = new StringReader(rdString);
                    string line1 = sr.ReadLine();
                    string line2 = sr.ReadLine();
                    LUI.HBTimeOut = int.Parse(line1);
                    LUI.MarkDT = DateTime.Parse(line2);
                    if (LUI.IsLostConn)
                        AccList.Add(LUI);
                }
            }
            return AccList;
        }
        private void LifeHB()
        {
            int sleepTime = this.LockInfo.CurrentUserAccessInfo.HBTimeOut * 3000 / 10;
            while (IsLife)
            {
                SaveUserAccessInfo();
                Thread.Sleep(sleepTime);
            }
        }
        public static string GetOnlyAccessID()
        {
            //年月日时分秒+3位随机数
            //YYMMDDHHMiSSRRR
            Random rd = new Random();
            int no = rd.Next(0, 999);
            string id = string.Format("{0}{1}", DateTime.Now.ToString("yyMMddHHmmss"), no);
            return id;
        }
    }
}

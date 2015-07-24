using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
namespace LIB_Common
{
    public class FunBase
    {
        public static string STR_DATETIME_TO_DB_ORC = "yyyy-MM-dd HH:mm:ss:ffffff";
        public static string STR_SAVE_DB_ORC = "YYYY-MM-DD HH24:MI:SS:ff";
        public static SerializedFormat serilaizef = SerializedFormat.Soap;

        #region 判断插入数据的字符串是否安全如果带有"'"就变为"''"
        /// <summary>
        /// 检测field字符串是否安全如果存在"'"将其转换为"''",默认字符型结尾不加","
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static string ChkField(string field)
        {
            return ChkField(field, false, false,true);
        }
        /// <summary>
        /// 检测field字符串是否安全
        /// </summary>
        /// <param name="field">字串</param>
        /// <param name="ischar">是否是字符型</param>
        /// <returns></returns>
        public static string ChkField(string field, bool ischar)
        {
            return ChkField(field, ischar, false,true);
        }
        /// <summary>
        /// 检测field字符串是否安全
        /// </summary>
        /// <param name="field">字串</param>
        /// <param name="ischar">是否是字符型</param>
        /// <param name="addComma">结尾是否加逗号</param>
        /// <returns></returns>
        public static string ChkField(string field, bool ischar, bool addComma)
        {
            return ChkField(field, ischar, addComma,true);
        }
        public static string ChkField(string field, bool ischar, bool addComma,bool IsTrim)
        {
            if (field == null) field = "";
            if (IsTrim)
                field.Trim();
            var result = new StringBuilder();
            if (ischar) result.Append("'");

            result.Append(field.Replace("'", "''"));

            if (ischar) result.Append("'");

            if (addComma)
                result.Append(",");
            return result.ToString();
        }
        #endregion

        #region 判断字符串是否为数字
        public static bool IsNumeric(string itemValue)
        {
            try
            {
                if (itemValue == null || itemValue.Length == 0)
                {
                    return false;
                }
                //用正则表达式判断是输入的是否为数字(去除可能发生的输入法输入中文问题)
                var numRegex = new Regex(@"^(-?\d*[.]*\d*)$");
                //Regex numRegex = new Regex(@"^(-?[0-9]*[.]*[0-9]*)$");
                var Result = numRegex.Match(itemValue);

                if (Result.Success)
                { return true; }
                else
                { return false; }
            }
            catch
            {
                return false;
            }
        }
        public static bool IsNaturalNum(string pValue)
        {
            try
            {
                if (pValue == null || pValue.Length == 0)
                {
                    return false;
                }
                var numRegex = new Regex(@"^([1-9][0-9]*)$");
                var Result = numRegex.Match(pValue);
                if (Result.Success)
                { return true; }
                else
                { return false; }
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 判断字符串实际长度
        public static int ChkStrLen(string str)
        {
            return ChkStrLen(str, Encoding.Default);
        }
        public static int ChkStrLen(string str, Encoding ecode)
        {
            return Encoding.GetEncoding(ecode.BodyName).GetByteCount(str);
        }
        public static bool IsAsciiStr(string str)
        {
            return Encoding.Default.GetByteCount(str) == str.Length;
        }
        #endregion

        #region 修正字符串
        /// <summary>
        /// 修正字符串不过超过长度则在末尾一位用'*'符表示,不足的用空格填充
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string RepairStr(string str, int len)
        {
            if (str.Length > len)
            {
                return str.Substring(0, len - 1) + "*";
            }
            else
            {
                return str.PadRight(len);
            }
        }
        #endregion

        #region 检测字符串在下载数组
        //检测字符串在下载数组中是否存在及其位置
        public static int GetCboIndex(string str, string[] tmpstrs, int istart, int len)
        {
            return GetCboIndex(false, str, tmpstrs, istart, len);
        }
        public static int GetCboIndex(bool ignoreCase, string str, string[] tmpstrs, int istart, int len)
        {
            for (var i = 0; i < tmpstrs.Length; i++)
            {
                if (len > tmpstrs[i].Length) continue;
                if (ignoreCase)
                {
                    if (str.ToUpper() == tmpstrs[i].Substring(istart, len).ToUpper()) return i;
                }
                else
                {
                    if (str == tmpstrs[i].Substring(istart, len)) return i;
                }
            }
            return -1;
        }
        //检测字符串在下载数组中是否存在及其位置
        public static int GetCboIndex(string str, string[] tmpstrs)
        {
            return GetCboIndex(str, tmpstrs, 0, str.Length);
        }
        public static int GetCboIndex(bool ignoreCase, string str, string[] tmpstrs)
        {
            return GetCboIndex(ignoreCase,str, tmpstrs, 0, str.Length);
        }
        //检测字符串在DataTable中是否存在及其位置
        public static int GetTableRow(string str, DataTable dt, int icol)
        {
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                if (str.Trim() == dt.Rows[i][icol].ToString().Trim()) return i;
            }
            return -1;
        }
        public static int GetTableRow(string str, DataTable dt, string colname)
        {
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                if (str.Trim() == dt.Rows[i][colname].ToString().Trim()) return i;
            }
            return -1;
        }
        //检测字符串在数组中是否存在及其位置
        public static int GetArrayIndex(string str, ArrayList tmpArray)
        {
            for (var i = 0; i < tmpArray.Count; i++)
            {
                if (str == tmpArray[i].ToString()) return i;
            }
            return -1;
        }
        #endregion

        #region 把数组中的各项变成逗号单引号分割的语句
        public static string BuildSql_In(ArrayList tmpArray)
        {
            var sbSql = new StringBuilder();
            if (tmpArray != null)
            {
                for (var i = 0; i < tmpArray.Count; i++)
                {
                    if (i > 0) sbSql.Append(",");
                    sbSql.Append("'");
                    sbSql.Append(tmpArray[i].ToString());
                    sbSql.Append("'");
                }
            }
            return sbSql.ToString();
        }
        public static string BuildSql_In(Array tmpArray, bool isGetEnumValue, bool isChar)
        {

            var sbSql = new StringBuilder();
            if (tmpArray != null)
            {
                for (var i = 0; i < tmpArray.Length; i++)
                {
                    if (isGetEnumValue)
                    {
                        sbSql.Append(ChkField(((int)tmpArray.GetValue(i)).ToString(), isChar, i < (tmpArray.Length - 1)));
                    }
                    else
                    {
                        sbSql.Append(ChkField(tmpArray.GetValue(i).ToString(), isChar, i < (tmpArray.Length - 1)));
                    }
                }
            }
            return sbSql.ToString();
        }
        public static string BuildSql_In(string[] tmpArray)
        {
            var sbSql = new StringBuilder();
            if (tmpArray != null)
            {
                for (var i = 0; i < tmpArray.Length; i++)
                {
                    if (i > 0) sbSql.Append(",");
                    sbSql.Append("'");
                    sbSql.Append(tmpArray[i].ToString());
                    sbSql.Append("'");
                }
            }
            return sbSql.ToString();
        }
        public static string BuildSql_In(DataTable dt, int icol)
        {
            var sbSql = new StringBuilder();
            if (dt != null)
            {
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    if (i > 0) sbSql.Append(",");
                    sbSql.Append("'");
                    sbSql.Append(dt.Rows[i][icol].ToString());
                    sbSql.Append("'");
                }
            }
            return sbSql.ToString();
        }
        public static string BuildSql_In(DataTable dt, string colname)
        {
            var sbSql = new StringBuilder();
            if (dt != null)
            {
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    if (i > 0) sbSql.Append(",");
                    sbSql.Append("'");
                    sbSql.Append(dt.Rows[i][colname].ToString());
                    sbSql.Append("'");
                }
            }
            return sbSql.ToString();
        }
        public static string BuildSql_In(List<Object> ls, bool isGetEnumValue, bool isChar)
        {
            var sbSql = new StringBuilder();
            if (ls != null)
            {
                for (var i = 0; i < ls.Count; i++)
                {
                    if (isGetEnumValue)
                    {
                        sbSql.Append(ChkField(((int)ls[i]).ToString(), isChar, i < (ls.Count - 1)));
                    }
                    else
                    {
                        sbSql.Append(ChkField(ls[i].ToString(), isChar, i < (ls.Count - 1)));
                    }
                }
            }
            return sbSql.ToString();
        }
        public static string BuildSql_In(List<string> ls, bool isChar)
        {
            var sbSql = new StringBuilder();
            if (ls != null)
            {
                for (var i = 0; i < ls.Count; i++)
                {
                    sbSql.Append(ChkField(ls[i], isChar, i < (ls.Count - 1)));
                }
            }
            return sbSql.ToString();
        }
        #endregion

        /// <summary>
        /// 得到字符串内所包含的左起指定数量的某个子串的起始字符在字符串中的位置
        /// </summary>
        /// <param name="str">父字符串</param>
        /// <param name="value">子字符串</param>
        /// <param name="start">开始位置</param>
        /// <param name="FindValueNumber">要返回的子串在父串中是第几个同样的子串</param>
        /// <returns>位置</returns>

        public static int GetIndexOfBy(string str, string value, int start, int FindValueNumber)
        {
            var tmpLocation = 0;
            var i = 0;
            do
            {
                tmpLocation = str.ToUpper().IndexOf(value, tmpLocation);
                i++;
            }
            while (i < FindValueNumber && tmpLocation >= 0);
            return tmpLocation;
        }

        #region 限定进程唯一打开
        //**Quentin Added 20090102@XiKai***
        public static bool GetRunningInstance(bool AutoActive)
        {
            var currentProcess = Process.GetCurrentProcess(); //获取当前进程 
            //获取当前运行程序完全限定名 
            var currentFileName = currentProcess.MainModule.FileName;
            //获取进程名为ProcessName的Process数组。 
            var processes = Process.GetProcessesByName(currentProcess.ProcessName);
            //遍历有相同进程名称正在运行的进程 
            var r = false;
            foreach (var process in processes)
            {
                if (process.MainModule.FileName == currentFileName)
                {
                    if (process.Id != currentProcess.Id) //根据进程ID排除当前进程 
                    {
                        r = true;
                        if (AutoActive)
                            HandleRunningInstance(process);//返回已运行的进程实例 
                    }
                }
            }
            return r;
        }

        //**Quentin Added 20090102@XiKai***
        public static bool GetRunningInstance()
        {
            return GetRunningInstance(false);
        }

        //**Quentin Added 20090102@XiKai***
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        private const int SW_SHOWNORMAL = 1;

        public static bool HandleRunningInstance(Process instance)
        {
            //确保窗口没有被最小化或最大化 
            ShowWindowAsync(instance.MainWindowHandle, SW_SHOWNORMAL);
            //设置为foreground window 
            return SetForegroundWindow(instance.MainWindowHandle);
        }
        #endregion

        #region 格式化字符串为指令的比特长度
        /// <summary>
        /// 格式化字符串为指令的比特长度
        /// </summary>
        /// <param name="Str"></param>
        /// <param name="Bytelen"></param>
        /// <param name="PadWay"></param>
        /// <returns></returns>
        public static string GetFormatString(string Str, int Bytelen, int PadWay)
        {
            var info = Encoding.GetEncoding("GB18030").GetBytes(Str);
            if (info.Length > Bytelen)
            {
                if (Str.Length < info.Length)
                {
                    Str = Str.Substring(0, Bytelen / 2);
                }
                else
                {
                    Str = Encoding.GetEncoding("GB18030").GetString(info, 0, Bytelen);
                }
            }
            if (info.Length < Bytelen)
            {
                var padstring = "";
                if (PadWay == 0)
                {

                    Str = padstring.PadLeft(Bytelen - info.Length, ' ') + Str;
                }
                else
                {
                    Str = Str + padstring.PadLeft(Bytelen - info.Length, ' ');
                }
            }
            return Str;
        }
        #endregion

        //用文件数据流读取Xml
        public static XmlDocument ReadXmdDoc(string xmlfile)
        {
            //XmlDocument subsysXml = new XmlDocument();
            //subsysXml.LoadXml(xmlpath);
            var sr = new StreamReader(xmlfile, Encoding.GetEncoding("GB2312"));
            var xmldoc = new XmlDocument();
            xmldoc.Load(sr);
            sr.Close();
            return xmldoc;
        }


        #region Byte处理
        public static byte[] GetReverseBytes(byte[] srcBytes)
        {
            var tmp = new byte[srcBytes.Length];
            srcBytes.CopyTo(tmp, 0);
            Array.Reverse(tmp);
            return tmp;
        }
        public static void MemSet(byte[] buf, byte val)
        {
            MemSet(buf, val, 0, buf.Length);

        }
        public static void MemSet(byte[] buf, byte val, int b_index, int size)
        {
            for (var i = b_index; i < size; i++)
                buf[i] = val;
        }

        public static UInt16 GetLittleEndianFromBigEndian(UInt16 bigNumber)
        {
            var tmpbytes = BitConverter.GetBytes(bigNumber);
            Array.Reverse(tmpbytes);
            return BitConverter.ToUInt16(tmpbytes, 0);
        }
        public static UInt16 GetLittleEndianFromBigEndian(byte[] bigBuffer, int StartIndex)
        {
            var bigNumber = BitConverter.ToUInt16(bigBuffer, StartIndex);
            return GetLittleEndianFromBigEndian(bigNumber);
        }
        public static void SetBigEndianFormUInt16(byte[] bytes, UInt16 value, int startIndex)
        {
            var tmpbytes = BitConverter.GetBytes(value);
            Array.Reverse(tmpbytes);
            Buffer.BlockCopy(tmpbytes, 0, bytes, startIndex, 2);
        }
        //public static UInt16 BytesToInt16(byte[] bytes)
        //{
        //    UInt16 intValue=0;
        //    for (int i = 0; i < bytes.Length; i++)
        //    {
        //        intValue += (UInt16)((bytes[i] & 0xFF) << (4 * (3 - i)));
        //    }
        //    return intValue;
        //}
        //public static Byte[] UInt16To2Bytes(UInt16 iValue)
        //{
        //    byte[] bytes = new byte[2];
        //    for (int i = 0; i < 2; i++)
        //    {
        //        bytes[i] = (byte)(iValue >> 4 * (3 - i) & 0xFF);
        //    }
        //    return bytes;
        //}

        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
        public static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        } 
        #endregion


        #region 按位解析
        public static void BitExplain(string[] code_Array, Byte[] bytes, out string swapCodeString)
        {
            var sb = new StringBuilder();
            var ba = new BitArray(bytes);
            for (var i = 0; i < code_Array.Length; i++)
            {
                if (i > ba.Count)
                    break;
                if (ba[i])
                {
                    sb.Append(string.Format("{0}-{1}", i, code_Array[i]));
                }
            }
            swapCodeString = sb.ToString();
        }
        public static void BitExplain(string[] code_Array, UInt16 code_Int, out string swapCodeString)
        {
            var sb = new StringBuilder();
            var tmpiSwapcode = 0;
            for (var i = 0; i < code_Array.Length; i++)
            {
                tmpiSwapcode = code_Int >> i;
                if ((tmpiSwapcode & 1) == 1)
                {
                    sb.Append(string.Format("{0}-{1}", i, code_Array[i]));
                }
            }
            swapCodeString = sb.ToString();
        }
        public static void BitExplain(string[] code_Array, int code_Int, out string swapCodeString)
        {
            var sb = new StringBuilder();
            var tmpiSwapcode = 0;
            for (var i = 0; i < code_Array.Length; i++)
            {
                tmpiSwapcode = code_Int >> i;
                if ((tmpiSwapcode & 1) == 1)
                {
                    sb.Append(string.Format("{0}-{1}", i, code_Array[i]));
                }
            }
            swapCodeString = sb.ToString();
        }
        public static void BitExplain(string[] code_Array, int code_Int, out List<string> swapCodeList)
        {
            swapCodeList = new List<string>();
            var tmpiSwapcode = 0;
            for (var i = 0; i < code_Array.Length; i++)
            {
                tmpiSwapcode = code_Int >> i;
                if ((tmpiSwapcode & 1) == 1)
                {
                    swapCodeList.Add(string.Format("{0}-{1}.", i, code_Array[i]));
                }
            }
        }
        #endregion

        #region BaseFunction-ReadXmlAttributes
        public static void ReadXmlAttributes(XmlNode pNode, ref Dictionary<string, string> dic)
        {
            foreach (XmlAttribute xml_Att in pNode.Attributes)
            {
                dic[xml_Att.Name.ToUpper()] = xml_Att.Value;
            }
        }
        public static void ReadXmlAttributes(XmlNode pNode, ref DataRow dr)
        {
            foreach (XmlAttribute xml_Att in pNode.Attributes)
            {
                dr[xml_Att.Name] = xml_Att.Value;
            }
        }
        public static void ReadXmlAttributes(XmlNode pNode, ref Hashtable ht)
        {
            var value = "";
            foreach (XmlAttribute xml_Att in pNode.Attributes)
            {
                value = xml_Att.Value;
                ht.Add(xml_Att.Name, value);
            }
        }
        #endregion

        //add by FYP 2013-02-04
        #region 获取WAP系统根目录
        public static string GetSysBasePath()
        {
            var strpath = "";

            try
            {
                strpath = ConfigurationManager.AppSettings["BasePath"];
            }
            catch
            {

            }
            if ((strpath == null) || (strpath.Trim().Length < 3))
            {
                strpath = AppDomain.CurrentDomain.BaseDirectory;
            }
            return strpath;
        }

        public static bool FileExists(string strFileName, out string strActualPath)
        {
            strActualPath = "";
            try
            {
                if (strFileName.IndexOf(':') > -1)
                {
                    strActualPath = strFileName;
                }
                else
                {
                    strActualPath = GetSysBasePath() + strFileName;
                }
                return File.Exists(strActualPath);

            }
            catch
            {
                return false;
            }
        }

        public static bool CreateFullPath(string strPath, out string strActualPath)
        {
            strActualPath = "";
            try
            {
                if (strPath.IndexOf(':') > -1)
                {
                    strActualPath = strPath;
                }
                else
                {
                    strActualPath = GetSysBasePath() + strPath;
                }

                return Directory.Exists(strActualPath);

            }
            catch
            {
                return false;
            }
        }
        

        #endregion

        #region Serialize / DeSerialize
        public static FieldCollection DeserializeFC(string _strFieldCollection, out string errmsg)
        {
            errmsg = "";
            try
            {
                var _filedCollection = MessageSerializer<FieldCollection>.GetObjectByString(_strFieldCollection, serilaizef,ref  errmsg);
                return _filedCollection;
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return null;
            }

        }

        public static QFieldCollection DeserializeQFC(string _strFieldCollection, out string errmsg)
        {
            errmsg = "";
            try
            {
                var _filedCollection = MessageSerializer<QFieldCollection>.GetObjectByString(_strFieldCollection, serilaizef,ref  errmsg);
                return _filedCollection;
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return null;
            }

        }

        public static string SerializeFCToString(FieldCollection _fieldCollection, out string errmsg)
        {
            errmsg = "";
            try
            {
                var _strFieldCollection = MessageSerializer<FieldCollection>.SaveToString(_fieldCollection, serilaizef, ref  errmsg);
                return _strFieldCollection;
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return null;
            }
        }

        public static string SerializeQFCToString(QFieldCollection _fieldCollection, out string errmsg)
        {
            errmsg = "";
            try
            {
                var _strFieldCollection = MessageSerializer<QFieldCollection>.SaveToString(_fieldCollection, serilaizef, ref  errmsg);
                return _strFieldCollection;
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return "";
            }
        } 


        #endregion

        #region Set Object's property by Reflection 
        public static bool SetPropertyValue(Object containerR,PropertyInfo[] propertys, DataRow drdata)
        {
            try
            {
                var dcprop = new Dictionary<string, int>();
                object objValue;

                var dcc = drdata.Table.Columns;


                for (var i = 0; i < propertys.Length; i++)
                {
                    dcprop.Add(propertys[i].Name.ToUpper(), i);
                }

                for (var i = 0; i < dcc.Count; i++)
                {
                    var colname = dcc[i].ColumnName.ToUpper();
                    objValue = drdata[colname];
                    if (dcprop.ContainsKey(colname))
                    {
                        try
                        {
                            propertys[dcprop[colname]].SetValue(containerR, Convert.ChangeType(objValue, propertys[dcprop[colname]].PropertyType), null);
                        }
                        catch
                        {

                        }

                    }
                }
                return true;
            }
            catch
            {
                return false;
                
            }
        }
        #endregion
    }
}

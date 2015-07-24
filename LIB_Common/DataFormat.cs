using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace LIB_Common
{
    public class DataFormat
    {        
        public bool IsNumeric(string itemValue)
        {
            try
            {
                if (string.IsNullOrEmpty(itemValue))
                    return false;
                //用正则表达式判断是输入的是否为数字(去除可能发生的输入法输入中文问题)
                var numRegex = new Regex(@"^(-?\d*[.]*\d*)$");
                var Result = numRegex.Match(itemValue);

                if (Result.Success)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public bool IsNaturalNumeric(string itemValue)
        {
            try
            {
                if (string.IsNullOrEmpty(itemValue))
                    return false;
                var numRegex = new Regex(@"^([1-9][0-9]*)$");
                var Result = numRegex.Match(itemValue);
                if (Result.Success)
                    return true; 
                else
                    return false; 
            }
            catch
            {
                return false;
            }
        }

        public int GetInt32Value(string value, int defaultValue)
        {
            if (IsNumeric(value))
                return Convert.ToInt32(value);
            else
                return defaultValue;
        }

        public int GetInt16Value(string value, int defaultValue)
        {           
            if (IsNumeric(value))
                return Convert.ToInt16(value);
            else
                return defaultValue;
        }

        public uint GetUint16Value(string value, uint defaultValue)
        {
            if (IsNumeric(value))
                return Convert.ToUInt16(value);
            else
                return defaultValue;
        }

        public double GetDoubleValue(string value, double defaultValue)
        {
            if (IsNumeric(value))
                return Convert.ToDouble(value);
            else
                return defaultValue;
        }

        public bool GetBoolValue(string value)
        {
            if (value.ToUpper() == "TRUE" || value.ToUpper() == "FALSE")
            {
                return Convert.ToBoolean(value);
            }
            else
                return false;
        }

        public bool GetBoolValue(int value)
        {
            return Convert.ToBoolean(value);
        }

        public decimal GetDecimalValue(string value, decimal defaultValue)
        {
            if (IsNumeric(value))
                return Convert.ToDecimal(value);
            else
                return defaultValue;
        }

        public decimal GetDecimalValue(DataRow dtRow, string rowName)
        {
            if (dtRow != null && dtRow.Table != null)
            {
                if (dtRow.Table.Columns.Contains(rowName))
                    return GetDecimalValue(dtRow[rowName].ToString(), 0);
            }

            return 0;
        }

        public string GetStringValue(DataRow dtRow,string rowName)
        {
            if (dtRow != null && dtRow.Table != null)
            {
                if (dtRow.Table.Columns.Contains(rowName))
                    return dtRow[rowName].ToString();
            }
            
            return "";
        }

        public int GetInt32Value(DataRow dtRow, string rowName)
        {
            if (dtRow != null && dtRow.Table != null)
            {
                if (dtRow.Table.Columns.Contains(rowName))
                    return GetInt32Value(dtRow[rowName].ToString(), 0);
            }

            return 0;
        }

        public int GetInt16Value(DataRow dtRow, string rowName)
        {
            if (dtRow != null && dtRow.Table != null)
            {
                if (dtRow.Table.Columns.Contains(rowName))
                    return GetInt16Value(dtRow[rowName].ToString(), 0);
            }
           
            return 0;
        }

        public DateTime GetDataTimeValue(DataRow dtRow, string rowName)
        {
            if (dtRow != null && dtRow.Table != null)
            {
                if (dtRow.Table.Columns.Contains(rowName))
                    return Convert.ToDateTime(dtRow[rowName].ToString());
            }
            
            return new DateTime();
        }

        public string FormatListToStr(List<string> sList, string separator)
        {
            var rtnStr = "";

            if (sList != null && sList.Count > 0)
            {
                rtnStr = sList.Aggregate(rtnStr, (current, value) => current + value + separator);
            }
            return rtnStr;
        }

        /// <summary>
        /// add by FYP 20140801 @Basf.Site
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public DateTime GetValueAsDatetime(string value, DateTime defaultvalue)
        {
            try
            {
                DateTime dttemp;
                if( ! DateTime.TryParse(value, out dttemp))
                {
                    dttemp = defaultvalue;
                }
                return dttemp;
            }
            catch
            {
                return defaultvalue;
            }
        }

        /// <summary>
        /// add by FYP 20140801 @Basf.Site
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public DateTime GetValueAsDatetime(string value)
        {
            return GetValueAsDatetime(value, new DateTime());
        }
        /// <summary>
        /// add by FYP 20140801 @Basf.Site
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public Decimal GetValueAsDecimal(string value, decimal defaultvalue)
        {            
            try
            {
                decimal dtemp;
                if (!decimal.TryParse(value,out dtemp))
                {
                    dtemp = defaultvalue;
                }
                return dtemp;
            }
            catch 
            {
                return defaultvalue;
            }
        }

        /// <summary>
        /// add by FYP 20140801 @Basf.Site
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Decimal GetValueAsDecimal(string value)
        {
            return GetValueAsDecimal(value, 0);
        }
        /// <summary>
        /// add by FYP 20140801 @Basf.Site
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public int GetValueAsInt(string value, int defaultvalue)
        {
            try
            {
                int dtemp;
                if (!int.TryParse(value, out dtemp))
                {
                    dtemp = defaultvalue;
                }
                return dtemp;
            }
            catch
            {
                return defaultvalue;
            }
        }

        /// <summary>
        /// add by FYP 20140801 @Basf.Site
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int GetValueAsInt(string value)
        {
            return GetValueAsInt(value, 0);
        }


    }
}

///*************************************************************************//
/// System/Module Name:Common Lib 
/// FileName:          this.FileName
/// Author:	           ZYR
///
/// Change History: 
/// Version	    Date		     By	        Details
/// 0.0.1       2012-08-06       ZYR         Create
///***************************************************************************//
/// Function: 
/// Provide TimeSpan helper class
///***************************************************************************//

using System;

namespace LIB_Common
{
    public static class TimeSpanHelper
    {
        public static TimeSpan FromDays(int days, string text)
        {
            return TimeSpan.FromTicks(0xc92a69c000L * days);
        }

        public static TimeSpan FromMilliseconds(int ms, string text)
        {
            return TimeSpan.FromTicks(0x2710L * ms);
        }

        public static TimeSpan FromMinutes(int minutes, string text)
        {
            return TimeSpan.FromTicks(0x23c34600L * minutes);
        }

        public static TimeSpan FromSeconds(int seconds, string text)
        {
            return TimeSpan.FromTicks(0x989680L * seconds);
        }
    }

    public static class DateTimeFormat
    {
        public const string WINDOWFFFFFF = "yyyy-MM-dd HH:mm:ss:ffffff";
        public const string ORACLEFFFFFF = "yyyy-MM-DD HH24:mi:ss:ff";
        public const string WINDOWSS = "yyyy-MM-dd HH:mm:ss";
        public const string ORACLESS = "yyyy-MM-DD HH24:mi:ss";
    }

   
}

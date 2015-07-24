using System;
using System.Collections.Generic;

namespace LIB_Common
{
    public enum eExceptionType
    { 
        Internal = 1,
        UserDefine = 2
    }

    public struct ExceptionDescriber
    {
        public eExceptionType ExceptionType;
        public UInt32 ExceptionID;
        public String ExceptionName;
        public String ClassName;
        public String MethodName;
        public String ExceptionDescription;        
    }
        
    public static class WAPExceptionHelper
    {
        private static readonly Dictionary<UInt32, ExceptionDescriber> WAPExceptionDict;
        
        static WAPExceptionHelper()
        {
            WAPExceptionDict = new Dictionary<uint, ExceptionDescriber>();
            RegisterInternalWAPException(0, "UndefinedException:", "Undefined:", "Undefined:", "This Exception is not defined:");
        }
                
        public static void RegisterWAPException(eExceptionType exceptionType,UInt32 exceptionID, String exceptionName, String className,
            String methodName,String exceptionDescription)
        {
            if (WAPExceptionDict.ContainsKey(exceptionID))
                return;
            var ed = new ExceptionDescriber
            {
                ExceptionType = exceptionType,
                ExceptionID = exceptionID,
                ExceptionName = exceptionName,
                ClassName = className,
                MethodName = methodName,
                ExceptionDescription = exceptionDescription
            };

            WAPExceptionDict.Add(ed.ExceptionID, ed);
        }

        public static void RegisterWAPException(FieldCollection fc)
        {
            var exceptionType = GetExceptionType(fc);
            if(exceptionType!="")
            {
                var t = (eExceptionType)Enum.Parse(typeof(eExceptionType), exceptionType, true);

                RegisterWAPException(t, (UInt32)GetExceptionID(fc), GetExceptionName(fc), GetClassName(fc),
                                     GetMethodName(fc), GetExceptionDescription(fc));
            }
        }

        public static void RegisterInternalWAPException(UInt32 exceptionID, String exceptionName,String className,
            String methodName, String exceptionDescription)
        { 
            RegisterWAPException(eExceptionType.Internal,exceptionID,exceptionName,className,methodName,exceptionDescription);
        }

        public static void RegisterUserWAPException(UInt32 exceptionID, String exceptionName, String className,
            String methodName, String exceptionDescription)
        {
            RegisterWAPException(eExceptionType.UserDefine, exceptionID, exceptionName, className, methodName, exceptionDescription);
        }

        public static ExceptionDescriber GetExceptionDescriber(UInt32 exceptionID)
        {
            var ed = new ExceptionDescriber();

            if (WAPExceptionDict != null && exceptionID>0)
            {           
                if (WAPExceptionDict.ContainsKey(exceptionID))
                {
                    WAPExceptionDict.TryGetValue(exceptionID, out ed);
                }
            }
           
            return ed;
            
        }

        public static string GetExceptionMessage(UInt32 exceptionID)
        {
            if (GetExceptionDescriber(exceptionID).ExceptionDescription != null)
                return GetExceptionDescriber(exceptionID).ExceptionDescription;
            else
                return "";
        }

        public static WAPGeneralException NewWAPException(UInt32 exceptionID, string className, string methodName, string exDescription, string extraInfo, Exception ex)
        {
            if (ex == null)
            {
               
                //modified by FYP 2014/03/19
                //return null;
                return new WAPGeneralException(exceptionID, className, methodName, extraInfo, exDescription, "", null);
            }
            else
            {
                return new WAPGeneralException(exceptionID, className, methodName, extraInfo, exDescription, ex.Message, ex.InnerException);
            }
        
        }

        public static WAPGeneralException GetWAPException(UInt32 exceptionID,String extraInfo,Exception ex)
        { 
            var ed = GetExceptionDescriber(exceptionID);
            
            if (ed.ExceptionID == 0)
            {
                ed = GetExceptionDescriber(0);
                ed.ExceptionName = ed.ExceptionName + exceptionID.ToString();
                ed.ClassName = ed.ClassName + exceptionID.ToString();
                ed.MethodName = ed.MethodName + exceptionID.ToString();
                ed.ExceptionDescription = ed.ExceptionDescription + exceptionID.ToString();
            }

            return  NewWAPException(ed.ExceptionID, ed.ClassName, ed.MethodName, ed.ExceptionDescription, extraInfo, ex);        
        }

        public static string GetExceptionType(FieldCollection fc)
        {
            if (fc != null)
            {
                var fcFormat = new FieldCollectionFormat();
                if (!fcFormat.IsEmptyStrField(fc, "ExceptionType"))
                    return fc["ExceptionType"].GetValueAsString();
            }          
            
            return "";
        }

        public static int GetExceptionID(FieldCollection fc)
        {
            if (fc != null)
            {
                var fcFormat = new FieldCollectionFormat();
                if (!fcFormat.IsEmptyStrField(fc, "ExceptionID"))
                    return fc["ExceptionID"].GetValueAsInt();
            }

            return 0;
        }

        public static string GetExceptionName(FieldCollection fc)
        {
            if (fc != null)
            {
                var fcFormat = new FieldCollectionFormat();
                if (!fcFormat.IsEmptyStrField(fc, "ExceptionName"))
                    return fc["ExceptionName"].GetValueAsString();
            }
            return "";
        }

        public static string GetClassName(FieldCollection fc)
        {
            if (fc != null)
            {
                var fcFormat = new FieldCollectionFormat();
                if (!fcFormat.IsEmptyStrField(fc, "ClassName"))
                    return fc["ClassName"].GetValueAsString();
            }
            return "";
        }

        public static string GetMethodName(FieldCollection fc)
        {
            if (fc != null)
            {
                var fcFormat = new FieldCollectionFormat();
                if (!fcFormat.IsEmptyStrField(fc, "MethodName"))
                    return fc["MethodName"].GetValueAsString();
            }
            return "";
        }

        public static string GetExceptionDescription(FieldCollection fc)
        {
            if (fc != null)
            {
                var fcFormat = new FieldCollectionFormat();
                if (!fcFormat.IsEmptyStrField(fc, "ExceptionDescription"))
                    return fc["ExceptionDescription"].GetValueAsString();
            }
            return "";
        }
    }

    [Serializable]
    public class WAPGeneralException : ApplicationException
    {

        private UInt32 _id;
        private string _className;
        private string _methodName;
        private string _eventName;
        private string _errorInfo;
        private string _exDescription;
       
        public UInt32 ExId
        {
            get { return _id; }
        }

        public string ExClassName
        {
            get { return _className; }
        }
        public string ExMethodName
        {
            get { return _methodName; }
        }
        public string ExEventName
        {
            get { return _eventName; }
        }

        public string ExErrorInfo
        {
            get { return _errorInfo; }
        }

        public string ExDescription
        {
            get { return _exDescription; }
        }

        public WAPGeneralException(UInt32 exId, string exClassName, string exMethodName, 
            string exEventName, string exDescription,string exErrorInfo,Exception innerEx)
        {
            _id = exId;
            _className = exClassName;
            _methodName = exMethodName;
            _eventName = exEventName;
            _errorInfo = exErrorInfo;
            _exDescription = exDescription;            
        }
        /// <summary>
        /// Format:ID|Type|FrmName|ControlName|Event|Message
        /// </summary>
        /// <returns></returns>
        public override string Message
        {
            get
            {
                string msg;

                if (InnerException == null)
                    msg = base.Message;
                else
                    msg = string.Format("{0}, {1}", base.Message, InnerException.Message);

                msg = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}", _id, _className, _methodName, _eventName, _errorInfo, _exDescription,msg);
                return msg;
            }
        }       
    }
}

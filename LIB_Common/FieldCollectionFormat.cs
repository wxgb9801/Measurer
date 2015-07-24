using System;
using System.Collections.Generic;
namespace LIB_Common
{
    ///***************************************************************************//
    ///     Function: 
    ///         功能:
    ///***************************************************************************//
    ///Inherit Interface:   
    ///    Inherit Class:   
    ///***************************************************************************//
    ///    Item Name:   FieldCollectionFormat
    ///       Author:   He Ye
    ///  Create Time:   1/9/2014 6:46:25 PM
    /// Machine Name:   SHA157L
    ///***************************************************************************//
    ///      Version:  
    ///  Description:   
    ///***************************************************************************//


    [Serializable]
    public class FieldCollectionFormat
    {       
        #region Public Method
        public bool IsEmptyStrField(FieldCollection fc, string fieldName)
        {
            if (fc == null)
                return true;

            if (IsExistField(fc, fieldName))
            {
                var fieldValue = fc[fieldName].GetValueAsString();
                if (string.IsNullOrEmpty(fieldValue))
                    return true;
                else
                    return false;
            }
            else
                return true;
        }

        public bool IsExistEmptyStrField(FieldCollection fc, List<string> fieldList, out List<string> emptyFieldList)
        {
            emptyFieldList = null;

            if (fc == null || fieldList == null || fieldList.Count <= 0 )
                return false;

            foreach (var fieldName in fieldList)
            {
                emptyFieldList = new List<string>();
                if (!string.IsNullOrEmpty(fieldName))
                {
                    if (IsExistField(fc, fieldName))
                    {
                        var fieldValue = fc[fieldName].GetValueAsString();
                        if (string.IsNullOrEmpty(fieldValue))
                            emptyFieldList.Add(fieldName);

                    }
                    else
                        emptyFieldList.Add(fieldName);
                }
            }

            if (emptyFieldList != null && emptyFieldList.Count > 0)
                return true;
            else
                return false;
        }

        public bool IsExistField(FieldCollection fc, string fieldName)
        {
            if (fc==null||fc[fieldName] == null||fieldName==string.Empty)
                return false;
            else
                return true;
        }

        public int GetIntValue(FieldCollection fc, string fieldName, int defaultValue)
        {
             if (!IsEmptyStrField(fc, fieldName))
                 //if(fc[fieldName].AllowModify)
                    return fc[fieldName].GetValueAsInt();

             return defaultValue;
        }   
   
        public uint GetUIntValue(FieldCollection fc, string fieldName, uint defaultValue)
        {
            if (!IsEmptyStrField(fc, fieldName))
                //if (fc[fieldName].AllowModify)
                    return fc[fieldName].GetValueAsUInt();

            return defaultValue;
        }

        public double GetDoubleValue(FieldCollection fc, string fieldName, double defaultValue)
        {          
            try
            {
                if (!IsEmptyStrField(fc, fieldName))
                    //if (fc[fieldName].AllowModify)
                        return Convert.ToDouble(fc[fieldName].GetValueAsSObject());

                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        public string GetStringValue(FieldCollection fc, string fieldName,string defaultValue)
        {
            if (!IsEmptyStrField(fc, fieldName))
                //if (fc[fieldName].AllowModify)
                    return fc[fieldName].GetValueAsString();

            return defaultValue;
        }

        public DateTime GetDateTimeValue(FieldCollection fc, string fieldName, DateTime defaultValue)
        {
            if (!IsEmptyStrField(fc, fieldName))
                //if (fc[fieldName].AllowModify)
                    return fc[fieldName].GetValueAsDateTime();

            return defaultValue;
        }

        public bool GetBoolValue(FieldCollection fc, string fieldName, bool defaultValue)
        {
            if (!IsEmptyStrField(fc, fieldName))
                return fc[fieldName].GetValueAsBool();

            return defaultValue;
        }

        public decimal GetDecimalValue(FieldCollection fc, string fieldName, decimal defaultValue)
        {
            if (!IsEmptyStrField(fc, fieldName))
                //if (fc[fieldName].AllowModify)
                    return fc[fieldName].GetValueAsDecimal();

            return defaultValue;
        }

        public List<string> GetStringList(FieldCollection fc, string fieldName, List<string> defaultValue)
        {
            try
            {
                if (!IsEmptyStrField(fc, fieldName))
                    //if (fc[fieldName].AllowModify)
                        return (List<string>)fc[fieldName].GetValueAsSObject();

                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }          
        }
        
        public FieldCollection fillFcField(FieldCollection fc, string fieldName,object objValue)
        {
            if (IsExistField(fc, fieldName))
            {
                fc[fieldName].AllowModify = true;
                fc[fieldName].Value = objValue;
            }

            return fc;
        }

        public FieldCollection fillDateTimeFcField(FieldCollection fc, string fieldName, object objValue)
        {
            if (IsExistField(fc, fieldName))
            {
                fc[fieldName].AllowModify = true;

                if ((DateTime)objValue == new DateTime())
                    fc[fieldName].Value = null;
                else
                    fc[fieldName].Value = objValue;
            }

            return fc;
        }

        public FieldCollection GetFCByClassID(string classID,bool ifModify)
        {
            var fc = SysBusinesObjectManager.GetBusPropertySchemaByClassID(classID);
            if (!ifModify&&fc!=null)
                fc.SetAllFeilds_AllowModify(false);

            return fc;
        }

     
        #endregion  
    }
}

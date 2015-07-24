using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace LIB_Common
{

    public interface IFields
    {
        FieldCollection ToFieldCollection();
        bool LoadFieldCollection(FieldCollection fc);
    }
    public class FieldDataMode : IFields
    {
        protected FieldCollection _dFields;

        public FieldCollection DFields
        {
            get { return _dFields; }
        }
        public FieldDataMode()
        {

        }
        public FieldDataMode(FieldCollection fc)
        {
            _dFields = fc;
        }
        FieldCollection IFields.ToFieldCollection()
        {
            return DFields.Clone();
        }

        bool IFields.LoadFieldCollection(FieldCollection fc)
        {
            return DFields.SetFieldsValue(fc);
        }
    }

    public interface IOPFieldObject_M
    {
        bool CreateData(IFields Fields);
        bool RemoveData(IFields Fields);
    }
    public interface IOPFieldObject_U
    {
        bool LoadData(IFields Fields);
        bool SaveData(IFields Fields);
    }
    public interface IOPFieldObject : IOPFieldObject_M, IOPFieldObject_U
    {

    }
    #region 字段 及 字段集合
    /// <summary>
    /// 简单的数据库字段信息结构
    /// </summary>
    [Serializable]
    public class Field : ICustomerSerializable, ICloneable
    {
        //字段是否已经使用
        private bool _isSetValue = false;
        public bool IsSetValue
        {
            get { return _isSetValue; }
            set { _isSetValue = value; }
        }
        private string _filename;
        private object _value;
        private object _oldvalue;
        private Type _type;
        private bool _isKey;
        private bool _onlyRead;
        private bool _isSafe;                       //如果是安全的则不需要将(')转化为('')
        private bool _allowModify = true;
        private string _relationshipTable;
        private string _relationshipDisField;
        private string _relationshipValField;
        private string _displaytext;
        private string _desc;
        private string _source;

        private bool _isAutoIDent = false;

        public string FieldName
        {
            get
            {
                return _filename;
            }
            set
            {
                _filename = value;
            }
        }
        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _isSetValue = true;
                _value = value;
            }
        }
        public Type type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }
        public object Old_Value
        {
            get
            {
                return _oldvalue;
            }
            set
            {
                _oldvalue = value;
            }

        }
        public bool IsKey
        {
            get
            {
                return _isKey;
            }
            set
            {
                _isKey = value;
            }

        }
        public string Desc
        {
            get
            {
                return _desc;
            }
            set
            {
                _desc = value;
            }
        }
        public bool ReadOnly
        {
            get { return _onlyRead; }
            set { _onlyRead = value; }
        }
        public bool IsUpdate
        {
            get
            {
                if (!_isSetValue)
                    return false;
                if (_value == null)
                {
                    if (_type == typeof(int) || _type == typeof(double))
                        _value = 0;
                    else
                        _value = DBNull.Value;
                }

                if (_oldvalue == null)
                {
                    if (_value == null || _value == DBNull.Value)
                        return false;
                    else
                        return true;
                    //if (_type == typeof(int) || _type == typeof(double))
                    //    _oldvalue = 0;
                    //else
                    //    _oldvalue = DBNull.Value;
                }

                //if (_oldvalue == null)  //Logan Modified 20130107@Shanghai
                //    return true;
                return (_oldvalue.ToString() != _value.ToString());
            }
        }
        public bool IsSafe
        {
            get { return _isSafe; }
            set { _isSafe = value; }
        }
        public bool AllowModify
        {
            get { return _allowModify; }
            set { _allowModify = value; }
        }
        public bool IsAutoIdent
        {
            get { return _isAutoIDent; }
            set { _isAutoIDent = value; }
        }

        public string Source
        {
            get
            {
                return _source;
            }
            set
            {
                _source = value;
            }
        }

        public string RelationshipTable
        {
            get { return _relationshipTable; }
            set { _relationshipTable = value; }
        }
        public string RelationshipDisField
        {
            get { return _relationshipDisField; }
            set { _relationshipDisField = value; }
        }
        public string RelationshipValField
        {
            get { return _relationshipValField; }
            set { _relationshipValField = value; }
        }
        public string DisplayText
        {
            get { return _displaytext; }
            set { _displaytext = value; }
        }

        private string _guid;
        public string GUID
        {
            get
            {
                if (_guid == null || _guid == String.Empty)
                {
                    _guid = Guid.NewGuid().ToString();
                }
                return _guid;
            }
            set
            {
                _guid = value;
            }
        }

        public Field()
            : this("")
        {

        }

        public Field(string name)
            : this("", name, typeof(string), null)
        {
        }

        public Field(string name, bool isKey)
            : this("", name, typeof(string), null, isKey, null, false, false)
        {
        }
        public Field(string name, object value, bool isKey, bool onlyread)
            : this("", name, value == null ? null : value.GetType(), value, isKey, null, onlyread, false)
        {
        }
        public Field(string name, object value, bool isKey, string desc)
            : this("", name, value == null ? null : value.GetType(), value, isKey, desc, false, false)
        {
        }
        public Field(string source, string name, Type ty, object value)
            : this(source, name, ty, value, false, null, false, false)
        {
        }
        public Field(string source, string name, Type ty, object value, string desc)
            : this(source, name, ty, value, false, desc, false, false)
        {
        }
        public Field(string source, string name, Type ty, object value, bool iskey, string desc, bool onlyread, bool isSafe)
        {
            _filename = name;
            _type = ty;
            _value = value;
            _oldvalue = value;
            _isKey = iskey;
            _desc = desc;
            _onlyRead = onlyread;
            _isSafe = isSafe;
            _allowModify = true;
            _source = source;

        }


        public void SyncField()
        {
            _oldvalue = Value;
        }
        public int GetValueAsInt()
        {
            var tmpValue = 0;
            int.TryParse(GetValueAsString(), out tmpValue);
            return tmpValue;
        }
        public string GetValueAsString()
        {
            if (_value == null)
                return "";
            else
                return _value.ToString();
        }
        public decimal GetValueAsDecimal()  //Add at 1.0.2 
        {
            decimal tmpValue = 0;
            decimal.TryParse(GetValueAsString(), out tmpValue);
            return tmpValue;
        }
        public bool GetValueAsBool()      //Add at 1.0.2 
        {
            var tmpValue = false;
            bool.TryParse(GetValueAsString(), out tmpValue);
            return tmpValue;
        }
        public DateTime GetValueAsDateTime() //Add at 1.0.2 
        {
            var tmpValue = new DateTime(); ;//DateTime.Now;
            DateTime.TryParse(GetValueAsString(), out tmpValue);
            return tmpValue;
        }
        public object GetValueAsSObject()
        {
            return _value;
        }
        public IFields GetValueAsIFields()
        {
            if (_value != null)
            {
                return (IFields)_value;
            }
            else
            {
                return null;
            }
        }
        public FieldCollection GetValueAsFieldCollection()
        {
            if (_value != null)
            {
                return (FieldCollection)_value;
            }
            else
            {
                return null;
            }
        }
        public uint GetValueAsUInt()
        {
            uint tmpValue = 0;
            uint.TryParse(GetValueAsString(), out tmpValue);
            return tmpValue;
        }
        public Field Clone()
        {
            var obj = _clone(_value);

            var fld = new Field(_source, _filename, _type, obj, _isKey, _desc, _onlyRead, _isSafe);
            fld.Old_Value = _clone(_oldvalue);
            fld.AllowModify = _allowModify;
            fld.IsSetValue = _isSetValue;
            return fld;

        }

        public object _clone(object Obj)
        {
            if (Obj == null) return null;
            var newObj = Obj;
            var T = Obj.GetType();
            if (Obj is ICloneable)
            {
                newObj = ((ICloneable)Obj).Clone();
            }
            else
            {
                if (T.IsSerializable)   //支持序列化的情况简单;若不支持，只能通过反射属性。     
                {
                    var ms = new MemoryStream();
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(ms, Obj); // 把对象序列化并返回相应的字节         
                    ms.Position = 0;
                    newObj = formatter.Deserialize(ms); // 把字节反序列化成相应的对象         
                    ms.Close();
                }
                else if (Obj is DateTime)
                {
                    var ltime = ((DateTime)Obj).Ticks;
                    newObj = new DateTime(ltime);
                }
                else
                {
                    //暂不支持反射属性的方式
                }
            }
            return newObj;
        }
        public override string ToString()
        {
            if (_filename != "" && GetValueAsString() != "")
                return string.Format("[{0}={1}]", _filename, GetValueAsString());
            else
                return "";
        }

        string ICustomerSerializable.ToSerializableString(eCustomerSerializableType type)
        {
            if (type == eCustomerSerializableType.NoStruct)
            {
                var sb = new StringBuilder();
                sb.Append(string.Format("{1}{0}{2}{0}{3}", CustomerSerializableProvider.FieldSplitString, _value, _oldvalue, _allowModify ? 1 : 0));
                return sb.ToString();
            }
            else
            {
                return "";
            }
        }

        void ICustomerSerializable.LoadObjectFromSerializableString(eCustomerSerializableType type, string serializableString)
        {

            var fld = new Field();
            if (type == eCustomerSerializableType.NoStruct)
            {
                var vars = serializableString.Split(new string[1] { CustomerSerializableProvider.FieldSplitString }, StringSplitOptions.RemoveEmptyEntries);
                if (vars.Length > 0)
                {
                    _value = vars[0];
                    if (vars.Length > 1)
                    {
                        _oldvalue = vars[1];
                        if (vars.Length > 2)
                        {
                            _allowModify = vars[2] == "1" ? true : false;
                        }
                    }
                }
            }
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public static Field CreateFiledByGUID(string guid)
        {
            if (guid != "")
            {
                var fld = new Field();
                fld._guid = guid;
                return fld;
            }
            else
                return null;
        }
    }
    /// <summary>
    /// 字段集合
    /// </summary>
    [Serializable]
    public class FieldCollection : List<Field>, ICustomerSerializable
    {

        private string _name;
        private FieldTable _childTables;

        public FieldTable ChildTable
        {
            get { return _childTables; }
            set { _childTables = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _guid;
        public string GUID
        {
            get
            {
                if (_guid == null || _guid == String.Empty)
                {
                    _guid = Guid.NewGuid().ToString();
                }
                return _guid;
            }
            set
            {
                _guid = value;

            }
        }
        #region 构造方法
        public FieldCollection()
        {

        }
        public FieldCollection(string name)
        {
            _name = name;
        }
        //public FieldCollection(IEnumerable<Field> fields)
        //    : base(fields)
        //{
        //}
        //public FieldCollection(string name, IEnumerable<Field> fields)
        //    : base(fields)
        //{
        //    _name = name;
        //}

        #endregion
        #region 新增方法
        public bool Add(Field field)
        {
            if (field != null && this[field.FieldName] == null)
            {
                base.Add(field);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Add(string FieldName)
        {
            return Add(new Field(FieldName));
        }
        public bool Add(string FieldName, string desc)
        {
            return Add(new Field("", FieldName, typeof(string), null, false, desc, false, false));
        }
        public bool Add(string FieldName, object value)
        {
            if (value != null)
                return Add(new Field("", FieldName, value.GetType(), value));
            else
                return false;
        }
        public bool Add(string FieldName, object value, string desc)
        {
            return Add(new Field(FieldName, value, false, desc));
        }
        public bool Add(string source, string FieldName, object value, string desc)
        {
            if (value != null)
                return Add(new Field(source, FieldName, value.GetType(), value, desc));
            else
                return false;
        }
        public bool Add(string source, string FieldName, Type ty, object value)
        {
            return Add(new Field(source, FieldName, ty, value));
        }
        public bool Add(string source, string FieldName, object value, bool isKey, bool onlyread)
        {
            if (value != null)
                return Add(new Field(source, FieldName, value.GetType(), value, isKey, null, onlyread, false));
            else
                return false;
        }
        public bool Add(string source, string FieldName, object value, bool isKey, string desc)
        {
            if (value != null)
                return Add(new Field(source, FieldName, value.GetType(), value, isKey, desc, false, false));
            else
                return false;
        }
        public bool Add(string source, string FieldName, Type ty, object value, bool isKey, string desc)
        {
            return Add(new Field(source, FieldName, ty, value, isKey, desc, false, false));
        }
        public bool Add(string source, string FieldName, object value, bool isKey, string desc, bool onlyread, bool isSafe)
        {
            if (value != null)
                return Add(new Field(source, FieldName, value.GetType(), value, isKey, desc, onlyread, isSafe));
            else
                return false;
        }
        #endregion
        public Field this[string FieldName]
        {
            get
            {
                foreach (var thefield in this)
                {
                    if (string.Equals(thefield.FieldName, FieldName, StringComparison.OrdinalIgnoreCase))
                        return thefield;
                }
                return null;
            }
            set
            {
                Field tmpfield = null;
                foreach (var thefield in this)
                {
                    if (thefield.FieldName == FieldName)
                    {
                        tmpfield = thefield;
                    }
                }
                if (tmpfield != null)
                {
                    tmpfield = value;
                }
            }
        }
        /// <summary>
        /// 输出字段名数组
        /// </summary>
        /// <returns></returns>
        public string[] ToArrayByName()
        {
            if (Count > 0)
            {
                var tmpstrs = new string[Count];
                for (var i = 0; i < Count; i++)
                {
                    tmpstrs[i] = this[i].FieldName;
                }
                return tmpstrs;
            }
            else
                return null;
        }
        /// <summary>
        /// 输出字段字段和值的hashtable
        /// </summary>
        /// <returns></returns>
        public Hashtable ToHashByValue()
        {
            if (Count > 0)
            {
                var hs = new Hashtable();
                foreach (var f in this)
                {
                    hs.Add(f.FieldName, f.Value);
                }
                return hs;
            }
            else
                return null;
        }
        ///// <summary>
        ///// 输出以主键为条件的where语句(不包含where关键字)
        ///// </summary>
        ///// <returns></returns>
        //public string ToKeyEqualValue()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    foreach (Field f in this)
        //    {
        //        if (f.IsKey)
        //        {
        //            sb.Append(" and ");
        //            sb.Append(f.FieldName);
        //            sb.Append(" = ");
        //            sb.Append(FunBase.ChkField(f.Value.ToString(), f.type == typeof(string)));
        //        }
        //    }
        //    return sb.ToString();
        //}
        public void SetAllFeilds_IsReadOnly(bool isReadOnly)
        {
            foreach (var thefield in this)
            {
                thefield.ReadOnly = isReadOnly;
            }
        }
        public void SetAllFeilds_AllowModify(bool isAllowModify)
        {
            foreach (var thefield in this)
            {
                thefield.AllowModify = isAllowModify;
            }
        }
        public void SetAllFeilds_ClearValue()
        {
            foreach (var thefield in this)
            {
                thefield.Value = null;
                thefield.SyncField();
            }
        }
        public DataTable ToDataTable(bool IsSchema)
        {
            if (Count > 0)
            {
                var dt = new DataTable();
                if (!IsSchema)
                {
                    dt.Columns.Add("FieldName");
                    dt.Columns.Add("Value");
                    dt.Columns.Add("Desc");
                    foreach (var f in this)
                    {
                        var dr = dt.NewRow();
                        dr["FieldName"] = f.FieldName;
                        dr["Value"] = f.GetValueAsString();
                        dr["Desc"] = f.Desc;
                        dt.Rows.Add(dr);
                    }
                }
                else
                {
                    foreach (var f in this)
                    {
                        var dc = new DataColumn(f.FieldName);
                        dc.ReadOnly = f.ReadOnly;
                        dt.Columns.Add(dc);
                    }
                }
                return dt;
            }
            else
                return null;
        }
        public DataRow ToDataRow()
        {
            if (Count > 0)
            {
                var dt = ToDataTable(true);
                var dr = dt.NewRow();
                foreach (var f in this)
                {
                    dr[f.FieldName] = f.GetValueAsString();
                }
                return dr;
            }
            else
                return null;
        }

        public void InsertToDataTable(ref DataTable dt)
        {
            DataRow dr = null;
            if (dt == null || dt.Columns.Count == 0)
            {
                dt = ToDataTable(true);
            }
            dr = dt.NewRow();
            foreach (var f in this)
            {
                if (dt.Columns.Contains(f.FieldName))
                {
                    if (f.Value == null || f.Value == DBNull.Value)
                        dr[f.FieldName] = string.Empty;
                    else
                        dr[f.FieldName] = f.GetValueAsString();
                }
            }
            dt.Rows.Add(dr);
        }
        #region 通过DataTable得到字段集合
        public static FieldCollection GetFieldsByDataTable(DataTable dt)
        {
            if (dt != null)
            {
                var tmpfields = new FieldCollection();
                for (var i = 0; i < dt.Columns.Count; i++)
                {
                    tmpfields.Add(dt.Columns[i].ColumnName, dt.Rows[0][i]);
                }
                return tmpfields;
            }
            else
                return null;
        }
        #endregion

        public FieldCollection Clone()
        {
            if (Count > 0)
            {
                var fc = new FieldCollection();
                fc._name = _name;
                foreach (var f in this)
                {
                    fc.Add(f.Clone());
                }
                return fc;
            }
            else
                return null;
        }
        public FieldCollection Copy()
        {
            if (Count > 0)
            {
                var fc = new FieldCollection();
                fc._name = string.Format("{0}_copy", _name);
                foreach (var f in this)
                {
                    fc.Add(f);
                }
                return fc;
            }
            else
                return null;
        }
        public void SyncFields()
        {
            for (var i = 0; i < Count; i++)
            {
                this[i].SyncField();
            }
        }

        public bool SetFieldsValue(FieldCollection fields)
        {
            return SetFieldsValue(fields, true);
        }
        public bool SetFieldsValue(FieldCollection fields, bool isSyncFieldCurrent)
        {
            var result = false;
            if (fields != null && fields.Count > 0)
            {
                foreach (var f in fields)
                {
                    if (this[f.FieldName] != null)
                    {
                        this[f.FieldName].Value = f.Value;
                        if (isSyncFieldCurrent)
                        {
                            this[f.FieldName].SyncField();
                        }
                    }
                }
                result = true;
            }
            else
                result = false;
            return result;
        }
        //WXG20140717@Basf.Site For DataPersistence UpdateData
        public bool UpdateFieldsValue(FieldCollection fields)
        {
            var result = false;
            if (fields != null && fields.Count > 0)
            {
                foreach (var f in fields)
                {
                    if (this[f.FieldName] != null)
                    {
                        var thisField = this[f.FieldName];
                        if ((!(thisField.ReadOnly || thisField.IsKey)) && thisField.AllowModify && f.IsUpdate)
                        {
                            this[f.FieldName].Value = f.Value;
                        }
                    }
                }
                result = true;
            }
            else
                result = false;
            return result;
        }
        public bool SetFieldsValue(FieldCollection fields, bool isSyncField, bool bCopyOldValue)
        {
            var result = false;
            try
            {
                foreach (var f in fields)
                {
                    //WXG20140710
                    //if (this[f.FieldName] != null)
                    if (this[f.FieldName] != null)
                    {
                        this[f.FieldName].Value = f.Value;
                        if (bCopyOldValue)
                        {
                            this[f.FieldName].Old_Value = f.Old_Value;
                        }

                        if (isSyncField)
                        {
                            this[f.FieldName].SyncField();
                        }
                    }
                }
                result = true;
            }
            catch
            {
                return false;
            }

            return result;
        }

        public bool SetFieldsOldValue(FieldCollection fields)
        {
            var result = false;
            try
            {
                foreach (var f in fields)
                {
                    if (this[f.FieldName] != null)
                    {
                        this[f.FieldName].Old_Value = f.Value;
                    }
                }
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public bool SetFieldsValue(DataRow dr)
        {
            return SetFieldsValue(dr, true);
        }
        public bool SetFieldsValue(DataRow dr, bool isSyncField)
        {
            var result = false;
            if (dr != null)
            {
                for (var i = 0; i < dr.Table.Columns.Count; i++)
                {
                    var colName = dr.Table.Columns[i].ColumnName;
                    if (this[colName] != null)
                    {
                        this[colName].Value = dr[i].ToString();
                        if (isSyncField)
                            this[colName].SyncField();
                    }
                }
                result = true;
            }
            else
                result = false;
            return result;
        }
        public override string ToString()
        {
            if (Count > 0)
            {
                var sb = new StringBuilder(string.Format("{0}|", Name));
                foreach (var f in this)
                {
                    sb.Append(f.ToString());
                }
                return sb.ToString();
            }
            else
                return null;
        }

        public void SetSource(FieldCollection fields)
        {
            for (var i = 0; i < Count; i++)
            {
                var thisField = this[0];
                var tmpField = fields[thisField.FieldName];
                if (tmpField != null)
                {
                    thisField.Source = tmpField.Source;
                }
            }
        }


        public Dictionary<string, string> ToDictionary(FieldCollection fc)
        {
            return GetDictionaryByFieldCollection(fc);
        }
        public static FieldCollection CreateFieldCollectionByDictionary(Dictionary<string, string> dicr)
        {
            if (dicr != null && dicr.Count > 0)
            {
                var fc = new FieldCollection();
                foreach (var item in dicr)
                {
                    fc.Add(item.Key, item.Value, item.Key);
                }
                return fc;
            }
            else
                return null;
        }
        public static FieldCollection CreateFieldCollectionByHashTable(Hashtable ht)
        {
            if (ht != null && ht.Count > 0)
            {
                var fc = new FieldCollection();
                foreach (var key in ht.Keys)
                {
                    fc.Add(key.ToString(), ht[key], key.ToString());
                }
                return fc;
            }
            else
                return null;
        }
        public static FieldCollection CreateFieldCollectionByList(List<string> lst)
        {
            if (lst != null && lst.Count > 0)
            {
                var fc = new FieldCollection();
                foreach (var key in lst)
                {
                    fc.Add(key.ToString(), String.Empty, key.ToString());
                }
                return fc;
            }
            else
                return null;
        }
        public static Dictionary<string, string> GetDictionaryByFieldCollection(FieldCollection fc)
        {
            if (fc != null)
            {
                var Dict = new Dictionary<string, string>();
                foreach (var f in fc)
                {
                    Dict.Add(f.FieldName, f.GetValueAsString());
                }
                return Dict;
            }
            else
                return null;
        }
        public static FieldCollection CreateFieldCollectionByDataRow(DataRow dr)
        {
            if (dr != null)
            {
                var fc = new FieldCollection();
                for (var i = 0; i < dr.Table.Columns.Count; i++)
                {
                    fc.Add(dr.Table.Columns[i].ColumnName, dr[i]);
                }
                return fc;
            }
            else
                return null;
        }
        public static FieldCollection CreateFiledCollectionByGUID(string guid)
        {
            var fc = new FieldCollection();
            fc._guid = guid;
            return fc;
        }
        string ICustomerSerializable.ToSerializableString(eCustomerSerializableType type)
        {
            if (type == eCustomerSerializableType.FullStruct)
            {
                throw new NotImplementedException();
            }
            var sb = new StringBuilder(Name);
            sb.Append(CustomerSerializableProvider.FCHeadBodySplitString);
            for (var i = 0; i < Count; i++)
            {
                if (i > 0)
                    sb.Append(CustomerSerializableProvider.CollectionSplitString);
                if (this[i] is ICustomerSerializable)
                {
                    sb.Append(((ICustomerSerializable)this[i]).ToSerializableString(type));
                }
            }
            if (_childTables != null)
            {
                sb.Append(CustomerSerializableProvider.FCHeadBodySplitString);
                var childSB = new StringBuilder();
                for (var j = 0; j < _childTables.Count; j++)
                {
                    if (j > 0)
                        sb.Append(CustomerSerializableProvider.TableSplitString);
                    childSB.Append(((ICustomerSerializable)_childTables[j]).ToSerializableString(type));
                }
                var childsString = childSB.ToString().Replace(CustomerSerializableProvider.CollectionSplitString, CustomerSerializableProvider.FieldChildSplitString);
                sb.Append(childsString);
            }
            return sb.ToString();
        }

        void ICustomerSerializable.LoadObjectFromSerializableString(eCustomerSerializableType type, string serializableString)
        {
            if (type == eCustomerSerializableType.FullStruct)
            {
                throw new NotImplementedException();
            }
            var keyString = serializableString.Split(new string[1] { CustomerSerializableProvider.FCHeadBodySplitString }, StringSplitOptions.RemoveEmptyEntries);
            FieldCollection fc = null;
            var name = "";
            var body = "";
            var childsring = "";
            if (keyString.Length > 1)
            {
                name = keyString[0];
                body = keyString[1];
            }
            if (keyString.Length > 2)
            {
                childsring = keyString[2];
            }
            fc = SysBusPropertySchemaManager.GetBusPropertySchema(name);
            if (fc == null)
            {
                return;
                //throw new ApplicationException(string.Format("指定的FieldCollection[{0}]不存在。", name));
            }
            var fields = body.Split(new string[1] { CustomerSerializableProvider.CollectionSplitString }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < fields.Length; i++)
            {
                ((ICustomerSerializable)fc[i]).LoadObjectFromSerializableString(type, fields[i]);

            }
            var childs = childsring.Split(new string[1] { CustomerSerializableProvider.FieldChildSplitString }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < childs.Length; i++)
            {
                ((ICustomerSerializable)_childTables[i]).LoadObjectFromSerializableString(type, childs[i]);

            }
            _name = name;
            Clear();
            AddRange(fc);
        }
    }

    /// <summary>
    /// 字段集合表格
    /// </summary>
    [Serializable]
    public class FieldTable : IEnumerable, ICustomerSerializable, ICloneable
    {
        private List<FieldCollection> _rows = new List<FieldCollection>();
        private FieldCollection _protoFieldCollection;

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string[] Keys
        {
            get
            {
                var _keyList = new List<string>();
                foreach (var f in _protoFieldCollection)
                {
                    if (f.IsKey)
                        _keyList.Add(f.FieldName);
                }
                return _keyList.ToArray();
            }
        }

        private string _ufoid;
        private string UFOID
        {
            get
            {
                return _ufoid;
            }
        }

        public int Count
        {
            get
            {
                if (_rows != null)
                    return _rows.Count;
                else
                    return 0;
            }
        }

        public FieldTable(string ufoid)
        {
            _ufoid = ufoid;
        }
        public FieldCollection ProtoFieldCollection
        {
            get { return _protoFieldCollection; }
            set { _protoFieldCollection = value; }
        }
        public FieldCollection GetNewRow()
        {
            if (_protoFieldCollection != null)
            {
                return _protoFieldCollection.Clone();
            }
            return null;
        }
        //WXG20150307@Haday.Site
        public bool InsertRow(int index,FieldCollection fcRow)
        {
            if (chkRowFormat(fcRow))
            {
                _rows.Insert(index,fcRow);
                return true;
            }
            else
                return false;
        }
        public bool AddRow(FieldCollection fcRow)
        {
            //验证添加的fcRow 是否是_protoFieldCollection 中的每一个Field元素是否匹配。
            if (chkRowFormat(fcRow))
            {
                _rows.Add(fcRow);
                return true;
            }
            else
                return false;
        }
        public void Remove(int i)
        {
            _rows.Remove(_rows[i]);
        }
        public void Remove(FieldCollection fcRow)
        {
            _rows.Remove(fcRow);
        }
        public IEnumerator GetRowsEnumerator()
        {
            for (var i = 0; i < _rows.Count; i++)
            {
                yield return _rows[i];
            }
        }
        public FieldTable()
        {
            //_protoFieldCollection = new FieldCollection();
            //_rows = new List<FieldCollection>();
        }
        public bool chkRowFormat(FieldCollection fcRow)
        {
            if (fcRow != null)
            {
                if (_protoFieldCollection == null)
                {
                    _protoFieldCollection = fcRow.Clone();
                    return true;
                }
                if (_protoFieldCollection.Count != fcRow.Count)
                    return false;
                for (var i = 0; i < _protoFieldCollection.Count; i++)
                {
                    if (_protoFieldCollection[i].FieldName != fcRow[i].FieldName)
                        return false;
                }
                return true;
            }
            else
                return false;
        }

        public FieldCollection this[int i]
        {
            get
            {
                return _rows[i];
            }
        }
        public FieldCollection this[string Name]
        {
            get
            {
                foreach (var fc in _rows)
                {
                    if (fc.Name == Name)
                        return fc;
                }
                return null;
            }

        }
        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            for (var i = 0; i < _rows.Count; i++)
            {
                yield return _rows[i];
            }
        }

        #endregion


        public DataTable ToDataTable()
        {
            var dt = _protoFieldCollection.ToDataTable(true);
            var dr = dt.NewRow();
            foreach (var fc in _rows)
            {
                fc.InsertToDataTable(ref dt);
            }
            return dt;
        }
        string ICustomerSerializable.ToSerializableString(eCustomerSerializableType type)
        {
            if (type == eCustomerSerializableType.FullStruct)
            {
                throw new NotImplementedException();
            }
            var sb = new StringBuilder(Name);
            sb.Append(CustomerSerializableProvider.TableHeadBodySplitString);
            sb.Append(_protoFieldCollection.Name);
            sb.Append(CustomerSerializableProvider.TableHeadBodySplitString);
            for (var i = 0; i < Count; i++)
            {
                if (i > 0)
                    sb.Append(CustomerSerializableProvider.TableSplitString);
                if (this[i] is ICustomerSerializable)
                {
                    sb.Append(((ICustomerSerializable)this[i]).ToSerializableString(type));
                }
            }
            return sb.ToString();
        }

        void ICustomerSerializable.LoadObjectFromSerializableString(eCustomerSerializableType type, string serializableString)
        {
            if (type == eCustomerSerializableType.FullStruct)
            {
                throw new NotImplementedException();
            }
            var keyString = serializableString.Split(new string[1] { CustomerSerializableProvider.TableHeadBodySplitString }, StringSplitOptions.RemoveEmptyEntries);
            FieldCollection fc = null;

            var protypeFCName = "";
            var bodys = "";
            if (keyString.Length == 3)
            {
                _name = keyString[0];
                protypeFCName = keyString[1];
                bodys = keyString[2];
            }
            else
            {
                return;
                //throw new ApplicationException("无效的FieldTable序列化串。");
            }
            fc = SysBusPropertySchemaManager.GetBusPropertySchema(protypeFCName);
            if (fc == null)
            {
                throw new ApplicationException(string.Format("指定的FieldCollection[{0}]不存在。", protypeFCName));
            }
            var fcs = bodys.Split(new string[1] { CustomerSerializableProvider.TableSplitString }, StringSplitOptions.RemoveEmptyEntries);
            _rows = new List<FieldCollection>();
            _protoFieldCollection = fc;
            for (var i = 0; i < fcs.Length; i++)
            {
                var thefc = fc.Clone();
                ((ICustomerSerializable)thefc).LoadObjectFromSerializableString(type, fcs[i]);
                _rows.Add(thefc);
            }
        }

        object ICloneable.Clone()
        {
            var ft = new FieldTable();
            ft._name = _name;
            ft._ufoid = _ufoid;
            ft._protoFieldCollection = _protoFieldCollection;
            ft._rows = new List<FieldCollection>();
            for (var i = 0; i < _rows.Count; i++)
            {
                ft._rows.Add(_rows[i].Clone());
            }
            return ft;
        }

    }

    [Serializable]
    public class WAPDataObject : List<FieldCollection>, ICloneable, ICustomerSerializable
    {
        private string _guid;
        public string GUID
        {
            get
            {
                if (string.IsNullOrEmpty(_guid))
                {
                    _guid = Guid.NewGuid().ToString();
                }
                return _guid;
            }
            set
            {
                _guid = value;
            }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public WAPDataObject()
        {
        }
        public WAPDataObject(string Guid)
        {
            _guid = Guid;
        }
        object ICloneable.Clone()
        {
            throw new NotImplementedException();
        }

        string ICustomerSerializable.ToSerializableString(eCustomerSerializableType type)
        {
            if (type == eCustomerSerializableType.FullStruct)
            {
                throw new NotImplementedException();
            }
            var sb = new StringBuilder();
            for (var i = 0; i < Count; i++)
            {
                if (i > 0)
                    sb.Append(CustomerSerializableProvider.WAPObjSplitSstring);
                if (this[i] is ICustomerSerializable)
                {
                    sb.Append(((ICustomerSerializable)this[i]).ToSerializableString(type));
                }
            }
            return sb.ToString();
        }

        void ICustomerSerializable.LoadObjectFromSerializableString(eCustomerSerializableType type, string serializableString)
        {
            Clear();
            var fcs = serializableString.Split(new string[1] { CustomerSerializableProvider.WAPObjSplitSstring }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < fcs.Length; i++)
            {
                var thefc = new FieldCollection();
                ((ICustomerSerializable)thefc).LoadObjectFromSerializableString(type, fcs[i]);
                Add(thefc);
            }
        }

        public static WAPDataObject CreateWAPDataObjectByGUID(string guid)
        {
            var wdo = new WAPDataObject();
            wdo._guid = guid;
            return wdo;
        }
    }
    #endregion

    #region 查询字段定义
    /// <summary>
    /// 查询字段的过滤条件
    /// </summary>
    public enum eFilterType
    {
        Same = 0,
        Than = 1,
        LessThan = 2,
        Between = 3,
        FromTo = 4,
        Include = 5,
        UnInclude = 6,
        Like = 7,
        IsNull = 8,
        SubSQL = 9,
        Than1 = 10,
        LessThan1 = 11,
        IsNotNull = 12
    }

    [Serializable]
    public class QField
    {
        private string _fieldname;
        private object[] _values;
        private Type _valuestype;
        private bool _isUse;
        private eFilterType _filterType;
        private bool _isSafe;        //如果是安全的则不需要将(')转化为('')
        private string _desc;

        #region 属性
        public string FieldName
        {
            get { return _fieldname; }
            set { _fieldname = value; }
        }
        public object[] Values
        {
            get
            {
                return _values;
            }
            set
            {
                _values = value;
            }
        }
        public Type ValuesType
        {
            get
            {
                return _valuestype;
            }
            set { _valuestype = value; }
        }
        public bool IsUse
        {
            get { return _isUse; }
            set { _isUse = value; }
        }
        public eFilterType FilterType
        {
            get { return _filterType; }
            set { _filterType = value; }
        }
        public bool IsSafe
        {
            get { return _isSafe; }
            set { _isSafe = value; }
        }
        public string Desc
        {
            get
            {
                return _desc;
            }
            set
            {
                _desc = value;
            }
        }
        #endregion
        public QField(string fieldName)
        {
            _fieldname = fieldName;
        }
        public QField(string fieldName, Type type)
        {
            _fieldname = fieldName;
            _valuestype = type;
            _filterType = eFilterType.Same;
        }
        public QField(string fieldName, object[] values)
            : this(fieldName, values, values[0].GetType(), eFilterType.Same, true, false, "")
        {
        }
        public QField(string fieldName, object[] values, Type type)
            : this(fieldName, values, type, eFilterType.Same, true, false, "")
        {
        }
        public QField(string fieldName, object[] values, Type type, eFilterType filterType)
            : this(fieldName, values, type, filterType, true, false, "")
        {
        }
        public QField(string fieldName, object[] values, Type type, eFilterType filterType, bool isUse)
            : this(fieldName, values, type, filterType, isUse, false, "")
        {
        }
        public QField(string fieldName, object[] values, Type type, eFilterType filterType, bool isUse, bool isSafe, string desc)
        {

            if (values == null || values.Length < 1)
                throw new Exception("WXG501|数组vaues长度必须大于0");
            _fieldname = fieldName;
            _values = values;
            _filterType = filterType;
            _isUse = isUse;
            _valuestype = type;
            _isSafe = isSafe;
        }
        #region 公用方法

        #endregion
    }
    /// <summary>
    /// 查询字段集合
    /// </summary>
    [Serializable]
    public class QFieldCollection : List<QField>
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #region 构造方法
        public QFieldCollection()
        {

        }
        public QFieldCollection(string name)
        {
            _name = name;
        }
        public QFieldCollection(IEnumerable<QField> fields)
            : base(fields)
        {

        }
        public QFieldCollection(string name, IEnumerable<QField> fields)
            : base(fields)
        {
            _name = name;
        }
        #endregion

        #region 新增方法
        public void Add(string fieldName)
        {
            base.Add(new QField(fieldName));
        }
        public void Add(string fieldName, Type type)
        {
            base.Add(new QField(fieldName, type));
        }
        public void Add(string fieldName, object[] values, Type type, eFilterType filterType)
        {
            base.Add(new QField(fieldName, values, type, filterType, true));
        }
        public void Add(string fieldName, object[] values, Type type, eFilterType filterType, bool isUse)
        {
            base.Add(new QField(fieldName, values, type, filterType, isUse));
        }
        public void Add(string fieldName, object[] values, Type type, eFilterType filterType, bool isUse, bool isSafe, string desc)
        {
            base.Add(new QField(fieldName, values, type, filterType, isUse, isSafe, desc));
        }
        public void Add(QFieldCollection QFS)
        {
            foreach (var qf in QFS)
            {
                Add(qf);
            }
        }
        #endregion

        public QField this[string FieldName]
        {
            get
            {
                foreach (var thefield in this)
                {
                    if (thefield.FieldName == FieldName)
                        return thefield;
                }
                return null;
            }
        }
        ///// <summary>
        ///// 输出字段名数组
        ///// </summary>
        ///// <returns></returns>
        //public string[] ToArrayByName()
        //{
        //    string[] tmpstrs = new string[base.Count];
        //    for (int i = 0; i < base.Count; i++)
        //    {
        //        tmpstrs[i] = base[i].FieldName;
        //    }
        //    return tmpstrs;
        //}
        ///// <summary>
        ///// 输出字段字段和值的hashtable
        ///// </summary>
        ///// <returns></returns>
        //public Hashtable ToHashByValue()
        //{
        //    Hashtable hs = new Hashtable();
        //    foreach (QField f in this)
        //    {
        //        hs.Add(f.FieldName, f.Values);
        //    }
        //    return hs;
        //}
        /// <summary>
        /// 输出以主键为条件的where语句(不包含where关键字)
        /// </summary>
        /// <returns></returns>
        public string ToWhereString()
        {
            var sb = new StringBuilder("");
            foreach (var f in this)
            {
                if (f.IsUse)
                {
                    if (f.Values[0].ToString().ToUpper() == "<NULL>")
                    {
                        f.FilterType = eFilterType.IsNull;
                    }
                    else if (f.Values[0].ToString().ToUpper() == "<!NULL>")
                    {
                        f.FilterType = eFilterType.IsNotNull;
                    }
                    switch (f.FilterType)
                    {
                        case (eFilterType.Like):
                            if (f.Values[0].ToString().ToUpper() == "<NULL>")
                            {
                                goto case eFilterType.IsNull;
                            }
                            sb.Append(string.Format(" and {0} like '%{1}%'",
                                f.FieldName,
                                f.IsSafe ? f.Values[0].ToString() : FunBase.ChkField(f.Values[0].ToString())));
                            break;
                        case (eFilterType.Same):
                            if (f.Values[0].ToString().ToUpper() == "<NULL>")
                            {
                                goto case eFilterType.IsNull;
                            }
                            sb.Append(string.Format(" and {0}={1}",
                                f.FieldName,
                                f.IsSafe ? f.Values[0].ToString() : FunBase.ChkField(f.Values[0].ToString(), f.ValuesType == typeof(string))));
                            break;
                        case (eFilterType.Than):
                            sb.Append(string.Format(" and {0}>{1}",
                                f.FieldName,
                                f.IsSafe ? f.Values[0].ToString() : FunBase.ChkField(f.Values[0].ToString(), f.ValuesType == typeof(string))));
                            break;
                        case (eFilterType.Than1):
                            if (f.ValuesType == typeof(DateTime))
                            {
                                var sT1 = string.Format("To_Date('{0}','{1}')",
                                             ((DateTime)f.Values[0]).ToString("yyyy-MM-dd HH:mm:ss"),
                                             "YYYY-MM-DD HH24:MI:SS");
                                sb.Append(string.Format(" and {0}>={1}", f.FieldName, sT1));
                            }
                            else
                            {
                                sb.Append(string.Format(" and {0}>={1}",
                                    f.FieldName,
                                    f.IsSafe ? f.Values[0].ToString() : FunBase.ChkField(f.Values[0].ToString(), f.ValuesType == typeof(string))));
                            }
                            break;
                        case (eFilterType.LessThan):
                            sb.Append(string.Format(" and {0}<{1}",
                                f.FieldName,
                                f.IsSafe ? f.Values[0].ToString() : FunBase.ChkField(f.Values[0].ToString(), f.ValuesType == typeof(string))));
                            break;
                        case (eFilterType.LessThan1):
                            if (f.ValuesType == typeof(DateTime))
                            {
                                var sT1 = string.Format("To_Date('{0}','{1}')",
                                             ((DateTime)f.Values[0]).ToString("yyyy-MM-dd HH:mm:ss"),
                                             "YYYY-MM-DD HH24:MI:SS");
                                sb.Append(string.Format(" and {0}<={1}", f.FieldName, sT1));
                            }
                            else
                            {
                                sb.Append(string.Format(" and {0}<={1}",
                                    f.FieldName,
                                    f.IsSafe ? f.Values[0].ToString() : FunBase.ChkField(f.Values[0].ToString(), f.ValuesType == typeof(string))));
                            }
                            break;
                        case (eFilterType.Between):
                            if (f.Values.Length < 2) throw new Exception("WXG502|Between时QField.Values.Length必须大于等于2");
                            if (f.ValuesType == typeof(DateTime))
                            {
                                string sT1, sT2;
                                sT1 = string.Format("To_Date('{0}','{1}')",
                                         ((DateTime)f.Values[0]).ToString("yyyy-MM-dd HH:mm:ss"),
                                         "YYYY-MM-DD HH24:MI:SS");
                                sT2 = string.Format("To_Date('{0}','{1}')",
                                         ((DateTime)f.Values[1]).ToString("yyyy-MM-dd HH:mm:ss"),
                                         "YYYY-MM-DD HH24:MI:SS");
                                sb.Append(string.Format(" and {0} Between {1} and {2}", f.FieldName, sT1, sT2));
                            }
                            else
                            {
                                sb.Append(string.Format(" and {0} Between {1} and {2}",
                                    f.FieldName,
                                    f.IsSafe ? f.Values[0].ToString() : FunBase.ChkField(f.Values[0].ToString(), f.ValuesType == typeof(string)),
                                    f.IsSafe ? f.Values[1].ToString() : FunBase.ChkField(f.Values[1].ToString(), f.ValuesType == typeof(string))));
                            }
                            break;
                        case (eFilterType.FromTo):
                            if (f.Values[0].ToString().Length > 0)
                            {
                                sb.Append(string.Format(" and {0}>{1}",
                                    f.FieldName,
                                    f.IsSafe ? f.Values[0].ToString() : FunBase.ChkField(f.Values[0].ToString(), f.ValuesType == typeof(string))));
                            }
                            if (f.Values[1].ToString().Length > 0)
                            {
                                sb.Append(string.Format(" and {0}<{1}",
                                    f.FieldName,
                                    f.IsSafe ? f.Values[0].ToString() : FunBase.ChkField(f.Values[1].ToString(), f.ValuesType == typeof(string))));
                            }
                            break;
                        case (eFilterType.Include):
                            sb.Append(string.Format(" and {0} in ({1})",
                                f.FieldName,
                                FunBase.BuildSql_In(f.Values, false, f.ValuesType == typeof(string))));
                            break;
                        case (eFilterType.UnInclude):
                            sb.Append(string.Format(" and {0} not in ({1})",
                                f.FieldName,
                                FunBase.BuildSql_In(f.Values, false, f.ValuesType == typeof(string))));
                            break;
                        case eFilterType.IsNull:
                            sb.Append(string.Format(" and {0} is Null", f.FieldName));
                            break;
                        case eFilterType.IsNotNull:
                            sb.Append(string.Format(" and {0} is not Null", f.FieldName));
                            break;
                        case eFilterType.SubSQL:
                            if (f.Values[0].ToString().Length > 0)
                                sb.Append(string.Format(" and ({0})", f.Values[0].ToString()));
                            break;
                    }
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 设置所有字段的可用性
        /// </summary>
        /// <param name="isUse">是否可用</param>
        public void SetAllFeildsIsUse(bool isUse)
        {
            foreach (var thefield in this)
            {
                thefield.IsUse = isUse;
            }
        }
        public void ClearAllFieldsValues()
        {
            foreach (var thefield in this)
            {
                thefield.Values = null;
                thefield.IsUse = true;
            }
        }
    }
    #endregion
}

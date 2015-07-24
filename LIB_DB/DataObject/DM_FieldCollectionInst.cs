using System;
using LIB_Common;

namespace LIB_DB.DataObject
{
    public class DM_FieldCollectionInst : FieldDataMode
    {
        public DM_FieldCollectionInst()
        {
            _dFields = SysBusinesObjectManager.GetBusPropertySchemaByClassID(this.GetType().Name.ToString());
        }
        public String FCGUID
        {
            get { return _dFields["FCGUID"].GetValueAsString(); }
            set { _dFields["FCGUID"].Value = value; }
        }


        public String FIELDSTRUCTTYPE
        {
            get { return _dFields["FIELDSTRUCTTYPE"].GetValueAsString(); }
            set { _dFields["FIELDSTRUCTTYPE"].Value = value; }
        }


        public Decimal PARENTTYPE
        {
            get { return _dFields["PARENTTYPE"].GetValueAsDecimal(); }
            set { _dFields["PARENTTYPE"].Value = value; }
        }


        public String PARENTFCID
        {
            get { return _dFields["PARENTFCID"].GetValueAsString(); }
            set { _dFields["PARENTFCID"].Value = value; }
        }


        public String WDOGUID
        {
            get { return _dFields["WDOGUID"].GetValueAsString(); }
            set { _dFields["WDOGUID"].Value = value; }
        }


        public String FIELDSSERIALSTRING
        {
            get { return _dFields["FIELDSSERIALSTRING"].GetValueAsString(); }
            set { _dFields["FIELDSSERIALSTRING"].Value = value; }
        }


        public DateTime CREATEON
        {
            get { return _dFields["CREATEON"].GetValueAsDateTime(); }
            set { _dFields["CREATEON"].Value = value; }
        }

    }
}

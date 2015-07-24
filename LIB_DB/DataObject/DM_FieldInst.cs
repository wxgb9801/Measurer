using System;
using LIB_Common;

namespace LIB_DB.DataObject
{
    public class DM_FieldInst : FieldDataMode
    {
        public DM_FieldInst()
        {
            _dFields = SysBusinesObjectManager.GetBusPropertySchemaByClassID(this.GetType().Name);
        }

        public String FIELDGUID
        {
            get { return _dFields["FIELDGUID"].GetValueAsString(); }
            set { _dFields["FIELDGUID"].Value = value; }
        }


        public String FCGUID
        {
            get { return _dFields["FCGUID"].GetValueAsString(); }
            set { _dFields["FCGUID"].Value = value; }
        }


        public Decimal FIELDINDEX
        {
            get { return _dFields["FIELDINDEX"].GetValueAsDecimal(); }
            set { _dFields["FIELDINDEX"].Value = value; }
        }


        public String FIELDSERIALSTRING1
        {
            get { return _dFields["FIELDSERIALSTRING1"].GetValueAsString(); }
            set { _dFields["FIELDSERIALSTRING1"].Value = value; }
        }


        public String FIELDSERIALSTRING2
        {
            get { return _dFields["FIELDSERIALSTRING2"].GetValueAsString(); }
            set { _dFields["FIELDSERIALSTRING2"].Value = value; }
        }


        public DateTime CREATEON
        {
            get { return _dFields["CREATEON"].GetValueAsDateTime(); }
            set { _dFields["CREATEON"].Value = value; }
        }

    }
}

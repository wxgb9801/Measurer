using System;
using LIB_Common;

namespace LIB_DB.DataObject
{
    public enum eWAPDataObjectStatus
    {
        Created=0,
        Used=1,
        Expired=2
    }
    public class DM_WAPDataObjectInst : FieldDataMode
    {
        public DM_WAPDataObjectInst()
        {
            base._dFields = SysBusinesObjectManager.GetBusPropertySchemaByClassID(this.GetType().Name);
        }
        public String WDOGUID
        {
            get { return _dFields["WDOGUID"].GetValueAsString(); }
            set { _dFields["WDOGUID"].Value = value; }
        }
        public String WDONAME
        {
            get { return _dFields["WDONAME"].GetValueAsString(); }
            set { _dFields["WDONAME"].Value = value; }
        }


        public Decimal ITEMLENGTH
        {
            get { return _dFields["ITEMLENGTH"].GetValueAsDecimal(); }
            set { _dFields["ITEMLENGTH"].Value = value; }
        }


        public Decimal PACKSIZE
        {
            get { return _dFields["PACKSIZE"].GetValueAsDecimal(); }
            set { _dFields["PACKSIZE"].Value = value; }
        }


        public DateTime CREATEON
        {
            get { return _dFields["CREATEON"].GetValueAsDateTime(); }
            set { _dFields["CREATEON"].Value = value; }
        }


        public String CREATEBY
        {
            get { return _dFields["CREATEBY"].GetValueAsString(); }
            set { _dFields["CREATEBY"].Value = value; }
        }

        public eWAPDataObjectStatus STATUS
        {
            get { return (eWAPDataObjectStatus)_dFields["STATUS"].GetValueAsInt(); }
            set { _dFields["STATUS"].Value = (int)value; }
        }
    }
}

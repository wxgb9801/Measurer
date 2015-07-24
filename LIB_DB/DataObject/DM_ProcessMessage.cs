using System;
using LIB_Common;

namespace LIB_DB.DataObject
{
    public class DM_ProcessMessage : FieldDataMode
    {
        public DM_ProcessMessage()
        {
            _dFields = SysBusinesObjectManager.GetBusPropertySchemaByClassID(this.GetType().Name);
        }

        public Decimal ID
        {
            get { return _dFields["ID"].GetValueAsDecimal(); }
            set { _dFields["ID"].Value = value; }
        }


        public String MESSAGEID
        {
            get { return _dFields["MESSAGEID"].GetValueAsString(); }
            set { _dFields["MESSAGEID"].Value = value; }
        }


        public String PROCCESSID
        {
            get { return _dFields["PROCCESSID"].GetValueAsString(); }
            set { _dFields["PROCCESSID"].Value = value; }
        }


        public String PROCESSTYPE
        {
            get { return _dFields["PROCESSTYPE"].GetValueAsString(); }
            set { _dFields["PROCESSTYPE"].Value = value; }
        }


        public String SOURE
        {
            get { return _dFields["SOURE"].GetValueAsString(); }
            set { _dFields["SOURE"].Value = value; }
        }


        public String DESTINATION
        {
            get { return _dFields["DESTINATION"].GetValueAsString(); }
            set { _dFields["DESTINATION"].Value = value; }
        }


        public int PRI
        {
            get { return _dFields["PRI"].GetValueAsInt(); }
            set { _dFields["PRI"].Value = value; }
        }


        public String PROCESSVEXTEXID
        {
            get { return _dFields["PROCESSVEXTEXID"].GetValueAsString(); }
            set { _dFields["PROCESSVEXTEXID"].Value = value; }
        }


        public String ACTIVITYCONTRACT
        {
            get { return _dFields["ACTIVITYCONTRACT"].GetValueAsString(); }
            set { _dFields["ACTIVITYCONTRACT"].Value = value; }
        }


        public String DATAOBJCTID
        {
            get { return _dFields["DATAOBJCTID"].GetValueAsString(); }
            set { _dFields["DATAOBJCTID"].Value = value; }
        }


        public int STATUS
        {
            get { return _dFields["STATUS"].GetValueAsInt(); }
            set { _dFields["STATUS"].Value = value; }
        }


        public DateTime SAVEON
        {
            get { return _dFields["SAVEON"].GetValueAsDateTime(); }
            set { _dFields["SAVEON"].Value = value; }
        }


        public String SAVEBY
        {
            get { return _dFields["SAVEBY"].GetValueAsString(); }
            set { _dFields["SAVEBY"].Value = value; }
        }


        public int LOADTIMES
        {
            get { return _dFields["LOADTIMES"].GetValueAsInt(); }
            set { _dFields["LOADTIMES"].Value = value; }
        }


        public string PROCESSREQUESTID
        {
            get { return _dFields["PROCESSREQUESTID"].GetValueAsString(); }
            set { _dFields["PROCESSREQUESTID"].Value = value; }
        }


    }
}

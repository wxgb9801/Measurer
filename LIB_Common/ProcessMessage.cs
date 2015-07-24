using System;
using System.Text;

namespace LIB_Common
{
    public enum eProcessMsgStatus
    {
        Created = 0,
        Processing = 1,
        Done = 2,
        ProcessFail = 3
    }
    [Serializable]
    public class ProcessMessage:ICloneable,ICustomerSerializable
    {
        private eProcessMsgStatus _status;

        private ActivityContract activityContract;

        private int priority;

        public ProcessMessage()
        {
            DataObject = new WAPDataObject();
        }

        public ActivityContract ActivityContract
        {
            get
            {
                if ((activityContract == null || activityContract.ActivityName != ActivityContractString)
                    && (ActivityContractString != null || ActivityContractString != string.Empty))
                {
                    activityContract = WAPActivityManager.GetActivityContract(ActivityContractString);
                }
                return activityContract;
            }
        }

        //private List<FieldTable> dataObject;
        //public List<FieldTable> DataObject
        //{
        //    get { return dataObject; }
        //    set { dataObject = value; }
        //}
        public string ActivityContractString { get; set; }

        public WAPDataObject DataObject { get; set; }

        public string Destination { get; set; }

        public string GUID { get; set; }

        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        public string ProcessID { get; set; }

        public string ProcessRequestID { get; set; }

        public string ProcessType { get; set; }

        //@0.0.2
        public string ProcessVertexID { get; set; }

        public string Source { get; set; }
        public eProcessMsgStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }
        public static ProcessMessage BuildProcessMsg(int priority, string source, string destination, string activityUFOID, WAPDataObject dataObject)
        {
            var processMessage = new ProcessMessage
            {
                GUID = Guid.NewGuid().ToString(),
                Priority = priority,
                Source = source,
                Destination = destination,
                ActivityContractString = activityUFOID,
                DataObject = dataObject,
                Status = eProcessMsgStatus.Created
            };

            return processMessage;
        }

        public static ProcessMessage BuildProcessMsg(string processType, string processRequestID, int priority, string source, string destination, string activityUFOID, WAPDataObject dataObject)
        {
            var processMessage = new ProcessMessage
            {
                GUID = Guid.NewGuid().ToString(),
                ProcessRequestID = processRequestID,
                ProcessType = processType,
                Priority = priority,
                Source = source,
                Destination = destination,
                ActivityContractString = activityUFOID,
                DataObject = dataObject,
                Status = eProcessMsgStatus.Created
            };

            return processMessage;
        }

        public static ProcessMessage BuildProcessMsg(int priority, string source, string destination, string activityUFOID, string processID, string processType, WAPDataObject dataObject)
        {
            var processMessage = new ProcessMessage
            {
                GUID = Guid.NewGuid().ToString(),
                Priority = priority,
                Source = source,
                Destination = destination,
                ProcessID = processID,
                ProcessType = processType,
                ActivityContractString = activityUFOID,
                DataObject = dataObject,
                Status = eProcessMsgStatus.Created
            };

            return processMessage;
        }

        public static ProcessMessage BuildProcessMsgByFC(int priority, string source, string destination, string activityUFOID, FieldCollection fieldCollection)
        {
            var processMessage = new ProcessMessage
            {
                GUID = Guid.NewGuid().ToString(),
                Priority = priority,
                Source = source,
                Destination = destination,
                ActivityContractString = activityUFOID
            };

            var wdo = new WAPDataObject { fieldCollection };

            processMessage.DataObject = wdo;
            processMessage.Status = eProcessMsgStatus.Created;
            return processMessage;
        }

        public static ProcessMessage BuildProcessMsgByFC(int priority, string source, string destination, string processID, string processType, string activityUFOID, FieldCollection fieldCollection)
        {
            var processMessage = new ProcessMessage
            {
                GUID = Guid.NewGuid().ToString(),
                Priority = priority,
                Source = source,
                Destination = destination,
                ProcessID = processID,
                ProcessType = processType,
                ActivityContractString = activityUFOID,
                Status = eProcessMsgStatus.Created
            };

            var wdo = new WAPDataObject { fieldCollection };

            processMessage.DataObject = wdo;

            return processMessage;
        }

        public static ProcessMessage BuildProcessMsgByFC(string source, string destination, string processID, string processType, string activityUFOID, FieldCollection fieldCollection)
        {
            var processMessage = new ProcessMessage
            {
                GUID = Guid.NewGuid().ToString(),
                Priority = 5,
                Source = source,
                Destination = destination,
                ProcessID = processID,
                ProcessType = processType,
                ActivityContractString = activityUFOID
            };

            var wdo = new WAPDataObject { fieldCollection };

            processMessage.DataObject = wdo;
            processMessage.Status = eProcessMsgStatus.Created;

            return processMessage;
        }

        object ICloneable.Clone()
        {
            var pm = (ProcessMessage)MemberwiseClone();
            pm.DataObject = (WAPDataObject)((ICloneable)pm.DataObject).Clone();
            return pm;
        }

        void ICustomerSerializable.LoadObjectFromSerializableString(eCustomerSerializableType type, string serializableString)
        {
            var msg = serializableString.Split(new[] { CustomerSerializableProvider.ProcessMessageSplitString }, StringSplitOptions.RemoveEmptyEntries);
            GUID = msg[0];
            ProcessID = msg[1];
            ProcessType = msg[2];
            Source = msg[3];
            Destination = msg[4];
            int.TryParse(msg[5], out priority);
            ProcessVertexID = msg[6];
            ActivityContractString = msg[7];
            Enum.TryParse(msg[8], out _status);
            if (msg.Length > 9)
            {
                var dataObj = msg[9];
                var wdo = new WAPDataObject();
                ((ICustomerSerializable)wdo).LoadObjectFromSerializableString(type, dataObj);
                DataObject = wdo;
            }
        }

        string ICustomerSerializable.ToSerializableString(eCustomerSerializableType type)
        {
            if (type == eCustomerSerializableType.FullStruct)
            {
                throw new NotImplementedException();
            }
            var sb = new StringBuilder(GUID);
            sb.Append(CustomerSerializableProvider.ProcessMessageSplitString);
            sb.Append(CustomerSerializableProvider.ChkString_ProcessAttribute(ProcessID));
            sb.Append(CustomerSerializableProvider.ProcessMessageSplitString);
            sb.Append(CustomerSerializableProvider.ChkString_ProcessAttribute(ProcessType));
            sb.Append(CustomerSerializableProvider.ProcessMessageSplitString);
            sb.Append(CustomerSerializableProvider.ChkString_ProcessAttribute(Source));
            sb.Append(CustomerSerializableProvider.ProcessMessageSplitString);
            sb.Append(CustomerSerializableProvider.ChkString_ProcessAttribute(Destination));
            sb.Append(CustomerSerializableProvider.ProcessMessageSplitString);
            sb.Append(priority.ToString());
            sb.Append(CustomerSerializableProvider.ProcessMessageSplitString);
            sb.Append(CustomerSerializableProvider.ChkString_ProcessAttribute(ProcessVertexID));
            sb.Append(CustomerSerializableProvider.ProcessMessageSplitString);
            sb.Append(ActivityContractString);
            sb.Append(CustomerSerializableProvider.ProcessMessageSplitString);
            sb.Append(((int)_status).ToString());
            if (DataObject != null)
            {
                sb.Append(CustomerSerializableProvider.ProcessMessageSplitString);
                ((ICustomerSerializable)DataObject).ToSerializableString(type);
            }
            return sb.ToString();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(string.Format("[GUID:{0}]", GUID));
            sb.Append(string.Format("[Source:{0}]", Source));
            sb.Append(string.Format("[Destination:{0}]", Destination));
            sb.Append(string.Format("[ProcessType:{0}]", ProcessType));
            sb.Append(string.Format("[Priority:{0}]", priority.ToString()));
            //sb.Append(string.Format("[ActivityContract:{0}]", activityContract));
            if (activityContract != null)
            {
                sb.Append(string.Format("[MethodName:{0}]", activityContract.MethodName));
                sb.Append(string.Format("[ActivityName:{0}]", activityContract.ActivityName));
            }
             return sb.ToString ();
        }
        //public static ProcessMessage BuildProcessMsgByFCTable(string source, string destination, string processID, string processType, string activityUFOID, FieldTable fieldTable)
        //{
        //    ProcessMessage processMessage = new ProcessMessage();
        //    processMessage.GUID = Guid.NewGuid().ToString();
        //    processMessage.Priority = 5;
        //    processMessage.Source = source;
        //    processMessage.Destination = destination;
        //    processMessage.ProcessID = processID;
        //    processMessage.ProcessType = processType;

        //    processMessage.ActivityContract = activityUFOID;

        //    List<FieldTable> fieldTableList = new List<FieldTable>();
        //    fieldTableList.Add(fieldTable);

        //    processMessage.DataObject = fieldTableList;

        //    return processMessage;
        //}
    } 
    
}

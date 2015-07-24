using System;
using System.Collections.Generic;

namespace LIB_Common
{
    [Serializable]
    public class ActivityContract
    {
        public ActivityContract(string ufoid)
        {
            _ufoid = ufoid;
            InputDataTypeList = new List<string>();
            OutputDataTypeList = new List<string>();
        }       


        private string _ufoid;

        public string UFOID
        {
            get { return _ufoid; }
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ActivityName { get; set; }

        public string MethodName { get; set; }

        public string OperatorName { get; set; }

        public ExecutionMode ExecutionMode { get; set; }


        public Dictionary<string, string> Events = new Dictionary<string, string>();

        public void AddEvent(string key, string value)
        {
            Events.Add(key, value);
        }

        public bool RemoveEvent(string key)
        {
            return Events.Remove(key);
        }

        public string GetEvent(string key)
        {
            if (Events != null && Events.ContainsKey(key))
            {
                return Events[key];
            }
            else
            {
                return "";
            }
        }               

        private List<string> InputDataTypeList;
        private List<string> OutputDataTypeList;  

        public void AddInputDataType(string datatype)
        {
            InputDataTypeList.Add(datatype);
        }

        public bool RemoveInputDataType(string datatype)
        {
            return InputDataTypeList.Remove(datatype);
        }

        public void AddOutputDataType(string datatype)
        {
            OutputDataTypeList.Add(datatype);
        }

        public bool RemoveOutputDataType(string datatype)
        {
            return OutputDataTypeList.Remove(datatype);
        }

        public List<string> GetInputDataType()
        {
            if (InputDataTypeList != null && InputDataTypeList.Count > 0)
                return InputDataTypeList;
            else
                return null;
        }

        public List<string> GetOutputDataType()
        {
            if (OutputDataTypeList != null && OutputDataTypeList.Count > 0)
                return OutputDataTypeList;
            else
                return null;
        }
    }

    public enum ExecutionMode
    {
        Syncronize,
        Asynchronize
    }
}

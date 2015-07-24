using System.Collections.Generic;
using System.Reflection;

namespace LIB_Common
{
    public static class WAPActivityManager
    {
        private static Dictionary<string, object> operatorDict = new Dictionary<string, object>();
        private static SmartRWLocker smartRWLockerForOperator = new SmartRWLocker();

        private static Dictionary<string, ActivityContract> activityContractDict = new Dictionary<string, ActivityContract>();
        private static SmartRWLocker smartRWLockerForActContract = new SmartRWLocker();

        public static void RegisterOperator(string operatorName, object instance)
        {
            using (smartRWLockerForOperator.Lock(AccessMode.Write))
            {
                if (!operatorDict.ContainsKey(operatorName))
                {
                    operatorDict.Add(operatorName, instance);
                }
                else
                {
                    operatorDict[operatorName] = instance;
                }
            }
        }

        public static bool UnregisterOperator(string operatorName)
        {
            using (smartRWLockerForOperator.Lock(AccessMode.Write))
            {
                if (operatorDict.ContainsKey(operatorName))
                {
                    return operatorDict.Remove(operatorName);
                }
                else
                    return false;
            }
        }

        public static object GetOperator(string operatorName)
        {
            using (smartRWLockerForOperator.Lock(AccessMode.Write))
            {
                if (operatorDict.ContainsKey(operatorName))
                {
                    return operatorDict[operatorName];
                }
                else
                    return null;
            }
        }

        public static bool IsExistOperator(string operatorName)
        {
            using (smartRWLockerForOperator.Lock(AccessMode.Write))
            {
                if (operatorDict.ContainsKey(operatorName))
                {
                    return true;
                }
                else
                    return false;
            }
        }

        //public static void ExecuteActivity(string operatorName, string eventname, object[] param)
        //{
        //    Object _instance = null;

        //    if (operatorDict.ContainsKey(operatorName))
        //    {
        //        operatorDict.TryGetValue(operatorName, out _instance);
        //    }

        //    if (_instance != null)
        //    {
        //        Type t = _instance.GetType();
        //        MethodInfo miHandler = t.GetMethod(eventname, BindingFlags.Public|BindingFlags.NonPublic | BindingFlags.Instance);
        //        if ((miHandler != null) && (miHandler.GetParameters().Length == param.Length))
        //        {
        //            miHandler.Invoke(_instance, param);
        //        }
        //    }
        //}

        public static object ExecuteActivity(string operatorName, string eventname, object[] param)
        {
            object _instance = null;
            object rtnObjct = null;
            if (operatorDict.ContainsKey(operatorName))
            {
                operatorDict.TryGetValue(operatorName, out _instance);
            }

            if (_instance != null)
            {
                var t = _instance.GetType();
                var miHandler = t.GetMethod(eventname, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if ((miHandler != null) && (miHandler.GetParameters().Length == param.Length))
                {
                    rtnObjct = miHandler.Invoke(_instance, param);
                }
            }

            return rtnObjct;
        }

        public static void RegisterActivityContract(string contractName, ActivityContract activityContract)
        {
            using (smartRWLockerForActContract.Lock(AccessMode.Write))
            {
                if (!activityContractDict.ContainsKey(contractName))
                {
                    activityContractDict.Add(contractName, activityContract);
                }
                else
                {
                    activityContractDict[contractName] = activityContract;
                }
            }
        }

        public static bool UnregisterActivityContract(string contractName)
        {
            using (smartRWLockerForActContract.Lock(AccessMode.Write))
            {
                if (activityContractDict.ContainsKey(contractName))
                {
                    return activityContractDict.Remove(contractName);
                }
                else
                {
                    return false;
                }
            }
        }

        public static ActivityContract GetActivityContract(string contractName)
        {
            using (smartRWLockerForActContract.Lock(AccessMode.Write))
            {
                if (activityContractDict.ContainsKey(contractName))
                {
                    return activityContractDict[contractName];
                }
                else
                {
                    return null;
                }
            }
        }

        public static bool IsExitActivityContract(string contractName)
        {
            using (smartRWLockerForActContract.Lock(AccessMode.Write))
            {
                if (activityContractDict.ContainsKey(contractName))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static object WAPActivityExecute(int ipriority, string sourceName, string activityName, WAPDataObject wapMessage)
        {
            var activityContract = GetActivityContract(activityName);
            if (activityContract != null)
            {
                var processMessage = ProcessMessage.BuildProcessMsg(ipriority, sourceName, activityContract.OperatorName,
                    activityName, wapMessage);

                var objPara = new object[1];

                var receiver = activityContract.OperatorName;
                var methodName = activityContract.MethodName;

                objPara[0] = processMessage;
                return ExecuteActivity(receiver, methodName, objPara);
            }
            else
            {
                return null;
            }
        }

        public static object WAPActivityExecute(int ipriority, string sourceName, string activityName, FieldCollection fc)
        {
            var wapdataObject = new WAPDataObject { fc };
            return WAPActivityExecute(5, sourceName, activityName, wapdataObject);
        }
    }
}

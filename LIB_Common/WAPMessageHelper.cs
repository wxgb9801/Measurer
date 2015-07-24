using System;
using System.Collections.Generic;
using System.Linq;

namespace LIB_Common
{
    public static class WAPMessageHelper
    {
        private static UInt32 DefaultMessageID;
        private static readonly Dictionary<UInt32, FieldCollection> MessageDict = new Dictionary<uint,FieldCollection>();

        public static void SetDefaultMessageID(UInt32 newID)
        {
            DefaultMessageID = newID;
        }

        public static UInt32 GetWAPMessageID(FieldCollection fc)
        {
            if(fc!=null)
            {
                var fcFormat = new FieldCollectionFormat();
                if(!fcFormat.IsEmptyStrField(fc,"MessageID"))
                    return (UInt32)fc["MessageID"].GetValueAsInt();
            }
            return 0;
        }

        public static Dictionary<UInt32, FieldCollection> GetWAPMessageList(string ModuleName)
        {
            if (MessageDict != null && MessageDict.Count > 0)
            {
                var msgDict = MessageDict.Values.Where(e => e["ModuleName"].GetValueAsString() == ModuleName).ToDictionary(e => e["MessageID"].GetValueAsUInt());
                return msgDict;
            }
            else
                return null;
        }

        public static void RegisterWAPMessage(FieldCollection fc)
        {
            if (fc == null)
                return;
            if(MessageDict.ContainsKey(GetWAPMessageID(fc)))
                return;
            MessageDict.Add(GetWAPMessageID(fc), fc);
        }

        public static FieldCollection GetWAPMessage(UInt32 messageID)
        {
            if (MessageDict != null && MessageDict.Count > 0)
            {
                if (MessageDict.ContainsKey(messageID))
                {
                    return MessageDict[messageID];
                }
                else
                {
                    return MessageDict[DefaultMessageID];
                }
            }
            else
                return null;
            
           
            
        }
    }
}

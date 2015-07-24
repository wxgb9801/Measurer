namespace LIB_Common
{
    public enum eCustomerSerializableType
    {
        NoStruct,
        FullStruct
    }
    public interface ICustomerSerializable
    {
        string ToSerializableString(eCustomerSerializableType type);
        void LoadObjectFromSerializableString(eCustomerSerializableType type,string serializableString);
    }
    public class CustomerSerializableProvider
    {
        public const string ProcessMessageSplitString = "|PM|";

        public const string TableHeadBodySplitString = "|HT|";
        public const string FCHeadBodySplitString = "|HF|";

        public const string WAPObjSplitSstring = "|W|";
        public const string TableSplitString = "|T|";
        public const string CollectionSplitString = "|F|";
        public const string FieldChildSplitString = "|C|";
        public const string FieldSplitString = "|V|";



        protected const string safeProcessMessageSplitString = "| PM |";
        protected const string safeTableHeadBodySplitString = "| HT |";
        protected const string safeFCHeadBodySplitString = "| HF |";
        protected const string safeWAPObjSplitSstring = "| W |";
        protected const string safeTableSplitString = "| T |";
        protected const string safeCollectionSplitString = "| F |";
        protected const string safeFieldChildSplitString = "| C |";
        protected const string safeFieldSplitString = "| V |";

        public static string ChkStringAll(string sourceString)
        {
            string descString = sourceString.Replace(ProcessMessageSplitString, safeProcessMessageSplitString)
                .Replace(TableHeadBodySplitString, safeTableHeadBodySplitString)
                .Replace(FCHeadBodySplitString, safeFCHeadBodySplitString)
                .Replace(WAPObjSplitSstring, safeWAPObjSplitSstring)
                .Replace(TableSplitString, safeTableSplitString)
                .Replace(CollectionSplitString, safeCollectionSplitString)
                .Replace(FieldChildSplitString, safeFieldChildSplitString)
                .Replace(FieldSplitString, safeFieldSplitString);
            return descString;
        }

        public static string ChkString_FieldValue(string sourceString)
        {
            string descString = sourceString.Replace(FieldSplitString, safeFieldSplitString);
            return descString;
        }

        public static string ChkString_ProcessAttribute(string sourceString)
        {
            string descString = sourceString.Replace(ProcessMessageSplitString, safeProcessMessageSplitString);
            return descString;
        }

        public static string ChkString_FieldCollectionName(string sourceString)
        {
            string descString = sourceString.Replace(FCHeadBodySplitString, safeFCHeadBodySplitString);
            return descString;
        }

        public static string ChkString_FieldTableName(string sourceString)
        {
            string descString = sourceString.Replace(TableHeadBodySplitString, safeTableHeadBodySplitString);
            return descString;
        }
    }
}

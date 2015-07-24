using System;
using System.Collections.Generic;

namespace LIB_Common
{
    /// <summary>
    /// 可序列化字符串列表
    /// </summary>
    [Serializable]
    public class StringList : List<string>
    {
        public StringList()
        {
        }
        public StringList(IEnumerable<string> collect)
            : base(collect)
        {
        }
    }
}

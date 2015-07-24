namespace LIB_Common
{
    /// <summary>
    /// 通用枚举静态类
    /// </summary>
    public static class CommonEnum
    {
        /// <summary>
        /// 功能权限操作值【低位为1,从右向左】
        /// </summary>
        public enum FunPrivilegeEnum
        {
            /// <summary>
            /// 可见
            /// </summary>
            Visible = 1,
            /// <summary>
            /// 可用
            /// </summary>
            Enable = 2,
            /// <summary>
            /// 可编辑
            /// </summary>
            Editable =3,
            /// <summary>
            /// 可执行
            /// </summary>
            Executable=4,
            /// <summary>
            /// 可移动
            /// </summary>
            Removable=5,
            /// <summary>
            /// 可设计
            /// </summary>
            Designable=6,
            /// <summary>
            /// 可输入
            /// </summary>
            Inputable=7,
            /// <summary>
            /// 可复制
            /// </summary>
            Copy=8,
            /// <summary>
            /// 可打印
            /// </summary>
            Print=9,

            /// <summary>
            /// 预留
            /// </summary>
            _Reserved00 = 10,
            _Reserved01 = 11,
            _Reserved02 = 12,
            _Reserved03 = 13,
            _Reserved04 = 14,
            _Reserved05 = 15,
            _Reserved06 = 16,
            _Reserved07 = 17,
            _Reserved08 = 18,
            _Reserved09 = 19,
            _Reserved10 = 20,
            _Reserved11 = 21,
            _Reserved12 = 22,
            _Reserved13 = 23,
            /// <summary>
            /// 业务
            /// </summary>
            Business = 24,
            /// <summary>
            /// 业务预留
            /// </summary>
            _Reserved15 = 25,
            _Reserved16 = 26,
            _Reserved17 = 27,
            _Reserved18 = 28,
            _Reserved19 = 29,
            _Reserved20 = 30,
            _Reserved21 = 31,
            _Reserved22 = 32
        }

        public enum TerminalType
        {
            PC = 1,
            RF = 2,
            Voice = 3,
            PTL = 4
        }

        public enum EncryptionType
        {
            Password = 1,
            FingerPrint = 2
        }

        public enum PasswordDegree
        {
            Login = 1,
            SpecialAccess = 2
        }

        public enum MessageType
        {
            Notification = 1,
            Command = 2,
        }

        
    }

    public enum QcStatus
    {
        QualityChecking = 2,
        QualityChecked = 1,
        UnQualityChecked = 0
    }

    public enum QualityStatus
    {
        WaitingQualified = 0,
        UnQualified = 1,
        Qualified = 2
    }
}

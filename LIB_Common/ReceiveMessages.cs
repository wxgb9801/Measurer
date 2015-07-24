using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;

namespace LIB_Common
{
    ///***************************************************************************//
    /// Function: 
    /// 定义与客户端消息交互的常量与方法
    ///*************************************************************************//
    ///    Item Name:   ReceiveMessages
    ///       Author:   Kord - Kord
    ///  Create Time:   2013/11/22 10:53:10
    /// Machine Name:   KORD-SPC
    ///***************************************************************************//
    [Serializable]
    public static class ReceiveMessages
    {
        #region Property
        /// <summary>
        /// 索引FieldCollection字段获取IP地址时,使用到的索引名称
        /// </summary>
        public const String IPADDRESS = "$IPADDRESS$";
        /// <summary>
        /// 索引FieldCollection字段获取用户代码时,使用到的索引名称
        /// </summary>
        public const String USERCODE = "$USERCODE$";
        #endregion

        #region Method
        public static Binding CreateTcpBinding()
        {
            const int setting = 64 * 1024 * 1024;
            var thirtyMinutes = new TimeSpan(0, 1, 0);
            // ReSharper disable once UseObjectOrCollectionInitializer
            var ntb = new NetTcpBinding(SecurityMode.None);
            ntb.CloseTimeout = thirtyMinutes;
            ntb.MaxBufferPoolSize = setting;
            ntb.MaxBufferSize = setting;
            ntb.MaxConnections = setting;
            ntb.MaxReceivedMessageSize = setting;
            ntb.Name = "WAP.Service.TcpBinding";
            ntb.Namespace = "http://DEMATIC.WAP.SERVICE.org";
            ntb.OpenTimeout = thirtyMinutes;
            ntb.PortSharingEnabled = true;
            ntb.ReceiveTimeout = thirtyMinutes;
            ntb.SendTimeout = thirtyMinutes;
            ntb.ReaderQuotas = new XmlDictionaryReaderQuotas { MaxStringContentLength = setting };
            return ntb;
        }
        //public static Boolean SendMessage(FieldCollection fields, CommonEnum.MessageType msgType)
        //{
        //    if (fields[IPADDRESS] == null && fields[USERCODE] == null)
        //    {
        //        throw new NullReferenceException("No access to the desired information data fields.");
        //        return false;
        //    };

        //    var binding = CreateTcpBinding();
        //    var address = new EndpointAddress("net.tcp://" + fields[IPADDRESS] + ":1361/WAP/WCFService/Client");
        //    var factory = new DuplexChannelFactory<IClientService>(new ClientServiceCallback(), binding, address);
        //    clientService = factory.CreateChannel();

        //    return false;
        //}
        #endregion
    }
}

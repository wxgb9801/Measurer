using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Linq;
namespace Commu_ClsLibrary
{
    namespace CommuPort
    {
        /// <summary>
        /// 定义输出调试信息代理
        /// </summary>
        /// <param name="Wstr"></param>
        /// <param name="Rstr"></param>
        /// <param name="time"></param>
        public delegate void PutDebugStEventHandler(string Wstr,string Rstr,TimeSpan time);

        /// <summary>
        /// 定义端口(Com)传输设置结构
        /// </summary>
        public struct PortInitInfo
        {
            public int          _BaudRate;
            public Parity       _Parity;
            public int          _DataBits;
            public StopBits     _StopBits;

            public PortInitInfo(int mBaudRate,Parity mParity, int mDataBits, StopBits mStopBits)
            {
                this._BaudRate    = mBaudRate;
                this._Parity      = mParity;
                this._DataBits    = mDataBits;
                this._StopBits    = mStopBits;
                
            }
        }
        /// <summary>
        /// 定义Com口通讯类
        /// </summary>
        public class ComPort:IDisposable
        {
            private string      _Readstr;
            private string      _endexpression;
            public string       Handle_Err              = "";
            public string       DebugReadExistingStr    = "";
            public bool         IsMessageFlag           = false;                    //是否读到缓冲区数据标志
            public bool         IsOpenOhterDebug        = true;                     //是否开启ThePort.ReadExisting()
            public SerialPort   ThePort;
            const int           TRYTIME = 50;
            public event PutDebugStEventHandler     PutDebug;                       //输出

            //private string[] V;

            public int RTimeOut
            {
                get
                {
                    return ThePort.ReadTimeout;
                }
                set
                {
                     ThePort.ReadTimeout=value;
                }
            }

            public int WTimeOut
            {
                get
                {
                    return ThePort.WriteTimeout;
                }
                set
                {
                    ThePort.WriteTimeout = value;
                }
            }

            public ComPort(string mPortName,PortInitInfo P)
            {

                ThePort = new SerialPort(mPortName, P._BaudRate, P._Parity, P._DataBits, P._StopBits);
                this.Open();
                ThePort.ReadTimeout     = 300;
                ThePort.WriteTimeout    = 100;
                ThePort.DataReceived    += new SerialDataReceivedEventHandler(ThePort_DataReceived);
                ThePort.ErrorReceived   += new SerialErrorReceivedEventHandler(ThePort_ErrorReceived);
            }

            void ThePort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
            {
                throw new Exception("The method or operation is not implemented." + e.EventType.ToString());
            }

            void ThePort_DataReceived(object sender, SerialDataReceivedEventArgs e)
            {
                //ReadTo_ByString();
            }

            private void ReadTo_ByString()
            {
                try
                {
                    _Readstr = "";
                    if (IsMessageFlag == false)
                    {
                        _Readstr = this.ThePort.ReadTo(_endexpression);
                    }
                    Handle_Err = "";
                }
                catch (System.TimeoutException ex)
                {
                    Handle_Err = "读取端口数据超时"+ex.Message;
                    throw new Exception(Handle_Err);
                }
                catch (System.InvalidOperationException ex)
                {
                    Handle_Err = "当前端口状态无效"+ ex.Message;
                    throw new Exception(Handle_Err);
                }
                catch (Exception ex)
                {
                    Handle_Err = ex.Message;
                    throw new Exception(Handle_Err);
                }
                finally
                {
                    IsMessageFlag = true;
                }
            }

            private void ReadExisting()
            {
                try
                {
                    if (IsMessageFlag == false)
                    {
                        _Readstr = this.ThePort.ReadExisting();
                    }
                }
                catch (System.InvalidOperationException ex)
                {
                    Handle_Err = "当前端口状态无效:" +ex.Message;
                }
                finally
                {
                    IsMessageFlag = true;
                }
            }
            #region //备用
            //private  void ReadAllByte()
            //{
            //    StringBuilder currentline = new StringBuilder();
            //    if (IsMessageFlag == false)
            //    {
            //        while (this.ThePort.BytesToRead > 0)
            //        {
            //            char ch = (char)this.ThePort.ReadByte();
            //            currentline.Append(ch);
            //        }
            //    }
            //    _Readstr = currentline.ToString();
            //    IsMessageFlag = true;
            //    currentline = new StringBuilder();
            //}
            #endregion
            public bool Open()
            {
                if (ThePort.IsOpen)
                {
                    return true;
                }
                else
                {
                    try
                    {
                        ThePort.Open();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Handle_Err = ex.Message;
                        throw new Exception( "端口" + this.ThePort.PortName +"打开失败!"+ "\n" +Handle_Err);
                    }
                }
            }
            public byte[] ExecCommand(byte[] sendbyte, UInt16 GetReceiveLenght, string EndExpression)
            {
                byte[] resultbyte = new byte[GetReceiveLenght];
                System.DateTime theDatime = System.DateTime.Now;
                _endexpression = EndExpression;
                this.ThePort.DiscardInBuffer();
                this.ThePort.DiscardOutBuffer();
                ThePort.DataReceived  -= new SerialDataReceivedEventHandler(ThePort_DataReceived);
                ThePort.ErrorReceived -= new SerialErrorReceivedEventHandler(ThePort_ErrorReceived);
                ThePort.Write(sendbyte, 0, sendbyte.Length);
                this.IsMessageFlag = false;
                resultbyte = GetReadByes(GetReceiveLenght);
                //this.ThePort.Read(resultbyte, 0, GetReceiveLenght);
                return resultbyte.ToArray<byte>();
            }
            public string ExecCommand(string SendStr, int GetReceiveLenght, string EndExpression)
            {
                System.DateTime theDatime = System.DateTime.Now;
                ThePort.ReceivedBytesThreshold = GetReceiveLenght;
                _endexpression = EndExpression;
                _Readstr                        = "";
                this.IsMessageFlag              = false;
                ThePort.Write(SendStr);
                GetReadStr();
                if (PutDebug!=null)             //通过事件输出串口发送和接收数据串
                    this.PutDebug(SendStr, _Readstr, System.DateTime.Now - theDatime);        
                return _Readstr;
            }

            private string GetReadStr()
            {
                int i = 0;

                while (this.IsMessageFlag == false && i < ThePort.ReadTimeout)
                {
                    System.Threading.Thread.Sleep(TRYTIME);
                    i = i + TRYTIME;
                }
                if (IsOpenOhterDebug) DebugReadExistingStr = _Readstr + this.ThePort.ReadExisting();

                if (this.IsMessageFlag)
                {
                    return _Readstr;
                }
                else
                {
                    Handle_Err = "此次操作" + "没有返回任何数据！";
                    return "";
                }
            }
            private byte[] GetReadByes(UInt16 GetReceiveLenght)
            {
                byte[] getBytes = new byte[GetReceiveLenght];
                int i = 0;
                do
                {
                    System.Threading.Thread.Sleep(TRYTIME);
                    i = i + TRYTIME;
                    int ReadBufLen = ThePort.BytesToRead;
                    if (ReadBufLen >= GetReceiveLenght)
                    {
                        ThePort.Read(getBytes, 0, GetReceiveLenght);
                        this.IsMessageFlag = true;
                    }
                    else
                    {
                        ReadBufLen = ThePort.BytesToRead;
                    }
                } while (this.IsMessageFlag == false && i< ThePort.ReadTimeout);

                //if (PutDebug != null)             //通过事件输出串口发送和接收数据串
                    //this.PutDebug(getBytes, System.DateTime.Now - theDatime);  
                //System.BitConverter.
                if (this.IsMessageFlag)
                {
                    return getBytes.ToArray<byte>();
                }
                else
                {
                    Handle_Err = "此次操作" + "没有返回任何数据！";
                    return new byte[0];
                }

            }
            public void Dispose()
            {
                this.ThePort.Dispose();
            }
            
            static public Int16 GetInt16(byte[] buf,int index)
            {
                byte[] temp = new byte[2];
                Buffer.BlockCopy(buf, 3, temp, 0, 2);
                if (BitConverter.IsLittleEndian)
                {
                    temp = temp.Reverse().ToArray<byte>();
                }
                return BitConverter.ToInt16(temp, 0);
            }
            static public Int32 GetInt32(byte[] buf, int index)
            {
                byte[] temp = new byte[4];
                Buffer.BlockCopy(buf, 3, temp, 0, 4);
                if (BitConverter.IsLittleEndian)
                {
                    temp = temp.Reverse().ToArray<byte>();
                }
                return BitConverter.ToInt32(temp, 0);
            }
            
            static public void SetInt32(byte[] buf,int index,Int32 Value)
            {
                byte[] tempbuf = BitConverter.GetBytes(Value);


            }

            static public ushort CRC16(byte[] puchMsg, ushort usDataLen)
            { 
                byte uchCRCHi = 0xFF;  /* 高CRC字节初始化 */ 
                byte uchCRCLo = 0xFF;  /* 低CRC 字节初始化 */ 
                byte uIndex;
 
                for(int i=0;i<usDataLen;i++) /* 传输消息缓冲区 */ 
                {
                    uIndex = (byte)(uchCRCHi ^ puchMsg[i]);/* 计算CRC */
                    uchCRCHi = (byte)(uchCRCLo ^ auchCRCHi[uIndex]);
                    uchCRCLo = auchCRCLo[uIndex];
                }

                return (ushort)(uchCRCHi << 8 | uchCRCLo);
            } 

            public    static byte[] auchCRCHi= {0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
                                        0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
                                        0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
                                        0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
                                        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1,
                                        0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
                                        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1,
                                        0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
                                        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
                                        0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 
                                        0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 
                                        0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
                                        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
                                        0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40,
                                        0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
                                        0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
                                        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
                                        0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
                                        0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
                                        0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
                                        0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
                                        0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40,
                                        0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1,
                                        0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
                                        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
                                        0x80, 0x41, 0x00, 0xC1, 0x81, 0x40 
                                        };
            static byte[] auchCRCLo = { 0x00, 0xC0, 0xC1, 0x01, 0xC3, 0x03, 0x02, 0xC2, 0xC6, 0x06, 
                                        0x07, 0xC7, 0x05, 0xC5, 0xC4, 0x04, 0xCC, 0x0C, 0x0D, 0xCD,
                                        0x0F, 0xCF, 0xCE, 0x0E, 0x0A, 0xCA, 0xCB, 0x0B, 0xC9, 0x09,
                                        0x08, 0xC8, 0xD8, 0x18, 0x19, 0xD9, 0x1B, 0xDB, 0xDA, 0x1A,
                                        0x1E, 0xDE, 0xDF, 0x1F, 0xDD, 0x1D, 0x1C, 0xDC, 0x14, 0xD4,
                                        0xD5, 0x15, 0xD7, 0x17, 0x16, 0xD6, 0xD2, 0x12, 0x13, 0xD3,
                                        0x11, 0xD1, 0xD0, 0x10, 0xF0, 0x30, 0x31, 0xF1, 0x33, 0xF3,
                                        0xF2, 0x32, 0x36, 0xF6, 0xF7, 0x37, 0xF5, 0x35, 0x34, 0xF4,
                                        0x3C, 0xFC, 0xFD, 0x3D, 0xFF, 0x3F, 0x3E, 0xFE, 0xFA, 0x3A,
                                        0x3B, 0xFB, 0x39, 0xF9, 0xF8, 0x38, 0x28, 0xE8, 0xE9, 0x29,
                                        0xEB, 0x2B, 0x2A, 0xEA, 0xEE, 0x2E, 0x2F, 0xEF, 0x2D, 0xED,
                                        0xEC, 0x2C, 0xE4, 0x24, 0x25, 0xE5, 0x27, 0xE7, 0xE6, 0x26,
                                        0x22, 0xE2, 0xE3, 0x23, 0xE1, 0x21, 0x20, 0xE0, 0xA0, 0x60,
                                        0x61, 0xA1, 0x63, 0xA3, 0xA2, 0x62, 0x66, 0xA6, 0xA7, 0x67,
                                        0xA5, 0x65, 0x64, 0xA4, 0x6C, 0xAC, 0xAD, 0x6D, 0xAF, 0x6F,
                                        0x6E, 0xAE, 0xAA, 0x6A, 0x6B, 0xAB, 0x69, 0xA9, 0xA8, 0x68,
                                        0x78, 0xB8, 0xB9, 0x79, 0xBB, 0x7B, 0x7A, 0xBA, 0xBE, 0x7E,
                                        0x7F, 0xBF, 0x7D, 0xBD, 0xBC, 0x7C, 0xB4, 0x74, 0x75, 0xB5,
                                        0x77, 0xB7, 0xB6, 0x76, 0x72, 0xB2, 0xB3, 0x73, 0xB1, 0x71,
                                        0x70, 0xB0, 0x50, 0x90, 0x91, 0x51, 0x93, 0x53, 0x52, 0x92,
                                        0x96, 0x56, 0x57, 0x97, 0x55, 0x95, 0x94, 0x54, 0x9C, 0x5C,
                                        0x5D, 0x9D, 0x5F, 0x9F, 0x9E, 0x5E, 0x5A, 0x9A, 0x9B, 0x5B,
                                        0x99, 0x59, 0x58, 0x98, 0x88, 0x48, 0x49, 0x89, 0x4B, 0x8B,
                                        0x8A, 0x4A, 0x4E, 0x8E, 0x8F, 0x4F, 0x8D, 0x4D, 0x4C, 0x8C,
                                        0x44, 0x84, 0x85, 0x45, 0x87, 0x47, 0x46, 0x86, 0x82, 0x42,
                                        0x43, 0x83, 0x41, 0x81, 0x80, 0x40
                                      };

        }
    }
}

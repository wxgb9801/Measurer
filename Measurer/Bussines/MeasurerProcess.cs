using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Commu_ClsLibrary.CommuPort;
using System.Data;
using LIB_DB;
using LIB_Common;
using System.Globalization;
using System.ComponentModel;
namespace Measurer
{
    public class MeasurerProcess
    {
        private bool connected = false;
        public bool Connected
        {
            get { return connected; }
        }

        public ComPort Com = null;
        public Task CurrentTask;
        public DBService theDB;

        public MeasurerProcess()
        {
            CurrentTask = new Task();
            theDB = DBServiceFactory.GetDBService("MeasurerXML");
        }
        public bool StartConnect()
        {

            PortInitInfo p = new PortInitInfo(9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
            Com = new ComPort(SysPara.ComPort, p);
            if (Com.Open())
            {
                connected = true;
                Program.WriteEventLog(this.GetType().FullName, string.Format("端口{0}打开成功！", SysPara.ComPort), "");
                return true;
            }
            else
            {
                Program.WriteEventLog(this.GetType().FullName, string.Format("端口{0}打开失败！", SysPara.ComPort), "");
                return false;
            }
        }
        public void StopConnect()
        {
            if (Com == null)
            {
                connected = false;
                return;
            }
            Com.Dispose();
            connected = false;
        }

        public Int16 ReadSn()
        {
            //动作	站号	功能码	起始地址高位	起始地址低位	总寄存器数高位	总寄存器数低位	CRC	
            //发送	01	03	21	00	00	01	8E 	36
            //动作	站号	功能码	字节数	寄存器值高位	寄存器值低位	CRC	
            //返回	01	03	02	00	02	39	85
            Int16 ReadSn = -1;
            try
            {
                byte[] ReadPVCMD = { 0x01, 0x03, 0x21, 0x00, 0x00, 0x01, 0x8E, 0x36 };
                byte[] ReadPVReturn = new byte[7];
                ReadPVReturn = Com.ExecCommand(ReadPVCMD, 7, "");
                if (ReadPVReturn.Length > 0 && ReadPVReturn.Length == 7)
                {
                    ReadSn = ComPort.GetInt16(ReadPVReturn, 3);
                }
                else
                {
                    throw new ApplicationException(string.Format("读取S/N失败！-{0}", "未取得数据"));
                }
                return ReadSn;
            }
            catch (Exception ex)
            {
                Program.WriteErrorLog(this.GetType().FullName, ex);
                return ReadSn;
            }
        }
        public Int32 ReadPV()
        {
            //读测量值PV
            //动作	站号	功能码	起始地址高位	起始地址低位	总寄存器数高位	总寄存器数低位	CRC	
            //发送	01	03	81	12	00	02	4C 	32
            //      01 03 81 12 01 00 02 4C 32
            //动作	站号	功能码	字节数	寄存器值高位	寄存器值低位	寄存器值高位	寄存器值低位	CRC	
            //返回	01	03	04	00(8112值)	00(8112值)	00(8113值)	00(8113值)	FA	33
            Int32 pv = 0;
            try
            {
                byte[] ReadPVCMD = { 0x01, 0x03, 0x81, 0x12, 0x00, 0x02, 0x4C, 0x32 };
                byte[] ReadPVReturn = new byte[9];
                ReadPVReturn = Com.ExecCommand(ReadPVCMD, 9, "");
                if (ReadPVReturn != null && ReadPVReturn.Length == 9)
                {
                    Int32Converter c = new Int32Converter();
                    pv = ComPort.GetInt32(ReadPVReturn, 3);
                }
                else
                {
                    throw new ApplicationException(string.Format("读取PV值失败！-{0}", "未取得数据"));
                }
                return pv;
            }
            catch (Exception ex)
            {
                Program.WriteErrorLog(this.GetType().FullName, ex);
                return pv;
            }
        }

        public bool SetingToDevice()
        {
            try
            {
                //Set Sn
                //Set Para_A
                //Set Para_B
                //Set Para_C
                //byte[] ReadPVCMD = { 0x01, 0x10, 0x81, 0x02, 0x00, 0x02, 0x04, 0x00, 0x00, 0x00, 0x09, 0x00, 0x00 };
                //byte[] ReadPVCMD1 = { 0x01, 0x03, 0x21, 0x00, 0x00, 0x01, 0x00, 0x00 };//, 0x8E, 0x36 };
                //byte[] ReadPVCMD2 = { 0x01, 0x03, 0x21, 0x00, 0x00, 0x01, 0x8E, 0x36 };//, , 0x36 };

                byte[] SetSn = { 0x01, 0x03, 0x21, 0x00, 0x00, 0x01, 0x00, 0x00 };
                byte[] SetPara_A = { 0x01, 0x10, 0x81, 0x00, 0x00, 0x02, 0x04, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00 };
                byte[] SetPara_B = { 0x01, 0x10, 0x81, 0x02, 0x00, 0x02, 0x04, 0x00, 0x00, 0x00, 0x09, 0x00, 0x00 };
                byte[] SetPara_C = { 0x01, 0x10, 0x81, 0x04, 0x00, 0x02, 0x04, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00 };



                Int16 returnSn = 0;
                int a = ComPort.CRC16(SetSn, 11);
                string x = a.ToString("x");
                byte[] cycvalue = BitConverter.GetBytes(a);
                cycvalue = cycvalue.Reverse<byte>().ToArray();
                Buffer.BlockCopy(cycvalue, 2, SetSn, 11, 2);
                byte[] ReadPVReturn = new byte[8];
                ReadPVReturn =Com.ExecCommand(SetSn, 8, "");
                if (ReadPVReturn.Length > 0 && ReadPVReturn.Length == 8)
                {
                    returnSn = ComPort.GetInt16(ReadPVReturn, 4);
                }
                else
                {
                    throw new ApplicationException(string.Format("读取PV值失败！-{0}", "未取得数据"));
                }
                return true;
            }
            catch (Exception ex)
            {
                Program.WriteErrorLog(this.GetType().FullName, ex);
                return false;
            }
        }
        public Task LoadCurrentTask()
        {
            Task task = null;
            try
            {
                StringBuilder sbstr = new StringBuilder("Select * From T_Task");
                DataTable dt = theDB.SelectDataTable(sbstr.ToString());
                if (dt.Rows.Count >= 1)
                {
                    task = new Task();
                    task = Task.CreateTaskByTable(dt);
                }
                return task;
            }
            catch (Exception ex)
            {
                Program.WriteErrorLog(this.GetType().FullName, ex);
                return task;
            }
        }

        public bool SaveCurrentTask(Task task)
        {
            try
            {
                bool result = false;
                StringBuilder sbstr = new StringBuilder("insert into T_Task(TaskNo,BatchNo,ItemNo,ItemName,Sn,PV,Result,redundantVaue,Status,StartTime,CompleteTime) Values(");
                sbstr.Append(string.Format("'{0}',", task.TaskNo));
                sbstr.Append(string.Format("'{0}',", task.BatchNo));
                sbstr.Append(string.Format("'{0}',", task.ItemNo));
                sbstr.Append(string.Format("'{0}',", task.ItemName));
                sbstr.Append(string.Format("'{0}',", task.Sn));
                sbstr.Append(string.Format("'{0}',", task.PV));
                sbstr.Append(string.Format("'{0}',", task.Result));
                sbstr.Append(string.Format("'{0}',", task.redundantVaue));
                sbstr.Append("0,");
                sbstr.Append(string.Format("'{0}',", task.StartTime));
                sbstr.Append(string.Format("'{0}'", task.CompleteTime));
                sbstr.Append(")");
                result = theDB.UpdataDataBase("Delete T_Task");
                return theDB.UpdataDataBase(sbstr.ToString());
            }
            catch(Exception ex)
            {
                Program.WriteErrorLog(this.GetType().FullName, ex);
                return false;
            }
        }
        public bool SaveRecordTask(Task task)
        {
            bool result=false;
            try
            {
                StringBuilder sbstr = new StringBuilder("Insert into T_Record(TaskNo,BatchNo,ItemNo,ItemName,Sn,PV,Result,redundantVaue,Status,StartTime,CompleteTime) Values(");
                sbstr.Append(string.Format("{0},", FunBase.ChkField(task.TaskNo,true)));
                sbstr.Append(string.Format("{0},", FunBase.ChkField(task.BatchNo,true)));
                sbstr.Append(string.Format("{0},", FunBase.ChkField(task.ItemNo,true)));
                sbstr.Append(string.Format("{0},", FunBase.ChkField(task.ItemName,true)));
                sbstr.Append(string.Format("{0},", FunBase.ChkField(task.Sn, true)));
                sbstr.Append(string.Format("{0},", FunBase.ChkField(task.PV.ToString(), true)));
                sbstr.Append(string.Format("{0},", FunBase.ChkField(task.Result.ToString(), true)));
                sbstr.Append(string.Format("{0},", FunBase.ChkField(task.redundantVaue.ToString(),true)));
                sbstr.Append("1,");
                sbstr.Append(string.Format("{0},", FunBase.ChkField(task.StartTime.ToString(Task.DateTimeFormat1))));
                sbstr.Append(string.Format("{0}",  FunBase.ChkField(task.CompleteTime.ToString(Task.DateTimeFormat1))));
                sbstr.Append(")");
                result = theDB.UpdataDataBase(sbstr.ToString());
                if(result)
                {
                    result = theDB.UpdataDataBase("Delete Table T_Task");
                }
                return result;
            }
            catch (Exception ex)
            {
                Program.WriteErrorLog(this.GetType().FullName, ex);
                return false;
            }
        }
        public bool CompleteTask(string taskNo)
        {
            StringBuilder sbstr = new StringBuilder("Update T_Record(TaskNo,BatchNo,ItemNo,ItemName,Sn,PV,Result,redundantVaue,StartTime,CompleteTime) Values(");
            return false;
        }
         
    }
}

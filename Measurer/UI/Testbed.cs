using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Commu_ClsLibrary.CommuPort;

namespace Measurer
{
    public partial class Testbed : Form
    {
        public ComPort cport = null;
        public Testbed()
        {
            InitializeComponent();
            btn_Read.Enabled = false;
        }
        private void btn_Read_Click(object sender, EventArgs e)
        {
            timer_ReadPV.Enabled = true;
            timer_ReadPV.Interval = 1000;
            timer_ReadPV.Start();
        }

        private void but_OpenCom_Click(object sender, EventArgs e)
        {
            but_OpenCom.Enabled = false;
            try
            {
                PortInitInfo p = new PortInitInfo(9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
                cport = new ComPort(cbo_Port.Text, p);
                if (cport.Open())
                {
                    MessageBox.Show(string.Format("端口{0}打开成功！", cbo_Port.Text));
                    btn_Read.Enabled = true;
                }
                else
                {
                    MessageBox.Show(string.Format("端口{0}打开失败！", cbo_Port.Text));
                    but_OpenCom.Enabled = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(string.Format("端口{0}打开失败！-{1}", cbo_Port.Text,ex.Message));
                but_OpenCom.Enabled = true;
            }
        }

        private void timer_ReadPV_Tick(object sender, EventArgs e)
        {
            //读测量值PV
            //动作	站号	功能码	起始地址高位	起始地址低位	总寄存器数高位	总寄存器数低位	CRC	
            //发送	01	03	81	12	00	02	4C 	32
            //      01 03 81 12 01 00 02 4C 32
            //动作	站号	功能码	字节数	寄存器值高位	寄存器值低位	寄存器值高位	寄存器值低位	CRC	
            //返回	01	03	04	00(8112值)	00(8112值)	00(8113值)	00(8113值)	FA	33
            try
            {
                byte[] ReadPVCMD = { 0x01, 0x03, 0x81, 0x12, 0x00, 0x02, 0x4C, 0x32 };
                byte[] ReadPVReturn = new byte[9];
                ReadPVReturn = cport.ExecCommand(ReadPVCMD, 9, "");
                if (ReadPVReturn.Length != null && ReadPVReturn.Length == 9)
                {
                    Int32Converter c = new Int32Converter();
          
                    Int32 pv = ComPort.GetInt32(ReadPVReturn, 3);
                    txt_PV.Text = pv.ToString();
                }
                else
                {
                    MessageBox.Show(string.Format("读取PV值失败！-{0}", "未取得数据"));
                }
            }
            catch(Exception ex)
            {
                 MessageBox.Show(string.Format("读取PV值失败！-{0}",ex.Message));
            }
        }
        private void btn_ReadSN_Click(object sender, EventArgs e)
        {
            //动作	站号	功能码	起始地址高位	起始地址低位	总寄存器数高位	总寄存器数低位	CRC	
            //发送	01	03	21	00	00	01	8E 	36
            //动作	站号	功能码	字节数	寄存器值高位	寄存器值低位	CRC	
            //返回	01	03	02	00	02	39	85
            try
            {
                byte[] ReadPVCMD = { 0x01, 0x03, 0x21, 0x00, 0x00, 0x01, 0x8E, 0x36 };
                byte[] ReadPVReturn = new byte[7];
                ReadPVReturn = cport.ExecCommand(ReadPVCMD, 7, "");
                if (ReadPVReturn.Length >0 && ReadPVReturn.Length == 7)
                {
                    Int16 sn = ComPort.GetInt16(ReadPVReturn, 3);
                    txt_SN.Text = sn.ToString();
                }
                else
                {
                     MessageBox.Show(string.Format("读取S/N失败！-{0}", "未取得数据"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("读取S/N失败！-{0}", ex.Message));
            }
        }
        private void but_CloseCom_Click(object sender, EventArgs e)
        {
            if (cport == null) return;
            cport.Dispose();
            btn_Read.Enabled=false;
            but_OpenCom.Enabled = true;
        }

        private void Testbed_Load(object sender, EventArgs e)
        {

        }

        private void Testbed_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Frm_Main fm = new Frm_Main();
            fm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] ReadPVCMD = { 0x01, 0x10, 0x81, 0x02, 0x00,0x02,0x04, 0x00, 0x00, 0x00, 0x09, 0x00, 0x00 };
            byte[] ReadPVCMD1 = { 0x01, 0x03, 0x21, 0x00, 0x00, 0x01,0x00,0x00};//, 0x8E, 0x36 };
            byte[] ReadPVCMD2 = { 0x01, 0x03, 0x21, 0x00, 0x00, 0x01, 0x8E, 0x36 };//, , 0x36 };

            int a = ComPort.CRC16(ReadPVCMD, 11);
           string x=a.ToString("x");
            byte[] cycvalue=  BitConverter.GetBytes(a);
            cycvalue = cycvalue.Reverse<byte>().ToArray();
            Buffer.BlockCopy(cycvalue, 2, ReadPVCMD, 11, 2);
            byte[] ReadPVReturn = new byte[8];
            ReadPVReturn = cport.ExecCommand(ReadPVCMD, 8, "");
            if (ReadPVReturn.Length > 0 && ReadPVReturn.Length == 8)
            {
                Int16 sn = ComPort.GetInt16(ReadPVReturn, 4);
                txt_SN.Text = sn.ToString();
            }
            else
            {
                MessageBox.Show(string.Format("读取S/N失败！-{0}", "未取得数据"));
            }
        }


    }
}

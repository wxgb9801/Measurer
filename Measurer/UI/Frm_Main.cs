using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Measurer
{
    public partial class Frm_Main : Form
    {
        Frm_Histroy frmhis = new Frm_Histroy();
        MeasurerProcess mp = new MeasurerProcess();
        Task ctask = null;
        Task lastTask = null;
        TimeSpan useTimespan = new TimeSpan();
        public string sn = "";
        public int Init_Pv = 0;
        public int act_Pv = 0;
        public Frm_Main()
        {
            InitializeComponent();
            Init_Pv = 50;
            act_Pv = 10;
        }
        public Frm_Main(Frm_Login login):this()
        {
            lbl_Operator.Text = string.Format("{0}({1})",login.UserID,login.UserName);
            btnSts_Init();

        }

        private void Frm_Main_Load(object sender, EventArgs e)
        {
            Timer_ReadData.Interval = SysPara.RefreshTime;
            lastTask = mp.LoadCurrentTask();
            if (lastTask != null)
            {
                FillTaskToUI(lastTask);
                ctask = lastTask;
                HideMeasureInfo();
            }
            else
            {
                ShowMeasureInfo();
            }
        }

        private void Timer_ReadData_Tick(object sender, EventArgs e)
        {
            if (ctask != null && mp.Connected)
            {
                act_Pv = mp.ReadPV(); 
                useTimespan = DateTime.Now - ctask.StartTime;
                ctask.PV = act_Pv;
                txt_SumLength.Text = (ctask.PV * (1 / SysPara.computeRatio)).ToString() ;
                lbl_PV.Text = ctask.PV.ToString("00000#");
                lbl_UseTime.Text = useTimespan.ToString("g");
            }
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            //检查批次号重复
            //创建当前任务
            //锁定任务修改
            //开启设备连接
            //读取计米器数据
            try
            {
                if (txt_batchNo.Text.Trim().Length < 1)
                {
                    txt_batchNo.Focus();
                    throw new ApplicationException("批次不能为空。");
                }
                if (txt_ItemNo.Text.Trim().Length < 1)
                {
                    txt_batchNo.Focus();
                    throw new ApplicationException("品规不能为空。");
                }
                ctask = new Task();
                FillTaskFormUI(ctask);
                Task lasttask = mp.LoadCurrentTask();
                if (lasttask != null)
                {
                    DialogResult dres = MessageBox.Show("已存在当前未完成任务是否继续，点(是)继续创建新任务，点(否)读取为完成任务。","任务已存在",
                                        MessageBoxButtons.YesNoCancel,MessageBoxIcon.Question,MessageBoxDefaultButton.Button1);
                    if (dres == DialogResult.Yes)
                    {
                        //ClearUITastInfo();
                        //ClearUIMeasureInfo();
                    }
                    else if (dres == DialogResult.No)
                    {
                        FillTaskToUI(lasttask);
                        btnSts_Started();
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                btnSts_Created();
                ShowMeasureInfo();
                FillTaskFormUI(ctask);

                bool res = mp.SaveCurrentTask(ctask);
                if (!res)
                {
                    throw new ApplicationException("保存作业至数据库失败。");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("新建作业失败！{0}",ex.Message));
            }
        }
        private void btn_Contiue_Click(object sender, EventArgs e)
        {
            bool res = true;
            try
            {
                if (!mp.Connected)
                {
                    res = false;
                    res = mp.StartConnect();
                    System.Threading.Thread.Sleep(1000);
                    sn = mp.ReadSn().ToString();
                    lbl_PSN.Text = sn;
                }
                if (res)
                {
                    btnSts_Started();
                    ShowMeasureInfo();
                    ctask.StartTime = DateTime.Now;
                    txt_StartTime.Text = ctask.StartTime.ToString(Task.DateTimeFormat1);
                    FillTaskFormUI(ctask);

                    ShowMeasureInfo();
                    Timer_ReadData.Enabled = true;
                    Timer_ReadData.Start();
                    gbx_ItemInfo.Enabled = false;
                }
                else
                {
                    throw new ApplicationException("连接设备失败。");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("新建作业失败！{0}", ex.Message));
            }

        }
        private void btn_Compelte_Click(object sender, EventArgs e)
        {
            btnSts_Complete();
            HideMeasureInfo();

            if (ctask.CompleteTime == new DateTime())
            {
                ctask.CompleteTime = DateTime.Now;
                txt_EndTime.Text = ctask.CompleteTime.ToString(Task.DateTimeFormat1);
            }
            mp.StopConnect();
        }

        private void btn_SaveRecord_Click(object sender, EventArgs e)
        {
            int.TryParse(txt_SumLength.Text, out ctask.PV);
            int.TryParse(txt_Rongyu.Text, out ctask.PV);
            bool res = mp.SaveRecordTask(ctask);
            if (!res)
            {
                MessageBox.Show("记录当前任务失败。");
            }
            else
            {
                MessageBox.Show("保存成功。");
                ClearUITastInfo();
                lastTask = null;
                ctask = new Task();
            }
        }

        private void Frm_Main_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
        private void btn_Histroy_Click(object sender, EventArgs e)
        {
            if (!frmhis.Created)
            {
                ShowHistoryQuery();
            }
            else
            {
                HideHistoryQuery();
            }
        }

        private void btn_conn_Click(object sender, EventArgs e)
        {
            btn_conn.Enabled = false;
            bool res = false;
            try
            {
                if (mp.Connected) return;
                res = mp.StartConnect();
                if (res)
                {
                    throw new ApplicationException("连接设备失败。");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("新建作业失败！{0}", ex.Message));
                btn_conn.Enabled = true;
            }

        }

        private void ShowMeasureInfo()
        {
            lbl_PSN.Text = "000000";
            pnl_ShowInfo.Visible = true;
            pnl_ShowInfo.TabIndex = 0;
            pnl_ShowInfo.Left = gbx_ShowInfo.Left + 5;
            pnl_ShowInfo.Top = gbx_ShowInfo.Top + 15;

            txt_SumLength.ReadOnly = !SysPara.AllowUpdateLength;

            txt_Rongyu.ReadOnly = false;
            txt_StartTime.ReadOnly = true;
            txt_EndTime.ReadOnly = true;

        }
        private void HideMeasureInfo()
        {
            pnl_ShowInfo.Visible = false;
            pnl_ShowInfo.TabIndex = 0;
            pnl_ShowInfo.Left = gbx_ShowInfo.Left + 5;
            pnl_ShowInfo.Top = gbx_ShowInfo.Top + 15;
        }

        private void ClearUITastInfo()
        {
            txt_batchNo.Text = "";
            txt_ItemName.Text = "";
            txt_ItemNo.Text = "";
            txt_StartTime.Text = "";
            txt_EndTime.Text = "";
            txt_Rongyu.Text = "0";
            txt_SumLength.Text = "0";
        }
        private void ClearUIMeasureInfo()
        {
            lbl_PV.Text = ctask.PV.ToString("00000#");
            lbl_UseTime.Text = useTimespan.ToString("g");
        }

        private void ShowHistoryQuery()
        {
            frmhis = new Frm_Histroy();
            frmhis.Show();
            int move = this.Left - frmhis.Width / 2;
            if (move > 0)
            {
                this.Left = move;
            }
            frmhis.Visible = false;
            frmhis.Top = this.Top;
            frmhis.Left = this.Left + this.Width;
            frmhis.Height = this.Height;
            frmhis.Visible = true;
        }
        private void HideHistoryQuery()
        {
            frmhis.Close();
            int move = this.Left + frmhis.Width / 2;
            if (move > 0)
            {
                this.Left = move;
            }

        }

        private void Frm_Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.WLog.Stop();
            Application.Exit();
            System.Environment.Exit(0); 
        }

        private void btnSts_Init()
        {
            btn_Create.Enabled = true;
            btn_Contiue.Enabled = true;
            btn_Compelte.Enabled = false;
            btn_SaveRecord.Enabled = false;
        }
        private void btnSts_Created()
        {
            btn_Create.Enabled = false;
            btn_Contiue.Enabled = true;
            btn_Compelte.Enabled = true;
        }
        private void btnSts_Started()
        {
            btn_Create.Enabled = false;
            btn_Contiue.Enabled = false;
            btn_Compelte.Enabled = true;
        }
        private void btnSts_Complete()
        {
            btn_Create.Enabled = false;
            btn_Contiue.Enabled = true;
            btn_Compelte.Enabled = false;
            btn_SaveRecord.Enabled = true;
        }

        private void FillTaskToUI(Task lasttask)
        {
            if (lasttask != null)
            {
                txt_batchNo.Text = lasttask.BatchNo;
                txt_ItemNo.Text = lasttask.ItemNo;
                txt_ItemName.Text = lasttask.ItemName;

                txt_StartTime.Text = lasttask.StartTime == new DateTime() ? "" : lasttask.StartTime.ToString(Task.DateTimeFormat1);
                txt_EndTime.Text = lasttask.CompleteTime == new DateTime() ? "" : lasttask.CompleteTime.ToString(Task.DateTimeFormat1);

                txt_SumLength.Text = lasttask.PV.ToString();

                if (lasttask.Status != eTaskSts.Start)
                {
                    btnSts_Started();
                }
            }
        }
        private void FillTaskFormUI(Task ctask)
        {
            Task lasttask = new Task();
            ctask.TaskNo = txt_batchNo.Text;
            ctask.BatchNo = txt_batchNo.Text;
            ctask.ItemNo = txt_ItemNo.Text;
            ctask.ItemName = txt_ItemName.Text;
            int.TryParse(txt_SumLength.Text, out ctask.PV);
            int.TryParse(txt_Rongyu.Text, out ctask.PV);
        }

        private void but_Setting_Click(object sender, EventArgs e)
        {
            try
            {
                if (mp.Connected)
                {

                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("参数设置失败！{0}", ex.Message));
            }
        }

    }
}
 
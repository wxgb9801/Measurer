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
    public partial class Frm_Histroy : Form
    {
        HistoryManger hm = new HistoryManger();
        public Frm_Histroy()
        {
            InitializeComponent();
        }

        private void btn_Querry_Click(object sender, EventArgs e)
        {
            string itemNo=txt_ItemNo.Text;
            DateTime fromdt = dtp_From.Value;
            DateTime todt = dtp_To.Value;
            dgv_Record.DataSource= hm.GetHistory(itemNo, fromdt, todt);
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.SaveFileDialog saveFileDlg = new System.Windows.Forms.SaveFileDialog();
            string currName = DateTime.Now.ToString("yyyyMMddHHmmss_") + this.Text;
            saveFileDlg.Title = "Export " + currName;
            saveFileDlg.FileName = currName;
            saveFileDlg.DefaultExt = "xls";
            saveFileDlg.Filter = "Excel文件 (*.xls)|*.xls";

            if (saveFileDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Application.DoEvents();
                Application.DoEvents();
                DataTable tmpdt=(DataTable)dgv_Record.DataSource;
                ExcelHelper.ExportDTtoExcel(tmpdt, string.Empty, saveFileDlg.FileName);
            }
        }
    }
}

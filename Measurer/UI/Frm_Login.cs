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
    public partial class Frm_Login : Form
    {
        public UserManger UserMng = new UserManger();
        public string UserID;
        public string UserName;
        public string UserGroup;
        public Frm_Login()
        {
            InitializeComponent();
        }

        private void btn_Login_Click(object sender, EventArgs e)
        {
            UserID = txt_User.Text;
            string pwd = txt_PWD.Text;
            int res = UserMng.Login(UserID, pwd, out UserName, out UserGroup);
            if (res == 0)
            {
                this.Hide();
                Frm_Main frm = new Frm_Main(this);
                frm.Show();
                //this.Close();
            }
            else
            {
                MessageBox.Show("用户名密码错误");
            }
        }

        private void btn_Quit_Click(object sender, EventArgs e)
        {
            this.Close();

        }
    }
}

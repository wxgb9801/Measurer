namespace Measurer
{
    partial class Testbed
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer_ReadPV = new System.Windows.Forms.Timer(this.components);
            this.txt_PV = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_Read = new System.Windows.Forms.Button();
            this.cbo_Port = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.but_CloseCom = new System.Windows.Forms.Button();
            this.but_OpenCom = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_SN = new System.Windows.Forms.TextBox();
            this.btn_ReadSN = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // timer_ReadPV
            // 
            this.timer_ReadPV.Interval = 500;
            this.timer_ReadPV.Tick += new System.EventHandler(this.timer_ReadPV_Tick);
            // 
            // txt_PV
            // 
            this.txt_PV.Location = new System.Drawing.Point(140, 117);
            this.txt_PV.Margin = new System.Windows.Forms.Padding(4);
            this.txt_PV.Name = "txt_PV";
            this.txt_PV.Size = new System.Drawing.Size(176, 28);
            this.txt_PV.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(63, 126);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "PV:";
            // 
            // btn_Read
            // 
            this.btn_Read.Location = new System.Drawing.Point(346, 116);
            this.btn_Read.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Read.Name = "btn_Read";
            this.btn_Read.Size = new System.Drawing.Size(308, 34);
            this.btn_Read.TabIndex = 2;
            this.btn_Read.Text = "Start Timer Read";
            this.btn_Read.UseVisualStyleBackColor = true;
            this.btn_Read.Click += new System.EventHandler(this.btn_Read_Click);
            // 
            // cbo_Port
            // 
            this.cbo_Port.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbo_Port.FormattingEnabled = true;
            this.cbo_Port.Location = new System.Drawing.Point(140, 18);
            this.cbo_Port.Margin = new System.Windows.Forms.Padding(4);
            this.cbo_Port.Name = "cbo_Port";
            this.cbo_Port.Size = new System.Drawing.Size(176, 32);
            this.cbo_Port.TabIndex = 7;
            this.cbo_Port.Text = "Com3";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(60, 27);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 18);
            this.label3.TabIndex = 8;
            this.label3.Text = "端口号:";
            // 
            // but_CloseCom
            // 
            this.but_CloseCom.Location = new System.Drawing.Point(508, 14);
            this.but_CloseCom.Margin = new System.Windows.Forms.Padding(4);
            this.but_CloseCom.Name = "but_CloseCom";
            this.but_CloseCom.Size = new System.Drawing.Size(146, 39);
            this.but_CloseCom.TabIndex = 28;
            this.but_CloseCom.Text = "关闭端口";
            this.but_CloseCom.UseVisualStyleBackColor = true;
            this.but_CloseCom.Click += new System.EventHandler(this.but_CloseCom_Click);
            // 
            // but_OpenCom
            // 
            this.but_OpenCom.Location = new System.Drawing.Point(346, 15);
            this.but_OpenCom.Margin = new System.Windows.Forms.Padding(4);
            this.but_OpenCom.Name = "but_OpenCom";
            this.but_OpenCom.Size = new System.Drawing.Size(146, 39);
            this.but_OpenCom.TabIndex = 27;
            this.but_OpenCom.Text = "打开端口";
            this.but_OpenCom.UseVisualStyleBackColor = true;
            this.but_OpenCom.Click += new System.EventHandler(this.but_OpenCom_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(63, 86);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 18);
            this.label2.TabIndex = 30;
            this.label2.Text = "P-SN:";
            // 
            // txt_SN
            // 
            this.txt_SN.Location = new System.Drawing.Point(140, 76);
            this.txt_SN.Margin = new System.Windows.Forms.Padding(4);
            this.txt_SN.Name = "txt_SN";
            this.txt_SN.Size = new System.Drawing.Size(176, 28);
            this.txt_SN.TabIndex = 29;
            // 
            // btn_ReadSN
            // 
            this.btn_ReadSN.Location = new System.Drawing.Point(346, 72);
            this.btn_ReadSN.Margin = new System.Windows.Forms.Padding(4);
            this.btn_ReadSN.Name = "btn_ReadSN";
            this.btn_ReadSN.Size = new System.Drawing.Size(308, 34);
            this.btn_ReadSN.TabIndex = 31;
            this.btn_ReadSN.Text = "Read P-SN";
            this.btn_ReadSN.UseVisualStyleBackColor = true;
            this.btn_ReadSN.Click += new System.EventHandler(this.btn_ReadSN_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(680, 110);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(131, 34);
            this.button1.TabIndex = 32;
            this.button1.Text = "Set";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Testbed
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 192);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btn_ReadSN);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_SN);
            this.Controls.Add(this.but_CloseCom);
            this.Controls.Add(this.but_OpenCom);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbo_Port);
            this.Controls.Add(this.btn_Read);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_PV);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Testbed";
            this.Text = "FrmTestBed";
            this.Load += new System.EventHandler(this.Testbed_Load);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Testbed_MouseDoubleClick);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer_ReadPV;
        private System.Windows.Forms.TextBox txt_PV;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_Read;
        private System.Windows.Forms.ComboBox cbo_Port;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button but_CloseCom;
        private System.Windows.Forms.Button but_OpenCom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_SN;
        private System.Windows.Forms.Button btn_ReadSN;
        private System.Windows.Forms.Button button1;
    }
}


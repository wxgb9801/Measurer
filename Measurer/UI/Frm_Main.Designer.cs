namespace Measurer
{
    partial class Frm_Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_Main));
            this.Timer_ReadData = new System.Windows.Forms.Timer(this.components);
            this.lbl_PV = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_PSN = new System.Windows.Forms.Label();
            this.btn_Create = new System.Windows.Forms.Button();
            this.pnl_ShowInfo = new System.Windows.Forms.Panel();
            this.lbl_UseTime = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_batchNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txt_ItemNo = new System.Windows.Forms.TextBox();
            this.txt_ItemName = new System.Windows.Forms.TextBox();
            this.txt_StartTime = new System.Windows.Forms.TextBox();
            this.txt_Rongyu = new System.Windows.Forms.TextBox();
            this.txt_EndTime = new System.Windows.Forms.TextBox();
            this.txt_SumLength = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btn_Compelte = new System.Windows.Forms.Button();
            this.btn_Histroy = new System.Windows.Forms.Button();
            this.btn_SaveRecord = new System.Windows.Forms.Button();
            this.btn_Contiue = new System.Windows.Forms.Button();
            this.lbl_Operator = new System.Windows.Forms.Label();
            this.gbx_ItemInfo = new System.Windows.Forms.GroupBox();
            this.gbx_ShowInfo = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.but_Setting = new System.Windows.Forms.Button();
            this.btn_conn = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label14 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnl_ShowInfo.SuspendLayout();
            this.gbx_ItemInfo.SuspendLayout();
            this.gbx_ShowInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Timer_ReadData
            // 
            this.Timer_ReadData.Tick += new System.EventHandler(this.Timer_ReadData_Tick);
            // 
            // lbl_PV
            // 
            this.lbl_PV.AutoSize = true;
            this.lbl_PV.Font = new System.Drawing.Font("宋体", 47.83216F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_PV.Location = new System.Drawing.Point(27, 92);
            this.lbl_PV.Name = "lbl_PV";
            this.lbl_PV.Size = new System.Drawing.Size(328, 95);
            this.lbl_PV.TabIndex = 0;
            this.lbl_PV.Text = "000000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 15.10489F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.label1.Location = new System.Drawing.Point(109, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 30);
            this.label1.TabIndex = 1;
            this.label1.Text = "测量值(PV)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12.08392F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(11, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "P-Sn:";
            // 
            // lbl_PSN
            // 
            this.lbl_PSN.AutoSize = true;
            this.lbl_PSN.Font = new System.Drawing.Font("宋体", 12.08392F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_PSN.Location = new System.Drawing.Point(87, 10);
            this.lbl_PSN.Name = "lbl_PSN";
            this.lbl_PSN.Size = new System.Drawing.Size(38, 25);
            this.lbl_PSN.TabIndex = 3;
            this.lbl_PSN.Text = "00";
            // 
            // btn_Create
            // 
            this.btn_Create.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btn_Create.Font = new System.Drawing.Font("隶书", 10.57343F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Create.Location = new System.Drawing.Point(15, 218);
            this.btn_Create.Name = "btn_Create";
            this.btn_Create.Size = new System.Drawing.Size(120, 53);
            this.btn_Create.TabIndex = 4;
            this.btn_Create.Text = "新建作业";
            this.btn_Create.UseVisualStyleBackColor = false;
            this.btn_Create.Click += new System.EventHandler(this.btn_Create_Click);
            // 
            // pnl_ShowInfo
            // 
            this.pnl_ShowInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.pnl_ShowInfo.Controls.Add(this.lbl_UseTime);
            this.pnl_ShowInfo.Controls.Add(this.label2);
            this.pnl_ShowInfo.Controls.Add(this.lbl_PV);
            this.pnl_ShowInfo.Controls.Add(this.label3);
            this.pnl_ShowInfo.Controls.Add(this.label1);
            this.pnl_ShowInfo.Controls.Add(this.lbl_PSN);
            this.pnl_ShowInfo.Location = new System.Drawing.Point(857, 41);
            this.pnl_ShowInfo.Name = "pnl_ShowInfo";
            this.pnl_ShowInfo.Size = new System.Drawing.Size(382, 215);
            this.pnl_ShowInfo.TabIndex = 8;
            // 
            // lbl_UseTime
            // 
            this.lbl_UseTime.AutoSize = true;
            this.lbl_UseTime.Font = new System.Drawing.Font("宋体", 12.08392F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_UseTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lbl_UseTime.Location = new System.Drawing.Point(263, 10);
            this.lbl_UseTime.Name = "lbl_UseTime";
            this.lbl_UseTime.Size = new System.Drawing.Size(116, 25);
            this.lbl_UseTime.TabIndex = 5;
            this.lbl_UseTime.Text = "00:00:00";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12.08392F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(187, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 25);
            this.label3.TabIndex = 4;
            this.label3.Text = "用时:";
            // 
            // txt_batchNo
            // 
            this.txt_batchNo.BackColor = System.Drawing.SystemColors.Info;
            this.txt_batchNo.Font = new System.Drawing.Font("宋体", 12.08392F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_batchNo.Location = new System.Drawing.Point(159, 30);
            this.txt_batchNo.Name = "txt_batchNo";
            this.txt_batchNo.Size = new System.Drawing.Size(231, 35);
            this.txt_batchNo.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12.08392F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(14, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 25);
            this.label4.TabIndex = 11;
            this.label4.Text = "批次号：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 12.08392F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(14, 92);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(137, 25);
            this.label5.TabIndex = 12;
            this.label5.Text = "品规编码：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 12.08392F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(14, 146);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(137, 25);
            this.label6.TabIndex = 13;
            this.label6.Text = "品规名称：";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 9.062937F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(12, 336);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 18);
            this.label7.TabIndex = 14;
            this.label7.Text = "操作员：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 12.08392F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(11, 33);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(137, 25);
            this.label8.TabIndex = 15;
            this.label8.Text = "开始时间：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 12.08392F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(11, 87);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(137, 25);
            this.label9.TabIndex = 16;
            this.label9.Text = "结束时间：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 12.08392F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(14, 145);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(138, 25);
            this.label10.TabIndex = 17;
            this.label10.Text = "长度(米)：";
            // 
            // txt_ItemNo
            // 
            this.txt_ItemNo.BackColor = System.Drawing.SystemColors.Info;
            this.txt_ItemNo.Font = new System.Drawing.Font("宋体", 12.08392F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_ItemNo.Location = new System.Drawing.Point(159, 85);
            this.txt_ItemNo.Name = "txt_ItemNo";
            this.txt_ItemNo.Size = new System.Drawing.Size(231, 35);
            this.txt_ItemNo.TabIndex = 18;
            // 
            // txt_ItemName
            // 
            this.txt_ItemName.BackColor = System.Drawing.SystemColors.Info;
            this.txt_ItemName.Font = new System.Drawing.Font("宋体", 12.08392F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_ItemName.Location = new System.Drawing.Point(159, 140);
            this.txt_ItemName.Name = "txt_ItemName";
            this.txt_ItemName.Size = new System.Drawing.Size(231, 35);
            this.txt_ItemName.TabIndex = 19;
            // 
            // txt_StartTime
            // 
            this.txt_StartTime.BackColor = System.Drawing.SystemColors.Info;
            this.txt_StartTime.Font = new System.Drawing.Font("宋体", 12.08392F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_StartTime.Location = new System.Drawing.Point(158, 26);
            this.txt_StartTime.Name = "txt_StartTime";
            this.txt_StartTime.Size = new System.Drawing.Size(231, 35);
            this.txt_StartTime.TabIndex = 21;
            // 
            // txt_Rongyu
            // 
            this.txt_Rongyu.BackColor = System.Drawing.SystemColors.Info;
            this.txt_Rongyu.Font = new System.Drawing.Font("宋体", 12.08392F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_Rongyu.Location = new System.Drawing.Point(158, 197);
            this.txt_Rongyu.Name = "txt_Rongyu";
            this.txt_Rongyu.Size = new System.Drawing.Size(231, 35);
            this.txt_Rongyu.TabIndex = 22;
            // 
            // txt_EndTime
            // 
            this.txt_EndTime.BackColor = System.Drawing.SystemColors.Info;
            this.txt_EndTime.Font = new System.Drawing.Font("宋体", 12.08392F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_EndTime.Location = new System.Drawing.Point(158, 81);
            this.txt_EndTime.Name = "txt_EndTime";
            this.txt_EndTime.Size = new System.Drawing.Size(231, 35);
            this.txt_EndTime.TabIndex = 22;
            // 
            // txt_SumLength
            // 
            this.txt_SumLength.BackColor = System.Drawing.SystemColors.Info;
            this.txt_SumLength.Font = new System.Drawing.Font("宋体", 12.08392F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_SumLength.Location = new System.Drawing.Point(158, 140);
            this.txt_SumLength.Name = "txt_SumLength";
            this.txt_SumLength.Size = new System.Drawing.Size(231, 35);
            this.txt_SumLength.TabIndex = 23;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 12.08392F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(17, 203);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(112, 25);
            this.label11.TabIndex = 24;
            this.label11.Text = "冗余值：";
            // 
            // btn_Compelte
            // 
            this.btn_Compelte.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btn_Compelte.Font = new System.Drawing.Font("隶书", 10.57343F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Compelte.Location = new System.Drawing.Point(295, 218);
            this.btn_Compelte.Name = "btn_Compelte";
            this.btn_Compelte.Size = new System.Drawing.Size(120, 53);
            this.btn_Compelte.TabIndex = 26;
            this.btn_Compelte.Text = "完成作业";
            this.btn_Compelte.UseVisualStyleBackColor = false;
            this.btn_Compelte.Click += new System.EventHandler(this.btn_Compelte_Click);
            // 
            // btn_Histroy
            // 
            this.btn_Histroy.BackColor = System.Drawing.Color.Gray;
            this.btn_Histroy.Font = new System.Drawing.Font("宋体", 10.57343F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Histroy.ForeColor = System.Drawing.Color.White;
            this.btn_Histroy.Location = new System.Drawing.Point(433, 289);
            this.btn_Histroy.Name = "btn_Histroy";
            this.btn_Histroy.Size = new System.Drawing.Size(150, 39);
            this.btn_Histroy.TabIndex = 27;
            this.btn_Histroy.Text = "历史查询";
            this.btn_Histroy.UseVisualStyleBackColor = false;
            this.btn_Histroy.Click += new System.EventHandler(this.btn_Histroy_Click);
            // 
            // btn_SaveRecord
            // 
            this.btn_SaveRecord.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_SaveRecord.Font = new System.Drawing.Font("宋体", 10.57343F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_SaveRecord.ForeColor = System.Drawing.Color.White;
            this.btn_SaveRecord.Location = new System.Drawing.Point(295, 289);
            this.btn_SaveRecord.Name = "btn_SaveRecord";
            this.btn_SaveRecord.Size = new System.Drawing.Size(120, 39);
            this.btn_SaveRecord.TabIndex = 29;
            this.btn_SaveRecord.Text = "结束存档";
            this.btn_SaveRecord.UseVisualStyleBackColor = false;
            this.btn_SaveRecord.Click += new System.EventHandler(this.btn_SaveRecord_Click);
            // 
            // btn_Contiue
            // 
            this.btn_Contiue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btn_Contiue.Font = new System.Drawing.Font("隶书", 10.57343F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Contiue.Location = new System.Drawing.Point(154, 218);
            this.btn_Contiue.Name = "btn_Contiue";
            this.btn_Contiue.Size = new System.Drawing.Size(120, 53);
            this.btn_Contiue.TabIndex = 30;
            this.btn_Contiue.Text = "开始作业";
            this.btn_Contiue.UseVisualStyleBackColor = false;
            this.btn_Contiue.Click += new System.EventHandler(this.btn_Contiue_Click);
            // 
            // lbl_Operator
            // 
            this.lbl_Operator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbl_Operator.AutoSize = true;
            this.lbl_Operator.Font = new System.Drawing.Font("宋体", 9.062937F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_Operator.Location = new System.Drawing.Point(82, 336);
            this.lbl_Operator.Name = "lbl_Operator";
            this.lbl_Operator.Size = new System.Drawing.Size(53, 18);
            this.lbl_Operator.TabIndex = 31;
            this.lbl_Operator.Text = "admin";
            // 
            // gbx_ItemInfo
            // 
            this.gbx_ItemInfo.Controls.Add(this.label4);
            this.gbx_ItemInfo.Controls.Add(this.txt_ItemNo);
            this.gbx_ItemInfo.Controls.Add(this.txt_batchNo);
            this.gbx_ItemInfo.Controls.Add(this.txt_ItemName);
            this.gbx_ItemInfo.Controls.Add(this.label5);
            this.gbx_ItemInfo.Controls.Add(this.label6);
            this.gbx_ItemInfo.Location = new System.Drawing.Point(14, 12);
            this.gbx_ItemInfo.Name = "gbx_ItemInfo";
            this.gbx_ItemInfo.Size = new System.Drawing.Size(403, 193);
            this.gbx_ItemInfo.TabIndex = 32;
            this.gbx_ItemInfo.TabStop = false;
            this.gbx_ItemInfo.Text = "品规信息";
            // 
            // gbx_ShowInfo
            // 
            this.gbx_ShowInfo.Controls.Add(this.txt_SumLength);
            this.gbx_ShowInfo.Controls.Add(this.label8);
            this.gbx_ShowInfo.Controls.Add(this.label9);
            this.gbx_ShowInfo.Controls.Add(this.txt_EndTime);
            this.gbx_ShowInfo.Controls.Add(this.label11);
            this.gbx_ShowInfo.Controls.Add(this.txt_Rongyu);
            this.gbx_ShowInfo.Controls.Add(this.label10);
            this.gbx_ShowInfo.Controls.Add(this.txt_StartTime);
            this.gbx_ShowInfo.Location = new System.Drawing.Point(435, 12);
            this.gbx_ShowInfo.Name = "gbx_ShowInfo";
            this.gbx_ShowInfo.Size = new System.Drawing.Size(403, 259);
            this.gbx_ShowInfo.TabIndex = 33;
            this.gbx_ShowInfo.TabStop = false;
            this.gbx_ShowInfo.Text = "统计结果";
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 7.552447F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.ForeColor = System.Drawing.Color.Navy;
            this.label13.Location = new System.Drawing.Point(75, 13);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(103, 15);
            this.label13.TabIndex = 34;
            this.label13.Text = "Version 1.00";
            // 
            // but_Setting
            // 
            this.but_Setting.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.but_Setting.Font = new System.Drawing.Font("宋体", 10.57343F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.but_Setting.ForeColor = System.Drawing.Color.White;
            this.but_Setting.Location = new System.Drawing.Point(154, 289);
            this.but_Setting.Name = "but_Setting";
            this.but_Setting.Size = new System.Drawing.Size(120, 39);
            this.but_Setting.TabIndex = 35;
            this.but_Setting.Text = "参数设置";
            this.but_Setting.UseVisualStyleBackColor = false;
            this.but_Setting.Visible = false;
            this.but_Setting.Click += new System.EventHandler(this.but_Setting_Click);
            // 
            // btn_conn
            // 
            this.btn_conn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_conn.Font = new System.Drawing.Font("宋体", 10.57343F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_conn.ForeColor = System.Drawing.Color.White;
            this.btn_conn.Location = new System.Drawing.Point(15, 289);
            this.btn_conn.Name = "btn_conn";
            this.btn_conn.Size = new System.Drawing.Size(120, 39);
            this.btn_conn.TabIndex = 36;
            this.btn_conn.Text = "连接设备";
            this.btn_conn.UseVisualStyleBackColor = false;
            this.btn_conn.Click += new System.EventHandler(this.btn_conn_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(66, 65);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 37;
            this.pictureBox1.TabStop = false;
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("宋体", 7.552447F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.label14.Location = new System.Drawing.Point(75, 40);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(103, 15);
            this.label14.TabIndex = 38;
            this.label14.Text = "@2015 BAODAI";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Location = new System.Drawing.Point(643, 281);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(195, 73);
            this.panel1.TabIndex = 39;
            // 
            // Frm_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(848, 362);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btn_conn);
            this.Controls.Add(this.but_Setting);
            this.Controls.Add(this.pnl_ShowInfo);
            this.Controls.Add(this.gbx_ItemInfo);
            this.Controls.Add(this.lbl_Operator);
            this.Controls.Add(this.btn_Contiue);
            this.Controls.Add(this.btn_SaveRecord);
            this.Controls.Add(this.btn_Histroy);
            this.Controls.Add(this.btn_Compelte);
            this.Controls.Add(this.btn_Create);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.gbx_ShowInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Frm_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "《计米器管理工具1.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_Main_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Frm_Main_FormClosed);
            this.Load += new System.EventHandler(this.Frm_Main_Load);
            this.pnl_ShowInfo.ResumeLayout(false);
            this.pnl_ShowInfo.PerformLayout();
            this.gbx_ItemInfo.ResumeLayout(false);
            this.gbx_ItemInfo.PerformLayout();
            this.gbx_ShowInfo.ResumeLayout(false);
            this.gbx_ShowInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer Timer_ReadData;
        private System.Windows.Forms.Label lbl_PV;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_PSN;
        private System.Windows.Forms.Button btn_Create;
        private System.Windows.Forms.Panel pnl_ShowInfo;
        private System.Windows.Forms.Label lbl_UseTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_batchNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txt_ItemNo;
        private System.Windows.Forms.TextBox txt_ItemName;
        private System.Windows.Forms.TextBox txt_StartTime;
        private System.Windows.Forms.TextBox txt_Rongyu;
        private System.Windows.Forms.TextBox txt_EndTime;
        private System.Windows.Forms.TextBox txt_SumLength;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btn_Compelte;
        private System.Windows.Forms.Button btn_Histroy;
        private System.Windows.Forms.Button btn_SaveRecord;
        private System.Windows.Forms.Button btn_Contiue;
        private System.Windows.Forms.Label lbl_Operator;
        private System.Windows.Forms.GroupBox gbx_ItemInfo;
        private System.Windows.Forms.GroupBox gbx_ShowInfo;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button but_Setting;
        private System.Windows.Forms.Button btn_conn;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Panel panel1;
    }
}
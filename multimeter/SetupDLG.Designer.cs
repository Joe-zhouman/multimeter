namespace multimeter
{
    partial class SetupDLG
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
            this.skinGroupBox1 = new CCWin.SkinControl.SkinGroupBox();
            this.skinLabel10 = new CCWin.SkinControl.SkinLabel();
            this.combox_parity = new CCWin.SkinControl.SkinComboBox();
            this.skinLabel6 = new CCWin.SkinControl.SkinLabel();
            this.combox_stopbits = new CCWin.SkinControl.SkinComboBox();
            this.skinLabel1 = new CCWin.SkinControl.SkinLabel();
            this.combox_databits = new CCWin.SkinControl.SkinComboBox();
            this.combox_comport = new CCWin.SkinControl.SkinComboBox();
            this.combox_baudrate = new CCWin.SkinControl.SkinComboBox();
            this.skinLabel2 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel3 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel4 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel5 = new CCWin.SkinControl.SkinLabel();
            this.combox_card1 = new CCWin.SkinControl.SkinComboBox();
            this.combox_card2 = new CCWin.SkinControl.SkinComboBox();
            this.listview_card1 = new CCWin.SkinControl.SkinListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.combox_func = new CCWin.SkinControl.SkinComboBox();
            this.listview_card2 = new CCWin.SkinControl.SkinListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.combox_func2 = new CCWin.SkinControl.SkinComboBox();
            this.skinLabel7 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel8 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel9 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel11 = new CCWin.SkinControl.SkinLabel();
            this.skinGroupBox2 = new CCWin.SkinControl.SkinGroupBox();
            this.edit_save_interval = new System.Windows.Forms.TextBox();
            this.edit_scan_interval = new System.Windows.Forms.TextBox();
            this.skinGroupBox1.SuspendLayout();
            this.skinGroupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // skinGroupBox1
            // 
            this.skinGroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.skinGroupBox1.BorderColor = System.Drawing.Color.LimeGreen;
            this.skinGroupBox1.Controls.Add(this.skinLabel10);
            this.skinGroupBox1.Controls.Add(this.combox_parity);
            this.skinGroupBox1.Controls.Add(this.skinLabel6);
            this.skinGroupBox1.Controls.Add(this.combox_stopbits);
            this.skinGroupBox1.Controls.Add(this.skinLabel1);
            this.skinGroupBox1.Controls.Add(this.combox_databits);
            this.skinGroupBox1.Controls.Add(this.combox_comport);
            this.skinGroupBox1.Controls.Add(this.combox_baudrate);
            this.skinGroupBox1.Controls.Add(this.skinLabel2);
            this.skinGroupBox1.Controls.Add(this.skinLabel3);
            this.skinGroupBox1.ForeColor = System.Drawing.Color.Blue;
            this.skinGroupBox1.Location = new System.Drawing.Point(21, 20);
            this.skinGroupBox1.Name = "skinGroupBox1";
            this.skinGroupBox1.RectBackColor = System.Drawing.Color.White;
            this.skinGroupBox1.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinGroupBox1.Size = new System.Drawing.Size(207, 253);
            this.skinGroupBox1.TabIndex = 27;
            this.skinGroupBox1.TabStop = false;
            this.skinGroupBox1.Text = "串口设置";
            this.skinGroupBox1.TitleBorderColor = System.Drawing.Color.LimeGreen;
            this.skinGroupBox1.TitleRectBackColor = System.Drawing.Color.White;
            this.skinGroupBox1.TitleRoundStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // skinLabel10
            // 
            this.skinLabel10.AutoSize = true;
            this.skinLabel10.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel10.BorderColor = System.Drawing.Color.White;
            this.skinLabel10.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel10.Location = new System.Drawing.Point(27, 37);
            this.skinLabel10.Name = "skinLabel10";
            this.skinLabel10.Size = new System.Drawing.Size(51, 20);
            this.skinLabel10.TabIndex = 4;
            this.skinLabel10.Text = "串口：";
            // 
            // combox_parity
            // 
            this.combox_parity.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.combox_parity.FormattingEnabled = true;
            this.combox_parity.Items.AddRange(new object[] {
            "None",
            "奇校验",
            "偶校验",
            "Mark",
            "Space"});
            this.combox_parity.Location = new System.Drawing.Point(98, 190);
            this.combox_parity.Name = "combox_parity";
            this.combox_parity.Size = new System.Drawing.Size(95, 22);
            this.combox_parity.TabIndex = 6;
            this.combox_parity.WaterText = "";
            this.combox_parity.SelectedValueChanged += new System.EventHandler(this.combox_parity_SelectedValueChanged);
            // 
            // skinLabel6
            // 
            this.skinLabel6.AutoSize = true;
            this.skinLabel6.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel6.BorderColor = System.Drawing.Color.White;
            this.skinLabel6.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel6.Location = new System.Drawing.Point(27, 74);
            this.skinLabel6.Name = "skinLabel6";
            this.skinLabel6.Size = new System.Drawing.Size(65, 20);
            this.skinLabel6.TabIndex = 3;
            this.skinLabel6.Text = "波特率：";
            // 
            // combox_stopbits
            // 
            this.combox_stopbits.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.combox_stopbits.FormattingEnabled = true;
            this.combox_stopbits.Items.AddRange(new object[] {
            "1",
            "1.5",
            "2"});
            this.combox_stopbits.Location = new System.Drawing.Point(98, 148);
            this.combox_stopbits.Name = "combox_stopbits";
            this.combox_stopbits.Size = new System.Drawing.Size(95, 22);
            this.combox_stopbits.TabIndex = 6;
            this.combox_stopbits.WaterText = "";
            this.combox_stopbits.SelectedValueChanged += new System.EventHandler(this.combox_stopbits_SelectedValueChanged);
            // 
            // skinLabel1
            // 
            this.skinLabel1.AutoSize = true;
            this.skinLabel1.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel1.BorderColor = System.Drawing.Color.White;
            this.skinLabel1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel1.Location = new System.Drawing.Point(27, 114);
            this.skinLabel1.Name = "skinLabel1";
            this.skinLabel1.Size = new System.Drawing.Size(65, 20);
            this.skinLabel1.TabIndex = 3;
            this.skinLabel1.Text = "数据位：";
            // 
            // combox_databits
            // 
            this.combox_databits.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.combox_databits.FormattingEnabled = true;
            this.combox_databits.Items.AddRange(new object[] {
            "8",
            "7",
            "6",
            "5"});
            this.combox_databits.Location = new System.Drawing.Point(98, 109);
            this.combox_databits.Name = "combox_databits";
            this.combox_databits.Size = new System.Drawing.Size(95, 22);
            this.combox_databits.TabIndex = 6;
            this.combox_databits.WaterText = "";
            this.combox_databits.SelectedValueChanged += new System.EventHandler(this.combox_databits_SelectedValueChanged);
            // 
            // combox_comport
            // 
            this.combox_comport.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.combox_comport.FormattingEnabled = true;
            this.combox_comport.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6",
            "COM7",
            "COM8",
            "COM9"});
            this.combox_comport.Location = new System.Drawing.Point(98, 34);
            this.combox_comport.Name = "combox_comport";
            this.combox_comport.Size = new System.Drawing.Size(95, 22);
            this.combox_comport.TabIndex = 6;
            this.combox_comport.WaterText = "";
            this.combox_comport.SelectedValueChanged += new System.EventHandler(this.combox_comport_SelectedValueChanged);
            // 
            // combox_baudrate
            // 
            this.combox_baudrate.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.combox_baudrate.FormattingEnabled = true;
            this.combox_baudrate.Items.AddRange(new object[] {
            "4800",
            "9600",
            "14400",
            "19200",
            "38400",
            "56000",
            "115200"});
            this.combox_baudrate.Location = new System.Drawing.Point(98, 69);
            this.combox_baudrate.Name = "combox_baudrate";
            this.combox_baudrate.Size = new System.Drawing.Size(95, 22);
            this.combox_baudrate.TabIndex = 6;
            this.combox_baudrate.WaterText = "";
            this.combox_baudrate.SelectedValueChanged += new System.EventHandler(this.combox_baudrate_SelectedValueChanged);
            // 
            // skinLabel2
            // 
            this.skinLabel2.AutoSize = true;
            this.skinLabel2.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel2.BorderColor = System.Drawing.Color.White;
            this.skinLabel2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel2.Location = new System.Drawing.Point(27, 153);
            this.skinLabel2.Name = "skinLabel2";
            this.skinLabel2.Size = new System.Drawing.Size(65, 20);
            this.skinLabel2.TabIndex = 3;
            this.skinLabel2.Text = "停止位：";
            // 
            // skinLabel3
            // 
            this.skinLabel3.AutoSize = true;
            this.skinLabel3.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel3.BorderColor = System.Drawing.Color.White;
            this.skinLabel3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel3.Location = new System.Drawing.Point(27, 190);
            this.skinLabel3.Name = "skinLabel3";
            this.skinLabel3.Size = new System.Drawing.Size(51, 20);
            this.skinLabel3.TabIndex = 3;
            this.skinLabel3.Text = "校验：";
            // 
            // skinLabel4
            // 
            this.skinLabel4.AutoSize = true;
            this.skinLabel4.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel4.BorderColor = System.Drawing.Color.White;
            this.skinLabel4.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel4.Location = new System.Drawing.Point(321, 25);
            this.skinLabel4.Name = "skinLabel4";
            this.skinLabel4.Size = new System.Drawing.Size(59, 20);
            this.skinLabel4.TabIndex = 29;
            this.skinLabel4.Text = "插槽1：";
            // 
            // skinLabel5
            // 
            this.skinLabel5.AutoSize = true;
            this.skinLabel5.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel5.BorderColor = System.Drawing.Color.White;
            this.skinLabel5.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel5.Location = new System.Drawing.Point(658, 27);
            this.skinLabel5.Name = "skinLabel5";
            this.skinLabel5.Size = new System.Drawing.Size(59, 20);
            this.skinLabel5.TabIndex = 29;
            this.skinLabel5.Text = "插槽2：";
            // 
            // combox_card1
            // 
            this.combox_card1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.combox_card1.FormattingEnabled = true;
            this.combox_card1.Items.AddRange(new object[] {
            "无",
            "7700"});
            this.combox_card1.Location = new System.Drawing.Point(393, 24);
            this.combox_card1.Name = "combox_card1";
            this.combox_card1.Size = new System.Drawing.Size(121, 22);
            this.combox_card1.TabIndex = 30;
            this.combox_card1.WaterText = "";
            this.combox_card1.SelectedValueChanged += new System.EventHandler(this.combox_card1_SelectedValueChanged);
            // 
            // combox_card2
            // 
            this.combox_card2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.combox_card2.FormattingEnabled = true;
            this.combox_card2.Items.AddRange(new object[] {
            "无",
            "7700"});
            this.combox_card2.Location = new System.Drawing.Point(726, 25);
            this.combox_card2.Name = "combox_card2";
            this.combox_card2.Size = new System.Drawing.Size(121, 22);
            this.combox_card2.TabIndex = 30;
            this.combox_card2.WaterText = "";
            this.combox_card2.SelectedValueChanged += new System.EventHandler(this.combox_card2_SelectedValueChanged);
            // 
            // listview_card1
            // 
            this.listview_card1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listview_card1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listview_card1.FullRowSelect = true;
            this.listview_card1.GridLines = true;
            this.listview_card1.Location = new System.Drawing.Point(276, 57);
            this.listview_card1.Name = "listview_card1";
            this.listview_card1.OwnerDraw = true;
            this.listview_card1.Size = new System.Drawing.Size(310, 436);
            this.listview_card1.TabIndex = 31;
            this.listview_card1.UseCompatibleStateImageBehavior = false;
            this.listview_card1.View = System.Windows.Forms.View.Details;
            this.listview_card1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listview_card1_MouseUp);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "   通道";
            this.columnHeader1.Width = 65;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "名称";
            this.columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "功能";
            this.columnHeader3.Width = 80;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "图形";
            this.columnHeader4.Width = 80;
            // 
            // combox_func
            // 
            this.combox_func.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.combox_func.FormattingEnabled = true;
            this.combox_func.Items.AddRange(new object[] {
            "空",
            "两线电阻",
            "四线电阻",
            "热电偶"});
            this.combox_func.Location = new System.Drawing.Point(160, 376);
            this.combox_func.Name = "combox_func";
            this.combox_func.Size = new System.Drawing.Size(93, 22);
            this.combox_func.TabIndex = 34;
            this.combox_func.Visible = false;
            this.combox_func.WaterText = "";
            this.combox_func.DropDownClosed += new System.EventHandler(this.combox_func_DropDownClosed);
            this.combox_func.TextChanged += new System.EventHandler(this.combox_func_TextChanged);
            // 
            // listview_card2
            // 
            this.listview_card2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.listview_card2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listview_card2.FullRowSelect = true;
            this.listview_card2.GridLines = true;
            this.listview_card2.Location = new System.Drawing.Point(611, 57);
            this.listview_card2.Name = "listview_card2";
            this.listview_card2.OwnerDraw = true;
            this.listview_card2.Size = new System.Drawing.Size(311, 436);
            this.listview_card2.TabIndex = 31;
            this.listview_card2.UseCompatibleStateImageBehavior = false;
            this.listview_card2.View = System.Windows.Forms.View.Details;
            this.listview_card2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listview_card2_MouseUp);
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "   通道";
            this.columnHeader5.Width = 65;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "名称";
            this.columnHeader6.Width = 80;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "功能";
            this.columnHeader7.Width = 80;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "图形";
            this.columnHeader8.Width = 80;
            // 
            // combox_func2
            // 
            this.combox_func2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.combox_func2.FormattingEnabled = true;
            this.combox_func2.Items.AddRange(new object[] {
            "空",
            "两线电阻",
            "四线电阻",
            "热电偶"});
            this.combox_func2.Location = new System.Drawing.Point(160, 427);
            this.combox_func2.Name = "combox_func2";
            this.combox_func2.Size = new System.Drawing.Size(93, 22);
            this.combox_func2.TabIndex = 34;
            this.combox_func2.Visible = false;
            this.combox_func2.WaterText = "";
            this.combox_func2.DropDownClosed += new System.EventHandler(this.combox_func2_DropDownClosed);
            this.combox_func2.TextChanged += new System.EventHandler(this.combox_func2_TextChanged);
            // 
            // skinLabel7
            // 
            this.skinLabel7.AutoSize = true;
            this.skinLabel7.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel7.BorderColor = System.Drawing.Color.White;
            this.skinLabel7.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel7.Location = new System.Drawing.Point(26, 297);
            this.skinLabel7.Name = "skinLabel7";
            this.skinLabel7.Size = new System.Drawing.Size(79, 20);
            this.skinLabel7.TabIndex = 29;
            this.skinLabel7.Text = "扫描间隔：";
            // 
            // skinLabel8
            // 
            this.skinLabel8.AutoSize = true;
            this.skinLabel8.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel8.BorderColor = System.Drawing.Color.White;
            this.skinLabel8.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel8.Location = new System.Drawing.Point(176, 297);
            this.skinLabel8.Name = "skinLabel8";
            this.skinLabel8.Size = new System.Drawing.Size(37, 20);
            this.skinLabel8.TabIndex = 29;
            this.skinLabel8.Text = "毫秒";
            // 
            // skinLabel9
            // 
            this.skinLabel9.AutoSize = true;
            this.skinLabel9.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel9.BorderColor = System.Drawing.Color.White;
            this.skinLabel9.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel9.Location = new System.Drawing.Point(28, 343);
            this.skinLabel9.Name = "skinLabel9";
            this.skinLabel9.Size = new System.Drawing.Size(37, 20);
            this.skinLabel9.TabIndex = 29;
            this.skinLabel9.Text = "每隔";
            // 
            // skinLabel11
            // 
            this.skinLabel11.AutoSize = true;
            this.skinLabel11.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel11.BorderColor = System.Drawing.Color.White;
            this.skinLabel11.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel11.Location = new System.Drawing.Point(126, 343);
            this.skinLabel11.Name = "skinLabel11";
            this.skinLabel11.Size = new System.Drawing.Size(107, 20);
            this.skinLabel11.TabIndex = 29;
            this.skinLabel11.Text = "次扫描保存数据";
            // 
            // skinGroupBox2
            // 
            this.skinGroupBox2.BackColor = System.Drawing.Color.Transparent;
            this.skinGroupBox2.BorderColor = System.Drawing.Color.Lime;
            this.skinGroupBox2.Controls.Add(this.edit_save_interval);
            this.skinGroupBox2.Controls.Add(this.edit_scan_interval);
            this.skinGroupBox2.Controls.Add(this.skinGroupBox1);
            this.skinGroupBox2.Controls.Add(this.listview_card1);
            this.skinGroupBox2.Controls.Add(this.combox_card1);
            this.skinGroupBox2.Controls.Add(this.skinLabel4);
            this.skinGroupBox2.Controls.Add(this.combox_card2);
            this.skinGroupBox2.Controls.Add(this.skinLabel5);
            this.skinGroupBox2.Controls.Add(this.skinLabel7);
            this.skinGroupBox2.Controls.Add(this.combox_func2);
            this.skinGroupBox2.Controls.Add(this.skinLabel11);
            this.skinGroupBox2.Controls.Add(this.skinLabel9);
            this.skinGroupBox2.Controls.Add(this.listview_card2);
            this.skinGroupBox2.Controls.Add(this.combox_func);
            this.skinGroupBox2.Controls.Add(this.skinLabel8);
            this.skinGroupBox2.ForeColor = System.Drawing.Color.Blue;
            this.skinGroupBox2.Location = new System.Drawing.Point(9, 42);
            this.skinGroupBox2.Name = "skinGroupBox2";
            this.skinGroupBox2.RectBackColor = System.Drawing.Color.White;
            this.skinGroupBox2.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinGroupBox2.Size = new System.Drawing.Size(945, 499);
            this.skinGroupBox2.TabIndex = 37;
            this.skinGroupBox2.TabStop = false;
            this.skinGroupBox2.TitleBorderColor = System.Drawing.Color.Red;
            this.skinGroupBox2.TitleRectBackColor = System.Drawing.Color.White;
            this.skinGroupBox2.TitleRoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinGroupBox2.Enter += new System.EventHandler(this.skinGroupBox2_Enter);
            // 
            // edit_save_interval
            // 
            this.edit_save_interval.Location = new System.Drawing.Point(71, 342);
            this.edit_save_interval.Name = "edit_save_interval";
            this.edit_save_interval.Size = new System.Drawing.Size(49, 21);
            this.edit_save_interval.TabIndex = 35;
            this.edit_save_interval.TextChanged += new System.EventHandler(this.edit_save_interval_TextChanged);
            // 
            // edit_scan_interval
            // 
            this.edit_scan_interval.Location = new System.Drawing.Point(98, 297);
            this.edit_scan_interval.Name = "edit_scan_interval";
            this.edit_scan_interval.Size = new System.Drawing.Size(72, 21);
            this.edit_scan_interval.TabIndex = 35;
            this.edit_scan_interval.TextChanged += new System.EventHandler(this.edit_scan_interval_TextChanged);
            // 
            // SetupDLG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1035, 583);
            this.Controls.Add(this.skinGroupBox2);
            this.Name = "SetupDLG";
            this.Text = "SetupDLG";
            this.Load += new System.EventHandler(this.SetupDLG_Load);
            this.skinGroupBox1.ResumeLayout(false);
            this.skinGroupBox1.PerformLayout();
            this.skinGroupBox2.ResumeLayout(false);
            this.skinGroupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CCWin.SkinControl.SkinGroupBox skinGroupBox1;
        private CCWin.SkinControl.SkinLabel skinLabel10;
        private CCWin.SkinControl.SkinComboBox combox_parity;
        private CCWin.SkinControl.SkinLabel skinLabel6;
        private CCWin.SkinControl.SkinComboBox combox_stopbits;
        private CCWin.SkinControl.SkinLabel skinLabel1;
        private CCWin.SkinControl.SkinComboBox combox_databits;
        private CCWin.SkinControl.SkinComboBox combox_comport;
        private CCWin.SkinControl.SkinComboBox combox_baudrate;
        private CCWin.SkinControl.SkinLabel skinLabel2;
        private CCWin.SkinControl.SkinLabel skinLabel3;
        private CCWin.SkinControl.SkinLabel skinLabel4;
        private CCWin.SkinControl.SkinLabel skinLabel5;
        private CCWin.SkinControl.SkinComboBox combox_card1;
        private CCWin.SkinControl.SkinComboBox combox_card2;
        private CCWin.SkinControl.SkinListView listview_card1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private CCWin.SkinControl.SkinComboBox combox_func;
        private CCWin.SkinControl.SkinListView listview_card2;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private CCWin.SkinControl.SkinComboBox combox_func2;
        private CCWin.SkinControl.SkinLabel skinLabel7;
        private CCWin.SkinControl.SkinLabel skinLabel8;
        private CCWin.SkinControl.SkinLabel skinLabel9;
        private CCWin.SkinControl.SkinLabel skinLabel11;
        private CCWin.SkinControl.SkinGroupBox skinGroupBox2;
        private System.Windows.Forms.TextBox edit_save_interval;
        private System.Windows.Forms.TextBox edit_scan_interval;
    }
}
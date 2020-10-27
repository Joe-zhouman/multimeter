namespace multimeter
{
    partial class MainDLG
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btn_setup = new CCWin.SkinControl.SkinButton();
            this.btn_start = new CCWin.SkinControl.SkinButton();
            this.btn_stop = new CCWin.SkinControl.SkinButton();
            this.listView_main = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.LastScan = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_setup
            // 
            this.btn_setup.BackColor = System.Drawing.Color.Transparent;
            this.btn_setup.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btn_setup.DownBack = null;
            this.btn_setup.Location = new System.Drawing.Point(0, 39);
            this.btn_setup.MouseBack = null;
            this.btn_setup.Name = "btn_setup";
            this.btn_setup.NormlBack = null;
            this.btn_setup.Size = new System.Drawing.Size(91, 30);
            this.btn_setup.TabIndex = 0;
            this.btn_setup.Text = "设置";
            this.btn_setup.UseVisualStyleBackColor = false;
            this.btn_setup.Click += new System.EventHandler(this.btn_setup_Click);
            // 
            // btn_start
            // 
            this.btn_start.BackColor = System.Drawing.Color.Transparent;
            this.btn_start.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btn_start.DownBack = null;
            this.btn_start.Location = new System.Drawing.Point(90, 39);
            this.btn_start.MouseBack = null;
            this.btn_start.Name = "btn_start";
            this.btn_start.NormlBack = null;
            this.btn_start.Size = new System.Drawing.Size(91, 30);
            this.btn_start.TabIndex = 0;
            this.btn_start.Text = "开始测量";
            this.btn_start.UseVisualStyleBackColor = false;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // btn_stop
            // 
            this.btn_stop.BackColor = System.Drawing.Color.Transparent;
            this.btn_stop.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btn_stop.DownBack = null;
            this.btn_stop.Enabled = false;
            this.btn_stop.Location = new System.Drawing.Point(181, 39);
            this.btn_stop.MouseBack = null;
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.NormlBack = null;
            this.btn_stop.Size = new System.Drawing.Size(91, 30);
            this.btn_stop.TabIndex = 0;
            this.btn_stop.Text = "停止测量";
            this.btn_stop.UseVisualStyleBackColor = false;
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click);
            // 
            // listView_main
            // 
            this.listView_main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView_main.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listView_main.GridLines = true;
            this.listView_main.Location = new System.Drawing.Point(6, 73);
            this.listView_main.Name = "listView_main";
            this.listView_main.Size = new System.Drawing.Size(1183, 597);
            this.listView_main.TabIndex = 1;
            this.listView_main.UseCompatibleStateImageBehavior = false;
            this.listView_main.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "序号";
            // 
            // serialPort1
            // 
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // LastScan
            // 
            this.LastScan.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LastScan.Location = new System.Drawing.Point(454, 44);
            this.LastScan.Name = "LastScan";
            this.LastScan.Size = new System.Drawing.Size(701, 26);
            this.LastScan.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Lime;
            this.label1.Location = new System.Drawing.Point(294, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "最近一次扫描结果：";
            // 
            // MainDLG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1211, 681);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LastScan);
            this.Controls.Add(this.listView_main);
            this.Controls.Add(this.btn_stop);
            this.Controls.Add(this.btn_start);
            this.Controls.Add(this.btn_setup);
            this.Name = "MainDLG";
            this.Text = "泰克2700";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CCWin.SkinControl.SkinButton btn_setup;
        private CCWin.SkinControl.SkinButton btn_start;
        private CCWin.SkinControl.SkinButton btn_stop;
        private System.Windows.Forms.ListView listView_main;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.TextBox LastScan;
        private System.Windows.Forms.Label label1;
    }
}


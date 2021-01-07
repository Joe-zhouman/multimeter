
namespace multimeter
{
    partial class LogoLoad
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogoLoad));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.LoadingLabel = new System.Windows.Forms.Label();
            this.LoadCompanyName = new System.Windows.Forms.Button();
            this.CompanyLogo = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CompanyLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 3000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.LoadingLabel);
            this.groupBox1.Controls.Add(this.LoadCompanyName);
            this.groupBox1.Controls.Add(this.CompanyLogo);
            this.groupBox1.Location = new System.Drawing.Point(-5, -5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(464, 451);
            this.groupBox1.TabIndex = 107;
            this.groupBox1.TabStop = false;
            // 
            // LoadingLabel
            // 
            this.LoadingLabel.BackColor = System.Drawing.Color.Transparent;
            this.LoadingLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LoadingLabel.Image = ((System.Drawing.Image)(resources.GetObject("LoadingLabel.Image")));
            this.LoadingLabel.Location = new System.Drawing.Point(33, 37);
            this.LoadingLabel.Name = "LoadingLabel";
            this.LoadingLabel.Size = new System.Drawing.Size(393, 339);
            this.LoadingLabel.TabIndex = 108;
            // 
            // LoadCompanyName
            // 
            this.LoadCompanyName.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("LoadCompanyName.BackgroundImage")));
            this.LoadCompanyName.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.LoadCompanyName.Enabled = false;
            this.LoadCompanyName.FlatAppearance.BorderSize = 0;
            this.LoadCompanyName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LoadCompanyName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LoadCompanyName.ForeColor = System.Drawing.Color.Black;
            this.LoadCompanyName.Location = new System.Drawing.Point(52, 344);
            this.LoadCompanyName.Name = "LoadCompanyName";
            this.LoadCompanyName.Size = new System.Drawing.Size(358, 58);
            this.LoadCompanyName.TabIndex = 113;
            this.LoadCompanyName.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.LoadCompanyName.UseVisualStyleBackColor = true;
            // 
            // CompanyLogo
            // 
            this.CompanyLogo.Image = ((System.Drawing.Image)(resources.GetObject("CompanyLogo.Image")));
            this.CompanyLogo.InitialImage = null;
            this.CompanyLogo.Location = new System.Drawing.Point(35, 20);
            this.CompanyLogo.Name = "CompanyLogo";
            this.CompanyLogo.Size = new System.Drawing.Size(393, 382);
            this.CompanyLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.CompanyLogo.TabIndex = 109;
            this.CompanyLogo.TabStop = false;
            // 
            // LogoLoad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(453, 440);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LogoLoad";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LogoLoad";
            this.Load += new System.EventHandler(this.LogoLoad_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CompanyLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button LoadCompanyName;
        private System.Windows.Forms.Label LoadingLabel;
        private System.Windows.Forms.PictureBox CompanyLogo;
    }
}
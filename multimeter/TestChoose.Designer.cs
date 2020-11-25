
namespace multimeter
{
    partial class TestChoose
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
            this.testchoose1 = new System.Windows.Forms.Button();
            this.testchoose2 = new System.Windows.Forms.Button();
            this.testchoose3 = new System.Windows.Forms.Button();
            this.testchoose4 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // testchoose1
            // 
            this.testchoose1.BackColor = System.Drawing.SystemColors.Control;
            this.testchoose1.Cursor = System.Windows.Forms.Cursors.PanSE;
            this.testchoose1.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.testchoose1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.testchoose1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.testchoose1.ForeColor = System.Drawing.Color.Black;
            this.testchoose1.Location = new System.Drawing.Point(12, 51);
            this.testchoose1.Name = "testchoose1";
            this.testchoose1.Size = new System.Drawing.Size(105, 198);
            this.testchoose1.TabIndex = 1;
            this.testchoose1.Text = "导热系数测量";
            this.testchoose1.UseVisualStyleBackColor = false;
            this.testchoose1.Click += new System.EventHandler(this.testchoose1_Click);
            // 
            // testchoose2
            // 
            this.testchoose2.BackColor = System.Drawing.SystemColors.Control;
            this.testchoose2.Cursor = System.Windows.Forms.Cursors.PanSE;
            this.testchoose2.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.testchoose2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.testchoose2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.testchoose2.ForeColor = System.Drawing.Color.Black;
            this.testchoose2.Location = new System.Drawing.Point(123, 51);
            this.testchoose2.Name = "testchoose2";
            this.testchoose2.Size = new System.Drawing.Size(122, 198);
            this.testchoose2.TabIndex = 2;
            this.testchoose2.Text = "固_固接触热阻测量";
            this.testchoose2.UseVisualStyleBackColor = false;
            this.testchoose2.Click += new System.EventHandler(this.testchoose2_Click);
            // 
            // testchoose3
            // 
            this.testchoose3.BackColor = System.Drawing.SystemColors.Control;
            this.testchoose3.Cursor = System.Windows.Forms.Cursors.PanSE;
            this.testchoose3.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.testchoose3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.testchoose3.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.testchoose3.ForeColor = System.Drawing.Color.Black;
            this.testchoose3.Location = new System.Drawing.Point(251, 51);
            this.testchoose3.Name = "testchoose3";
            this.testchoose3.Size = new System.Drawing.Size(105, 198);
            this.testchoose3.TabIndex = 3;
            this.testchoose3.Text = "热界面材料测量_热流计间";
            this.testchoose3.UseVisualStyleBackColor = false;
            this.testchoose3.Click += new System.EventHandler(this.testchoose3_Click);
            // 
            // testchoose4
            // 
            this.testchoose4.BackColor = System.Drawing.SystemColors.Control;
            this.testchoose4.Cursor = System.Windows.Forms.Cursors.PanSE;
            this.testchoose4.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.testchoose4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.testchoose4.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.testchoose4.ForeColor = System.Drawing.Color.Black;
            this.testchoose4.Location = new System.Drawing.Point(362, 51);
            this.testchoose4.Name = "testchoose4";
            this.testchoose4.Size = new System.Drawing.Size(99, 198);
            this.testchoose4.TabIndex = 90;
            this.testchoose4.Text = "热界面材料测量_试件间";
            this.testchoose4.UseVisualStyleBackColor = false;
            this.testchoose4.Click += new System.EventHandler(this.testchoose4_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 16F);
            this.label1.Location = new System.Drawing.Point(139, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 30);
            this.label1.TabIndex = 91;
            this.label1.Text = "请选择实验类型";
            // 
            // TestChoose
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(471, 272);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.testchoose4);
            this.Controls.Add(this.testchoose3);
            this.Controls.Add(this.testchoose2);
            this.Controls.Add(this.testchoose1);
            this.Name = "TestChoose";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "实验类型选项";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button testchoose1;
        private System.Windows.Forms.Button testchoose2;
        private System.Windows.Forms.Button testchoose3;
        private System.Windows.Forms.Button testchoose4;
        private System.Windows.Forms.Label label1;
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DataProcessor;
using System.IO;
using System.Runtime.CompilerServices;
using CCWin;
using CCWin.SkinControl;
using System.Runtime.InteropServices;
using System.Threading;

namespace multimeter {
    public partial class SetupTest {
        private void TestResult_Click(object sender, EventArgs e) {
            #region //数据结果
            if(latestIniFile == "") {
                OpenFileDialog file = new OpenFileDialog();
                file.Title = @"请选择测试的配置文件!";
                file.Filter = "配置文件(*.ini)|*.ini";
                file.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                file.ShowDialog();
                latestIniFile = file.FileName;
            }if(latestIniFile == "") {
                MessageBox.Show(@"请选择配置文件!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (latestDataFile == "") {
                OpenFileDialog file = new OpenFileDialog();
                file.Title = @"请选择测试的配置文件!";
                file.Filter = "数据文件(*.csv)|*.csv";
                file.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                file.ShowDialog();
                latestDataFile = file.FileName;
            }
            if (latestDataFile == "") {
                MessageBox.Show(@"请选择配置文件!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            chart1.BringToFront(); //将曲线表格放在最顶层
            chart1.Size = new Size(806, 439);
            switch (TestChoose) {
                case "Test1": {
                        //显示对应监视窗口TEST1
                        NoneGroupBox.Size = new Size(0, 0);
                        TextGroupbox1.Size = new Size(0, 0);
                        TextGroupbox2.Size = new Size(0, 0);
                        TextGroupbox3.Size = new Size(0, 0);
                        TextGroupbox4.Size = new Size(0, 0);
                        ViewGroupBox1.Size = new Size(1531, 966);
                        ViewGroupBox2.Size = new Size(0, 0);
                        ViewGroupBox3.Size = new Size(0, 0);
                        ViewGroupBox4.Size = new Size(0, 0);
                        chart1.Size = new Size(0, 0); //将曲线表格放在隐藏
                        ResultGroupBox.Size = new Size(0, 0);
                        skinGroupBox2.Size = new Size(0, 0);
                    }
                    break;
                case "Test2": {
                        //显示对应监视窗口TEST2
                        NoneGroupBox.Size = new Size(0, 0);
                        TextGroupbox1.Size = new Size(0, 0);
                        TextGroupbox2.Size = new Size(0, 0);
                        TextGroupbox3.Size = new Size(0, 0);
                        TextGroupbox4.Size = new Size(0, 0);
                        ViewGroupBox1.Size = new Size(0, 0);
                        ViewGroupBox2.Size = new Size(1531, 966);
                        ViewGroupBox3.Size = new Size(0, 0);
                        ViewGroupBox4.Size = new Size(0, 0);
                        ResultGroupBox.Size = new Size(0, 0);
                        skinGroupBox2.Size = new Size(0, 0);
                    }
                    break;
                case "Test3": {
                        //显示对应监视窗口TEST3
                        NoneGroupBox.Size = new Size(0, 0);
                        TextGroupbox1.Size = new Size(0, 0);
                        TextGroupbox2.Size = new Size(0, 0);
                        TextGroupbox3.Size = new Size(0, 0);
                        TextGroupbox4.Size = new Size(0, 0);
                        ViewGroupBox1.Size = new Size(0, 0);
                        ViewGroupBox2.Size = new Size(0, 0);
                        ViewGroupBox3.Size = new Size(1531, 966);
                        ViewGroupBox4.Size = new Size(0, 0);
                        ResultGroupBox.Size = new Size(0, 0);
                        skinGroupBox2.Size = new Size(0, 0);
                    }
                    break;
                case "Test4": {
                        //显示对应监视窗口TEST4
                        NoneGroupBox.Size = new Size(0, 0);
                        TextGroupbox1.Size = new Size(0, 0);
                        TextGroupbox2.Size = new Size(0, 0);
                        TextGroupbox3.Size = new Size(0, 0);
                        TextGroupbox4.Size = new Size(0, 0);
                        ViewGroupBox1.Size = new Size(0, 0);
                        ViewGroupBox2.Size = new Size(0, 0);
                        ViewGroupBox3.Size = new Size(0, 0);
                        ViewGroupBox4.Size = new Size(1531, 966);
                        ResultGroupBox.Size = new Size(0, 0);
                        skinGroupBox2.Size = new Size(0, 0);
                    }
                    break;
            }

            #endregion
        }
    }
}

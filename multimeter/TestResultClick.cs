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
using CCWin.SkinClass;

namespace multimeter {
    public partial class SetupTest  {
        private void TestResult_Click(object sender, EventArgs e) {
            #region //数据结果
            //测试
            latestIniFile = @"C:\Users\Joe\source\Joe-zhouman\multimeter\multimeter\bin\Debug\AutoSave\itc-2020-11-09-12.ini";
            latestDataFile = @"C:\Users\Joe\source\Joe-zhouman\multimeter\multimeter\bin\Debug\AutoSave\AutoSave-2020-11-09-12-17-45.5147.csv";
            SetIniFileName();
            if(latestIniFile == "") {
                MessageBox.Show(@"请选择配置文件!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            SetCsvFileName();
            if (latestDataFile == "") {
                MessageBox.Show(@"请选择数据文件!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            DataTable channelTable = new DataTable();
            var csvExp = Solution.ReadCsvFile(ref channelTable, latestDataFile);
            if (null != csvExp) {
                MessageBox.Show(@"请选择数据文件!"+"\n"+csvExp.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (_method.ToString().ToLower() != INIHelper.Read("TestMethod", "method", "", latestIniFile)) {
                MessageBox.Show(@"请选择对应的配置文件!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            var testResult = Solution.CalAve(channelTable);
            var csvType = (TestMethod) (int) testResult["TestMethod"];
            if (_method != csvType) {
                MessageBox.Show(@"请选择对应的配置文件!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            heatMeter1.SetTemp(testResult);
            heatMeter2.SetTemp(testResult);
            string force = INIHelper.Read("Pressure", "force", "1", latestIniFile);
            switch (_method) {
                case TestMethod.Kappa: {
                        //显示对应监视窗口TEST1       
                        double[] T = { 81.2, 70.1, 59.8, 50, 45, 44, 43, 40.9, 30, 20.1, 10.8, 0.9 };
                        double[] HMLocation = { 10, 11, 10, 4, 10, 9, 4, 4, 12, 10, 10 };
                        //DataResult.Test1DataProcess(T, HMLocation, 1256, 1256, 452.16, 300, 300, 10);

                    }
                    break;
                case TestMethod.ITC: {
                        //显示对应监视窗口TEST2
                        sample1.SetTemp(testResult);
                        sample2.SetTemp(testResult);
                    }
                    break;
                case TestMethod.ITM: {
                        //显示对应监视窗口TEST3
                        double[] T = { 81.2, 70.1, 59.8, 50, 45, 44, 43, 40, 37, 35, 30, 20.1, 10.8, 0.9 };
                        double[] HMLocation = { 10, 11, 10, 4, 10, 9, 4, 4, 9, 19, 4, 12, 10, 10 };
                        //DataResult.Test3DataProcess(T, HMLocation, 300, 300, 1256, 1256, 425.16, 426.16,
                        //             10, 10, 10, chart1);
                    }
                    break;
                case TestMethod.ITMS: {
                        //显示对应监视窗口TEST4 
                        double[] T = { 81.2, 70.1, 59.8, 50, 45, 44, 43, 40, 37, 35, 30, 20.1, 10.8, 0.9 };
                        double[] HMLocation = { 10, 11, 10, 4, 10, 9, 4, 4, 9, 19, 4, 12, 10, 10 };
                        //DataResult.Test4DataProcess(T, HMLocation,100, 300, 300, 1256, 1256, 425.16, 426.16,
                                     //10, 10, 10,10, chart1);
                    }
                    break;
                default: {
                        MessageBox.Show(@"请选择测试方法!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
            }
            TestResultChart testResultChart = new TestResultChart(heatMeter1, heatMeter2, sample1, sample2, _method);
            testResultChart.Show();
            TestResultTemp testResultTemp = new TestResultTemp(heatMeter1, heatMeter2, sample1, sample2, force, _method);
            testResultTemp.Show();
            #endregion
        }

        private void SetCsvFileName() {
            if (latestDataFile == "") {
                OpenFileDialog file = new OpenFileDialog {
                    Title = @"请选择测试的数据文件!",
                    Filter = @"数据文件(*.csv)|*.csv",
                    InitialDirectory = AppDomain.CurrentDomain.BaseDirectory
                };
                file.ShowDialog();
                latestDataFile = file.FileName;
            }
        }

        private void SetIniFileName() {
            if (latestIniFile == "") {
                OpenFileDialog file = new OpenFileDialog {
                    Title = @"请选择测试的配置文件!",
                    Filter = @"配置文件(*.ini)|*.ini",
                    InitialDirectory = AppDomain.CurrentDomain.BaseDirectory
                };
                file.ShowDialog();
                latestIniFile = file.FileName;
            }
        }
    }
}

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
        private string thickness = null;
        private void TestResult_Click(object sender, EventArgs e) {
            #region //数据结果
            //测试
            //latestIniFile = @"C:\Users\Joe\source\Joe-zhouman\multimeter\multimeter\bin\Debug\AutoSave\itc-2020-11-09-12.ini";
            //latestDataFile = @"C:\Users\Joe\source\Joe-zhouman\multimeter\multimeter\bin\Debug\AutoSave\AutoSave-2020-11-09-12-17-45.5147.csv";
            SetIniFileName();
            if(latestIniFile == "") {
                MessageBox.Show(@"请选择配置文件!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SetCsvFileName();
            if (latestDataFile == "") {
                MessageBox.Show(@"请选择数据文件!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataTable channelTable = new DataTable();
            var csvExp = Solution.ReadCsvFile(ref channelTable, latestDataFile);
            if (null != csvExp) {
                MessageBox.Show(@"请选择正确的数据文件!"+"\n"+csvExp.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_method.ToString().ToLower() != INIHelper.Read("TestMethod", "method", "", latestIniFile)) {
                MessageBox.Show(@"请选择对应的配置文件!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var testResult = Solution.CalAve(channelTable);
            var csvType = (TestMethod) (int) testResult["TestMethod"];
            if (_method != csvType) {
                MessageBox.Show(@"请选择对应的配置文件!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            heatMeter1.SetTemp(testResult);
            heatMeter2.SetTemp(testResult);
            string force = INIHelper.Read("Pressure", "force", "1", latestIniFile);
            //string thickness = null;
            switch (_method) {
                case TestMethod.Kappa: {
                        sample1.SetTemp(testResult);
                    }
                    break;
                case TestMethod.ITC: {
                        //显示对应监视窗口TEST2
                        sample1.SetTemp(testResult);
                        sample2.SetTemp(testResult);
                    }
                    break;
                case TestMethod.ITM: {
                        thickness = INIHelper.Read("ITM", "thickness", "1", latestIniFile);
                    }
                    break;
                case TestMethod.ITMS: {
                        sample1.SetTemp(testResult);
                        sample2.SetTemp(testResult);
                        thickness = INIHelper.Read("ITM", "thickness", "1", latestIniFile);
                    }
                    break;
                default: {
                        MessageBox.Show(@"请选择测试方法!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
            }
            //TestResultChart testResultChart = new TestResultChart(heatMeter1, heatMeter2, sample1, sample2, _method);
            //testResultChart.Show();
            //TestResultTemp testResultTemp = new TestResultTemp(heatMeter1, heatMeter2, sample1, sample2, _method, force, thickness);
            //testResultTemp.Show();
            TestResultTemp();
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

        //-----------------
        private void TestResultTemp() {
            switch (_method) {
                case TestMethod.Kappa: {
                        if (true != Solution.GetResults(heatMeter1, heatMeter2, ref sample1)) {
                            MessageBox.Show(@"计算失败,数据误差过大", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        ShowKappa();
                    }
                    break;
                case TestMethod.ITC: {
                        double itc = 0.0;
                        if (true != Solution.GetResults(heatMeter1, heatMeter2, ref sample1, ref sample2, ref itc)) {
                            MessageBox.Show(@"计算失败,数据误差过大", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        ShowItc(itc);
                    }
                    break;
                case TestMethod.ITM: {
                        double itmKappa = 0.0;
                        double Thickness = double.Parse(thickness);
                        if (true!=Solution.GetResults(heatMeter1, heatMeter2, Thickness, ref itmKappa)) {
                            MessageBox.Show(@"计算失败,数据误差过大", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        ShowItm(itmKappa);
                    }
                    break;
                case TestMethod.ITMS:
                {
                        double itmKappa = 0.0;
                        double Thickness = double.Parse(thickness);
                        if (true!= Solution.GetResults(heatMeter1, heatMeter2, ref sample1, ref sample2, Thickness, ref itmKappa)) {
                            MessageBox.Show(@"计算失败,数据误差过大", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        ShowItms(itmKappa);
                    }
                    break;
                default:
                    break;
            }
        }
        private void ShowKappa() {
            Tlable1_1.Text = "Tu1 = " + heatMeter1.Temp[0].ToString() + " ℃";
            Tlable1_2.Text = "Tu2 = " + heatMeter1.Temp[1].ToString() + " ℃";
            Tlable1_3.Text = "Tu3 = " + heatMeter1.Temp[2].ToString() + " ℃";
            Tlable1_4.Text = "Tu4 = " + heatMeter1.Temp[3].ToString() + " ℃";
            Tlable1_5.Text = "Ts1 = " + sample1.Temp[0].ToString() + " ℃";
            Tlable1_6.Text = "Ts2 = " + sample1.Temp[1].ToString() + " ℃";
            Tlable1_7.Text = "Ts3 = " + sample1.Temp[2].ToString() + " ℃";
            Tlable1_8.Text = "Tl1 = " + heatMeter2.Temp[0].ToString() + " ℃";
            Tlable1_9.Text = "Tl2 = " + heatMeter2.Temp[1].ToString() + " ℃";
            Tlable1_10.Text = "Tl3 = " + heatMeter2.Temp[2].ToString() + " ℃";
            Tlable1_11.Text = "Tl4 = " + heatMeter2.Temp[3].ToString() + " ℃";
            k1_s.Text = "Ks = " + sample1.Kappa + "W/mK";
        }
        private void ShowItc(double itc) {
            Tlable2_1.Text = "Tu1 = " + heatMeter1.Temp[0].ToString() + " ℃";
            Tlable2_2.Text = "Tu2 = " + heatMeter1.Temp[1].ToString() + " ℃";
            Tlable2_3.Text = "Tu3 = " + heatMeter1.Temp[2].ToString() + " ℃";
            Tlable2_4.Text = "Tu4 = " + heatMeter1.Temp[3].ToString() + " ℃";
            Tlable2_5.Text = "Tsu1 = " + sample1.Temp[0].ToString() + " ℃";
            Tlable2_6.Text = "Tsu2 = " + sample1.Temp[1].ToString() + " ℃";
            Tlable2_7.Text = "Tsu3 = " + sample1.Temp[2].ToString() + " ℃";
            Tlable2_8.Text = "Tsl1 = " + sample2.Temp[0].ToString() + " ℃";
            Tlable2_9.Text = "Tsl2= " + sample2.Temp[1].ToString() + " ℃";
            Tlable2_10.Text = "Tsl3 = " + sample2.Temp[2].ToString() + " ℃";
            Tlable2_11.Text = "Tl1 = " + heatMeter2.Temp[0].ToString() + " ℃";
            Tlable2_12.Text = "Tl2 = " + heatMeter2.Temp[1].ToString() + " ℃";
            Tlable2_13.Text = "Tl3 = " + heatMeter2.Temp[2].ToString() + " ℃";
            Tlable2_14.Text = "Tl4 = " + heatMeter2.Temp[3].ToString() + " ℃";
            K2_s1.Text = "Ks1 = " + sample1.Kappa + "W/mK";
            K2_s2.Text = "Ks2 = " + sample2.Kappa + "W/mK";
            TCRtest2.Text = "Rt = " + itc.ToString() + "K/(W mm^2)";
        }
        private void ShowItm(double itmKappa) {
            Tlable3_1.Text = "Tu1 = " + heatMeter1.Temp[0].ToString() + " ℃";
            Tlable3_2.Text = "Tu2 = " + heatMeter1.Temp[1].ToString() + " ℃";
            Tlable3_3.Text = "Tu3 = " + heatMeter1.Temp[2].ToString() + " ℃";
            Tlable3_4.Text = "Tu4 = " + heatMeter1.Temp[3].ToString() + " ℃";
            Tlable3_5.Text = "Tl1 = " + heatMeter2.Temp[0].ToString() + " ℃";
            Tlable3_6.Text = "Tl2 = " + heatMeter2.Temp[1].ToString() + " ℃";
            Tlable3_7.Text = "Tl3 = " + heatMeter2.Temp[2].ToString() + " ℃";
            Tlable3_8.Text = "Tl4 = " + heatMeter2.Temp[3].ToString() + " ℃";
            k3_s.Text = "Ks=" + itmKappa.ToString() + "K/(W mm^2)";
        }
        private void ShowItms(double itmKappa) {
            Tlable4_1.Text = "Tu1 = " + heatMeter1.Temp[0].ToString() + " ℃";
            Tlable4_2.Text = "Tu2 = " + heatMeter1.Temp[1].ToString() + " ℃";
            Tlable4_3.Text = "Tu3 = " + heatMeter1.Temp[2].ToString() + " ℃";
            Tlable4_4.Text = "Tu4 = " + heatMeter1.Temp[3].ToString() + " ℃";
            Tlable4_5.Text = "Tsu1 = " + sample1.Temp[0].ToString() + " ℃";
            Tlable4_6.Text = "Tsu2 = " + sample1.Temp[1].ToString() + " ℃";
            Tlable4_7.Text = "Tsu3 = " + sample1.Temp[2].ToString() + " ℃";
            Tlable4_8.Text = "Tsl1 = " + sample2.Temp[0].ToString() + " ℃";
            Tlable4_9.Text = "Tsl2= " + sample2.Temp[1].ToString() + " ℃";
            Tlable4_10.Text = "Tsl3 = " + sample2.Temp[2].ToString() + " ℃";
            Tlable4_11.Text = "Tl1 = " + heatMeter2.Temp[0].ToString() + " ℃";
            Tlable4_12.Text = "Tl2 = " + heatMeter2.Temp[1].ToString() + " ℃";
            Tlable4_13.Text = "Tl3 = " + heatMeter2.Temp[2].ToString() + " ℃";
            Tlable4_14.Text = "Tl4 = " + heatMeter2.Temp[3].ToString() + " ℃";
            k4_s1.Text = "Ks1 = " + sample1.Kappa + "W/mK";
            k4_s2.Text = "Ks2 = " + sample2.Kappa + "W/mK";
            k4_f.Text = "Ks = " + itmKappa.ToString() + "K/(W mm^2)";
        }

        //-----------------


    }
}

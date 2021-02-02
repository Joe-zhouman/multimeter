using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using BusinessLogic;
using DataAccess;
using Model;

namespace multimeter {
    public partial class SetupTest {
        private void CurrentTestResult_Click(object sender, EventArgs e) {
            #region //数据结果

            if (_latestResultFile == "") {
                MessageBox.Show(@"数据未采集完成,无法计算测试结果!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            GetResult(_latestResultFile);
            //TestResultChart testResultChart = new TestResultChart(heatMeter1, heatMeter2, sample1, sample2, _method);
            //testResultChart.Show();
            //TestResultTemp testResultTemp = new TestResultTemp(heatMeter1, heatMeter2, sample1, sample2, _method, force, thickness);
            //testResultTemp.Show();

            #endregion
        }

        private void HistoryTestResult_Click(object sender, EventArgs e) {
            var dataFile = SetCsvFileName();
            if (dataFile == "") {
                MessageBox.Show(@"请选择数据文件!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            GetResult(dataFile);
        }

        private void GetResult(string dataFile) {
            if (!Enum.TryParse(IniReadAndWrite.ReadTestMethod(dataFile), out TestMethod method)) {
                MessageBox.Show(@"无效的数据文件!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _method = method;
            TestDevice device;
            switch (method) {
                case TestMethod.KAPPA: {
                    FileToBoxKappa(out device, dataFile);
                    ShowKappaMenu();
                }
                    break;
                case TestMethod.ITC: {
                    FileToBoxItc(out device, dataFile);
                    ShowItcMenu();
                }
                    break;
                case TestMethod.ITMS: {
                    //显示对应监视窗口TEST2
                    FileToBoxItms(out device, dataFile);
                    ShowItmsMenu();
                }
                    break;
                case TestMethod.ITM: {
                    FileToBoxItm(out device, dataFile);
                    ShowItmMenu();
                }
                    break;
                default: {
                    MessageBox.Show(@"无效的测试方法!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            var testResult = new Dictionary<string, double>();
            try {
                Solution.GetTestResult(ref testResult, dataFile, device.Channels.ToArray());
            }
            catch (Exception exception) {
                Log.Error(exception);
                MessageBox.Show($@"数据文件读取失败!
{exception.Message}", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DeviceOpt.ReadTemp(ref device, testResult);
            var errInfo = Solution.GetResults(ref device);
            switch (method) {
                case TestMethod.KAPPA: {
                    ShowResultErrorInfo(Test1_remark, errInfo);
                    ShowKappa(device);
                    TextResultGroupbox1.Visible = true;
                }
                    break;
                case TestMethod.ITC: {
                    ShowResultErrorInfo(Test2_remark, errInfo);
                    ShowItc(device);
                    TextResultGroupbox2.Visible = true;
                }
                    break;
                case TestMethod.ITM: {
                    ShowResultErrorInfo(Test3_remark, errInfo);
                    ShowItm(device);
                    TextResultGroupbox3.Visible = true;
                }
                    break;
                case TestMethod.ITMS: {
                    ShowResultErrorInfo(Test4_remark, errInfo);
                    ShowItms(device);
                    TextResultGroupbox4.Visible = true;
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ExportResult_Enable(true);
            /*List<GroupBox> groupBox = new List<GroupBox>()
                    {TextGroupbox1, TextGroupbox2, TextGroupbox3, TextGroupbox4};
                groupBox.ForEach(c => {
                    foreach (var control in c.Controls)
                    {
                        if (control.GetType().ToString() == "System.Windows.Forms.Label")
                        {
                            ((Label)control).Enabled = true;
                        }
                    }
                });*/
        }

        private static void ShowResultErrorInfo(RichTextBox label, string errInfo) {
            label.Text = errInfo;
            if (errInfo == "") return;
            Log.Info("计算误差过大");
            MessageBox.Show(@"计算失败,数据误差过大", @"警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private string SetCsvFileName() {
            var file = new OpenFileDialog {
                Title = @"请选择测试的数据文件",
                Filter = @"数据文件(*.rst)|*.rst",
                InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AutoSave")
            };
            file.ShowDialog();
            return file.FileName;
        }


        //-----------------
        private void ShowKappa(TestDevice device) {
            Tlable1_1.Text = TempString(@"Tu1 = ", device.HeatMeter1.Probes[0]);
            Tlable1_2.Text = TempString(@"Tu2 = ", device.HeatMeter1.Probes[1]);
            Tlable1_3.Text = TempString(@"Tu3 = ", device.HeatMeter1.Probes[2]);
            Tlable1_4.Text = TempString(@"Tu4 = ", device.HeatMeter1.Probes[3]);
            Tlable1_5.Text = TempString(@"Ts1 = ", device.Sample1.Probes[0]);
            Tlable1_6.Text = TempString(@"Ts2 = ", device.Sample1.Probes[1]);
            Tlable1_7.Text = TempString(@"Ts3 = ", device.Sample1.Probes[2]);
            Tlable1_8.Text = TempString(@"Ts4 = ", device.Sample1.Probes[3]);
            Tlable1_9.Text = TempString(@"Tl1 = ", device.HeatMeter2.Probes[0]);
            Tlable1_10.Text = TempString(@"Tl2 = ", device.HeatMeter2.Probes[1]);
            Tlable1_11.Text = TempString(@"Tl3 = ", device.HeatMeter2.Probes[2]);
            Tlable1_12.Text = TempString(@"Tl4 = ", device.HeatMeter2.Probes[3]);
            k1_s.Text = $@"{device.Sample1.Kappa}W/mK";
        }

        private string TempString(string s, Probe probe) {
            return probe == null ? @"测试点位未启动" : $@"{s}{probe.Temp:G4} ℃";
        }

        private void ShowItc(TestDevice device) {
            Tlable2_1.Text = TempString(@"Tu1 = ", device.HeatMeter1.Probes[0]);
            Tlable2_2.Text = TempString(@"Tu2 = ", device.HeatMeter1.Probes[1]);
            Tlable2_3.Text = TempString(@"Tu3 = ", device.HeatMeter1.Probes[2]);
            Tlable2_4.Text = TempString(@"Tu4 = ", device.HeatMeter1.Probes[3]);
            Tlable2_5.Text = TempString(@"Tsu1 = ", device.Sample1.Probes[0]);
            Tlable2_6.Text = TempString(@"Tsu2 = ", device.Sample1.Probes[1]);
            Tlable2_7.Text = TempString(@"Tsu3 = ", device.Sample1.Probes[2]);
            Tlable2_8.Text = TempString(@"Tsu4 = ", device.Sample1.Probes[3]);
            Tlable2_9.Text = TempString(@"Tsl1 = ", device.Sample2.Probes[0]);
            Tlable2_10.Text = TempString(@"Tsl2 = ", device.Sample2.Probes[1]);
            Tlable2_11.Text = TempString(@"Tsl3 = ", device.Sample2.Probes[2]);
            Tlable2_12.Text = TempString(@"Tsl4 = ", device.Sample2.Probes[3]);
            Tlable2_13.Text = TempString(@"Tl1 = ", device.HeatMeter2.Probes[0]);
            Tlable2_14.Text = TempString(@"Tl2 = ", device.HeatMeter2.Probes[1]);
            Tlable2_15.Text = TempString(@"Tl3 = ", device.HeatMeter2.Probes[2]);
            Tlable2_16.Text = TempString(@"Tl4 = ", device.HeatMeter2.Probes[3]);
            K2_s1.Text = $@"{device.Sample1.Kappa}W/mK";
            K2_s2.Text = $@"{device.Sample2.Kappa}W/mK";
            TCRtest2.Text = $@"{device.Itc:0.000e+0}mm²K/W";
        }

        private void ShowItm(TestDevice device) {
            Tlable3_1.Text = TempString(@"Tu1 = ", device.HeatMeter1.Probes[0]);
            Tlable3_2.Text = TempString(@"Tu2 = ", device.HeatMeter1.Probes[1]);
            Tlable3_3.Text = TempString(@"Tu3 = ", device.HeatMeter1.Probes[2]);
            Tlable3_4.Text = TempString(@"Tu4 = ", device.HeatMeter1.Probes[3]);
            Tlable3_5.Text = TempString(@"Tl1 = ", device.HeatMeter2.Probes[0]);
            Tlable3_6.Text = TempString(@"Tl2 = ", device.HeatMeter2.Probes[1]);
            Tlable3_7.Text = TempString(@"Tl3 = ", device.HeatMeter2.Probes[2]);
            Tlable3_8.Text = TempString(@"Tl4 = ", device.HeatMeter2.Probes[3]);
            k3_s.Text = $@"{device.Itm.Kappa:0.000e+0}W/mK";
            TCRtest3.Text = $@"{device.Itc:0.000e+0}mm²K/W";
        }

        private void ShowItms(TestDevice device) {
            Tlable4_1.Text = TempString(@"Tu1 = ", device.HeatMeter1.Probes[0]);
            Tlable4_2.Text = TempString(@"Tu2 = ", device.HeatMeter1.Probes[1]);
            Tlable4_3.Text = TempString(@"Tu3 = ", device.HeatMeter1.Probes[2]);
            Tlable4_4.Text = TempString(@"Tu4 = ", device.HeatMeter1.Probes[3]);
            Tlable4_5.Text = TempString(@"Tsu1 = ", device.Sample1.Probes[0]);
            Tlable4_6.Text = TempString(@"Tsu2 = ", device.Sample1.Probes[1]);
            Tlable4_7.Text = TempString(@"Tsu3 = ", device.Sample1.Probes[2]);
            Tlable4_8.Text = TempString(@"Tsu4 = ", device.Sample1.Probes[3]);
            Tlable4_9.Text = TempString(@"Tsl1 = ", device.Sample2.Probes[0]);
            Tlable4_10.Text = TempString(@"Tsl2 = ", device.Sample2.Probes[1]);
            Tlable4_11.Text = TempString(@"Tsl3 = ", device.Sample2.Probes[2]);
            Tlable4_12.Text = TempString(@"Tsl4 = ", device.Sample2.Probes[3]);
            Tlable4_13.Text = TempString(@"Tl1 = ", device.HeatMeter2.Probes[0]);
            Tlable4_14.Text = TempString(@"Tl2 = ", device.HeatMeter2.Probes[1]);
            Tlable4_15.Text = TempString(@"Tl3 = ", device.HeatMeter2.Probes[2]);
            Tlable4_16.Text = TempString(@"Tl4 = ", device.HeatMeter2.Probes[3]);
            k4_s1.Text = $@"Ks1 = {device.Sample1.Kappa}W/mK";
            k4_s2.Text = $@"Ks2 = {device.Sample2.Kappa}W/mK";
            k4_f.Text = $@"Ks = {device.Itm.Kappa:0.000e+0}W/mK";
            TCRtest4.Text = $@"Rt = {device.Itc:0.000e+0}mm²K/W";
        }

        //-----------------
    }
}
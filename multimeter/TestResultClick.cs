using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using DataProcessor;

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
            if (!Enum.TryParse(INIHelper.Read("TestMethod", "method", "", dataFile), out TestMethod method)) {
                log.Info("未能正确读取数据文件的方法");
                MessageBox.Show(@"无效的数据文件!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _method = method;
            var channelList = new List<string>();
            var heatMeter1 = new HeatMeter("HeatMeter1", 3);
            heatMeter1.ReadFromIni(dataFile);
            channelList.AddRange(heatMeter1.Channel);
            var heatMeter2 = new HeatMeter("HeatMeter2");
            heatMeter2.ReadFromIni(dataFile);
            channelList.AddRange(heatMeter2.Channel);
            Sample sample1;
            Sample sample2;
            switch (method) {
                case TestMethod.KAPPA: {
                    FileToBoxKappa(out sample1, out sample2, heatMeter1, heatMeter2, dataFile, dataFile);
                    channelList.AddRange(sample1.Channel);
                    ShowKappaMenu();
                }
                    break;
                case TestMethod.ITC: {
                    FileToBoxITC(out sample1, out sample2, heatMeter1, heatMeter2, dataFile, dataFile);
                    channelList.AddRange(sample1.Channel);
                    channelList.AddRange(sample2.Channel);
                    ShowItcMenu();
                }
                    break;
                case TestMethod.ITMS: {
                    //显示对应监视窗口TEST2
                    FileToBoxITMS(out sample1, out sample2, heatMeter1, heatMeter2, dataFile, dataFile);
                    channelList.AddRange(sample1.Channel);
                    channelList.AddRange(sample2.Channel);
                    ShowItmsMenu();
                }
                    break;
                case TestMethod.ITM: {
                    FileToBoxITM(out sample1, out sample2, heatMeter1, heatMeter2, dataFile);
                    ShowItmMenu();
                }
                    break;

                default: {
                    MessageBox.Show(@"请选择测试方法!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            channelList.Sort();
            var testResult = new Dictionary<string, double>();
            var e = Solution.ReadData(ref testResult, channelList.ToArray(), dataFile);
            if (null != e) {
                log.Error(e);
                MessageBox.Show($@"数据文件读取失败!
{e.Message}", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            heatMeter1.ReadTemp(testResult);
            heatMeter2.ReadTemp(testResult);
            sample1?.ReadTemp(testResult);
            sample2?.ReadTemp(testResult);
            switch (method) {
                case TestMethod.KAPPA: {
                    if (!Solution.GetResults(heatMeter1, heatMeter2, ref sample1)) {
                        ShowResultErrorInfo(Test1_remark);
                    }

                    ShowKappa(heatMeter1, heatMeter2, sample1);
                    TextResultGroupbox1.Visible = true;
                }
                    break;
                case TestMethod.ITC: {
                    var itc = 0.0;
                    if (!Solution.GetResults(heatMeter1, heatMeter2, ref sample1, ref sample2, ref itc)) {
                        ShowResultErrorInfo(Test2_remark);
                    }

                    ShowItc(heatMeter1, heatMeter2, sample1, sample2, itc);
                    TextResultGroupbox2.Visible = true;
                }
                    break;
                case TestMethod.ITM: {
                    var itc = 0.0;
                    var thickness = double.Parse(INIHelper.Read("ITM", "thickness", "1", dataFile));
                    if (!Solution.GetResults(heatMeter1, heatMeter2, ref itc)) {
                        ShowResultErrorInfo(Test3_remark);
                    }

                    var itmKappa = thickness / itc;
                    ShowItm(heatMeter1, heatMeter2, itc, itmKappa);
                    TextResultGroupbox3.Visible = true;
                }
                    break;
                case TestMethod.ITMS: {
                    var itc = 0.0;
                    var thickness = double.Parse(INIHelper.Read("ITM", "thickness", "1", dataFile));
                    if (!Solution.GetResults(heatMeter1, heatMeter2, ref sample1, ref sample2,
                        ref itc)) {
                        ShowResultErrorInfo(Test4_remark);
                    }

                    var itmKappa = thickness / itc;
                    ShowItms(heatMeter1, heatMeter2, sample1, sample2, itc, itmKappa);
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

        private void ShowResultErrorInfo(Label label) {
            log.Info("计算误差过大");
            MessageBox.Show(@"计算失败,数据误差过大", @"警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            label.Text = @"警告：计算失败,数据误差过大！！！";
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
        private void ShowKappa(HeatMeter heatMeter1, HeatMeter heatMeter2, Sample sample1) {
            Tlable1_1.Text = $@"Tu1 = {heatMeter1.Temp[0]} ℃";
            Tlable1_2.Text = $@"Tu2 = {heatMeter1.Temp[1]} ℃";
            Tlable1_3.Text = $@"Tu3 = {heatMeter1.Temp[2]} ℃";
            //Tlable1_4.Text = $@"Tu4 = {_heatMeter1.Temp[3]} ℃";
            Tlable1_5.Text = $@"Ts1 = {sample1.Temp[0]} ℃";
            Tlable1_6.Text = $@"Ts2 = {sample1.Temp[1]} ℃";
            Tlable1_7.Text = $@"Ts3 = {sample1.Temp[2]} ℃";
            Tlable1_8.Text = $@"Tl1 = {heatMeter2.Temp[0]} ℃";
            Tlable1_9.Text = $@"Tl2 = {heatMeter2.Temp[1]} ℃";
            Tlable1_10.Text = $@"Tl3 = {heatMeter2.Temp[2]} ℃";
            Tlable1_11.Text = $@"Tl4 = {heatMeter2.Temp[3]} ℃";
            k1_s.Text = $@"Ks = {sample1.Kappa}W/mK";
        }

        private void ShowItc(HeatMeter heatMeter1, HeatMeter heatMeter2, Sample sample1, Sample sample2, double itc) {
            Tlable2_1.Text = $@"Tu1 = {heatMeter1.Temp[0]} ℃";
            Tlable2_2.Text = $@"Tu2 = {heatMeter1.Temp[1]} ℃";
            Tlable2_3.Text = $@"Tu3 = {heatMeter1.Temp[2]} ℃";
            //Tlable2_4.Text = $@"Tu4 = {_heatMeter1.Temp[3]} ℃";
            Tlable2_5.Text = $@"Tsu1 = {sample1.Temp[0]} ℃";
            Tlable2_6.Text = $@"Tsu2 = {sample1.Temp[1]} ℃";
            Tlable2_7.Text = $@"Tsu3 = {sample1.Temp[2]} ℃";
            Tlable2_8.Text = $@"Tsl1 = {sample2.Temp[0]} ℃";
            Tlable2_9.Text = $@"Tsl2= {sample2.Temp[1]} ℃";
            Tlable2_10.Text = $@"Tsl3 = {sample2.Temp[2]} ℃";
            Tlable2_11.Text = $@"Tl1 = {heatMeter2.Temp[0]} ℃";
            Tlable2_12.Text = $@"Tl2 = {heatMeter2.Temp[1]} ℃";
            Tlable2_13.Text = $@"Tl3 = {heatMeter2.Temp[2]} ℃";
            Tlable2_14.Text = $@"Tl4 = {heatMeter2.Temp[3]} ℃";
            K2_s1.Text = $@"Ks1 = {sample1.Kappa}W/mK";
            K2_s2.Text = $@"Ks2 = {sample2.Kappa}W/mK";
            TCRtest2.Text = $@"Rt = {itc}K/(W mm²)";
        }

        private void ShowItm(HeatMeter heatMeter1, HeatMeter heatMeter2, double itc, double itmKappa) {
            Tlable3_1.Text = $@"Tu1 = {heatMeter1.Temp[0]} ℃";
            Tlable3_2.Text = $@"Tu2 = {heatMeter1.Temp[1]} ℃";
            Tlable3_3.Text = $@"Tu3 = {heatMeter1.Temp[2]} ℃";
            //Tlable3_4.Text = $@"Tu4 = {_heatMeter1.Temp[3]} ℃";
            Tlable3_5.Text = $@"Tl1 = {heatMeter2.Temp[0]} ℃";
            Tlable3_6.Text = $@"Tl2 = {heatMeter2.Temp[1]} ℃";
            Tlable3_7.Text = $@"Tl3 = {heatMeter2.Temp[2]} ℃";
            Tlable3_8.Text = $@"Tl4 = {heatMeter2.Temp[3]} ℃";
            k3_s.Text = $@"Ks={itmKappa}K/(W mm²)";
            TCRtest3.Text = $@"Rt = {itc}K/(W mm²)";
        }

        private void ShowItms(HeatMeter heatMeter1, HeatMeter heatMeter2, Sample sample1, Sample sample2, double itc,
            double itmKappa) {
            Tlable4_1.Text = $@"Tu1 = {heatMeter1.Temp[0]} ℃";
            Tlable4_2.Text = $@"Tu2 = {heatMeter1.Temp[1]} ℃";
            Tlable4_3.Text = $@"Tu3 = {heatMeter1.Temp[2]} ℃";
            //Tlable4_4.Text = $@"Tu4 = {_heatMeter1.Temp[3]} ℃";
            Tlable4_5.Text = $@"Tsu1 = {sample1.Temp[0]} ℃";
            Tlable4_6.Text = $@"Tsu2 = {sample1.Temp[1]} ℃";
            Tlable4_7.Text = $@"Tsu3 = {sample1.Temp[2]} ℃";
            Tlable4_8.Text = $@"Tsl1 = {sample2.Temp[0]} ℃";
            Tlable4_9.Text = $@"Tsl2= {sample2.Temp[1]} ℃";
            Tlable4_10.Text = $@"Tsl3 = {sample2.Temp[2]} ℃";
            Tlable4_11.Text = $@"Tl1 = {heatMeter2.Temp[0]} ℃";
            Tlable4_12.Text = $@"Tl2 = {heatMeter2.Temp[1]} ℃";
            Tlable4_13.Text = $@"Tl3 = {heatMeter2.Temp[2]} ℃";
            Tlable4_14.Text = $@"Tl4 = {heatMeter2.Temp[3]} ℃";
            k4_s1.Text = $@"Ks1 = {sample1.Kappa}W/mK";
            k4_s2.Text = $@"Ks2 = {sample2.Kappa}W/mK";
            k4_f.Text = $@"Ks = {itmKappa}K/(W mm²)";
            TCRtest4.Text = $@"Rt = {itc}K/(W mm²)";
        }

        //-----------------
    }
}
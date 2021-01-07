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
        private void CurrentTestResult_Click(object sender, EventArgs e) {
            #region //数据结果
            if (_latestDataFile == "") {
                MessageBox.Show(@"数据未采集完成,无法计算测试结果!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            GetResult(_latestDataFile);
            //TestResultChart testResultChart = new TestResultChart(heatMeter1, heatMeter2, sample1, sample2, _method);
            //testResultChart.Show();
            //TestResultTemp testResultTemp = new TestResultTemp(heatMeter1, heatMeter2, sample1, sample2, _method, force, thickness);
            //testResultTemp.Show();
            #endregion
        }
        private void HistoryTestResult_Click(object sender, EventArgs e) {
            string dataFile =  SetCsvFileName();
            if (dataFile == "")
            {
                MessageBox.Show(@"请选择数据文件!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            GetResult(dataFile);
        }

        private void GetResult(string dataFile) {
            if (!Enum.TryParse(INIHelper.Read("TestMethod", "method", "", dataFile), out _method)) {
                MessageBox.Show(@"无效的数据文件!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            var channelList = new List<string>();
            _heatMeter1.ReadFromIni(dataFile);
            channelList.AddRange(_heatMeter1.Channel);
            _heatMeter2.ReadFromIni(dataFile);
            channelList.AddRange(_heatMeter2.Channel);
            switch (_method) {
                case TestMethod.KAPPA: {
                    testchoose1_Click(this,new EventArgs());
                    _sample1.ReadFromIni(dataFile);
                    channelList.AddRange(_heatMeter1.Channel);
                }
                    break;
                case TestMethod.ITC: {
                    testchoose2_Click(this, new EventArgs());
                    _sample1.ReadFromIni(dataFile);
                    channelList.AddRange(_sample1.Channel);
                    _sample2.ReadFromIni(dataFile);
                    channelList.AddRange(_sample2.Channel);
                }
                    break;
                case TestMethod.ITMS: {
                        //显示对应监视窗口TEST2
                    testchoose3_Click(this, new EventArgs()); 
                    _sample1.ReadFromIni(dataFile);
                    channelList.AddRange(_sample1.Channel);
                    _sample2.ReadFromIni(dataFile);
                    channelList.AddRange(_sample2.Channel);
                    }
                    break;
                case TestMethod.ITM: {
                    testchoose4_Click(this, new EventArgs());
                    }
                    break;

                default:
                {
                    MessageBox.Show(@"请选择测试方法!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            channelList.Sort();
            Exception e = Solution.ReadData(ref _testResult, channelList.ToArray(), dataFile);
            if (null != e)
            {
                MessageBox.Show($@"数据文件读取失败!
{e.Message}", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _heatMeter1.ReadTemp(_testResult);
            _heatMeter2.ReadTemp(_testResult);
            _sample1?.ReadTemp(_testResult);
            _sample2?.ReadTemp(_testResult);
            switch (_method) {
                        case TestMethod.KAPPA: {
                            if (!Solution.GetResults(_heatMeter1, _heatMeter2, ref _sample1)) {
                                MessageBox.Show(@"计算失败,数据误差过大", @"警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                            ShowKappa();
                            TextResultGroupbox1.Visible = true;
                        }
                            break;
                        case TestMethod.ITC: {

                            double itc = 0.0;
                            if (!Solution.GetResults(_heatMeter1, _heatMeter2, ref _sample1, ref _sample2, ref itc)) {
                                MessageBox.Show(@"计算失败,数据误差过大", @"警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                            ShowItc(itc);
                            TextResultGroupbox2.Visible = true;
                        }
                            break;
                        case TestMethod.ITM: {
                            double itmKappa = 0.0;
                            double thickness = double.Parse(INIHelper.Read("ITM", "thickness", "1", dataFile));
                            if (!Solution.GetResults(_heatMeter1, _heatMeter2, thickness, ref itmKappa)) {
                                MessageBox.Show(@"计算失败,数据误差过大", @"警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                            ShowItm(itmKappa);
                            TextResultGroupbox3.Visible = true;
                        }
                            break;
                        case TestMethod.ITMS: {
                            double itmKappa = 0.0;
                            double thickness = double.Parse(INIHelper.Read("ITM", "thickness", "1", dataFile));
                            if (!Solution.GetResults(_heatMeter1, _heatMeter2, ref _sample1, ref _sample2, thickness,
                                ref itmKappa)) {
                                MessageBox.Show(@"计算失败,数据误差过大", @"警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                            ShowItms(itmKappa);
                            TextResultGroupbox4.Visible = true;
                        }
                            break;
                        default: {
                            MessageBox.Show(@"请选择测试方法!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
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
        private string SetCsvFileName() {
            OpenFileDialog file = new OpenFileDialog {
                Title = @"请选择测试的数据文件",
                Filter = @"数据文件(*.rst)|*.rst",
                InitialDirectory =Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"AutoSave")
            };
            file.ShowDialog();
            return file.FileName;
        }


        //-----------------
        private void ShowKappa()
        {
            Tlable1_1.Text = $@"Tu1 = {_heatMeter1.Temp[0]} ℃";
            Tlable1_2.Text = $@"Tu2 = {_heatMeter1.Temp[1]} ℃";
            Tlable1_3.Text = $@"Tu3 = {_heatMeter1.Temp[2]} ℃";
            //Tlable1_4.Text = $@"Tu4 = {_heatMeter1.Temp[3]} ℃";
            Tlable1_5.Text = $@"Ts1 = {_sample1.Temp[0]} ℃";
            Tlable1_6.Text = $@"Ts2 = {_sample1.Temp[1]} ℃";
            Tlable1_7.Text = $@"Ts3 = {_sample1.Temp[2]} ℃";
            Tlable1_8.Text = $@"Tl1 = {_heatMeter2.Temp[0]} ℃";
            Tlable1_9.Text = $@"Tl2 = {_heatMeter2.Temp[1]} ℃";
            Tlable1_10.Text = $@"Tl3 = {_heatMeter2.Temp[2]} ℃";
            Tlable1_11.Text = $@"Tl4 = {_heatMeter2.Temp[3]} ℃";
            k1_s.Text = $@"Ks = {_sample1.Kappa}W/mK";
        }
        private void ShowItc(double itc)
        {
            Tlable2_1.Text = $@"Tu1 = {_heatMeter1.Temp[0]} ℃";
            Tlable2_2.Text = $@"Tu2 = {_heatMeter1.Temp[1]} ℃";
            Tlable2_3.Text = $@"Tu3 = {_heatMeter1.Temp[2]} ℃";
            //Tlable2_4.Text = $@"Tu4 = {_heatMeter1.Temp[3]} ℃";
            Tlable2_5.Text = $@"Tsu1 = {_sample1.Temp[0]} ℃";
            Tlable2_6.Text = $@"Tsu2 = {_sample1.Temp[1]} ℃";
            Tlable2_7.Text = $@"Tsu3 = {_sample1.Temp[2]} ℃";
            Tlable2_8.Text = $@"Tsl1 = {_sample2.Temp[0]} ℃";
            Tlable2_9.Text = $@"Tsl2= {_sample2.Temp[1]} ℃";
            Tlable2_10.Text = $@"Tsl3 = {_sample2.Temp[2]} ℃";
            Tlable2_11.Text = $@"Tl1 = {_heatMeter2.Temp[0]} ℃";
            Tlable2_12.Text = $@"Tl2 = {_heatMeter2.Temp[1]} ℃";
            Tlable2_13.Text = $@"Tl3 = {_heatMeter2.Temp[2]} ℃";
            Tlable2_14.Text = $@"Tl4 = {_heatMeter2.Temp[3]} ℃";
            K2_s1.Text = $@"Ks1 = {_sample1.Kappa}W/mK";
            K2_s2.Text = $@"Ks2 = {_sample2.Kappa}W/mK";
            TCRtest2.Text = $@"Rt = {itc}K/(W mm²)";
        }
        private void ShowItm(double itmKappa)
        {
            Tlable3_1.Text = $@"Tu1 = {_heatMeter1.Temp[0]} ℃";
            Tlable3_2.Text = $@"Tu2 = {_heatMeter1.Temp[1]} ℃";
            Tlable3_3.Text = $@"Tu3 = {_heatMeter1.Temp[2]} ℃";
            //Tlable3_4.Text = $@"Tu4 = {_heatMeter1.Temp[3]} ℃";
            Tlable3_5.Text = $@"Tl1 = {_heatMeter2.Temp[0]} ℃";
            Tlable3_6.Text = $@"Tl2 = {_heatMeter2.Temp[1]} ℃";
            Tlable3_7.Text = $@"Tl3 = {_heatMeter2.Temp[2]} ℃";
            Tlable3_8.Text = $@"Tl4 = {_heatMeter2.Temp[3]} ℃";
            k3_s.Text = $@"Ks={itmKappa}K/(W mm²)";
        }

        private void ShowItms(double itmKappa) {
            Tlable4_1.Text = $@"Tu1 = {_heatMeter1.Temp[0]} ℃";
            Tlable4_2.Text = $@"Tu2 = {_heatMeter1.Temp[1]} ℃";
            Tlable4_3.Text = $@"Tu3 = {_heatMeter1.Temp[2]} ℃";
            //Tlable4_4.Text = $@"Tu4 = {_heatMeter1.Temp[3]} ℃";
            Tlable4_5.Text = $@"Tsu1 = {_sample1.Temp[0]} ℃";
            Tlable4_6.Text = $@"Tsu2 = {_sample1.Temp[1]} ℃";
            Tlable4_7.Text = $@"Tsu3 = {_sample1.Temp[2]} ℃";
            Tlable4_8.Text = $@"Tsl1 = {_sample2.Temp[0]} ℃";
            Tlable4_9.Text = $@"Tsl2= {_sample2.Temp[1]} ℃";
            Tlable4_10.Text = $@"Tsl3 = {_sample2.Temp[2]} ℃";
            Tlable4_11.Text = $@"Tl1 = {_heatMeter2.Temp[0]} ℃";
            Tlable4_12.Text = $@"Tl2 = {_heatMeter2.Temp[1]} ℃";
            Tlable4_13.Text = $@"Tl3 = {_heatMeter2.Temp[2]} ℃";
            Tlable4_14.Text = $@"Tl4 = {_heatMeter2.Temp[3]} ℃";
            k4_s1.Text = $@"Ks1 = {_sample1.Kappa}W/mK";
            k4_s2.Text = $@"Ks2 = {_sample2.Kappa}W/mK";
            k4_f.Text = $@"Ks = {itmKappa}K/(W mm²)";
        }
        //-----------------


    }
}

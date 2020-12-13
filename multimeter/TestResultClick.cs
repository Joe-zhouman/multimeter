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

            GetResult(_latestIniFile,_latestDataFile);
            //TestResultChart testResultChart = new TestResultChart(heatMeter1, heatMeter2, sample1, sample2, _method);
            //testResultChart.Show();
            //TestResultTemp testResultTemp = new TestResultTemp(heatMeter1, heatMeter2, sample1, sample2, _method, force, thickness);
            //testResultTemp.Show();
            #endregion
        }
        private void HistoryTestResult_Click(object sender, EventArgs e) {
            string iniFile =  SetIniFileName();
            if (iniFile == "")
            {
                MessageBox.Show(@"请选择配置文件!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string csvFile =  SetCsvFileName();
            if (csvFile == "")
            {
                MessageBox.Show(@"请选择数据文件!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            GetResult(iniFile,csvFile);
        }

        private void GetResult(string iniFile,string csvFile) {
            DataTable channelTable = new DataTable();
            Exception csvExp = Solution.ReadCsvFile(ref channelTable, csvFile);
            if (null != csvExp)
            {
                MessageBox.Show($"请选择正确的数据文件!\n{csvExp.Message}", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_method.ToString().ToLower() != INIHelper.Read("TestMethod", "method", "", iniFile))
            {
                MessageBox.Show(@"请选择对应的配置文件!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Dictionary<string, double> result = Solution.CalAve(channelTable);
            TestMethod csvType = (TestMethod)(int)result["TestMethod"];
            if (_method != csvType)
            {
                MessageBox.Show(@"请选择对应的配置文件!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _heatMeter1.SetTemp(result);
            _heatMeter2.SetTemp(result);
            switch (_method)
            {
                case TestMethod.KAPPA:
                    {
                        _sample1.SetTemp(result);
                        if (!Solution.GetResults(_heatMeter1, _heatMeter2, ref _sample1))
                        {
                            MessageBox.Show(@"计算失败,数据误差过大", @"警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        ShowKappa();
                        TextResultGroupbox1.Visible = false;
                    }
                    break;
                case TestMethod.ITC:
                    {
                        //显示对应监视窗口TEST2
                        _sample1.SetTemp(result);
                        _sample2.SetTemp(result);
                        double itc = 0.0;
                        if (!Solution.GetResults(_heatMeter1, _heatMeter2, ref _sample1, ref _sample2, ref itc))
                        {
                            MessageBox.Show(@"计算失败,数据误差过大", @"警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        ShowItc(itc);
                        TextResultGroupbox2.Visible = false;
                    }
                    break;
                case TestMethod.ITM:
                    {
                        double itmKappa = 0.0;
                        double thickness = double.Parse(INIHelper.Read("ITM", "thickness", "1", iniFile));
                        if (!Solution.GetResults(_heatMeter1, _heatMeter2, thickness, ref itmKappa))
                        {
                            MessageBox.Show(@"计算失败,数据误差过大", @"警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        ShowItm(itmKappa);
                        TextResultGroupbox3.Visible = false;
                    }
                    break;
                case TestMethod.ITMS:
                    {
                        _sample1.SetTemp(result);
                        _sample2.SetTemp(result);
                        double itmKappa = 0.0;
                        double thickness = double.Parse(INIHelper.Read("ITM", "thickness", "1", iniFile));
                        if (!Solution.GetResults(_heatMeter1, _heatMeter2, ref _sample1, ref _sample2, thickness, ref itmKappa))
                        {
                            MessageBox.Show(@"计算失败,数据误差过大", @"警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        ShowItms(itmKappa);
                        TextResultGroupbox4.Visible = false;
                    }
                    break;
                default:
                    {
                        MessageBox.Show(@"请选择测试方法!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
            }
            List<GroupBox> groupBox = new List<GroupBox>()
                    {TextGroupbox1, TextGroupbox2, TextGroupbox3, TextGroupbox4};
                groupBox.ForEach(c => {
                    foreach (var control in c.Controls)
                    {
                        if (control.GetType().ToString() == "System.Windows.Forms.Label")
                        {
                            ((Label)control).Enabled = true;
                        }
                    }
                });
        }
        private string SetCsvFileName() {
            OpenFileDialog file = new OpenFileDialog {
                Title = @"请选择测试的数据文件!",
                Filter = @"数据文件(*.csv)|*.csv",
                InitialDirectory =Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"AutoSave")
            };
            file.ShowDialog();
            return file.FileName;
        }

        private string SetIniFileName() {
            OpenFileDialog file = new OpenFileDialog {
                Title = @"请选择测试的配置文件!",
                Filter = @"配置文件(*.ini)|*.ini",
                InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AutoSave")
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

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
            var filepath = SlnIni.CreateDefaultSlnIni();
            if (heatMeter1 == null) {
                heatMeter1 = new HeatMeter("HeatMeter1");
                heatMeter1.ReadFromIni(filepath);
                heatMeter1.LoadTempPara(filepath);
            }

            if (heatMeter2 == null) {
                heatMeter2 = new HeatMeter("HeatMeter2");
                heatMeter2.ReadFromIni(filepath);
                heatMeter2.LoadTempPara(filepath);
            }
            heatMeter1.SetTemp(testResult);
            heatMeter2.SetTemp(testResult);
            
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
                        Sample sample1 = new Sample("Sample1");
                        Sample sample2 = new Sample("Sample2");
                        sample1.ReadFromIni(latestIniFile);
                        sample1.LoadTempPara(filepath);
                        sample2.ReadFromIni(latestIniFile);
                        sample2.LoadTempPara(filepath);
                        sample1.SetTemp(testResult);
                        sample2.SetTemp(testResult);

                        List<double> T = new List<double>();
                        T.AddRange(heatMeter1.Temp);
                        T.AddRange(sample1.Temp);
                        T.AddRange(sample2.Temp);
                        T.AddRange(heatMeter2.Temp);
                        List<double> position = new List<double>();
                        for (int i = 0; i < 4; i++) {
                            position.Add(double.Parse(heatMeter1.Position[i]));
                        }
                        for (int i = 0; i < 4; i++)
                        {
                            position.Add(double.Parse(heatMeter2.Position[i]));
                        }
                        for (int i = 0; i < 3; i++)
                        {
                            position.Add(double.Parse(sample1.Position[i]));
                        }
                        for (int i = 0; i < 3; i++)
                        {
                            position.Add(double.Parse(sample2.Position[i]));
                        }
                        //position.AddRange(heatMeter1.Position.ConvertTo<double[]>());
                        //position.AddRange(sample1.Position.ConvertTo<double[]>());
                        //position.AddRange(sample2.Position.ConvertTo<double[]>());
                        //position.AddRange(heatMeter2.Position.ConvertTo<double[]>());
                        //DataResult.Test2DataProcess(T.ToArray(), position.ToArray(), 300, 300, 1256, 1256, 425.16,426.16,
                                     //10, 10, 10, chart1);
                    }
                    break;
                case TestMethod.ITM: {
                        //显示对应监视窗口TEST3
                        double[] T = { 81.2, 70.1, 59.8, 50, 45, 44, 43, 40, 37, 35, 30, 20.1, 10.8, 0.9 };
                        double[] HMLocation = { 10, 11, 10, 4, 10, 9, 4, 4, 9, 19, 4, 12, 10, 10 };
                        //DataResult.Test2DataProcess(T, HMLocation, 300, 300, 1256, 1256, 425.16, 426.16,
                                     //10, 10, 10, chart1);
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
            TestResultTemp testResultTemp = new TestResultTemp(heatMeter1, heatMeter2, sample1, sample2, _method);
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

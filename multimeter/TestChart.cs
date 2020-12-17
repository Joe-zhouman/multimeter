using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DataProcessor;

namespace multimeter {
    public partial class SetupTest {
        private List<double> _latestTempList;
        private int _timerCyclesNum;
        public void Chart_Init(HeatMeter heatMeter1, HeatMeter heatMeter2, Sample sample1, Sample sample2) {
            _heatMeter1 = heatMeter1;
            _heatMeter2 = heatMeter2;
            _sample1 = sample1;
            _sample2 = sample2;
            chart1.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
            chart1.ChartAreas[0].AxisY.IsStartedFromZero = false;       //y轴自适应
            chart1.ChartAreas[0].AxisX.Maximum = 250;
            chart1.ChartAreas[0].AxisX.Minimum = 0;

            int numChannel = 0;
            List<string> channelList = new List<string>();
            if (heatMeter1 != null) {
                numChannel += heatMeter1.TestPoint;
                channelList.AddRange(heatMeter1.Channel);
            }

            if (sample1 != null) {
                numChannel += sample1.TestPoint;
                channelList.AddRange(sample1.Channel);
            }

            if (sample2 != null) {
                numChannel += sample2.TestPoint;
                channelList.AddRange(sample2.Channel);
            }

            if (heatMeter2 != null) {
                numChannel += heatMeter2.TestPoint;
                channelList.AddRange(heatMeter2.Channel);
            }

            for (int i = 0; i < numChannel; i++) {
                chart1.Series[i].IsVisibleInLegend = true;
                chart1.Series[i].LegendText = channelList[i];
                chart1.Series[i].Points.Clear();
            }

            for (int i = numChannel; i < 13; i++) chart1.Series[i].IsVisibleInLegend = false;
            //设置图表显示样式
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";         //毫秒格式： hh:mm:ss.fff ，后面几个f则保留几位毫秒小数，此时要注意轴的最大值和最小值不要差太大
            chart1.ChartAreas[0].AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Seconds;
            chart1.ChartAreas[0].AxisX.LabelStyle.Interval = 5;                //坐标值间隔1S
            chart1.ChartAreas[0].AxisX.LabelStyle.IsEndLabelVisible = false;   //防止X轴坐标跳跃
            chart1.ChartAreas[0].AxisX.MajorGrid.IntervalType = DateTimeIntervalType.Seconds;
            chart1.ChartAreas[0].AxisX.MajorGrid.Interval = 5;                 //网格间隔
            chart1.ChartAreas[0].AxisX.Minimum = DateTime.Now.ToOADate();      //当前时间
            chart1.ChartAreas[0].AxisX.Maximum = DateTime.Now.ToOADate();
            _timerCyclesNum = 0;
        }


        public void ShowChart(Dictionary<string, double> testResult) {
            List<double> T = new List<double>();
            if (_heatMeter1 != null) {
                _heatMeter1.SetTemp(testResult);

                T.AddRange(_heatMeter1.Temp);
            }

            if (_sample1 != null) {
                _sample1.SetTemp(testResult);
                T.AddRange(_sample1.Temp);
            }

            if (_sample2 != null) {
                _sample2.SetTemp(testResult);
                T.AddRange(_sample2.Temp);
            }

            if (_heatMeter2 != null) {
                _heatMeter2.SetTemp(testResult);
                T.AddRange(_heatMeter2.Temp.Take(3));
            }

            for (int i = 0; i < T.Count; i++) chart1.Series[i].Points.AddXY(DateTime.Now.ToOADate(), T[i]);
            chart1.ChartAreas[0].AxisX.Maximum = DateTime.Now.AddSeconds(5).ToOADate();   //X坐标后移1秒
            chart1.ChartAreas[0].AxisX.Minimum = DateTime.Now.AddSeconds(-50).ToOADate();//此刻后10分钟作为最初X轴，

            /*double residual = 1;
            if (_latestTempList != null) {
                for (int i = 0; i < T.Count; i++) {
                    residual = Math.Abs(1 - T[i] / _latestTempList[i]);
                    if (residual > 1e-4) break;
                }

                if (residual < 1e-4) {
                    DialogResult = DialogResult.Yes;
                    TestResultChart_FormClosing(this, new FormClosingEventArgs(CloseReason.ApplicationExitCall, true));
                }
            }

            _latestTempList = T;*/
        }

        //private void TestResultChart_FormClosing(object sender, FormClosingEventArgs e) {
        //    DialogResult = DialogResult.Cancel;
        //    Hide();
        //    e.Cancel = true;
        //}

        private void chart1_MouseMove(object sender, MouseEventArgs e) {
            HitTestResult result = chart1.HitTest(e.X, e.Y);
            if (result.ChartElementType == ChartElementType.DataPoint) {
                if (!(result.Object is DataPoint a)) return;
                chartValue.BringToFront();
                chartValue.Location = e.Location;
                DateTime datetime = DateTime.FromOADate(a.XValue);
                chartValue.Text = $@"Ch{a.LegendText},Time:{datetime},Temp:{a.YValues[0]}";

            }
            else if (result.ChartElementType != ChartElementType.Nothing) {
                Cursor = Cursors.Default;
                chartValue.Text = "";
            }
        }

        private void TestTime_Timer_Tick(object sender, EventArgs e) {
            int time = (int)(0.001 * _timerCyclesNum * TestTime_Timer.Interval);
            TestTime.Text = $@"（已用时间{time}S）";
            _timerCyclesNum++;
        }

        private void chart1_MouseClick(object sender, MouseEventArgs e) {
            TestChartGroupBox.Size = new Size(0, 0);
            switch (_method) {
                case TestMethod.KAPPA: {
                    TextGroupbox1.Size = new Size(1250, 855);
                }
                    break;
                case TestMethod.ITC: {
                    TextGroupbox2.Size = new Size(1250, 855);
                }
                    break;
                case TestMethod.ITM: {
                    TextGroupbox3.Size = new Size(1250, 855);
                }
                    break;
                case TestMethod.ITMS: {
                    TextGroupbox4.Size = new Size(1250, 855);
                }
                    break;
                default: {
                    return;
                }
            }
        }



    }
}
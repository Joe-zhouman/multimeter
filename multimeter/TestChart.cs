using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DataProcessor;

namespace multimeter {
    public partial class SetupTest {
        private int _timerCyclesNum;
        private DateTime X_minValue;    //采样初始时间
        private DateTime X_maxValue;    //采样最近时间
        private void Chart_Init() {
            
            chart1.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
            chart1.ChartAreas[0].AxisY.IsStartedFromZero = false;       //y轴自适应

            this.chart1.Series[0].Color = Color.Red;
            this.chart1.Series[1].Color = Color.Lime;
            this.chart1.Series[2].Color = Color.Blue;
            this.chart1.Series[3].Color = Color.OrangeRed;
            this.chart1.Series[4].Color = Color.Fuchsia;
            this.chart1.Series[5].Color = Color.Aqua;
            this.chart1.Series[6].Color = Color.Green;
            this.chart1.Series[7].Color = Color.Orange;
            this.chart1.Series[8].Color = Color.SaddleBrown;
            this.chart1.Series[9].Color = Color.Maroon;
            this.chart1.Series[10].Color = Color.Pink;
            this.chart1.Series[11].Color = Color.Olive;
            this.chart1.Series[12].Color = Color.SlateGray;

            List<CheckBox> checkBoxes = new List<CheckBox>()
                    { checkBox1,checkBox2,checkBox3,checkBox4,checkBox5, checkBox6, 
                        checkBox7,checkBox8,checkBox9,checkBox10,checkBox11,checkBox12,checkBox13};

            int numChannel = channels.Length;
            for (int i = 0; i < numChannel; i++) {
                checkBoxes[i].Visible =true;
                checkBoxes[i].Text = channels[i];           
                checkBoxes[i].ForeColor = chart1.Series[i].Color;
                chart1.Series[i].LegendText = channels[i];
                chart1.Series[i].Points.Clear();
            }

            for (int i = numChannel; i < 13; i++){
                checkBoxes[i].Visible = false;
                chart1.Series[i].Points.Clear();
            }
            

            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";         //毫秒格式： hh:mm:ss.fff ，后面几个f则保留几位毫秒小数，此时要注意轴的最大值和最小值不要差太大
            chart1.ChartAreas[0].AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Seconds;
            chart1.ChartAreas[0].AxisX.LabelStyle.Interval = 5;                //坐标值间隔1S
            chart1.ChartAreas[0].AxisX.LabelStyle.IsEndLabelVisible = false;   //防止X轴坐标跳跃
            chart1.ChartAreas[0].AxisX.MajorGrid.IntervalType = DateTimeIntervalType.Seconds;
            chart1.ChartAreas[0].AxisX.MajorGrid.Interval = 5;                 //网格间隔
            chart1.ChartAreas[0].AxisX.Minimum = DateTime.Now.ToOADate();      //当前时间
            chart1.ChartAreas[0].AxisX.Maximum = DateTime.Now.ToOADate();
            _timerCyclesNum = 0;
            X_minValue = DateTime.Now;          
            X_maxValue = DateTime.Now;          
        }


        private void ShowChart() {
            List<double> T = new List<double>();
            if (_heatMeter1 != null) {
                T.AddRange(_heatMeter1.Temp);
            }

            if (_sample1 != null) {
                T.AddRange(_sample1.Temp);
            }

            if (_sample2 != null) {
                T.AddRange(_sample2.Temp);
            }

            if (_heatMeter2 != null) {
                T.AddRange(_heatMeter2.Temp);
            }
            if (T.Count != 0) {
                X_maxValue = DateTime.Now;
            } //更新采样最近时间

            for (int i = 0; i < T.Count; i++) {
                if (T[i] >= 0 && T[i] <= 500) {
                    chart1.Series[i].Points.AddXY(DateTime.Now.ToOADate(), T[i]);
                }//设定温度合理显示范围，防止温度数据异常Chart出现“大红叉”
            }
            chart1.ChartAreas[0].AxisX.Maximum = DateTime.Now.AddSeconds(5).ToOADate();
            XAdapt();

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
            int sec = (int)(0.001 * _timerCyclesNum * TestTime_Timer.Interval);
            TestTime.Text = "测试时长 " +SecToTimeSpan(sec);

            int interval = (int)Math.Abs(((DateTime.Now - X_maxValue).TotalSeconds));
            if (interval >= 7) {
                TestTime.Text = "";
                TestTime_Timer.Enabled = false;
                MessageBox.Show(@"采集数据异常，请尝试重启软件和数采仪", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }//每隔*S检测采集是否正常
            _timerCyclesNum++;
        }

        public string SecToTimeSpan(int sec) {
            string timespan = "";
            TimeSpan ts = new TimeSpan(0, 0, sec);
            if (ts.Hours > 0) {
                timespan = string.Format("{0:00}", ts.Hours)
                                        + ":" + string.Format("{0:00}", ts.Minutes)
                                        + ":" + string.Format("{0:00}", ts.Seconds);
            }
            if (ts.Hours == 0 && ts.Minutes > 0) {
                timespan ="00:" + string.Format("{0:00}", ts.Minutes)
                                + ":" + string.Format("{0:00}", ts.Seconds);
            }
            if (ts.Hours == 0 && ts.Minutes == 0) {
                timespan ="00:00:" + string.Format("{0:00}", ts.Seconds);
            }
            return timespan;
        } //将秒转换成hh:mm:ss

        private void HideChart_Click(object sender, EventArgs e) {
            TestChartGroupBox.Size = new Size(0, 0);
            switch (_method) {
                case TestMethod.KAPPA: 
                {
                    TextGroupbox1.Size = new Size(1250, 855);
                }
                    break;
                case TestMethod.ITC: 
                {
                    TextGroupbox2.Size = new Size(1250, 855);
                }
                    break;
                case TestMethod.ITM:
                {
                    TextGroupbox3.Size = new Size(1250, 855);
                }
                    break;
                case TestMethod.ITMS:
                {
                    TextGroupbox4.Size = new Size(1250, 855);
                }
                    break;
                default:
                {
                    return;
                }
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            CheckedChanged(checkBox1);
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e) {
            CheckedChanged(checkBox2);
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e) {
            CheckedChanged(checkBox3);
        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e) {
            CheckedChanged(checkBox4);
        }
        private void checkBox5_CheckedChanged(object sender, EventArgs e) {
            CheckedChanged(checkBox5);
        }
        private void checkBox6_CheckedChanged(object sender, EventArgs e) {
            CheckedChanged(checkBox6);
        }
        private void checkBox7_CheckedChanged(object sender, EventArgs e) {
            CheckedChanged(checkBox7);
        }
        private void checkBox8_CheckedChanged(object sender, EventArgs e) {
            CheckedChanged(checkBox8);
        }
        private void checkBox9_CheckedChanged(object sender, EventArgs e) {
            CheckedChanged(checkBox9);
        }
        private void checkBox10_CheckedChanged(object sender, EventArgs e) {
            CheckedChanged(checkBox10);
        }
        private void checkBox11_CheckedChanged(object sender, EventArgs e) {
            CheckedChanged(checkBox11);
        }
        private void checkBox12_CheckedChanged(object sender, EventArgs e) {
            CheckedChanged(checkBox12);
        }
        private void checkBox13_CheckedChanged(object sender, EventArgs e) {
            CheckedChanged(checkBox13) ;
        }
        private void XAxis_chenkBox_CheckedChanged(object sender, EventArgs e) {
            XAdapt();
        }

        private void YAxis_checkBox_CheckedChanged(object sender, EventArgs e) {
            if (YAxis_checkBox.Checked) {
                chart1.ChartAreas[0].CursorY.IsUserEnabled = true;
                chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            }//允许纵轴放大
            else {
                chart1.ChartAreas[0].CursorY.IsUserEnabled = false;
                chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = false;
            }
        }

        private void CheckedChanged(CheckBox checkBox) {
            string str = checkBox.Name.Replace("checkBox", "");
            int i = int.Parse(str)-1;
            if (checkBox.Checked) {
                chart1.Series[i].Enabled = true;
            }
            else {
                chart1.Series[i].Enabled = false;
            }

        }
        private void XAdapt() {
            if (!XAxis_checkBox.Checked && X_maxValue > X_minValue) {
                int interval = (int)((X_maxValue - X_minValue).TotalSeconds / 10);
                this.chart1.ChartAreas[0].AxisX.LabelStyle.Interval = interval;                //坐标值间隔*S
                this.chart1.ChartAreas[0].AxisX.MajorGrid.Interval = interval;                 //网格间隔
                chart1.ChartAreas[0].AxisX.Minimum = X_minValue.ToOADate();
            }//X轴设置成全局显示
            else {
                this.chart1.ChartAreas[0].AxisX.LabelStyle.Interval = 5;                //坐标值间隔*S
                this.chart1.ChartAreas[0].AxisX.MajorGrid.Interval = 5;                 //网格间隔
                chart1.ChartAreas[0].AxisX.Minimum = DateTime.Now.AddSeconds(-50).ToOADate();
            } //X轴设置成实时显示
        }



    }
}
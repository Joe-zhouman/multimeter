using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DataProcessor;

namespace multimeter {
    public partial class TestResultChart : Form {
        private List<double> _latestTempList;
        private int _chartX;
        private HeatMeter _heatMeter1;
        private HeatMeter _heatMeter2;
        private Sample _sample1;
        private Sample _sample2;

        public
            TestResultChart() //HeatMeter _heatMeter1, HeatMeter heatMeter2, Sample sample1, Sample sample2, TestMethod testMethod
        {
            InitializeComponent();
        }

        private void TestResultChart_Load(object sender, EventArgs e) {
        }

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
            }

            for (int i = numChannel; i < 13; i++) chart1.Series[i].IsVisibleInLegend = false;
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

            for (int i = 0; i < T.Count; i++) chart1.Series[i].Points.AddXY(_chartX, T[i]);
            if (_chartX > 250) {
                chart1.ChartAreas[0].AxisX.Minimum = _chartX - 250;
                chart1.ChartAreas[0].AxisX.Maximum = _chartX;
            }

            _chartX++;
            double residual = 1;
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

            _latestTempList = T;
        }

        private void TestResultChart_FormClosing(object sender, FormClosingEventArgs e) {
            DialogResult = DialogResult.Cancel;
            Hide();
            e.Cancel = true;
        }

        private void chart1_MouseMove(object sender, MouseEventArgs e) {
            HitTestResult result = chart1.HitTest(e.X, e.Y);
            if (result.ChartElementType == ChartElementType.DataPoint) {
                if (!(result.Object is DataPoint a)) return;
                chartValue.BringToFront();
                chartValue.Location = e.Location;
                chartValue.Text = $@"Ch{a.LegendText},Time:{a.XValue},Temp:{a.YValues[0]}";


            }
            else if (result.ChartElementType != ChartElementType.Nothing) {
                Cursor = Cursors.Default;
                chartValue.Text = "";
            }
        }


    }
}
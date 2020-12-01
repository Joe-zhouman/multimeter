using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DataProcessor;

namespace multimeter {
    public partial class TestResultChart : Form {
        private List<string> _channelList;
        private List<double> _latestTempList;
        private int chartX;
        private HeatMeter heatMeter1;
        private HeatMeter heatMeter2;
        private Sample sample1;
        private Sample sample2;
        private TestMethod testMethod;

        public
            TestResultChart() //HeatMeter heatMeter1, HeatMeter heatMeter2, Sample sample1, Sample sample2, TestMethod testMethod
        {
            InitializeComponent();
        }

        private void TestResultChart_Load(object sender, EventArgs e) {
        }

        public void Chart_Init(HeatMeter heatMeter1, HeatMeter heatMeter2, Sample sample1, Sample sample2,
            TestMethod testMethod) {
            this.heatMeter1 = heatMeter1;
            this.heatMeter2 = heatMeter2;
            this.sample1 = sample1;
            this.sample2 = sample2;
            this.testMethod = testMethod;
            chart1.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
            chart1.ChartAreas[0].AxisX.Maximum = 250;

            int numChannel = 0;
            List<string> channelList = new List<string>();
            if (heatMeter1 != null) {
                numChannel += 4;
                channelList.AddRange(heatMeter1.Channel);
            }

            if (sample1 != null) {
                numChannel += 3;
                channelList.AddRange(sample1.Channel);
            }

            if (sample2 != null) {
                numChannel += 3;
                channelList.AddRange(sample2.Channel);
            }

            if (heatMeter2 != null) {
                numChannel += 3;
                channelList.AddRange(heatMeter2.Channel.Take(3));
            }

            for (int i = 0; i < numChannel; i++) {
                chart1.Series[i].IsVisibleInLegend = true;
                chart1.Series[i].LegendText = channelList[i];
            }

            for (int i = numChannel; i < 13; i++) chart1.Series[i].IsVisibleInLegend = false;
        }


        public void ShowChart(Dictionary<string, double> testResult) {
            List<double> T = new List<double>();
            if (heatMeter1 != null) {
                heatMeter1.SetTemp(testResult);

                T.AddRange(heatMeter1.Temp);
            }

            if (sample1 != null) {
                sample1.SetTemp(testResult);
                T.AddRange(sample1.Temp);
            }

            if (sample2 != null) {
                sample2.SetTemp(testResult);
                T.AddRange(sample2.Temp);
            }

            if (heatMeter2 != null) {
                heatMeter2.SetTemp(testResult);
                T.AddRange(heatMeter2.Temp.Take(3));
            }

            for (int i = 0; i < T.Count; i++) chart1.Series[i].Points.AddXY(chartX, T[i]);
            if (chartX > 250) {
                chart1.ChartAreas[0].AxisX.Minimum = chartX - 250;
                chart1.ChartAreas[0].AxisX.Maximum = chartX;
            }

            chartX++;
            double residual = 1;
            if (_latestTempList != null) {
                for (int i = 0; i < T.Count; i++) {
                    residual = Math.Abs(1 - T[i] / _latestTempList[i]);
                    if (residual > 1e-3) break;
                }

                if (residual < 1e-3) {
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
                DataPoint a = result.Object as DataPoint;
                chartValue.BringToFront();
                chartValue.Location = e.Location;
                chartValue.Text = "(" + a.XValue + "," + a.YValues[0] + ")";
            }
            else if (result.ChartElementType != ChartElementType.Nothing) {
                Cursor = Cursors.Default;
            }
        }
    }
}
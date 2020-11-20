using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataProcessor;
using CCWin.SkinClass;
namespace multimeter
{
    public partial class TestResultChart : Form
    {
        private HeatMeter heatMeter1;
        private HeatMeter heatMeter2;
        private Sample sample1;
        private Sample sample2;
        private TestMethod testMethod;
        private int chartX;

        public TestResultChart()     //HeatMeter heatMeter1, HeatMeter heatMeter2, Sample sample1, Sample sample2, TestMethod testMethod
        {
            InitializeComponent();
            

        }

        private void TestResultChart_Load(object sender, EventArgs e)
        {
           
        }

        public void Chart_Init(HeatMeter heatMeter1, HeatMeter heatMeter2, Sample sample1, Sample sample2, TestMethod testMethod)
        {
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
            if (sample2 != null)
            {
                numChannel += 3;
                channelList.AddRange(sample2.Channel);
            }
            if (heatMeter2 != null)
            {
                numChannel += 3;
                channelList.AddRange(heatMeter2.Channel.Take(3));
            }

            for (int i = 0; i < numChannel; i++) {
                chart1.Series[i].IsVisibleInLegend = true;
                chart1.Series[i].LegendText = channelList[i];
            }

            for (int i = numChannel; i < 13; i++) {
                chart1.Series[i].IsVisibleInLegend = false;
            }                
        }

        public void ShowChart(Dictionary<string, double>testResult)
        {
            List<double> T = new List<double>();
            if (heatMeter1 != null) {
                heatMeter1.SetTemp(testResult);

                T.AddRange(heatMeter1.Temp);
            }
            if (sample1 != null) {
                sample1.SetTemp(testResult);
                T.AddRange(sample1.Temp);
            }
            if (sample2 != null)
            {
                sample2.SetTemp(testResult);
                T.AddRange(sample2.Temp);
            }
            if (heatMeter2 != null)
            {
                heatMeter2.SetTemp(testResult);
                T.AddRange(heatMeter2.Temp.Take(3));
            }
            for (int i = 0; i < T.Count; i++){
                chart1.Series[i].Points.AddXY(chartX, T[i]);
            }
            if (chartX > 250){
                chart1.ChartAreas[0].AxisX.Minimum = chartX - 250;
                chart1.ChartAreas[0].AxisX.Maximum = chartX;
            }
            chartX++;
        }
        private void TestResultChart_FormClosing(object sender, FormClosingEventArgs e){
            Hide();
            e.Cancel = true;         
        }
    }
}

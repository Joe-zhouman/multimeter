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
        public TestResultChart(HeatMeter heatMeter1, HeatMeter heatMeter2, Sample sample1, Sample sample2, TestMethod testMethod)   
        {
            InitializeComponent();
            this.heatMeter1 = heatMeter1;
            this.heatMeter2 = heatMeter2;
            this.sample1 = sample1;
            this.sample2 = sample2;     
            this.testMethod = testMethod;    
        }

        private void TestResultChart_Load(object sender, EventArgs e)
        {                
        }

        public void Chart_Init()
        {
            chart1.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
            chart1.ChartAreas[0].AxisX.Maximum = 500;
            switch (testMethod)
            {
                case TestMethod.Kappa:{
                        for (int i = 0; i < 11; i++)
                            chart1.Series[i].IsVisibleInLegend = true;
                        for (int i = 11; i < 14; i++)
                            chart1.Series[i].IsVisibleInLegend = false;

                        chart1.Series[0].LegendText = "Tu1";
                        chart1.Series[1].LegendText = "Tu2";
                        chart1.Series[2].LegendText = "Tu3";
                        chart1.Series[3].LegendText = "Tu4";
                        chart1.Series[4].LegendText = "Ts1";
                        chart1.Series[5].LegendText = "Ts2";
                        chart1.Series[6].LegendText = "Ts3";
                        chart1.Series[7].LegendText = "Tl1";
                        chart1.Series[8].LegendText = "Tl2";
                        chart1.Series[9].LegendText = "Tl3";
                        chart1.Series[10].LegendText = "Tl4";
                    }
                    break;
                case TestMethod.ITC:{
                        for (int i = 0; i < 14; i++)
                            chart1.Series[i].IsVisibleInLegend = true;

                        chart1.Series[0].LegendText = "Tu1";
                        chart1.Series[1].LegendText = "Tu2";
                        chart1.Series[2].LegendText = "Tu3";
                        chart1.Series[3].LegendText = "Tu4";
                        chart1.Series[4].LegendText = "Tsu1";
                        chart1.Series[5].LegendText = "Tsu2";
                        chart1.Series[6].LegendText = "Tsu3";
                        chart1.Series[7].LegendText = "Tsl1";
                        chart1.Series[8].LegendText = "Tsl2";
                        chart1.Series[9].LegendText = "Tsl3";
                        chart1.Series[10].LegendText = "Tl1";
                        chart1.Series[11].LegendText = "Tl2";
                        chart1.Series[12].LegendText = "Tl3";
                        chart1.Series[13].LegendText = "Tl4";
                    }
                     break;
                 case TestMethod.ITM:{
                        for (int i = 0; i < 8; i++)
                            chart1.Series[i].IsVisibleInLegend = true;
                        for (int i = 8; i < 14; i++)
                            chart1.Series[i].IsVisibleInLegend = false;

                        chart1.Series[0].LegendText = "Tu1";
                        chart1.Series[1].LegendText = "Tu2";
                        chart1.Series[2].LegendText = "Tu3";
                        chart1.Series[3].LegendText = "Tu4";
                        chart1.Series[4].LegendText = "Tl1";
                        chart1.Series[5].LegendText = "Tl2";
                        chart1.Series[6].LegendText = "Tl3";
                        chart1.Series[7].LegendText = "Tl4";

                    }
                     break;
                 case TestMethod.ITMS:{
                        for (int i = 0; i < 14; i++)
                            chart1.Series[i].IsVisibleInLegend = true;

                        chart1.Series[0].LegendText = "Tu1";
                        chart1.Series[1].LegendText = "Tu2";
                        chart1.Series[2].LegendText = "Tu3";
                        chart1.Series[3].LegendText = "Tu4";
                        chart1.Series[4].LegendText = "Tsu1";
                        chart1.Series[5].LegendText = "Tsu2";
                        chart1.Series[6].LegendText = "Tsu3";
                        chart1.Series[7].LegendText = "Tsl1";
                        chart1.Series[8].LegendText = "Tsl2";
                        chart1.Series[9].LegendText = "Tsl3";
                        chart1.Series[10].LegendText = "Tl1";
                        chart1.Series[11].LegendText = "Tl2";
                        chart1.Series[12].LegendText = "Tl3";
                        chart1.Series[13].LegendText = "Tl4";
                    }
                     break;
                 default:{
                         return;
                     }
             }                 
        }

        public void showChart(){
            switch (testMethod){
                case TestMethod.Kappa:{
                        List<double> T = new List<double>();
                        T.AddRange(heatMeter1.Temp);
                        T.AddRange(sample1.Temp);
                        T.AddRange(heatMeter2.Temp);
                        for (int i = 0; i < 11; i++){
                            chart1.Series[i].Points.AddXY(chartX, T[i]);
                        }
                        if (chartX > 500){
                            chart1.ChartAreas[0].AxisX.Minimum = chartX - 500;
                            chart1.ChartAreas[0].AxisX.Maximum = chartX;
                        }
                        chartX++;
                    }
                    break;
                case TestMethod.ITC:{
                        /* List<double> T = new List<double>();
                          T.AddRange(heatMeter1.Temp);
                          T.AddRange(sample1.Temp);
                          T.AddRange(sample2.Temp);
                          T.AddRange(heatMeter2.Temp);  */
                        double[] T = new double[14] { 1,2,3,4,5,6,7,8,9,10,11,12,13,14};
                        for (int i = 0; i < 14; i++){
                            chart1.Series[i].Points.AddXY(chartX, T[i]);
                        }
                        if (chartX > 500){
                            chart1.ChartAreas[0].AxisX.Minimum = chartX - 500;
                            chart1.ChartAreas[0].AxisX.Maximum = chartX;
                        }
                        chartX++;
                    }
                    break;
                case TestMethod.ITM:{
                        List<double> T = new List<double>();
                        T.AddRange(heatMeter1.Temp);
                        T.AddRange(heatMeter2.Temp);
                        for (int i = 0; i < 8; i++){
                            chart1.Series[i].Points.AddXY(chartX, T[i]);
                        }
                        if (chartX > 500){
                            chart1.ChartAreas[0].AxisX.Minimum = chartX - 500;
                            chart1.ChartAreas[0].AxisX.Maximum = chartX;
                        }
                        chartX++;

                    }
                    break;
                case TestMethod.ITMS:{
                        List<double> T = new List<double>();
                        T.AddRange(heatMeter1.Temp);
                        T.AddRange(sample1.Temp);
                        T.AddRange(sample2.Temp);
                        T.AddRange(heatMeter2.Temp);
                        for (int i = 0; i < 14; i++){
                            chart1.Series[i].Points.AddXY(chartX, T[i]);
                        }
                        if (chartX > 500){
                            chart1.ChartAreas[0].AxisX.Minimum = chartX - 500;
                            chart1.ChartAreas[0].AxisX.Maximum = chartX;
                        }
                        chartX++;
                    }
                    break;
                default:{
                        return;
                    }
            }
        }


    }
}

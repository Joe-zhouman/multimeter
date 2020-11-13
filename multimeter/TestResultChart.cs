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
        public TestResultChart(HeatMeter heatMeter1,HeatMeter heatMeter2,Sample sample1,Sample sample2,TestMethod testMethod)
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

            switch (testMethod){
                case TestMethod.Kappa:{
                    }
                    break;
                case TestMethod.ITC:{
                        List<double> T = new List<double>();
                        T.AddRange(heatMeter1.Temp);
                        T.AddRange(sample1.Temp);
                        T.AddRange(sample2.Temp);
                        T.AddRange(heatMeter2.Temp);
                        List<double> position = new List<double>();
                        position.AddRange(heatMeter1.Position.ConvertTo<double[]>());
                        position.AddRange(sample1.Position.ConvertTo<double[]>());
                        position.AddRange(sample2.Position.ConvertTo<double[]>());
                        position.AddRange(heatMeter2.Position.ConvertTo<double[]>());
                        DataResult.Test2DataProcess(T.ToArray(), position.ToArray(), 300, 300, 1256, 1256, 425.16, 426.16,
                                         10, 10, 10, chart1);
                    }
                    break;
                case TestMethod.ITM:{

                    }
                    break;
                case TestMethod.ITMS:{
                    }
                    break;
                default:{
                        return;
                    }
            }

           

        }
    }
}

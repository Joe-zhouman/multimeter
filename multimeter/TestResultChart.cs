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
    }
}

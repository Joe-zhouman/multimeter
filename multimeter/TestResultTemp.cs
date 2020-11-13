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
    public partial class TestResultTemp : Form
    {
        private HeatMeter heatMeter1;
        private HeatMeter heatMeter2;
        private Sample sample1;
        private Sample sample2;
        private TestMethod testMethod;
        public TestResultTemp(HeatMeter heatMeter1, HeatMeter heatMeter2, Sample sample1, Sample sample2, TestMethod testMethod)
        {
            InitializeComponent();
            this.heatMeter1 = heatMeter1;
            this.heatMeter2 = heatMeter2;
            this.sample1 = sample1;
            this.sample2 = sample2;
            this.testMethod = testMethod;
        }

        private void TestResultTemp_Load(object sender, EventArgs e)
        {
            switch (testMethod){
                case TestMethod.Kappa:{   
                    }
                    break;
                case TestMethod.ITC:{
                    //ViewGroupBox1.Size = new Size(1531, 966);
                    ViewGroupBox2.Size = new Size(607, 811);
                    //ViewGroupBox3.Size = new Size(0, 0);
                    //ViewGroupBox4.Size = new Size(0, 0);
                    List<TextBox> heatMeterTempBoxes1 = new List<TextBox>
                        {T_TextBox2_1, T_TextBox2_2, T_TextBox2_3, T_TextBox2_4};
                    List<TextBox> heatMeterTempBoxes2 = new List<TextBox>
                        {T_TextBox2_11, T_TextBox2_12, T_TextBox2_13, T_TextBox2_14};
                    for (int i = 0; i < 4; i++)
                    {
                        heatMeterTempBoxes1[i].Text = heatMeter1.Temp[i].ToString();
                        heatMeterTempBoxes2[i].Text = heatMeter2.Temp[i].ToString();
                    }

                    List<TextBox> sampleTempBoxes1 = new List<TextBox>
                        {T_TextBox2_5,T_TextBox2_6, T_TextBox2_7};
                    List<TextBox> sampleTempBoxes2 = new List<TextBox>
                        {T_TextBox2_8, T_TextBox2_9,T_TextBox2_10};
                    for (int i = 0; i < 3; i++)
                    {
                        sampleTempBoxes1[i].Text = sample1.Temp[i].ToString();
                        sampleTempBoxes2[i].Text = sample1.Temp[i].ToString();
                    }
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

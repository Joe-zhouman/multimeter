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
            //ViewGroupBox1.Size = new Size(1531, 966);
            ViewGroupBox2.Size = new Size(607, 811);
            //ViewGroupBox3.Size = new Size(0, 0);
            //ViewGroupBox4.Size = new Size(0, 0);
            switch (testMethod)
            {
                case TestMethod.Kappa:
                    {
                        ShowKappa();
                    }
                    break;
                case TestMethod.ITC:
                    {
                        double[] k = new double[4];
                        double[] b = new double[4];
                        double itc = 0.0;
                        if(true !=Solution.GetResults(heatMeter1, heatMeter2, sample1, sample2, ref k, ref b, ref itc))
                        {
                            MessageBox.Show(@"计算失败,数据误差过大", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        
                        ShowItc();
                    }
                    break;
                case TestMethod.ITM:
                    {
                        ShowItm();
                    }
                    break;
                case TestMethod.ITMS:
                    {
                        ShowItms();
                    }
                    break;
                default:
                    break;
            }
        }
        private void ShowKappa()
        {

        }
        private void ShowItc()
        {
            T_TextBox2_1.Text = heatMeter1.Temp[0].ToString();
            T_TextBox2_2.Text = heatMeter1.Temp[1].ToString();
            T_TextBox2_3.Text = heatMeter1.Temp[2].ToString();
            T_TextBox2_4.Text = heatMeter1.Temp[3].ToString();
            T_TextBox2_5.Text = sample1.Temp[0].ToString();
            T_TextBox2_6.Text = sample1.Temp[1].ToString();
            T_TextBox2_7.Text = sample1.Temp[2].ToString();
            T_TextBox2_8.Text = sample2.Temp[0].ToString();
            T_TextBox2_9.Text = sample2.Temp[1].ToString();
            T_TextBox2_10.Text = sample2.Temp[2].ToString();
            T_TextBox2_11.Text = heatMeter2.Temp[0].ToString();
            T_TextBox2_12.Text = heatMeter2.Temp[1].ToString();
            T_TextBox2_13.Text = heatMeter2.Temp[2].ToString();
            T_TextBox2_14.Text = heatMeter2.Temp[3].ToString();
            Ks1TextBox2_3.Text = sample1.Kappa;
            Ks1TextBox2_3.Text = sample2.Kappa;
        }
        private void ShowItm()
        {

        }
        private void ShowItms()
        {

        }
    }
}

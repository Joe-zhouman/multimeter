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
        private readonly HeatMeter heatMeter1;
        private readonly HeatMeter heatMeter2;
        private Sample sample1;
        private Sample sample2;
        private readonly TestMethod testMethod;
        private readonly string force;
        public TestResultTemp(HeatMeter heatMeter1, HeatMeter heatMeter2, Sample sample1, Sample sample2,string force, TestMethod testMethod)
        {
            InitializeComponent();
            this.heatMeter1 = heatMeter1;
            this.heatMeter2 = heatMeter2;
            this.sample1 = sample1;
            this.sample2 = sample2;
            this.testMethod = testMethod;
            this.force = force;
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
                        double itc = 0.0;
                        if(true !=Solution.GetResults(heatMeter1, heatMeter2,ref sample1,ref sample2, ref itc))
                        {
                            MessageBox.Show(@"计算失败,数据误差过大", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        
                        ShowItc(itc);
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
        private void ShowItc(double itc)
        {
            Tlable2_1.Text = "Tu1 = " + heatMeter1.Temp[0].ToString() + " ℃";
            Tlable2_2.Text = "Tu2 = " + heatMeter1.Temp[1].ToString() + " ℃";
            Tlable2_3.Text = "Tu3 = " + heatMeter1.Temp[2].ToString() + " ℃";
            Tlable2_4.Text = "Tu4 = " + heatMeter1.Temp[3].ToString() + " ℃";
            Tlable2_5.Text = "Tsu1 = " + sample1.Temp[0].ToString() + " ℃";
            Tlable2_6.Text = "Tsu2 = " + sample1.Temp[1].ToString() + " ℃";
            Tlable2_7.Text = "Tsu3 = " + sample1.Temp[2].ToString() + " ℃";
            Tlable2_8.Text = "Tsl1 = " + sample2.Temp[0].ToString() + " ℃";
            Tlable2_9.Text = "Tsl2= " + sample2.Temp[1].ToString() + " ℃";
            Tlable2_10.Text = "Tsl3 = " + sample2.Temp[2].ToString() + " ℃";
            Tlable2_11.Text = "Tl1 = " + heatMeter2.Temp[0].ToString() + " ℃";
            Tlable2_12.Text = "Tl2 = " + heatMeter2.Temp[1].ToString() + " ℃";
            Tlable2_13.Text = "Tl3 = " + heatMeter2.Temp[2].ToString() + " ℃";
            Tlable2_14.Text = "Tl4 = " + heatMeter2.Temp[3].ToString() + " ℃";
            K2_s1.Text = "Ks1 = " + sample1.Kappa + "W/mK";
            K2_s2.Text = "Ks2 = " + sample2.Kappa + "W/mK";
            TCRtest2.Text = "Rt = " + itc.ToString() + "K/(W mm^2)";
            ForceView2.Text = force;
        }
        private void ShowItm()
        {

        }
        private void ShowItms()
        {

        }
    }
}

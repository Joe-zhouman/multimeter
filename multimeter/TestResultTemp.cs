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
        private Sample _sample1;
        private Sample _sample2;
        private HeatMeter HeatMeter1 { get;set; }
        private HeatMeter HeatMeter2 { get; set; }

        private TestMethod TestMethod { get; set; }
        private string Force { get; set; }
        private string Thickness { get; set; }
        public Sample Sample1 { get => _sample1; set => _sample1 = value; }
        public Sample Sample2 { get => _sample2; set => _sample2 = value; }

        public TestResultTemp(HeatMeter heatMeter1, HeatMeter heatMeter2, Sample sample1, Sample sample2, TestMethod testMethod, string force,string thickness = null)
        {
            InitializeComponent();
            HeatMeter1 = heatMeter1;
            HeatMeter2 = heatMeter2;
            Sample1 = sample1;
            Sample2 = sample2;
            TestMethod = testMethod;
            Force = force;
            Thickness = thickness;
        }

        private void TestResultTemp_Load(object sender, EventArgs e)
        {
            //ViewGroupBox1.Size = new Size(1531, 966);
            ViewGroupBox2.Size = new Size(607, 811);
            //ViewGroupBox3.Size = new Size(0, 0);
            //ViewGroupBox4.Size = new Size(0, 0);
            switch (TestMethod)
            {
                case TestMethod.Kappa:
                    {
                        if (true != Solution.GetResults(HeatMeter1, HeatMeter2, ref _sample1))
                        {
                            MessageBox.Show(@"计算失败,数据误差过大", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        ShowKappa();
                    }
                    break;
                case TestMethod.ITC:
                    {
                        double itc = 0.0;
                        if(true !=Solution.GetResults(HeatMeter1, HeatMeter2,ref _sample1,ref _sample2, ref itc))
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
            Tlable2_1.Text = "Tu1 = " + HeatMeter1.Temp[0].ToString() + " ℃";
            Tlable2_2.Text = "Tu2 = " + HeatMeter1.Temp[1].ToString() + " ℃";
            Tlable2_3.Text = "Tu3 = " + HeatMeter1.Temp[2].ToString() + " ℃";
            Tlable2_4.Text = "Tu4 = " + HeatMeter1.Temp[3].ToString() + " ℃";
            Tlable2_5.Text = "Tsu1 = " + Sample1.Temp[0].ToString() + " ℃";
            Tlable2_6.Text = "Tsu2 = " + Sample1.Temp[1].ToString() + " ℃";
            Tlable2_7.Text = "Tsu3 = " + Sample1.Temp[2].ToString() + " ℃";
            Tlable2_8.Text = "Tsl1 = " + Sample2.Temp[0].ToString() + " ℃";
            Tlable2_9.Text = "Tsl2= " + Sample2.Temp[1].ToString() + " ℃";
            Tlable2_10.Text = "Tsl3 = " + Sample2.Temp[2].ToString() + " ℃";
            Tlable2_11.Text = "Tl1 = " + HeatMeter2.Temp[0].ToString() + " ℃";
            Tlable2_12.Text = "Tl2 = " + HeatMeter2.Temp[1].ToString() + " ℃";
            Tlable2_13.Text = "Tl3 = " + HeatMeter2.Temp[2].ToString() + " ℃";
            Tlable2_14.Text = "Tl4 = " + HeatMeter2.Temp[3].ToString() + " ℃";
            K2_s1.Text = "Ks1 = " + Sample1.Kappa + "W/mK";
            K2_s2.Text = "Ks2 = " + Sample2.Kappa + "W/mK";
            TCRtest2.Text = "Rt = " + itc.ToString() + "K/(W mm^2)";
            ForceView2.Text = Force;
        }
        private void ShowItm()
        {

        }
        private void ShowItms()
        {

        }

        private void ViewGroupBox4_Enter(object sender, EventArgs e)
        {

        }
    }
}

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

namespace multimeter {
    public partial class SetupTest {
        private void testchoose1_Click(object sender, EventArgs e) {
            ButtonEnable();
            _method = TestMethod.KAPPA;
            ParSetting_Click(sender, e);
        }

        private void testchoose2_Click(object sender, EventArgs e) {
            ButtonEnable();
            _method = TestMethod.ITC;
            ParSetting_Click(sender, e);
        }

        private void testchoose3_Click(object sender, EventArgs e) {
            ButtonEnable();
            _method = TestMethod.ITM;
            ParSetting_Click(sender, e);
        }

        private void testchoose4_Click(object sender, EventArgs e) {
            ButtonEnable();
            _method = TestMethod.ITMS;
            ParSetting_Click(sender, e);
        }


        private void testchoose1_MouseMove(object sender, MouseEventArgs e) {
            test1.BackColor = Color.Lime;
        }

        private void testchoose1_MouseLeave(object sender, EventArgs e) {
            test1.BackColor = Color.White;
        }

        private void testchoose2_MouseMove(object sender, MouseEventArgs e) {
            test2.BackColor = Color.Lime;
        }

        private void testchoose2_MouseLeave(object sender, EventArgs e) {
            test2.BackColor = Color.White;
        }

        private void testchoose3_MouseMove(object sender, MouseEventArgs e) {
            test3.BackColor = Color.Lime;
        }

        private void testchoose3_MouseLeave(object sender, EventArgs e) {
            test3.BackColor = Color.White;
        }

        private void testchoose4_MouseMove(object sender, MouseEventArgs e) {
            test4.BackColor = Color.Lime;
        }

        private void testchoose4_MouseLeave(object sender, EventArgs e) {
            test4.BackColor = Color.White;
        }

        private void ButtonEnable() {
            TestRun_Enable(true);
            Monitor_Enable(false);
            CurrentTestResult_Enable(false);
            HistoryTestResult_Enable(true);
            SerialPort_Enable(true);
            AdvancedSetting_Enable(true);
            MenuGroupBox.Visible = true;
            TestChoiseGroupBox.Visible = false;
        }
        private void ParSetting_Click(object sender, EventArgs e)
        {
            #region //参数设置窗口打开

            string force;
            string slnFilePath = SlnIni.CreateDefaultSlnIni();
            switch (_method)
            {
                case TestMethod.KAPPA:
                    {
                        //显示对应设置窗口TEST1
                        EmptyGroupBox.Size = new Size(0, 0);
                        TextGroupbox1.Size = new Size(1250, 855);
                        TextGroupbox2.Size = new Size(0, 0);
                        TextGroupbox3.Size = new Size(0, 0);
                        TextGroupbox4.Size = new Size(0, 0);
                        //skinGroupBox2.Size = new Size(0, 0);
                        
                        _sample1 = new Sample("Sample1");
                        _sample2 = null;
                        string filePath = SlnIni.CreateDefaultKappaIni();
                        SlnIni.LoadKappaInfo(ref _sample1, out force, filePath,slnFilePath);
                        ForceTextBox1.Text = force;
                        List<TextBox> heatMeterPositionBoxes1 = new List<TextBox>
                        {LengthTextBox1_1, LengthTextBox1_2, LengthTextBox1_3, LengthTextBox1_4};
                        List<TextBox> heatMeterPositionBoxes2 = new List<TextBox>
                        {LengthTextBox1_8, LengthTextBox1_9, LengthTextBox1_10, LengthTextBox1_11};
                        List<TextBox> heatMeterChannelBoxes1 = new List<TextBox>
                        {ChannelTextBox1_1, ChannelTextBox1_2, ChannelTextBox1_3, ChannelTextBox1_4};
                        List<TextBox> heatMeterChannelBoxes2 = new List<TextBox>
                        {ChannelTextBox1_8, ChannelTextBox1_9, ChannelTextBox1_10, ChannelTextBox1_11};
                        HeatMeterToBox(_heatMeter1, heatMeterPositionBoxes1, heatMeterChannelBoxes1, D1TextBox1_1, K1TextBox1_1);
                        HeatMeterToBox(_heatMeter2, heatMeterPositionBoxes2, heatMeterChannelBoxes2, D2TextBox1_2, K2TextBox1_2);

                        List<TextBox> samplePositionBoxes = new List<TextBox>
                        {LengthTextBox1_5, LengthTextBox1_6, LengthTextBox1_7};
                        List<TextBox> sampleChannelBoxes = new List<TextBox> {
                        ChannelTextBox1_5, ChannelTextBox1_6, ChannelTextBox1_7
                    };
                        SampleToBox(_sample1, samplePositionBoxes, sampleChannelBoxes, dsTextBox1_1);
                    }
                    break;
                case TestMethod.ITC:
                    {
                        //显示对应设置窗口TEST2
                        EmptyGroupBox.Size = new Size(0, 0);
                        TextGroupbox1.Size = new Size(0, 0);
                        TextGroupbox2.Size = new Size(1250, 855);
                        TextGroupbox3.Size = new Size(0, 0);
                        TextGroupbox4.Size = new Size(0, 0);
                        
                        _sample1 = new Sample("Sample1");
                        _sample2 = new Sample("Sample2");
                        string filePath = SlnIni.CreateDefaultSlnIni();
                        SlnIni.LoadItcInfo(ref _sample1, ref _sample2, out force, filePath,slnFilePath);
                        ForceTextBox2.Text = force;
                        List<TextBox> heatMeterPositionBoxes1 = new List<TextBox>
                        {LengthTextBox2_1, LengthTextBox2_2, LengthTextBox2_3, LengthTextBox2_4};
                        List<TextBox> heatMeterPositionBoxes2 = new List<TextBox>
                        {LengthTextBox2_11, LengthTextBox2_12, LengthTextBox2_13, LengthTextBox2_14};
                        List<TextBox> heatMeterChannelBoxes1 = new List<TextBox>
                        {ChannelTextBox2_1, ChannelTextBox2_2, ChannelTextBox2_3, ChannelTextBox2_4};
                        List<TextBox> heatMeterChannelBoxes2 = new List<TextBox>
                        {ChannelTextBox2_11, ChannelTextBox2_12, ChannelTextBox2_13, ChannelTextBox2_14};
                        HeatMeterToBox(_heatMeter1, heatMeterPositionBoxes1, heatMeterChannelBoxes1, D1TextBox2_1, K1TextBox2_1);
                        HeatMeterToBox(_heatMeter2, heatMeterPositionBoxes2, heatMeterChannelBoxes2, D2TextBox2_2, K2TextBox2_2);
                        List<TextBox> samplePositionBoxes1 = new List<TextBox>
                        {LengthTextBox2_5, LengthTextBox2_6, LengthTextBox2_7};
                        List<TextBox> sampleChannelBoxes1 = new List<TextBox> {
                        ChannelTextBox2_5, ChannelTextBox2_6, ChannelTextBox2_7
                    };
                        SampleToBox(_sample1, samplePositionBoxes1, sampleChannelBoxes1, ds1TextBox2_1);
                        List<TextBox> samplePositionBoxes2 = new List<TextBox>
                        {LengthTextBox2_8, LengthTextBox2_9, LengthTextBox2_10};
                        List<TextBox> sampleChannelBoxes2 = new List<TextBox> {
                        ChannelTextBox2_8, ChannelTextBox2_9, ChannelTextBox2_10
                    };
                        SampleToBox(_sample2, samplePositionBoxes2, sampleChannelBoxes2, ds2TextBox2_2);
                    }
                    break;
                case TestMethod.ITM:
                    {
                        //显示对应设置窗口TEST3
                        EmptyGroupBox.Size = new Size(0, 0);
                        TextGroupbox1.Size = new Size(0, 0);
                        TextGroupbox2.Size = new Size(0, 0);
                        TextGroupbox3.Size = new Size(1250, 855);
                        TextGroupbox4.Size = new Size(0, 0);
                        string filePath = SlnIni.CreateDefaultItmIni();
                        _sample1 = null;
                        _sample2 = null;
                        SlnIni.LoadItmInfo(out force, out string thickness, filePath);
                        ForceTextBox3.Text = force;
                        FilmThickness1.Text = thickness;
                        List<TextBox> heatMeterPositionBoxes1 = new List<TextBox>
                        {LengthTextBox3_1, LengthTextBox3_2, LengthTextBox3_3, LengthTextBox3_4};
                        List<TextBox> heatMeterPositionBoxes2 = new List<TextBox>
                        {LengthTextBox3_5, LengthTextBox3_6, LengthTextBox3_7, LengthTextBox3_8};
                        List<TextBox> heatMeterChannelBoxes1 = new List<TextBox>
                        {ChannelTextBox3_1, ChannelTextBox3_2, ChannelTextBox3_3, ChannelTextBox3_4};
                        List<TextBox> heatMeterChannelBoxes2 = new List<TextBox>
                        {ChannelTextBox3_5, ChannelTextBox3_6, ChannelTextBox3_7, ChannelTextBox3_8};
                        HeatMeterToBox(_heatMeter1, heatMeterPositionBoxes1, heatMeterChannelBoxes1, D1TextBox3_1, K1TextBox3_1);
                        HeatMeterToBox(_heatMeter2, heatMeterPositionBoxes2, heatMeterChannelBoxes2, D2TextBox3_2, K2TextBox3_2);
                    }
                    break;
                case TestMethod.ITMS:
                    {
                        //显示对应设置窗口TEST4
                        EmptyGroupBox.Size = new Size(0, 0);
                        TextGroupbox1.Size = new Size(0, 0);
                        TextGroupbox2.Size = new Size(0, 0);
                        TextGroupbox3.Size = new Size(0, 0);
                        TextGroupbox4.Size = new Size(1250, 855);
                        
                        _sample1 = new Sample("Sample1");
                        _sample2 = new Sample("Sample2");
                        string filePath = SlnIni.CreateDefaultItmsIni();
                        SlnIni.LoadItmsInfo(ref _sample1, ref _sample2, out force, out string thickness,
                            filePath,slnFilePath);
                        ForceTextBox4.Text = force;
                        FilmThickness2.Text = thickness;
                        List<TextBox> heatMeterPositionBoxes1 = new List<TextBox>
                        {LengthTextBox4_1, LengthTextBox4_2, LengthTextBox4_3, LengthTextBox4_4};
                        List<TextBox> heatMeterPositionBoxes2 = new List<TextBox>
                        {LengthTextBox4_11, LengthTextBox4_12, LengthTextBox4_13, LengthTextBox4_14};
                        List<TextBox> heatMeterChannelBoxes1 = new List<TextBox>
                        {ChannelTextBox4_1, ChannelTextBox4_2, ChannelTextBox4_3, ChannelTextBox4_4};
                        List<TextBox> heatMeterChannelBoxes2 = new List<TextBox>
                        {ChannelTextBox4_11, ChannelTextBox4_12, ChannelTextBox4_13, ChannelTextBox4_14};
                        HeatMeterToBox(_heatMeter1, heatMeterPositionBoxes1, heatMeterChannelBoxes1, D1TextBox4_1, K1TextBox4_1);
                        HeatMeterToBox(_heatMeter2, heatMeterPositionBoxes2, heatMeterChannelBoxes2, D2TextBox4_2, K4TextBox4_2);
                        List<TextBox> samplePositionBoxes1 = new List<TextBox>
                        {LengthTextBox4_5, LengthTextBox4_6, LengthTextBox4_7};
                        List<TextBox> sampleChannelBoxes1 = new List<TextBox> {
                        ChannelTextBox4_5, ChannelTextBox4_6, ChannelTextBox4_7
                    };
                        SampleToBox(_sample1, samplePositionBoxes1, sampleChannelBoxes1, ds1TextBox4_1);
                        List<TextBox> samplePositionBoxes2 = new List<TextBox>
                        {LengthTextBox4_8, LengthTextBox4_9, LengthTextBox4_10};
                        List<TextBox> sampleChannelBoxes2 = new List<TextBox> {
                        ChannelTextBox4_8, ChannelTextBox4_9, ChannelTextBox4_10
                    };
                        SampleToBox(_sample2, samplePositionBoxes2, sampleChannelBoxes2, ds2TextBox4_2);
                    }
                    break;
            }

            _testResultChart.Chart_Init(_heatMeter1, _heatMeter2, _sample1, _sample2);


            #endregion
        }
    }
}

using DataProcessor;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace multimeter
{
    public partial class SetupTest {
        private void apply_btm(object sender, EventArgs e) {
            string filePath = SlnIni.CreateDefaultSettingIni();
            switch (_method) {
                case TestMethod.KAPPA: apply_btm_1_Click(filePath);
                    break;
                case TestMethod.ITC:
                    apply_btm_2_Click(filePath);
                    break;
                case TestMethod.ITM:
                    apply_btm_3_Click(filePath);
                    break;
                case TestMethod.ITMS:
                    apply_btm_4_Click(filePath);
                    break;
                default: {
                    MessageBox.Show(@"请选择测试方法!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            INIHelper.Write("TestMethod", "method", _method.ToString(), filePath);
            INIHelper.Write("Data", "scan_interval", AppCfg.devicepara.Scan_interval.ToString(), filePath);
        }
        private void apply_btm_1_Click(string filePath) {
            if (User == User.ADVANCE) {
                List<TextBox> heatMeterPositionBoxes1 = new List<TextBox>()
                    {LengthTextBox1_1, LengthTextBox1_2, LengthTextBox1_3};
                List<TextBox> heatMeterChannelBoxes1 = new List<TextBox>()
                {
                    ChannelTextBox1_1, ChannelTextBox1_2, ChannelTextBox1_3
                };
                BoxToHeatMeter(ref _heatMeter1,heatMeterPositionBoxes1,heatMeterChannelBoxes1,K1TextBox1_1,S1TextBox1_1);
                List<TextBox> heatMeterPositionBoxes2 = new List<TextBox>()
                    {LengthTextBox1_8, LengthTextBox1_9, LengthTextBox1_10, LengthTextBox1_11};
                List<TextBox> heatMeterChannelBoxes2 = new List<TextBox>()
                {
                    ChannelTextBox1_8, ChannelTextBox1_9, ChannelTextBox1_10,ChannelTextBox1_11
                };
                BoxToHeatMeter(ref _heatMeter2, heatMeterPositionBoxes2, heatMeterChannelBoxes2, K2TextBox1_2, S2TextBox1_2);
            }

            
            
            List<TextBox> samplePositionBoxes = new List<TextBox>()
                {LengthTextBox1_5, LengthTextBox1_6, LengthTextBox1_7};
            List<TextBox> sampleChannelBoxes = new List<TextBox>()
            {
                ChannelTextBox1_5, ChannelTextBox1_6, ChannelTextBox1_7
            };
            BoxToSample(ref _sample1, samplePositionBoxes, sampleChannelBoxes, STextBox1_1);

            if (!CheckOtherText(ForceTextBox1.Text, _sample1))
            {
                MessageBox.Show(@"错误的数值,请重新设置!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            List<string> channelList = new List<string>();
            if (CheckChannelText(ref channelList, _sample1))
            {
                SlnIni.SaveKappaInfo(_sample1, ForceTextBox1.Text, filePath);
                if (User == User.ADVANCE) {
                    SlnIni.SaveHeatMeterInfo(_heatMeter1,_heatMeter2,SlnIni.CreateDefaultSettingIni());
                }
                SlnIni.WriteChannelInfo(channelList);
            }
            else
            {
                MessageBox.Show(@"错误的频道,请重新设置!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void apply_btm_2_Click(string filePath)
        {
            if (User == User.ADVANCE)
            {
                List<TextBox> heatMeterPositionBoxes1 = new List<TextBox>()
                    {LengthTextBox2_1, LengthTextBox2_2, LengthTextBox2_3};
                List<TextBox> heatMeterChannelBoxes1 = new List<TextBox>()
                {
                    ChannelTextBox2_1, ChannelTextBox2_2, ChannelTextBox2_3
                };
                BoxToHeatMeter(ref _heatMeter1, heatMeterPositionBoxes1, heatMeterChannelBoxes1, K1TextBox2_1, S1TextBox2_1);
                List<TextBox> heatMeterPositionBoxes2 = new List<TextBox>()
                    {LengthTextBox2_11, LengthTextBox2_12, LengthTextBox2_13, LengthTextBox2_14};
                List<TextBox> heatMeterChannelBoxes2 = new List<TextBox>()
                {
                    ChannelTextBox2_11, ChannelTextBox2_12, ChannelTextBox2_13,ChannelTextBox2_14
                };
                BoxToHeatMeter(ref _heatMeter2, heatMeterPositionBoxes2, heatMeterChannelBoxes2, K2TextBox2_2, S2TextBox2_2);
            }


            List<TextBox> samplePositionBoxes1 = new List<TextBox>()
                {LengthTextBox2_5, LengthTextBox2_6, LengthTextBox2_7};
            List<TextBox> sampleChannelBoxes1 = new List<TextBox>()
            {
                ChannelTextBox2_5, ChannelTextBox2_6, ChannelTextBox2_7
            };
            BoxToSample(ref _sample1, samplePositionBoxes1, sampleChannelBoxes1, SuTextBox2_1);
            List<TextBox> samplePositionBoxes2 = new List<TextBox>()
                {LengthTextBox2_8, LengthTextBox2_9, LengthTextBox2_10};
            List<TextBox> sampleChannelBoxes2 = new List<TextBox>()
            {
                ChannelTextBox2_8, ChannelTextBox2_9, ChannelTextBox2_10
            };
            BoxToSample(ref _sample2, samplePositionBoxes2, sampleChannelBoxes2, SlTextBox2_2);
            if (!CheckOtherText(ForceTextBox2.Text, _sample1, _sample2))
            {
                MessageBox.Show(@"错误的数值,请重新设置!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            List<string> channelList = new List<string>();
            if (CheckChannelText(ref channelList, _sample1, _sample2))
            {
                SlnIni.SaveItcInfo(_sample1, _sample2, ForceTextBox2.Text, filePath);
                if (User == User.ADVANCE)
                {
                    SlnIni.SaveHeatMeterInfo(_heatMeter1, _heatMeter2, SlnIni.CreateDefaultSettingIni());
                }
                SlnIni.WriteChannelInfo(channelList);
            }
            else
            {
                MessageBox.Show(@"错误的频道,请重新设置!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void apply_btm_3_Click(string filePath)
        {
            if (User == User.ADVANCE)
            {
                List<TextBox> heatMeterPositionBoxes1 = new List<TextBox>()
                    {LengthTextBox3_1, LengthTextBox3_2, LengthTextBox3_3};
                List<TextBox> heatMeterChannelBoxes1 = new List<TextBox>()
                {
                    ChannelTextBox3_1, ChannelTextBox3_2, ChannelTextBox3_3
                };
                BoxToHeatMeter(ref _heatMeter1, heatMeterPositionBoxes1, heatMeterChannelBoxes1, K1TextBox3_1, S1TextBox3_1);
                List<TextBox> heatMeterPositionBoxes2 = new List<TextBox>()
                    {LengthTextBox3_5, LengthTextBox3_6, LengthTextBox3_7, LengthTextBox3_8};
                List<TextBox> heatMeterChannelBoxes2 = new List<TextBox>()
                {
                    ChannelTextBox3_5, ChannelTextBox3_6, ChannelTextBox3_7,ChannelTextBox3_8
                };
                BoxToHeatMeter(ref _heatMeter2, heatMeterPositionBoxes2, heatMeterChannelBoxes2, K2TextBox3_2, S2TextBox3_2);
            }
           

            if (!CheckOtherText(ForceTextBox3.Text, thickness: FilmThickness1.Text))
            {
                MessageBox.Show(@"错误的数值,请重新设置!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            SlnIni.SaveItmInfo(ForceTextBox3.Text, FilmThickness1.Text, filePath);
            List<string> channelList = new List<string>();
            if (CheckChannelText(ref channelList))
            {
                if (User == User.ADVANCE)
                {
                    SlnIni.SaveHeatMeterInfo(_heatMeter1, _heatMeter2, SlnIni.CreateDefaultSettingIni());
                }
                SlnIni.WriteChannelInfo(channelList);
            }
            else
            {
                MessageBox.Show(@"错误的频道,请重新设置!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void apply_btm_4_Click(string filePath)
        {
            if (User == User.ADVANCE)
            {
                List<TextBox> heatMeterPositionBoxes1 = new List<TextBox>()
                    {LengthTextBox4_1, LengthTextBox4_2, LengthTextBox4_3};
                List<TextBox> heatMeterChannelBoxes1 = new List<TextBox>()
                {
                    ChannelTextBox4_1, ChannelTextBox4_2, ChannelTextBox4_3
                };
                BoxToHeatMeter(ref _heatMeter1, heatMeterPositionBoxes1, heatMeterChannelBoxes1, K1TextBox4_1, S1TextBox4_1);
                List<TextBox> heatMeterPositionBoxes2 = new List<TextBox>()
                    {LengthTextBox4_11, LengthTextBox4_12, LengthTextBox4_13, LengthTextBox4_14};
                List<TextBox> heatMeterChannelBoxes2 = new List<TextBox>()
                {
                    ChannelTextBox4_11, ChannelTextBox4_12, ChannelTextBox4_13,ChannelTextBox4_14
                };
                BoxToHeatMeter(ref _heatMeter2, heatMeterPositionBoxes2, heatMeterChannelBoxes2, K4TextBox4_2, S2TextBox4_2);
            }
           

            List<TextBox> samplePositionBoxes1 = new List<TextBox>()
                {LengthTextBox4_5, LengthTextBox4_6, LengthTextBox4_7};
            List<TextBox> sampleChannelBoxes1 = new List<TextBox>()
            {
                ChannelTextBox4_5, ChannelTextBox4_6, ChannelTextBox4_7
            };
            BoxToSample(ref _sample1, samplePositionBoxes1, sampleChannelBoxes1, SuTextBox4_1);
            List<TextBox> samplePositionBoxes2 = new List<TextBox>()
                {LengthTextBox4_8, LengthTextBox4_9, LengthTextBox4_10};
            List<TextBox> sampleChannelBoxes2 = new List<TextBox>()
            {
                ChannelTextBox4_8, ChannelTextBox4_9, ChannelTextBox4_10
            };
            BoxToSample(ref _sample2, samplePositionBoxes2, sampleChannelBoxes2, SlTextBox4_2);

            if (!CheckOtherText(ForceTextBox4.Text, _sample1, _sample2, FilmThickness2.Text))
            {
                MessageBox.Show(@"错误的数值,请重新设置!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            List<string> channelList = new List<string>();


            if (CheckChannelText(ref channelList, _sample1, _sample2))
            {
                SlnIni.SaveItmsInfo(_sample1, _sample2, ForceTextBox4.Text, FilmThickness2.Text, filePath);
                if (User == User.ADVANCE)
                {
                    SlnIni.SaveHeatMeterInfo(_heatMeter1, _heatMeter2, SlnIni.CreateDefaultSettingIni());
                }
                SlnIni.WriteChannelInfo(channelList);
            }
            else
            {
                MessageBox.Show(@"错误的频道,请重新设置!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CheckChannelText(ref List<string> channelList, Sample sample1 = null, Sample sample2 = null)
        {

            channelList.AddRange(_heatMeter1.Channel);
            channelList.AddRange(_heatMeter2.Channel);
            if (sample1 != null)
                channelList.AddRange(sample1.Channel);
            if (sample2 != null)
                channelList.AddRange(sample2.Channel);
            return CheckData.CheckChannelList(channelList);
        }

        private bool CheckOtherText(string force, Sample sample1 = null, Sample sample2 = null, string thickness = null)
        {
            List<string> positionList = new List<string>();
            positionList.AddRange(_heatMeter1.Position);
            positionList.Add(_heatMeter1.Area);
            positionList.AddRange(_heatMeter2.Position);
            positionList.Add(_heatMeter2.Area);
            positionList.Add(force);
            if (sample1 != null)
            {
                positionList.AddRange(sample1.Position);
                positionList.Add(sample1.Area);
            }

            if (sample2 != null)
            {
                positionList.AddRange(sample2.Position);
                positionList.Add(sample2.Area);
            }

            if (thickness != null)
            {
                positionList.Add(thickness);
            }

            return CheckData.CheckDoubleList(positionList);
        }
        private void HeatMeterToBox(HeatMeter heatMeter, List<TextBox> positionBoxes, List<TextBox> channelBoxes, TextBox areaBox,
            TextBox kappaBox)
        {
            for (int i = 0; i < heatMeter.TestPoint; i++)
            {
                positionBoxes[i].Text = heatMeter.Position[i];
                channelBoxes[i].Text = heatMeter.Channel[i];
            }
            areaBox.Text = heatMeter.Area;
            kappaBox.Text = heatMeter.Kappa;
        }
        private void SampleToBox(Sample sample, List<TextBox> positionBoxes, List<TextBox> channelBoxes, TextBox areaBox
        )
        {
            for (int i = 0; i < sample.TestPoint; i++)
            {
                positionBoxes[i].Text = sample.Position[i];
                channelBoxes[i].Text = sample.Channel[i];
            }
            areaBox.Text = sample.Area;
        }
        private void BoxToHeatMeter(ref HeatMeter heatMeter, List<TextBox> positionBoxes, List<TextBox> channelBoxes,
            TextBox kappaBox,TextBox areaBox)
        {
            for (int i = 0; i < heatMeter.TestPoint; i++)
            {
                heatMeter.Position[i] = positionBoxes[i].Text;
                heatMeter.Channel[i] = channelBoxes[i].Text;
            }
            heatMeter.Area = areaBox.Text;
            heatMeter.Kappa = kappaBox.Text;
        }
        private void BoxToSample(ref Sample sample, List<TextBox> positionBoxes, List<TextBox> channelBoxes, TextBox areaBox
        )
        {
            for (int i = 0; i < sample.TestPoint; i++)
            {
                sample.Position[i] = positionBoxes[i].Text;
                sample.Channel[i] = channelBoxes[i].Text;
            }
            sample.Area = areaBox.Text;
        }
        public void AllTextBoxEnable()
        {
            List<GroupBox> groupBoxs = new List<GroupBox>()
                {TextGroupbox1, TextGroupbox2, TextGroupbox3, TextGroupbox4};
            groupBoxs.ForEach(c=> {
                foreach (var control in c.Controls) {
                    if (control.GetType().ToString() == "System.Windows.Forms.TextBox") {
                        ((TextBox)control).Enabled = true;
                    }
                }
            });
        }

        public void NormalTextBoxEnable(bool enable) {
            List<TextBox> textBoxes = new List<TextBox>();
            switch (_method) {
                case TestMethod.KAPPA: {
                    List<TextBox> textBoxes1 = new List<TextBox>()
                        {LengthTextBox1_5, LengthTextBox1_6, LengthTextBox1_7,
                        ChannelTextBox1_5, ChannelTextBox1_6, ChannelTextBox1_7,
                        STextBox1_1,ForceTextBox1};
                    textBoxes.AddRange(textBoxes1);
                } break;
                case TestMethod.ITC: {
                    List<TextBox> textBoxes2 = new List<TextBox>() {
                        LengthTextBox2_5, LengthTextBox2_6, LengthTextBox2_7,
                        ChannelTextBox2_5, ChannelTextBox2_6, ChannelTextBox2_7,
                        LengthTextBox2_8, LengthTextBox2_9, LengthTextBox2_10,
                        ChannelTextBox2_8, ChannelTextBox2_9, ChannelTextBox2_10,
                        SuTextBox2_1,SlTextBox2_2,ForceTextBox2};
                    textBoxes.AddRange(textBoxes2);
                    } break;
                case TestMethod.ITM: {
                    List<TextBox> textBoxes3 = new List<TextBox>() {
                        ForceTextBox3, FilmThickness1};
                    textBoxes.AddRange(textBoxes3);
                    } break;
                case TestMethod.ITMS: {
                    List<TextBox> textBoxes4 = new List<TextBox>() {
                        LengthTextBox4_5, LengthTextBox4_6, LengthTextBox4_7,
                        ChannelTextBox4_5, ChannelTextBox4_6, ChannelTextBox4_7,
                        LengthTextBox4_8, LengthTextBox4_9, LengthTextBox4_10,
                        ChannelTextBox4_8, ChannelTextBox4_9, ChannelTextBox4_10,
                        SuTextBox4_1,SlTextBox4_2, ForceTextBox4, FilmThickness2};
                    textBoxes.AddRange(textBoxes4);
                    } break;
                default: {
                    return;
                }
            }
            foreach (TextBox textBox in textBoxes) {
                if (enable) {
                    textBox.Enabled = true;
                }
                else textBox.Enabled = false;
            }
        }


    }
}
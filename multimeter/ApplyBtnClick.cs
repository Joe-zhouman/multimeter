using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DataProcessor;
using System.IO;
using System.Runtime.CompilerServices;
using CCWin;
using CCWin.SkinControl;
using System.Runtime.InteropServices;
using System.Threading;

namespace multimeter
{
    public partial class SetupTest
    {
        private void apply_btm_1_Click(object sender, EventArgs e)
        {

            string filePath = SlnIni.CreateDefaultKappaIni();

            List<TextBox> samplePositionBoxes = new List<TextBox>()
                {LengthTextBox1_5, LengthTextBox1_6, LengthTextBox1_7};
            List<TextBox> sampleChannelBoxes = new List<TextBox>()
            {
                ChannelTextBox1_5, ChannelTextBox1_6, ChannelTextBox1_7
            };
            BoxToSample(ref sample1, samplePositionBoxes, sampleChannelBoxes, ds1TextBox2_1);

            if (!CheckOtherText(ForceTextBox1.Text, sample1))
            {
                MessageBox.Show(@"错误的数值,请重新设置!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            List<string> channelList = new List<string>();
            if (CheckChannelText(ref channelList, sample1))
            {
                SlnIni.SaveKappaInfo(sample1, ForceTextBox1.Text, filePath);
                SlnIni.WriteChannelInfo(channelList);
            }
            else
            {
                MessageBox.Show(@"错误的频道,请重新设置!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void apply_btm_2_Click(object sender, EventArgs e)
        {

            string filePath = SlnIni.CreateDefaultItcIni();

            List<TextBox> samplePositionBoxes1 = new List<TextBox>()
                {LengthTextBox2_5, LengthTextBox2_6, LengthTextBox2_7};
            List<TextBox> sampleChannelBoxes1 = new List<TextBox>()
            {
                ChannelTextBox2_5, ChannelTextBox2_6, ChannelTextBox2_7
            };
            BoxToSample(ref sample1, samplePositionBoxes1, sampleChannelBoxes1, ds1TextBox2_1);
            List<TextBox> samplePositionBoxes2 = new List<TextBox>()
                {LengthTextBox2_8, LengthTextBox2_9, LengthTextBox2_10};
            List<TextBox> sampleChannelBoxes2 = new List<TextBox>()
            {
                ChannelTextBox2_8, ChannelTextBox2_9, ChannelTextBox2_10
            };
            BoxToSample(ref sample2, samplePositionBoxes2, sampleChannelBoxes2, ds2TextBox2_2);
            if (!CheckOtherText(ForceTextBox2.Text, sample1, sample2))
            {
                MessageBox.Show(@"错误的数值,请重新设置!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            List<string> channelList = new List<string>();
            if (CheckChannelText(ref channelList, sample1, sample2))
            {
                SlnIni.SaveItcInfo(sample1, sample2, ForceTextBox2.Text, filePath);
                SlnIni.WriteChannelInfo(channelList);
            }
            else
            {
                MessageBox.Show(@"错误的频道,请重新设置!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void apply_btm_3_Click(object sender, EventArgs e)
        {
            string filePath = SlnIni.CreateDefaultItmIni();

            if (!CheckOtherText(ForceTextBox3.Text, thickness: FilmThickness1.Text))
            {
                MessageBox.Show(@"错误的数值,请重新设置!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            SlnIni.SaveItmInfo(ForceTextBox3.Text, FilmThickness1.Text, filePath);
            List<string> channelList = new List<string>();
            if (CheckChannelText(ref channelList))
            {
                SlnIni.WriteChannelInfo(channelList);
            }
            else
            {
                MessageBox.Show(@"错误的频道,请重新设置!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void apply_btm_4_Click(object sender, EventArgs e)
        {
            string filePath = SlnIni.CreateDefaultItmsIni();

            List<TextBox> samplePositionBoxes1 = new List<TextBox>()
                {LengthTextBox4_5, LengthTextBox4_6, LengthTextBox4_7};
            List<TextBox> sampleChannelBoxes1 = new List<TextBox>()
            {
                ChannelTextBox4_5, ChannelTextBox4_6, ChannelTextBox4_7
            };
            BoxToSample(ref sample1, samplePositionBoxes1, sampleChannelBoxes1, ds1TextBox4_1);
            List<TextBox> samplePositionBoxes2 = new List<TextBox>()
                {LengthTextBox4_8, LengthTextBox4_9, LengthTextBox4_10};
            List<TextBox> sampleChannelBoxes2 = new List<TextBox>()
            {
                ChannelTextBox4_8, ChannelTextBox4_9, ChannelTextBox4_10
            };
            BoxToSample(ref sample2, samplePositionBoxes2, sampleChannelBoxes2, ds2TextBox4_2);

            if (!CheckOtherText(ForceTextBox4.Text, sample1, sample2, FilmThickness2.Text))
            {
                MessageBox.Show(@"错误的数值,请重新设置!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            List<string> channelList = new List<string>();


            if (CheckChannelText(ref channelList, sample1, sample2))
            {
                SlnIni.SaveItmsInfo(sample1, sample2, ForceTextBox4.Text, FilmThickness2.Text, filePath);
                SlnIni.WriteChannelInfo(channelList);
            }
            else
            {
                MessageBox.Show(@"错误的频道,请重新设置!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private bool CheckChannelText(ref List<string> channelList, Sample sample1 = null, Sample sample2 = null)
        {

            channelList.AddRange(heatMeter1.Channel);
            channelList.AddRange(heatMeter2.Channel);
            if (sample1 != null)
                channelList.AddRange(sample1.Channel);
            if (sample2 != null)
                channelList.AddRange(sample2.Channel);
            return CheckData.CheckChannelList(channelList);
        }

        private bool CheckOtherText(string force, Sample sample1 = null, Sample sample2 = null, string thickness = null)
        {
            List<string> positionList = new List<string>();
            positionList.AddRange(heatMeter1.Position);
            positionList.Add(heatMeter1.Diameter);
            positionList.AddRange(heatMeter2.Position);
            positionList.Add(heatMeter2.Diameter);
            positionList.Add(force);
            if (sample1 != null)
            {
                positionList.AddRange(sample1.Position);
                positionList.Add(sample1.Diameter);
            }

            if (sample2 != null)
            {
                positionList.AddRange(sample2.Position);
                positionList.Add(sample2.Diameter);
            }

            if (thickness != null)
            {
                positionList.Add(thickness);
            }

            return CheckData.CheckDoubleList(positionList);
        }

        public void TextBoxEnable()
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
    }
}
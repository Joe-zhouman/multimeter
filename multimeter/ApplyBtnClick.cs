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

namespace multimeter {
    public partial class SetupTest {
        private void apply_btm_1_Click(object sender, EventArgs e) {
            
            string filePath = SlnIni.CreateDefaultKappaIni();
            INIHelper.Write("Pressure", "force", ForceTextBox1.Text, filePath);
            Sample sample1 = new Sample("Sample1");
            List<TextBox> samplePositionBoxes = new List<TextBox>()
                {LengthTextBox1_5, LengthTextBox1_6, LengthTextBox1_7};
            List<TextBox> sampleChannelBoxes = new List<TextBox>() {
                ChannelTextBox1_5, ChannelTextBox1_6, ChannelTextBox1_7};
            BoxToSample(ref sample1, samplePositionBoxes, sampleChannelBoxes);

            List<string> channelList = new List<string>();
            if (CheckChannelList(ref channelList, sample1)) {
                sample1.SaveToIni(filePath);
            }
            else {
                MessageBox.Show(@"错误的频道,请重新设置!",@"警告",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void apply_btm_2_Click(object sender, EventArgs e) {

            string filePath = SlnIni.CreateDefaultItcIni();

            INIHelper.Write("Pressure", "force", ForceTextBox2.Text, filePath);

            Sample sample1 = new Sample("Sample1");
            Sample sample2 = new Sample("Sample2");
            List<TextBox> samplePositionBoxes1 = new List<TextBox>()
                {LengthTextBox2_5, LengthTextBox2_6, LengthTextBox2_7};
            List<TextBox> sampleChannelBoxes1 = new List<TextBox>() {
                ChannelTextBox2_5, ChannelTextBox2_6, ChannelTextBox2_7};
            BoxToSample(ref sample1, samplePositionBoxes1, sampleChannelBoxes1);
            List<TextBox> samplePositionBoxes2 = new List<TextBox>()
                {LengthTextBox2_8, LengthTextBox2_9, LengthTextBox2_10};
            List<TextBox> sampleChannelBoxes2 = new List<TextBox>() {
                ChannelTextBox2_8, ChannelTextBox2_9, ChannelTextBox2_10};
            BoxToSample(ref sample2, samplePositionBoxes2, sampleChannelBoxes2);
            List<string> channelList = new List<string>();
            if (CheckChannelList(ref channelList, sample1)) {
                sample1.SaveToIni(filePath);
                sample2.SaveToIni(filePath);
            } else {
                MessageBox.Show(@"错误的频道,请重新设置!", @"警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void apply_btm_3_Click(object sender, EventArgs e) {
            string filePath = SlnIni.CreateDefaultItmIni();
            INIHelper.Write("Pressure", "force", ForceTextBox3.Text, filePath);
            INIHelper.Write("ITM", "thickness", FilmThickness1.Text, filePath);
            List<string> channelList = new List<string>();
            if (!CheckChannelList(ref channelList)) {
                MessageBox.Show(@"错误的频道,请重新设置!", @"警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void apply_btm_4_Click(object sender, EventArgs e) {
            string filePath = SlnIni.CreateDefaultItmsIni();
            INIHelper.Write("Pressure", "force", ForceTextBox4.Text, filePath);
            INIHelper.Write("ITM", "thickness", FilmThickness2.Text, filePath);
            Sample sample1 = new Sample("Sample1");
            Sample sample2 = new Sample("Sample2");
            List<TextBox> samplePositionBoxes1 = new List<TextBox>()
                {LengthTextBox4_5, LengthTextBox4_6, LengthTextBox4_7};
            List<TextBox> sampleChannelBoxes1 = new List<TextBox>() {
                ChannelTextBox4_5, ChannelTextBox4_6, ChannelTextBox4_7};
            BoxToSample(ref sample1, samplePositionBoxes1, sampleChannelBoxes1);
            List<TextBox> samplePositionBoxes2 = new List<TextBox>()
                {LengthTextBox4_8, LengthTextBox4_9, LengthTextBox4_10};
            List<TextBox> sampleChannelBoxes2 = new List<TextBox>() {
                ChannelTextBox4_8, ChannelTextBox4_9, ChannelTextBox4_10};
            BoxToSample(ref sample2, samplePositionBoxes2, sampleChannelBoxes2);
            List<string> channelList = new List<string>();
            if (CheckChannelList(ref channelList, sample1)) {
                sample1.SaveToIni(filePath);
                sample2.SaveToIni(filePath);
            } else {
                MessageBox.Show(@"错误的频道,请重新设置!", @"警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        private bool CheckChannelList(List<string> channelList) {
            List<int> chnIntList = new List<int>();
            foreach (string chn in channelList) {
                try {
                    chnIntList.Add(Convert.ToInt16(chn));
                }
                catch (Exception) {
                    return false;
                }
            }
            chnIntList.Sort();
            if (chnIntList.First() >= 101 && chnIntList.Last() <=122) {
                return !HasSameElem(chnIntList);
            }

            if (chnIntList.First() >= 201 && chnIntList.Last() <= 222) {
                return !HasSameElem(chnIntList);
            }
            return true;
        }
        private bool CheckChannelList(ref List<string> channelList,Sample sample1 = null, Sample sample2 = null) {
            
            channelList.AddRange(heatMeter1.Channel);
            channelList.AddRange(heatMeter2.Channel);
            if(sample1 != null)
                channelList.AddRange(sample1.Channel);
            if(sample2 != null)
                channelList.AddRange(sample2.Channel);
            return CheckChannelList(channelList);
        }

        private bool HasSameElem<T>(IReadOnlyCollection<T> list) {
            if (list == null) return false;
            for (int i = 0; i < list.Count() - 1; i++) {
                if (list.ElementAt(i).Equals(list.ElementAt(i + 1)) ) {
                    return true;
                }
            }
            return false;
        }

        private void WriteChannelInfo(List<string> channelList) {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini");
            if (channelList.First().First() == '1') {
                INIHelper.Write("Card1", "enable", "0", filePath);
                for (int i = 101; i < 122; i++) {
                    INIHelper.Write(i.ToString(), "func", channelList.Contains(i.ToString()) ? "1" : "0", filePath);
                }
            }
            else {
                INIHelper.Write("Card1", "enable", "0", filePath);
                for (int i = 201; i < 222; i++) {
                    INIHelper.Write(i.ToString(), "func", channelList.Contains(i.ToString()) ? "1" : "0", filePath);
                }
            }
        }
    }
}
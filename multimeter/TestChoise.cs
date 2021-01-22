using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DataProcessor;

namespace multimeter {
    public partial class SetupTest {
        private void testchoose1_Click(object sender, EventArgs e) {
            ButtonEnable();
            _method = TestMethod.KAPPA;
            var slnFilePath = SlnIni.CreateDefaultIni();
            ShowKappaMenu();
            var filePath = SlnIni.CreateDefaultSettingIni();
            FileToBoxKappa(out _sample1, out _sample2,  _heatMeter1,  _heatMeter2, filePath, slnFilePath);
        }

        private void FileToBoxKappa(out Sample sample1,out Sample sample2,HeatMeter heatMeter1,HeatMeter heatMeter2, string filePath, string slnFilePath) {
            sample1 = new Sample("Sample1");
            sample2 = null;
            SlnIni.LoadKappaInfo(ref sample1, out var force, filePath, slnFilePath);
            ForceTextBox1.Text = force;
            var heatMeterPositionBoxes1 = new List<TextBox>
                {LengthTextBox1_1, LengthTextBox1_2, LengthTextBox1_3};
            var heatMeterPositionBoxes2 = new List<TextBox>
                {LengthTextBox1_8, LengthTextBox1_9, LengthTextBox1_10, LengthTextBox1_11};
            var heatMeterChannelBoxes1 = new List<TextBox>
                {ChannelTextBox1_1, ChannelTextBox1_2, ChannelTextBox1_3};
            var heatMeterChannelBoxes2 = new List<TextBox>
                {ChannelTextBox1_8, ChannelTextBox1_9, ChannelTextBox1_10, ChannelTextBox1_11};
            HeatMeterToBox(heatMeter1, heatMeterPositionBoxes1, heatMeterChannelBoxes1, S1TextBox1_1,
                K1TextBox1_1);
            HeatMeterToBox(heatMeter2, heatMeterPositionBoxes2, heatMeterChannelBoxes2, S2TextBox1_2,
                K2TextBox1_2);

            var samplePositionBoxes = new List<TextBox>
                {LengthTextBox1_5, LengthTextBox1_6, LengthTextBox1_7};
            var sampleChannelBoxes = new List<TextBox> {
                ChannelTextBox1_5, ChannelTextBox1_6, ChannelTextBox1_7
            };
            SampleToBox(sample1, samplePositionBoxes, sampleChannelBoxes, STextBox1_1);
        }

        private void ShowKappaMenu() {
            //显示对应设置窗口TEST1
            EmptyGroupBox.Size = new Size(0, 0);
            TextGroupbox1.Size = new Size(1250, 855);
            TextGroupbox2.Size = new Size(0, 0);
            TextGroupbox3.Size = new Size(0, 0);
            TextGroupbox4.Size = new Size(0, 0);
            TestChartGroupBox.Size = new Size(0, 0);
        }

        private void testchoose2_Click(object sender, EventArgs e) {
            ButtonEnable();
            _method = TestMethod.ITC;
            var slnFilePath = SlnIni.CreateDefaultIni();
            ShowItcMenu();

            var filePath = SlnIni.CreateDefaultSettingIni();
            FileToBoxITC(out _sample1, out _sample2, _heatMeter1, _heatMeter2, filePath, slnFilePath);
        }

        private void FileToBoxITC(out Sample sample1, out Sample sample2, HeatMeter heatMeter1, HeatMeter heatMeter2, string filePath, string slnFilePath) {
            sample1 = new Sample("Sample1");
            sample2 = new Sample("Sample2");
            SlnIni.LoadItcInfo(ref sample1, ref sample2, out var force, filePath, slnFilePath);
            ForceTextBox2.Text = force;
            var heatMeterPositionBoxes1 = new List<TextBox>
                {LengthTextBox2_1, LengthTextBox2_2, LengthTextBox2_3};
            var heatMeterPositionBoxes2 = new List<TextBox>
                {LengthTextBox2_11, LengthTextBox2_12, LengthTextBox2_13, LengthTextBox2_14};
            var heatMeterChannelBoxes1 = new List<TextBox>
                {ChannelTextBox2_1, ChannelTextBox2_2, ChannelTextBox2_3};
            var heatMeterChannelBoxes2 = new List<TextBox>
                {ChannelTextBox2_11, ChannelTextBox2_12, ChannelTextBox2_13, ChannelTextBox2_14};
            HeatMeterToBox(heatMeter1, heatMeterPositionBoxes1, heatMeterChannelBoxes1, S1TextBox2_1,
                K1TextBox2_1);
            HeatMeterToBox(heatMeter2, heatMeterPositionBoxes2, heatMeterChannelBoxes2, S2TextBox2_2,
                K2TextBox2_2);
            var samplePositionBoxes1 = new List<TextBox>
                {LengthTextBox2_5, LengthTextBox2_6, LengthTextBox2_7};
            var sampleChannelBoxes1 = new List<TextBox> {
                ChannelTextBox2_5, ChannelTextBox2_6, ChannelTextBox2_7
            };
            SampleToBox(sample1, samplePositionBoxes1, sampleChannelBoxes1, SuTextBox2_1);
            var samplePositionBoxes2 = new List<TextBox>
                {LengthTextBox2_8, LengthTextBox2_9, LengthTextBox2_10};
            var sampleChannelBoxes2 = new List<TextBox> {
                ChannelTextBox2_8, ChannelTextBox2_9, ChannelTextBox2_10
            };
            SampleToBox(sample2, samplePositionBoxes2, sampleChannelBoxes2, SlTextBox2_2);
        }

        private void ShowItcMenu() {
            //显示对应设置窗口TEST2
            EmptyGroupBox.Size = new Size(0, 0);
            TextGroupbox1.Size = new Size(0, 0);
            TextGroupbox2.Size = new Size(1250, 855);
            TextGroupbox3.Size = new Size(0, 0);
            TextGroupbox4.Size = new Size(0, 0);
            TestChartGroupBox.Size = new Size(0, 0);
        }

        private void testchoose3_Click(object sender, EventArgs e) {
            ButtonEnable();
            _method = TestMethod.ITM;
            var slnFilePath = SlnIni.CreateDefaultIni();
            ShowItmMenu();

            var filePath = SlnIni.CreateDefaultSettingIni();
            FileToBoxITM(out _sample1, out _sample2, _heatMeter1, _heatMeter2, filePath);
        }

        private void FileToBoxITM(out Sample sample1, out Sample sample2, HeatMeter heatMeter1, HeatMeter heatMeter2, string filePath) {
            sample1 = null;
            sample2 = null;
            SlnIni.LoadItmInfo(out var force, out var thickness, filePath);
            ForceTextBox3.Text = force;
            FilmThickness1.Text = thickness;
            var heatMeterPositionBoxes1 = new List<TextBox>
                {LengthTextBox3_1, LengthTextBox3_2, LengthTextBox3_3};
            var heatMeterPositionBoxes2 = new List<TextBox>
                {LengthTextBox3_5, LengthTextBox3_6, LengthTextBox3_7, LengthTextBox3_8};
            var heatMeterChannelBoxes1 = new List<TextBox>
                {ChannelTextBox3_1, ChannelTextBox3_2, ChannelTextBox3_3};
            var heatMeterChannelBoxes2 = new List<TextBox>
                {ChannelTextBox3_5, ChannelTextBox3_6, ChannelTextBox3_7, ChannelTextBox3_8};
            HeatMeterToBox(heatMeter1, heatMeterPositionBoxes1, heatMeterChannelBoxes1, S1TextBox3_1,
                K1TextBox3_1);
            HeatMeterToBox(heatMeter2, heatMeterPositionBoxes2, heatMeterChannelBoxes2, S2TextBox3_2,
                K2TextBox3_2);
        }

        private void ShowItmMenu() {
            //显示对应设置窗口TEST3
            EmptyGroupBox.Size = new Size(0, 0);
            TextGroupbox1.Size = new Size(0, 0);
            TextGroupbox2.Size = new Size(0, 0);
            TextGroupbox3.Size = new Size(1250, 855);
            TextGroupbox4.Size = new Size(0, 0);
            TestChartGroupBox.Size = new Size(0, 0);
        }

        private void testchoose4_Click(object sender, EventArgs e) {
            ButtonEnable();
            _method = TestMethod.ITMS;
            var slnFilePath = SlnIni.CreateDefaultIni();
            ShowItmsMenu();

            
            var filePath = SlnIni.CreateDefaultSettingIni();
            FileToBoxITMS(out _sample1, out _sample2, _heatMeter1, _heatMeter2, filePath, slnFilePath);
        }

        private void FileToBoxITMS(out Sample sample1, out Sample sample2, HeatMeter heatMeter1, HeatMeter heatMeter2, string filePath, string slnFilePath) {
            sample1 = new Sample("Sample1");
            sample2 = new Sample("Sample2");
            SlnIni.LoadItmsInfo(ref sample1, ref sample2, out var force, out var thickness,
                filePath, slnFilePath);
            ForceTextBox4.Text = force;
            FilmThickness2.Text = thickness;
            var heatMeterPositionBoxes1 = new List<TextBox>
                {LengthTextBox4_1, LengthTextBox4_2, LengthTextBox4_3};
            var heatMeterPositionBoxes2 = new List<TextBox>
                {LengthTextBox4_11, LengthTextBox4_12, LengthTextBox4_13, LengthTextBox4_14};
            var heatMeterChannelBoxes1 = new List<TextBox>
                {ChannelTextBox4_1, ChannelTextBox4_2, ChannelTextBox4_3};
            var heatMeterChannelBoxes2 = new List<TextBox>
                {ChannelTextBox4_11, ChannelTextBox4_12, ChannelTextBox4_13, ChannelTextBox4_14};
            HeatMeterToBox(heatMeter1, heatMeterPositionBoxes1, heatMeterChannelBoxes1, S1TextBox4_1,
                K1TextBox4_1);
            HeatMeterToBox(heatMeter2, heatMeterPositionBoxes2, heatMeterChannelBoxes2, S2TextBox4_2,
                K4TextBox4_2);
            var samplePositionBoxes1 = new List<TextBox>
                {LengthTextBox4_5, LengthTextBox4_6, LengthTextBox4_7};
            var sampleChannelBoxes1 = new List<TextBox> {
                ChannelTextBox4_5, ChannelTextBox4_6, ChannelTextBox4_7
            };
            SampleToBox(sample1, samplePositionBoxes1, sampleChannelBoxes1, SuTextBox4_1);
            var samplePositionBoxes2 = new List<TextBox>
                {LengthTextBox4_8, LengthTextBox4_9, LengthTextBox4_10};
            var sampleChannelBoxes2 = new List<TextBox> {
                ChannelTextBox4_8, ChannelTextBox4_9, ChannelTextBox4_10
            };
            SampleToBox(sample2, samplePositionBoxes2, sampleChannelBoxes2, SlTextBox4_2);
        }

        private void ShowItmsMenu() {
            //显示对应设置窗口TEST4
            EmptyGroupBox.Size = new Size(0, 0);
            TextGroupbox1.Size = new Size(0, 0);
            TextGroupbox2.Size = new Size(0, 0);
            TextGroupbox3.Size = new Size(0, 0);
            TextGroupbox4.Size = new Size(1250, 855);
            TestChartGroupBox.Size = new Size(0, 0);
        }


        private void testchoose1_MouseMove(object sender, MouseEventArgs e) {
            test1.BackColor = Color.DodgerBlue;
        }

        private void testchoose1_MouseLeave(object sender, EventArgs e) {
            test1.BackColor = Color.White;
        }

        private void testchoose2_MouseMove(object sender, MouseEventArgs e) {
            test2.BackColor = Color.DodgerBlue;
        }

        private void testchoose2_MouseLeave(object sender, EventArgs e) {
            test2.BackColor = Color.White;
        }

        private void testchoose3_MouseMove(object sender, MouseEventArgs e) {
            test3.BackColor = Color.DodgerBlue;
        }

        private void testchoose3_MouseLeave(object sender, EventArgs e) {
            test3.BackColor = Color.White;
        }

        private void testchoose4_MouseMove(object sender, MouseEventArgs e) {
            test4.BackColor = Color.DodgerBlue;
        }

        private void testchoose4_MouseLeave(object sender, EventArgs e) {
            test4.BackColor = Color.White;
        }

        private void ButtonEnable() {
            TestRun_Enable(true);
            Monitor_Enable(false);
            CurrentTestResult_Enable(false);
            HistoryTestResult_Enable(true);
            ExportResult_Enable(false);
            SerialPort_Enable(true);
            AdvancedSetting_Enable(true);
            MenuGroupBox.Visible = true;
            TestChoiseGroupBox.Visible = false;
            SoftwareNameLabel.Visible = false;
            TextResultGroupbox1.Visible = false;
            TextResultGroupbox2.Visible = false;
            TextResultGroupbox3.Visible = false;
            TextResultGroupbox4.Visible = false;
            TestTime.Text = "";
        }

    }
}
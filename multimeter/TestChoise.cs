using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DataAccess;
using Model;

namespace multimeter {
    public partial class SetupTest {
        public void TestChoosiest1_Click(object sender, EventArgs e) {
            ButtonEnable();
            _method = TestMethod.KAPPA;
            ShowKappaMenu();
            FileToBoxKappa(out _device, IniReadAndWrite.IniFilePath);
        }

        private void FileToBoxKappa(out TestDevice device, string settingFilePath) {
            DeviceInit(out device, settingFilePath, TestMethod.KAPPA);
            ForceTextBox1.Text = device.Force;
            var heatMeterPositionBoxes1 = new List<TextBox>
                {LengthTextBox1_1, LengthTextBox1_2, LengthTextBox1_3, LengthTextBox1_4};
            var heatMeterPositionBoxes2 = new List<TextBox>
                {LengthTextBox1_9, LengthTextBox1_10, LengthTextBox1_11, LengthTextBox1_12};
            var heatMeterChannelBoxes1 = new List<TextBox>
                {ChannelTextBox1_1, ChannelTextBox1_2, ChannelTextBox1_3, ChannelTextBox1_4};
            var heatMeterChannelBoxes2 = new List<TextBox>
                {ChannelTextBox1_9, ChannelTextBox1_10, ChannelTextBox1_11, ChannelTextBox1_12};
            SpecimenToBox(device.HeatMeter1, heatMeterPositionBoxes1, heatMeterChannelBoxes1, S1TextBox1_1,
                K1TextBox1_1);
            SpecimenToBox(device.HeatMeter2, heatMeterPositionBoxes2, heatMeterChannelBoxes2, S2TextBox1_2,
                K2TextBox1_2);
            var samplePositionBoxes = new List<TextBox>
                {LengthTextBox1_5, LengthTextBox1_6, LengthTextBox1_7, LengthTextBox1_8};
            var sampleChannelBoxes = new List<TextBox> {
                ChannelTextBox1_5, ChannelTextBox1_6, ChannelTextBox1_7, ChannelTextBox1_8
            };
            SpecimenToBox(device.Sample1, samplePositionBoxes, sampleChannelBoxes, STextBox1_1);
        }

        private static void DeviceInit(out TestDevice device, string filePath, TestMethod method) {
            device = new TestDevice(method);
            IniReadAndWrite.ReadDevicePara(ref device, filePath);
            IniReadAndWrite.ReadTempPara(ref device, filePath);
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

        public void TestChoosiest2_Click(object sender, EventArgs e) {
            ButtonEnable();
            _method = TestMethod.ITC;
            ShowItcMenu();
            FileToBoxItc(out _device, IniReadAndWrite.IniFilePath);
        }

        private void FileToBoxItc(out TestDevice device, string settingFilePath) {
            DeviceInit(out device, settingFilePath, TestMethod.ITC);
            ForceTextBox2.Text = device.Force;
            var heatMeterPositionBoxes1 = new List<TextBox>
                {LengthTextBox2_1, LengthTextBox2_2, LengthTextBox2_3, LengthTextBox2_4};
            var heatMeterPositionBoxes2 = new List<TextBox>
                {LengthTextBox2_13, LengthTextBox2_14, LengthTextBox2_15, LengthTextBox2_16};
            var heatMeterChannelBoxes1 = new List<TextBox>
                {ChannelTextBox2_1, ChannelTextBox2_2, ChannelTextBox2_3, ChannelTextBox2_4};
            var heatMeterChannelBoxes2 = new List<TextBox>
                {ChannelTextBox2_13, ChannelTextBox2_14, ChannelTextBox2_15, ChannelTextBox2_16};
            SpecimenToBox(device.HeatMeter1, heatMeterPositionBoxes1, heatMeterChannelBoxes1, S1TextBox2_1,
                K1TextBox2_1);
            SpecimenToBox(device.HeatMeter2, heatMeterPositionBoxes2, heatMeterChannelBoxes2, S2TextBox2_2,
                K2TextBox2_2);
            var samplePositionBoxes1 = new List<TextBox>
                {LengthTextBox2_5, LengthTextBox2_6, LengthTextBox2_7, LengthTextBox2_8};
            var sampleChannelBoxes1 = new List<TextBox> {
                ChannelTextBox2_5, ChannelTextBox2_6, ChannelTextBox2_7, ChannelTextBox2_8
            };
            SpecimenToBox(device.Sample1, samplePositionBoxes1, sampleChannelBoxes1, SuTextBox2_1);
            var samplePositionBoxes2 = new List<TextBox>
                {LengthTextBox2_9, LengthTextBox2_10, LengthTextBox2_11, LengthTextBox2_12};
            var sampleChannelBoxes2 = new List<TextBox> {
                ChannelTextBox2_9, ChannelTextBox2_10, ChannelTextBox2_11, ChannelTextBox2_12
            };
            SpecimenToBox(device.Sample2, samplePositionBoxes2, sampleChannelBoxes2, SlTextBox2_2);
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

        public void TestChoosiest3_Click(object sender, EventArgs e) {
            ButtonEnable();
            _method = TestMethod.ITM;
            ShowItmMenu();

            FileToBoxItm(out _device, IniReadAndWrite.IniFilePath);
        }

        private void FileToBoxItm(out TestDevice device, string settingFilePath) {
            DeviceInit(out device, settingFilePath, TestMethod.ITM);
            ForceTextBox3.Text = device.Force;
            ItmToBox(device.Itm, FilmThickness1);
            var heatMeterPositionBoxes1 = new List<TextBox>
                {LengthTextBox3_1, LengthTextBox3_2, LengthTextBox3_3, LengthTextBox3_4};
            var heatMeterPositionBoxes2 = new List<TextBox>
                {LengthTextBox3_5, LengthTextBox3_6, LengthTextBox3_7, LengthTextBox3_8};
            var heatMeterChannelBoxes1 = new List<TextBox>
                {ChannelTextBox3_1, ChannelTextBox3_2, ChannelTextBox3_3, ChannelTextBox3_4};
            var heatMeterChannelBoxes2 = new List<TextBox>
                {ChannelTextBox3_5, ChannelTextBox3_6, ChannelTextBox3_7, ChannelTextBox3_8};
            SpecimenToBox(device.HeatMeter1, heatMeterPositionBoxes1, heatMeterChannelBoxes1, S1TextBox3_1,
                K1TextBox3_1);
            SpecimenToBox(device.HeatMeter2, heatMeterPositionBoxes2, heatMeterChannelBoxes2, S2TextBox3_2,
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

        public void TestChoose4_Click(object sender, EventArgs e) {
            ButtonEnable();
            _method = TestMethod.ITMS;

            ShowItmsMenu();
            FileToBoxItms(out _device, IniReadAndWrite.IniFilePath);
        }

        private void FileToBoxItms(out TestDevice device, string settingFilePath) {
            DeviceInit(out device, settingFilePath, TestMethod.ITMS);
            ForceTextBox4.Text = device.Force;
            ItmToBox(device.Itm, FilmThickness2);
            var heatMeterPositionBoxes1 = new List<TextBox>
                {LengthTextBox4_1, LengthTextBox4_2, LengthTextBox4_3, LengthTextBox4_4};
            var heatMeterPositionBoxes2 = new List<TextBox>
                {LengthTextBox4_13, LengthTextBox4_14, LengthTextBox4_15, LengthTextBox4_16};
            var heatMeterChannelBoxes1 = new List<TextBox>
                {ChannelTextBox4_1, ChannelTextBox4_2, ChannelTextBox4_3, ChannelTextBox4_4};
            var heatMeterChannelBoxes2 = new List<TextBox>
                {ChannelTextBox4_13, ChannelTextBox4_14, ChannelTextBox4_15, ChannelTextBox4_16};
            SpecimenToBox(device.HeatMeter1, heatMeterPositionBoxes1, heatMeterChannelBoxes1, S1TextBox4_1,
                K1TextBox4_1);
            SpecimenToBox(device.HeatMeter2, heatMeterPositionBoxes2, heatMeterChannelBoxes2, S2TextBox4_2,
                K4TextBox4_2);
            var samplePositionBoxes1 = new List<TextBox>
                {LengthTextBox4_5, LengthTextBox4_6, LengthTextBox4_7, LengthTextBox4_8};
            var sampleChannelBoxes1 = new List<TextBox> {
                ChannelTextBox4_5, ChannelTextBox4_6, ChannelTextBox4_7, ChannelTextBox4_8
            };
            SpecimenToBox(device.Sample1, samplePositionBoxes1, sampleChannelBoxes1, SuTextBox4_1);
            var samplePositionBoxes2 = new List<TextBox>
                {LengthTextBox4_9, LengthTextBox4_10, LengthTextBox4_11, LengthTextBox4_12};
            var sampleChannelBoxes2 = new List<TextBox> {
                ChannelTextBox4_9, ChannelTextBox4_10, ChannelTextBox4_11, ChannelTextBox4_12
            };
            SpecimenToBox(device.Sample2, samplePositionBoxes2, sampleChannelBoxes2, SlTextBox4_2);
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
            StatusTextBox_Init();
            MenuGroupBox.Visible = true;
            TestChoiseGroupBox.Visible = false;
            SoftwareNameLabel.Visible = false;
            TextResultGroupbox1.Visible = false;
            TextResultGroupbox2.Visible = false;
            TextResultGroupbox3.Visible = false;
            TextResultGroupbox4.Visible = false;
            TestTime.Text = "";
        }

        private void SpecimenToBox(Specimen heatMeter, List<TextBox> positionBoxes, List<TextBox> channelBoxes,
            TextBox areaBox,
            TextBox kappaBox) {
            SpecimenToBox(heatMeter, positionBoxes, channelBoxes, areaBox);
            kappaBox.Text = heatMeter.Kappa;
        }

        private void SpecimenToBox(Specimen sample, List<TextBox> positionBoxes, List<TextBox> channelBoxes,
            TextBox areaBox) {
            for (var i = 0; i < sample.TestPoint; i++) {
                channelBoxes[i].Text = sample.Channel[i];
                positionBoxes[i].Text = sample.Channel[i] == "0" ? @"0" : sample.Position[i];
            }

            areaBox.Text = sample.Area;
        }

        public void ItmToBox(Itm itm, TextBox thicknessBox) {
            thicknessBox.Text = itm.Thickness;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using CCWin.Win32.Const;
using DataProcessor;

namespace multimeter {
    public partial class SetupTest : Form {

        private HeatMeter heatMeter1; 
        private HeatMeter heatMeter2;
        private Sample sample1;
        private Sample sample2;
        private string latestDataFile;
        private string latestIniFile;
        private string recvstr;
        private List<string> recentTenData;
        private TestMethod _method;
        private TestResultChart testResultChart=new TestResultChart();
        

        public SetupTest()
        {

            InitializeComponent();

        }

        public void button1_Click(object sender, EventArgs e) {
            #region //选择测试方法1
            TestChoose1.BackColor = Color.DodgerBlue;
            TestChoose2.BackColor = Color.LightGray;
            TestChoose3.BackColor = Color.LightGray;
            TestChoose4.BackColor = Color.LightGray;
            TestRun.Enabled = true;
            TestResult.Enabled = true;
            _method = TestMethod.KAPPA;
            ParSetting_Click(sender,e);
            
            #endregion
        }

        public void TestChoose2_Click(object sender, EventArgs e) {
            #region //选择测试方法2
            TestChoose1.BackColor = Color.LightGray;
            TestChoose2.BackColor = Color.DodgerBlue;
            TestChoose3.BackColor = Color.LightGray;
            TestChoose4.BackColor = Color.LightGray;
            TestRun.Enabled = true;
            TestResult.Enabled = true;
            _method = TestMethod.ITC;
            ParSetting_Click(sender, e);
            

            #endregion
        }

        public void TestChoose3_Click(object sender, EventArgs e) {
            #region //选择测试方法3
            TestChoose1.BackColor = Color.LightGray;
            TestChoose2.BackColor = Color.LightGray;
            TestChoose3.BackColor = Color.DodgerBlue;
            TestChoose4.BackColor = Color.LightGray;
            TestRun.Enabled = true;
            TestResult.Enabled = true;
            _method = TestMethod.ITM;
            ParSetting_Click(sender, e);
            

            #endregion
        }

        public void TestChoose4_Click(object sender, EventArgs e) {
            #region //选择测试方法4
            TestChoose1.BackColor = Color.LightGray;
            TestChoose2.BackColor = Color.LightGray;
            TestChoose3.BackColor = Color.LightGray;
            TestChoose4.BackColor = Color.DodgerBlue;
            TestRun.Enabled = true;
            TestResult.Enabled = true;
            _method = TestMethod.ITMS;
            ParSetting_Click(sender, e);

            #endregion
        }

        private void SerialPort_Click(object sender, EventArgs e) {
            skinGroupBox1.BringToFront();
            skinGroupBox1.Size = new Size(248, 247);
            
        }

        private void SerialPortEnsure_Click(object sender, EventArgs e)
        {
            skinGroupBox1.Size = new Size(0, 0);
        }
        public void AdvancedSetting_Click(object sender, EventArgs e)
        {
            LoginForm loginForm=new LoginForm();
            loginForm.Show(this);
        }
        private void ParSetting_Click(object sender, EventArgs e) {
            #region //参数设置窗口打开

            string force;
            switch (_method) {
                case TestMethod.KAPPA: {
                        //显示对应设置窗口TEST1
                    EmptyGroupBox.Size = new Size(0, 0);
                    TextGroupbox1.Size = new Size(1250, 855);
                    TextGroupbox2.Size = new Size(0, 0);
                    TextGroupbox3.Size = new Size(0, 0);
                    TextGroupbox4.Size = new Size(0, 0);
                    //skinGroupBox2.Size = new Size(0, 0);
                    string filePath = SlnIni.CreateDefaultKappaIni();
                    sample1 = new Sample("Sample1");
                        sample2 = null;
                    SlnIni.LoadKappaInfo(ref sample1, out force, filePath);
                    ForceTextBox1.Text = force;
                    List<TextBox> heatMeterPositionBoxes1 = new List<TextBox>
                        {LengthTextBox1_1, LengthTextBox1_2, LengthTextBox1_3, LengthTextBox1_4};
                    List<TextBox> heatMeterPositionBoxes2 = new List<TextBox>
                        {LengthTextBox1_8, LengthTextBox1_9, LengthTextBox1_10, LengthTextBox1_11};
                    List<TextBox> heatMeterChannelBoxes1 = new List<TextBox>
                        {ChannelTextBox1_1, ChannelTextBox1_2, ChannelTextBox1_3, ChannelTextBox1_4};
                    List<TextBox> heatMeterChannelBoxes2 = new List<TextBox>
                        {ChannelTextBox1_8, ChannelTextBox1_9, ChannelTextBox1_10, ChannelTextBox1_11};
                    HeatMeterToBox(heatMeter1, heatMeterPositionBoxes1, heatMeterChannelBoxes1,D1TextBox1_1, K1TextBox1_1);
                    HeatMeterToBox(heatMeter2, heatMeterPositionBoxes2, heatMeterChannelBoxes2,D2TextBox1_2, K2TextBox1_2);

                    List<TextBox> samplePositionBoxes = new List<TextBox>
                        {LengthTextBox1_5, LengthTextBox1_6, LengthTextBox1_7};
                    List<TextBox> sampleChannelBoxes = new List<TextBox> {
                        ChannelTextBox1_5, ChannelTextBox1_6, ChannelTextBox1_7
                    };
                    SampleToBox(sample1, samplePositionBoxes, sampleChannelBoxes,dsTextBox1_1);
                }
                    break;
                case TestMethod.ITC: {
                        //显示对应设置窗口TEST2
                    EmptyGroupBox.Size = new Size(0, 0);
                    TextGroupbox1.Size = new Size(0, 0);
                    TextGroupbox2.Size = new Size(1250, 855);
                    TextGroupbox3.Size = new Size(0, 0);
                    TextGroupbox4.Size = new Size(0, 0);
                    string filePath = SlnIni.CreateDefaultItcIni();
                    sample1 = new Sample("Sample1");
                    sample2 = new Sample("Sample2");
                    SlnIni.LoadItcInfo(ref sample1, ref sample2, out force, filePath);
                    ForceTextBox2.Text = force;
                    List<TextBox> heatMeterPositionBoxes1 = new List<TextBox>
                        {LengthTextBox2_1, LengthTextBox2_2, LengthTextBox2_3, LengthTextBox2_4};
                    List<TextBox> heatMeterPositionBoxes2 = new List<TextBox>
                        {LengthTextBox2_11, LengthTextBox2_12, LengthTextBox2_13, LengthTextBox2_14};
                    List<TextBox> heatMeterChannelBoxes1 = new List<TextBox>
                        {ChannelTextBox2_1, ChannelTextBox2_2, ChannelTextBox2_3, ChannelTextBox2_4};
                    List<TextBox> heatMeterChannelBoxes2 = new List<TextBox>
                        {ChannelTextBox2_11, ChannelTextBox2_12, ChannelTextBox2_13, ChannelTextBox2_14};
                    HeatMeterToBox(heatMeter1, heatMeterPositionBoxes1, heatMeterChannelBoxes1,D1TextBox2_1, K1TextBox2_1);
                    HeatMeterToBox(heatMeter2, heatMeterPositionBoxes2, heatMeterChannelBoxes2,D2TextBox2_2, K2TextBox2_2);
                    List<TextBox> samplePositionBoxes1 = new List<TextBox>
                        {LengthTextBox2_5, LengthTextBox2_6, LengthTextBox2_7};
                    List<TextBox> sampleChannelBoxes1 = new List<TextBox> {
                        ChannelTextBox2_5, ChannelTextBox2_6, ChannelTextBox2_7
                    };
                    SampleToBox(sample1, samplePositionBoxes1, sampleChannelBoxes1,ds1TextBox2_1);
                    List<TextBox> samplePositionBoxes2 = new List<TextBox>
                        {LengthTextBox2_8, LengthTextBox2_9, LengthTextBox2_10};
                    List<TextBox> sampleChannelBoxes2 = new List<TextBox> {
                        ChannelTextBox2_8, ChannelTextBox2_9, ChannelTextBox2_10
                    };
                    SampleToBox(sample2, samplePositionBoxes2, sampleChannelBoxes2,ds2TextBox2_2);
                }
                    break;
                case TestMethod.ITM: {
                        //显示对应设置窗口TEST3
                    EmptyGroupBox.Size = new Size(0, 0);
                    TextGroupbox1.Size = new Size(0, 0);
                    TextGroupbox2.Size = new Size(0, 0);
                    TextGroupbox3.Size = new Size(1250, 855);
                    TextGroupbox4.Size = new Size(0, 0);
                    string filePath = SlnIni.CreateDefaultItmIni();
                        sample1 = null;
                        sample2 = null;
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
                    HeatMeterToBox(heatMeter1, heatMeterPositionBoxes1, heatMeterChannelBoxes1,D1TextBox3_1, K1TextBox3_1);
                    HeatMeterToBox(heatMeter2, heatMeterPositionBoxes2, heatMeterChannelBoxes2,D2TextBox3_2, K2TextBox3_2);
                }
                    break;
                case TestMethod.ITMS: {
                        //显示对应设置窗口TEST4
                    EmptyGroupBox.Size = new Size(0, 0);
                    TextGroupbox1.Size = new Size(0, 0);
                    TextGroupbox2.Size = new Size(0, 0);
                    TextGroupbox3.Size = new Size(0, 0);
                    TextGroupbox4.Size = new Size(1250, 855);
                    string filePath = SlnIni.CreateDefaultItmsIni();
                    sample1 = new Sample("Sample1");
                    sample2 = new Sample("Sample2");
                    SlnIni.LoadItmsInfo(ref sample1, ref sample2, out force, out string thickness,
                        filePath);
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
                    HeatMeterToBox(heatMeter1, heatMeterPositionBoxes1, heatMeterChannelBoxes1,D1TextBox4_1, K1TextBox4_1);
                    HeatMeterToBox(heatMeter2, heatMeterPositionBoxes2, heatMeterChannelBoxes2,D2TextBox4_2, K4TextBox4_2);
                    List<TextBox> samplePositionBoxes1 = new List<TextBox>
                        {LengthTextBox4_5, LengthTextBox4_6, LengthTextBox4_7};
                    List<TextBox> sampleChannelBoxes1 = new List<TextBox> {
                        ChannelTextBox4_5, ChannelTextBox4_6, ChannelTextBox4_7
                    };
                    SampleToBox(sample1, samplePositionBoxes1, sampleChannelBoxes1,ds1TextBox4_1);
                    List<TextBox> samplePositionBoxes2 = new List<TextBox>
                        {LengthTextBox4_8, LengthTextBox4_9, LengthTextBox4_10};
                    List<TextBox> sampleChannelBoxes2 = new List<TextBox> {
                        ChannelTextBox4_8, ChannelTextBox4_9, ChannelTextBox4_10
                    };
                    SampleToBox(sample2, samplePositionBoxes2, sampleChannelBoxes2,ds2TextBox4_2);
                }
                    break;
            }

            testResultChart.Chart_Init(heatMeter1,heatMeter2,sample1,sample2,_method);
            

            #endregion
        }

        private void TestRun_Click(object sender, EventArgs e) {
            #region //测试运行

            //以下代码为在测试运行时，一部分按键失效
            TestChoose1.Enabled = false;
            TestChoose2.Enabled = false;
            TestChoose3.Enabled = false;
            TestChoose4.Enabled = false;
            SerialPort.Enabled = false;
            ParSetting.Enabled = false;
            TestRun.Enabled = false;
            TestStop.Enabled = true;
            TestResult.Enabled = false;
            btn_start();

            #endregion
        }

        private void TestStop_Click(object sender, EventArgs e) {
            #region //测试暂停

            //以下代码为在测试暂停时，一部分按键恢复
            TestChoose1.Enabled = true;
            TestChoose2.Enabled = true;
            TestChoose3.Enabled = true;
            TestChoose4.Enabled = true;
            SerialPort.Enabled = true;
            ParSetting.Enabled = true;
            TestRun.Enabled = true;
            TestStop.Enabled = false;
            TestResult.Enabled = true;

            #endregion

            btn_stop();
        }
        private void Monitor_Click(object sender, EventArgs e)
        {
            if (testResultChart.IsAccessible == true)
                testResultChart.Hide();
            else testResultChart.Show();
        }
        private void TestResultChart_FormClosing(object sender, EventArgs e) {
            if(testResultChart.DialogResult == DialogResult.Cancel) return;
            btn_stop();
            MessageBox.Show(@"计算已收敛,是否结束计算并显示结果?", @"提示", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (testResultChart.DialogResult != DialogResult.Yes) return;
            TestResult_Click(this,e);

        }
        //---------------------------------------------------------------------------------------串口设置-------------------------------------------------------------------------------------------------


        private void combox_comport_SelectedValueChanged(object sender, EventArgs e) {
            AppCfg.devicepara.SerialPort = combox_comport.Text;
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini"); //在当前程序路径创建
            INIHelper.Write("Serial", "port", combox_comport.Text, filePath);
        }

        private void combox_baudrate_SelectedValueChanged(object sender, EventArgs e) {
            AppCfg.devicepara.SerialBaudRate = combox_baudrate.Text;
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini"); //在当前程序路径创建
            INIHelper.Write("Serial", "baudrate", combox_baudrate.Text, filePath);
        }

        private void combox_databits_SelectedValueChanged(object sender, EventArgs e) {
            AppCfg.devicepara.SerialDataBits = combox_databits.Text;
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini"); //在当前程序路径创建
            INIHelper.Write("Serial", "databits", combox_databits.Text, filePath);
        }

        private void combox_stopbits_SelectedValueChanged(object sender, EventArgs e) {
            AppCfg.devicepara.SerialStopBits = combox_stopbits.Text;
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini"); //在当前程序路径创建
            INIHelper.Write("Serial", "stopbites", combox_stopbits.Text, filePath);
        }

        private void combox_parity_SelectedValueChanged(object sender, EventArgs e) {
            AppCfg.devicepara.SerialParity = combox_parity.Text;
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini"); //在当前程序路径创建
            INIHelper.Write("Serial", "parity", combox_parity.Text, filePath);
        }

        private void edit_scan_interval_TextChanged(object sender, EventArgs e) {
            if (edit_scan_interval.Text == "") return;
            int scanInterval = CheckData.CheckTextChange(edit_scan_interval.Text);
            if (scanInterval == -1) MessageBox.Show(@"错误的采集频率", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

            AppCfg.devicepara.Scan_interval = scanInterval;
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini"); //在当前程序路径创建
            INIHelper.Write("SYS", "scan_interval", edit_scan_interval.Text, filePath);
        }

        private void edit_save_interval_TextChanged(object sender, EventArgs e) {
            if (edit_save_interval.Text == "")
                return;
            int saveInterval = CheckData.CheckTextChange(edit_save_interval.Text);
            if (saveInterval == -1) MessageBox.Show(@"错误的自动保存频率", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            AppCfg.devicepara.Save_interval = saveInterval;
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini"); //在当前程序路径创建
            INIHelper.Write("SYS", "save_interval", edit_save_interval.Text, filePath);
        }

        //--------------------------------------------------------------------------------------------串口采集--------------------------------------------------------------------------------------------------
        

        private void ReadPara() {
            #region //读取ini

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini");
            AppCfg.devicepara.SerialPort = INIHelper.Read("Serial", "port", "COM1", filePath);
            AppCfg.devicepara.SerialBaudRate = INIHelper.Read("Serial", "baudrate", "9600", filePath);
            AppCfg.devicepara.SerialDataBits = INIHelper.Read("Serial", "databits", "8", filePath);

            AppCfg.devicepara.SerialStopBits = INIHelper.Read("Serial", "stopbites", "1", filePath);
            AppCfg.devicepara.SerialParity = INIHelper.Read("Serial", "parity", "none", filePath);
            foreach (Card i in AppCfg.devicepara.Cardlist1) {
                i.name = INIHelper.Read(i.CHN, "name", "", filePath);
                string func = INIHelper.Read(i.CHN, "func", "0", filePath);
                i.func = int.Parse(func);
            }

            foreach (Card i in AppCfg.devicepara.Cardlist2) {
                i.name = INIHelper.Read(i.CHN, "name", "", filePath);
                string func = INIHelper.Read(i.CHN, "func", "0", filePath);
                i.func = int.Parse(func);
            }

            AppCfg.devicepara.Card1_enable = int.Parse(INIHelper.Read("Card1", "enable", "", filePath));
            AppCfg.devicepara.Card2_enable = int.Parse(INIHelper.Read("Card2", "enable", "", filePath));
            AppCfg.devicepara.Scan_interval = int.Parse(INIHelper.Read("SYS", "scan_interval", "2000", filePath));
            AppCfg.devicepara.Save_interval = int.Parse(INIHelper.Read("SYS", "save_interval", "50", filePath));

            #endregion
        }

        private void btn_start() {
            #region //开始串口采集

            //btn_stop.Enabled = true;
            //btn_start.Enabled = false;
            ReadPara();
            TotalCHN = "";
            count = 0;
            if (recentTenData != null) {
                recentTenData.Clear();
            }
            latestDataFile = "";
            latestIniFile = "";
            if ((latestIniFile = SlnIni.AutoSaveIni(_method))==null) {
                MessageBox.Show(@"请选择测试方法后在进行测试", @"警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try {
                serialPort1.BaudRate = int.Parse(AppCfg.devicepara.SerialBaudRate);
                serialPort1.PortName = AppCfg.devicepara.SerialPort;
                switch (AppCfg.devicepara.SerialParity) {
                    case "None":
                        serialPort1.Parity = Parity.None;
                        break;
                    case "奇校验":
                        serialPort1.Parity = Parity.Odd;
                        break;
                    case "偶校验":
                        serialPort1.Parity = Parity.Even;
                        break;
                    case "Mark":
                        serialPort1.Parity = Parity.Mark;
                        break;
                    case "Space":
                        serialPort1.Parity = Parity.Space;
                        break;
                    default:
                        serialPort1.Parity = Parity.None;
                        break;
                }

                switch (AppCfg.devicepara.SerialStopBits) {
                    case "1":
                        serialPort1.StopBits = StopBits.One;
                        break;
                    case "2":
                        serialPort1.StopBits = StopBits.Two;
                        break;
                    case "1.5":
                        serialPort1.StopBits = StopBits.OnePointFive;
                        break;

                    default:
                        serialPort1.Parity = Parity.None;
                        break;
                }

                serialPort1.DataBits = int.Parse(AppCfg.devicepara.SerialDataBits);

                serialPort1.Open();
            }
            catch {
                MessageBox.Show("无法打开串口！");
                //btn_start.Enabled = true;
                //btn_stop.Enabled = false;
                return;
            }

            string TwoRlist = "";
            int TwoR_num = 0;
            if (AppCfg.devicepara.Card1_enable != 0)
                foreach (Card i in AppCfg.devicepara.Cardlist1)
                    if (i.func == 1) {
                        TwoR_num++;
                        if (TwoRlist.Length == 0) {
                            TwoRlist = i.CHN;
                            if (TotalCHN.Length == 0)
                                TotalCHN = i.CHN;
                            else
                                TotalCHN = TotalCHN + "," + i.CHN;
                        }
                        else {
                            TwoRlist = TwoRlist + "," + i.CHN;
                            TotalCHN = TotalCHN + "," + i.CHN;
                        }
                    }

            if (AppCfg.devicepara.Card2_enable != 0)
                foreach (Card i in AppCfg.devicepara.Cardlist2)
                    if (i.func == 1) {
                        TwoR_num++;
                        if (TwoRlist.Length == 0) {
                            TwoRlist = i.CHN;
                            if (TotalCHN.Length == 0)
                                TotalCHN = i.CHN;
                            else
                                TotalCHN = TotalCHN + "," + i.CHN;
                        }
                        else {
                            TwoRlist = TwoRlist + "," + i.CHN;
                            TotalCHN = TotalCHN + "," + i.CHN;
                        }
                    }

            string FourRlist = "";
            int FourR_num = 0;

            #region 四线电阻选项,当前版本无用

            //if (AppCfg.devicepara.Card1_enable != 0)
            //{
            //    foreach (Card i in AppCfg.devicepara.Cardlist1)
            //    {
            //        if (i.func == 2)
            //        {
            //            FourR_num++;
            //            if (FourRlist.Length == 0)
            //            {
            //                FourRlist = i.CHN;
            //                if (TotalCHN.Length == 0)
            //                    TotalCHN = i.CHN;
            //                else
            //                    TotalCHN = TotalCHN + "," + i.CHN;

            //            }
            //            else
            //            {
            //                FourRlist = FourRlist + "," + i.CHN;
            //                TotalCHN = TotalCHN + "," + i.CHN;
            //            }
            //        }
            //    }
            //}

            //if (AppCfg.devicepara.Card2_enable != 0)
            //{
            //    foreach (Card i in AppCfg.devicepara.Cardlist2)
            //    {
            //        if (i.func == 2)
            //        {
            //            FourR_num++;
            //            if (FourRlist.Length == 0)
            //            {
            //                FourRlist = i.CHN;
            //                if (TotalCHN.Length == 0)
            //                    TotalCHN = i.CHN;
            //                else
            //                    TotalCHN = TotalCHN + "," + i.CHN;
            //            }
            //            else
            //            {
            //                FourRlist = FourRlist + "," + i.CHN;
            //                TotalCHN = TotalCHN + "," + i.CHN;
            //            }
            //        }
            //    }
            //}

            #endregion

            string Templist = "";
            int Temp_num = 0;

            #region 热电偶选项,当前版本无用

            //if (AppCfg.devicepara.Card1_enable != 0)
            //{
            //    foreach (Card i in AppCfg.devicepara.Cardlist1)
            //    {
            //        if (i.func == 3)
            //        {
            //            Temp_num++;
            //            if (Templist.Length == 0)
            //            {
            //                Templist = i.CHN;
            //                if (TotalCHN.Length == 0)
            //                    TotalCHN = i.CHN;
            //                else
            //                    TotalCHN = TotalCHN + "," + i.CHN;
            //            }
            //            else
            //            {
            //                Templist = Templist + "," + i.CHN;
            //                TotalCHN = TotalCHN + "," + i.CHN;
            //            }
            //        }
            //    }
            //}

            //if (AppCfg.devicepara.Card2_enable != 0)
            //{
            //    foreach (Card i in AppCfg.devicepara.Cardlist2)
            //    {
            //        if (i.func == 3)
            //        {
            //            Temp_num++;
            //            if (Templist.Length == 0)
            //            {
            //                Templist = i.CHN;
            //                if (TotalCHN.Length == 0)
            //                    TotalCHN = i.CHN;
            //                else
            //                    TotalCHN = TotalCHN + "," + i.CHN;
            //            }
            //            else
            //            {
            //                Templist = Templist + "," + i.CHN;
            //                TotalCHN = TotalCHN + "," + i.CHN;
            //            }
            //        }
            //    }
            //}

            #endregion

            TotalNum = TwoR_num + FourR_num + Temp_num;

            sendmsg(TwoRlist, FourRlist, Templist, TwoR_num, FourR_num, Temp_num);       
            #endregion
        }

        public string resolvcmd(string s1, string s2, string s3, int i1, int i2, int i3) {
            #region

            string st = @"*CLS 
*RST   
FORM:ELEM READ 
TRAC:CLE   
*ESE 0   
*SRE 1   
:STAT:MEAS:ENAB 512   
*s1*  
*s2*
*s3*  
:TEMP:TC:ODET OFF   
:TRAC:FEED SENS   
:TRAC:POIN *nchannel*  
INIT:CONT OFF 
TRIG:SOUR IMM 
TRIG:COUN 1   
TRIG:DEL 0  
SAMP:COUN *nchannel*  
ROUT:SCAN (@*allchannel*)
ROUT:SCAN:TSO IMM
ROUT:SCAN:LSEL INT";


            if (i1 > 0) {
                string str = @"SENS:FUNC 'RES',(@*channel*)   
SENS:RES:NPLC 1,(@*channel*)   
SENS:RES:RANG:AUTO ON,(@*channel*)";
                str = str.Replace("*channel*", s1);
                st = st.Replace("*s1*", str);
            }

            if (i2 > 0) {
                string str = @"SENS:FUNC 'TEMP',(@*channel*)   
SENS:TEMP:NPLC 1,(@*channel*)   
:TEMP:TRAN TC,(@*channel*)   
:TEMP:TC:TYPE J,(@*channel*)   ";
                str = str.Replace("*channel*", s2);
                st = st.Replace("*s2*", str);
            }

            if (i3 > 0) {
                string str = @"SENS:FUNC 'FRES',(@*channel*)
SENS:FRES:NPLC 1,(@*channel*)
SENS:FRES:RANG:AUTO ON,(@*channel*)";
                str = str.Replace("*channel*", s3);
                st = st.Replace("*s3*", str);
            }

            int num = i1 + i2 + i3;
            if (num > 0) st = st.Replace("*nchannel*", num.ToString());
            st = st.Replace("*allchannel*", TotalCHN);
            return st;

            #endregion
        }

        public void sendmsg(string s1, string s2, string s3, int i1, int i2, int i3) {
            #region

            string st3 = resolvcmd(s1, s2, s3, i1, i2, i3);

            string[] str = st3.Split('\n');
            foreach (string i in str) {
                serialPort1.WriteLine(i);
                Thread.Sleep(100);
            }

            Thread.Sleep(100);

            Thread thread;
            MessageBox.Show(TotalCHN);
            string[] chn = TotalCHN.Split(',');
            listView_main.Clear();
            listView_main.Columns.Add(((int)_method).ToString(), 120);
            for (int i = 0; i < TotalNum; i++) listView_main.Columns.Add(chn[i], 120);
            enablescan = true;
            thread = new Thread(() => //新开线程，执行接收数据操作
            {
                while (enablescan) //如果标识为true
                {
                    Thread.Sleep(1);
                    try {
                        serialPort1.WriteLine(":READ?");
                        Thread.Sleep(AppCfg.devicepara.Scan_interval);
                    }
                    catch (Exception ex) {
                        ;
                    }
                }
            });
            thread.Start(); //启动线程
            thread.IsBackground = true;

            #endregion
        }

        public byte[] str2ASCII(string xmlStr) {
            return Encoding.Default.GetBytes(xmlStr);
        }

        private void btn_stop() {
            serialPort1.Close();
            //btn_start.Enabled = true;
            //btn_stop.Enabled = false;
            listView_main.Clear();
            
            enablescan = false;
        }


        public void SavetData(string name, ListView listView) {
            #region

            if (listView.Items.Count == 0)
                // MessageBox.Show("未找到可导入的数据");
                return;

            string FileName = name + "-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss.ffff") + ".csv";
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AutoSave", FileName);
            latestDataFile = filePath;
            //MessageBox.Show(filePath);
            try {
                int size = 1024;
                int sizeCnt = (int) Math.Ceiling(listView.Items.Count / (double) 2000);
                StreamWriter write = new StreamWriter(filePath, false, Encoding.Default, size * sizeCnt);
                write.Write(recvstr);
                //获取listView标题行
                //for (int t = 0; t < listView.Columns.Count; t++) write.Write(listView.Columns[t].Text + ",");
                //write.WriteLine();
                ////获取listView数据行
                //for (int lin = 0; lin < listView.Items.Count; lin++) {
                //    string Tem = null;
                //    for (int k = 0; k < listView.Columns.Count; k++) {
                //        string TemString = listView.Items[lin].SubItems[k].Text;
                //        Tem += TemString;
                //        Tem += ",";
                //    }

                //    write.WriteLine(Tem);
                //}

                write.Flush();
                write.Close();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }

            #endregion
        }

        private void serialPort1_DataReceived_1(object sender, SerialDataReceivedEventArgs e) {
            #region

            string str = serialPort1.ReadExisting();

            //richTextBox1.Text = richTextBox1.Text + str+"\n";

            //byte[] array = System.Text.Encoding.ASCII.GetBytes(str);  //数组array为对应的ASCII数组     
            //string ASCIIstr2 = null;
            //for (int i = 0; i < array.Length; i++)
            //{
            //    int asciicode = (int)(array[i]);
            //    ASCIIstr2 += Convert.ToString(asciicode);//字符串ASCIIstr2 为对应的ASCII字符串
            //}
            //richTextBox2.Text = richTextBox2.Text + ASCIIstr2;


            if (str.IndexOf((char) 19) != -1)
                str = str.Substring(str.IndexOf((char) 19), str.Length - str.IndexOf((char) 19));

            string nextstr = "";
            if (str.IndexOf((char) 13) != -1) {
                str = str.Substring(0, str.IndexOf((char) 13));
                recvstr += str;

                if (recvstr.Length > 0) {
                    count++;
                    recvstr = recvstr.Replace((char) 19, (char) 0);
                    recvstr = recvstr.Replace((char) 13, (char) 0);
                    recvstr = recvstr.Replace((char) 0x11, (char) 0);
                    recvstr = recvstr.Replace("\0", "");


                    string tmp = count + "," + recvstr;
                    ListViewItem item = new ListViewItem(tmp.Split(','));
                    listView_main.Items.Add(item);
                    LastScan.Text = recvstr;
                    if(count % 50 == 0) {
                        //recentTenData.Add(recvstr);

                    }

                    if (recentTenData != null && recentTenData.Count > 10) {
                        recentTenData.RemoveAt(0);
                    }
                    
                    if (count % AppCfg.devicepara.Save_interval == 0) {
                        SavetData("AutoSave", listView_main);
                        listView_main.Items.Clear();
                    }
                    //MessageBox.Show(@"数据已收敛", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    List<string> channels = recvstr.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    List<double> dataList = TotalCHN.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(double.Parse).ToList();
                    Dictionary<string, double> testResult = new Dictionary<string, double>();
                    for (int i = 0; i < channels.Count; i++)
                    {
                        testResult.Add(channels[i], dataList[i]);
                    }

                    testResultChart.ShowChart(testResult);

                }

                //if(str.IndexOf((char)13)!=)
                //recvstr = str.Substring(str.IndexOf((char)13), str.Length - str.IndexOf((char)13));
                recvstr = "";
            }
            else {
                recvstr += str;
            }


            //int flag = 0;
            //foreach (char i in str)
            //{

            //    if (i == 0x2b || i == 0x2d)
            //    {
            //        break;
            //    }
            //    flag++;
            //}

            //string recvcontent = str.Substring(flag, str.Length - flag);
            //if (recvcontent.Length > 0)
            //{
            //    count++;
            //    recvcontent=recvcontent.Replace((char)13, (char)0);          
            //    recvcontent = recvcontent.Replace((char)0x11, (char)0);
            //    recvcontent = recvcontent.Replace("\0", "");

            //    string tmp = count.ToString() + "," + recvcontent;
            //    ListViewItem item = new ListViewItem(tmp.Split(','));
            //    listView_main.Items.Add(item);
            //    textBox1.Text = recvcontent;

            //    if(count%AppCfg.devicepara.Save_interval==0)
            //    {         
            //        SavetData("sss", listView_main);
            //        listView_main.Items.Clear();
            //    }         
            //}
            
         
            
            
            #endregion
        }


        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------




        internal class AppCfg //全局变量
        {
            internal static ParaInfo devicepara = new ParaInfo();
        }

        #region //串口采集

        private string TotalCHN = "";
        private int TotalNum;
        private int count;
        private bool enablescan;

        #endregion

        private void SetupTest_Load(object sender, EventArgs e)
        {
            {

                #region //不同设置窗口默认显示
                EmptyGroupBox.Size = new Size(1250, 855);
                TextGroupbox1.Size = new Size(0, 0);
                TextGroupbox2.Size = new Size(0, 0);
                TextGroupbox3.Size = new Size(0, 0);
                TextGroupbox4.Size = new Size(0, 0);
                //ResultGroupBox.Size = new Size(650, 886);
                skinGroupBox1.Size = new Size(0, 0);
                TestRun.Enabled = false;
                TestStop.Enabled = false;
                TestResult.Enabled = false;

                #endregion

                #region //串口设置

                CheckForIllegalCrossThreadCalls = false; //去掉线程安全
                #region //初始化变量
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AutoSave"));
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bak"));
                latestIniFile = "";
                latestDataFile = "";
                heatMeter1 = new HeatMeter("HeatMeter1");
                heatMeter2 = new HeatMeter("HeatMeter2");
                SlnIni.CreateDefaultIni();
                string slnFilePath = SlnIni.CreateDefaultSlnIni();
                SlnIni.LoadHeatMeterInfo(ref heatMeter1, ref heatMeter2, slnFilePath);
                recentTenData = new List<string>();
                #endregion
                edit_scan_interval.Text = AppCfg.devicepara.Scan_interval.ToString();
                edit_save_interval.Text = AppCfg.devicepara.Save_interval.ToString();

                switch (AppCfg.devicepara.SerialPort)
                {
                    case "COM1":
                        combox_comport.SelectedIndex = 0;
                        break;
                    case "COM2":
                        combox_comport.SelectedIndex = 1;
                        break;
                    case "COM3":
                        combox_comport.SelectedIndex = 2;
                        break;
                    case "COM4":
                        combox_comport.SelectedIndex = 3;
                        break;
                    case "COM5":
                        combox_comport.SelectedIndex = 4;
                        break;
                    case "COM6":
                        combox_comport.SelectedIndex = 5;
                        break;
                    case "COM7":
                        combox_comport.SelectedIndex = 6;
                        break;
                    case "COM8":
                        combox_comport.SelectedIndex = 7;
                        break;
                    default:
                        combox_comport.SelectedIndex = 0;
                        break;
                }


                switch (AppCfg.devicepara.SerialBaudRate)
                {
                    case "4800":
                        combox_baudrate.SelectedIndex = 0;
                        break;
                    case "9600":
                        combox_baudrate.SelectedIndex = 1;
                        break;
                    case "14400":
                        combox_baudrate.SelectedIndex = 2;
                        break;
                    case "19200":
                        combox_baudrate.SelectedIndex = 3;
                        break;
                    case "38400":
                        combox_baudrate.SelectedIndex = 4;
                        break;
                    case "57600":
                        combox_baudrate.SelectedIndex = 5;
                        break;
                    case "115200":
                        combox_baudrate.SelectedIndex = 6;
                        break;
                    default:
                        combox_baudrate.SelectedIndex = 2;
                        break;
                }


                switch (AppCfg.devicepara.SerialDataBits)
                {
                    case "8":
                        combox_databits.SelectedIndex = 0;
                        break;
                    case "7":
                        combox_databits.SelectedIndex = 1;
                        break;
                    case "6":
                        combox_databits.SelectedIndex = 2;
                        break;
                    case "5":
                        combox_databits.SelectedIndex = 3;
                        break;

                    default:
                        combox_databits.SelectedIndex = 2;
                        break;
                }

                switch (AppCfg.devicepara.SerialStopBits)
                {
                    case "1":
                        combox_stopbits.SelectedIndex = 0;
                        break;
                    case "1.5":
                        combox_stopbits.SelectedIndex = 1;
                        break;
                    case "2":
                        combox_stopbits.SelectedIndex = 2;
                        break;

                    default:
                        combox_stopbits.SelectedIndex = 0;
                        break;
                }


                switch (AppCfg.devicepara.SerialParity)
                {
                    case "None":
                        combox_parity.SelectedIndex = 0;
                        break;
                    case "奇校验":
                        combox_parity.SelectedIndex = 1;
                        break;
                    case "偶校验":
                        combox_parity.SelectedIndex = 2;
                        break;
                    case "Mark":
                        combox_parity.SelectedIndex = 3;
                        break;
                    case "Space":
                        combox_parity.SelectedIndex = 4;
                        break;

                    default:
                        combox_parity.SelectedIndex = 0;
                        break;
                }

                #endregion

                #region //加载测试选择窗口
                TestChoose testChoose=new TestChoose();
                this.Enabled = false;
                testChoose.Show(this);
                #endregion

                //创建必要的文件夹

                //this.FormBorderStyle = FormBorderStyle.FixedToolWindow;

                //this.Location = Screen.PrimaryScreen.WorkingArea.Location;
                //this.Width = Screen.PrimaryScreen.WorkingArea.Width;
                //this.Height = Screen.PrimaryScreen.WorkingArea.Height;
                testResultChart.FormClosing += TestResultChart_FormClosing;
                ReadPara(); ;



                
            }
        }

        private void label114_Click(object sender, EventArgs e)
        {

        }

        private void Unit4_15_Click(object sender, EventArgs e)
        {

        }

    }
}
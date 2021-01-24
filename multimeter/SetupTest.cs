using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DataProcessor;
using multimeter.Properties;
using log4net;
namespace multimeter
{
    public partial class SetupTest : Form
    {
        private HeatMeter _heatMeter1;
        private HeatMeter _heatMeter2;
        private string _latestDataFile;
        private string _latestResultFile;
        private TestMethod _method;
        private string _recvstr;
        private string _autoSaveFilePath;
        private Sample _sample1;
        private Sample _sample2;
        private Dictionary<string, double> _testResult = new Dictionary<string, double>();
        private bool _testResultChartUpdate;
        private bool _saveParameter;
        public User User;
        private List<string> _temp;
        private List<string> _lastTemp;
        private bool _convergent = false;
        #region logger

        private static readonly ILog log
            = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region //串口采集
        private string TotalCHN = "";
        string[] channels;
        private int TotalNum;
        private int count;
        private bool enablescan;
        #endregion

        private static class AppCfg
        {
            internal static ParaInfo devicepara = new ParaInfo();
        }//全局变量


        public SetupTest()
        {
            InitializeComponent();
        }

        public void TestChooseFormShow_Click(object sender, EventArgs e)
        {
            TextGroupbox1.Size = new Size(0, 0);
            TextGroupbox2.Size = new Size(0, 0);
            TextGroupbox3.Size = new Size(0, 0);
            TextGroupbox4.Size = new Size(0, 0);
            skinGroupBox1.Size = new Size(0, 0);
            TestChartGroupBox.Size = new Size(0, 0);

            SoftwareNameLabel.Visible = true;
            MenuGroupBox.Visible = false;
            TestChoiseGroupBox.BringToFront();
            TestChoiseGroupBox.Visible = true;

        }

        private void ModifyParameter_Click(object sender, EventArgs e)
        {
            if (_saveParameter)
            {
                if (!apply_btm()) { return; }
                if (User == User.NORMAL) NormalTextBoxEnable(false);
                ModifyParameter_Enable(true, true);
                TestChooseFormShow_Enable(true);
                _saveParameter = false;
                ModifyParameterLabel.Text = "修改参数";
            }
            else
            {
                if (User == User.NORMAL) NormalTextBoxEnable(true);
                ModifyParameter_Enable(true, false);
                TestChooseFormShow_Enable(false);
                _saveParameter = true;
                ModifyParameterLabel.Text = "确定参数";
            }
        }
        private void TestRun_Click(object sender, EventArgs e)
        {
            TestTime.Text = "";
            if (serialPort1.IsOpen)
            {
                btn_stop();
                TestChooseFormShow_Enable(true);
                TestRun_Enable(true);
                //Monitor_Enable(false);
                CurrentTestResult_Enable(true);
                HistoryTestResult_Enable(true);
                SerialPort_Enable(true);
                AdvancedSetting_Enable(true);
                ModifyParameter_Enable(true, true);
                TestRunLabel.Text = "  运行  ";
                SerialPort_Timer.Enabled = false;
                ChartShow_Timer.Enabled = false;
                TestTime_Timer.Enabled = false;

            }
            else
            {
                if (_saveParameter) ModifyParameter_Click(sender, e);
                if (!apply_btm()) { return; }//再次确认设置参数
                btn_start();
                if (!serialPort1.IsOpen) return;
                Chart_Init();
                TestChooseFormShow_Enable(false);
                TestRun_Enable(false);
                Monitor_Enable(true);
                CurrentTestResult_Enable(true);
                HistoryTestResult_Enable(false);
                SerialPort_Enable(false);
                AdvancedSetting_Enable(false);
                ModifyParameter_Enable(false, false);
                TestRunLabel.Text = "  停止  ";
                Monitor_Click(sender, e);
                SerialPort_Timer.Enabled = true;
                ChartShow_Timer.Enabled = true;
                TestTime_Timer.Enabled = true;
            }
        }

        private void Monitor_Click(object sender, EventArgs e)
        {
            TextGroupbox1.Size = new Size(0, 0);
            TextGroupbox2.Size = new Size(0, 0);
            TextGroupbox3.Size = new Size(0, 0);
            TextGroupbox4.Size = new Size(0, 0);
            TestChartGroupBox.Size = new Size(1250, 855);

        }

        private void SerialPort_Click(object sender, EventArgs e)
        {
            skinGroupBox1.BringToFront();
            skinGroupBox1.Size = new Size(253, 201);
        }

        private void SerialPortEnsure_Click(object sender, EventArgs e)
        {

            AppCfg.devicepara.SerialPort = combox_comport.Text;

            AppCfg.devicepara.SerialBaudRate = combox_baudrate.Text;

            AppCfg.devicepara.SerialDataBits = combox_databits.Text;

            AppCfg.devicepara.SerialStopBits = combox_stopbits.Text;

            AppCfg.devicepara.SerialParity = combox_parity.Text;
            if (edit_scan_interval.Text == "")
            {
                return;
            }

            int scanInterval = CheckData.CheckTextChange(edit_scan_interval.Text);
            if (scanInterval == -1)
            {
                MessageBox.Show(@"错误的采集频率", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            AppCfg.devicepara.Scan_interval = scanInterval;
            if (edit_save_interval.Text == "")
            {
                return;
            }

            int saveInterval = CheckData.CheckTextChange(edit_save_interval.Text);
            if (saveInterval == -1)
            {
                MessageBox.Show(@"错误的自动保存频率", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            AppCfg.devicepara.Save_interval = saveInterval;
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini");
            INIHelper.Write("Serial", "port", combox_comport.Text, filePath);
            INIHelper.Write("Serial", "baudrate", combox_baudrate.Text, filePath);
            INIHelper.Write("Serial", "databits", combox_databits.Text, filePath);
            INIHelper.Write("Serial", "stopbites", combox_stopbits.Text, filePath);
            INIHelper.Write("Serial", "parity", combox_parity.Text, filePath);
            INIHelper.Write("SYS", "scan_interval", edit_scan_interval.Text, filePath);
            INIHelper.Write("SYS", "save_interval", edit_save_interval.Text, filePath);
            skinGroupBox1.Size = new Size(0, 0);
        }

        public void AdvancedSetting_Click(object sender, EventArgs e)
        {
            AlphaT0Setting alphaT0Setting = new AlphaT0Setting();
            alphaT0Setting.Show();
        }
        private void HelpButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"..\..\HelpDocument\help.pdf");
        }

        //private void TestResultChart_FormClosing(object sender, EventArgs e) {
        //    if (_testResultChart.DialogResult == DialogResult.Cancel) return;
        //    btn_stop();
        //    MessageBox.Show(@"计算已收敛,是否结束计算并显示结果?", @"提示", MessageBoxButtons.YesNo,
        //        MessageBoxIcon.Question);
        //    if (_testResultChart.DialogResult != DialogResult.Yes) return;
        //    CurrentTestResult_Click(this, e);
        //}

        private void ReadPara()
        {
            #region //读取ini

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini");
            AppCfg.devicepara.SerialPort = INIHelper.Read("Serial", "port", "COM1", filePath);
            AppCfg.devicepara.SerialBaudRate = INIHelper.Read("Serial", "baudrate", "9600", filePath);
            AppCfg.devicepara.SerialDataBits = INIHelper.Read("Serial", "databits", "8", filePath);

            AppCfg.devicepara.SerialStopBits = INIHelper.Read("Serial", "stopbites", "1", filePath);
            AppCfg.devicepara.SerialParity = INIHelper.Read("Serial", "parity", "none", filePath);
            foreach (Card i in AppCfg.devicepara.Cardlist1)
            {
                i.name = INIHelper.Read(i.CHN, "name", "", filePath);
                string func = INIHelper.Read(i.CHN, "func", "0", filePath);
                i.func = int.Parse(func);
            }

            foreach (Card i in AppCfg.devicepara.Cardlist2)
            {
                i.name = INIHelper.Read(i.CHN, "name", "", filePath);
                string func = INIHelper.Read(i.CHN, "func", "0", filePath);
                i.func = int.Parse(func);
            }

            AppCfg.devicepara.Card1_enable = int.Parse(INIHelper.Read("Card1", "enable", "", filePath));
            AppCfg.devicepara.Card2_enable = int.Parse(INIHelper.Read("Card2", "enable", "", filePath));
            AppCfg.devicepara.Scan_interval = int.Parse(INIHelper.Read("SYS", "scan_interval", "2000", filePath));
            AppCfg.devicepara.Save_interval = int.Parse(INIHelper.Read("SYS", "save_interval", "50", filePath));
            AppCfg.devicepara.AutoCloseInterval = int.Parse(INIHelper.Read("SYS", "autoSaveInterval", "1500", filePath));
            AppCfg.devicepara.ConvergentLim = double.Parse(INIHelper.Read("SYS", "convergentLim", "1e-3", filePath));
            #endregion
        }

        private void btn_start()
        {
            #region //开始串口采集

            //btn_stop.Enabled = true;
            //btn_start.Enabled = false;
            ReadPara();
            TotalCHN = "";
            count = 0;
            _latestDataFile = "";
            _latestResultFile = "";
            string fileName = _method + "-DataAutoSave.csv";
            _autoSaveFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AutoSave", _method.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss.ffff"));

            try
            {
                var di = Directory.CreateDirectory(_autoSaveFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"自动保存文件创建失败,请重试
{ex.Message}", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                log.Error(ex);
                btn_stop();
                return;
            }
            _latestDataFile = Path.Combine(_autoSaveFilePath, fileName);

            try
            {
                serialPort1.BaudRate = int.Parse(AppCfg.devicepara.SerialBaudRate);
                serialPort1.PortName = AppCfg.devicepara.SerialPort;
                switch (AppCfg.devicepara.SerialParity)
                {
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

                switch (AppCfg.devicepara.SerialStopBits)
                {
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
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show("无法打开串口！");
                //btn_start.Enabled = true;
                //btn_stop.Enabled = false;
                return;
            }
            serialPort1.DiscardInBuffer();  //丢弃来自串口驱动程序的接收缓冲区的数据
            string TwoRlist = "";
            int TwoR_num = 0;
            if (AppCfg.devicepara.Card1_enable != 0)
                foreach (Card i in AppCfg.devicepara.Cardlist1)
                    if (i.func == 1)
                    {
                        TwoR_num++;
                        if (TwoRlist.Length == 0)
                        {
                            TwoRlist = i.CHN;
                            if (TotalCHN.Length == 0)
                                TotalCHN = i.CHN;
                            else
                                TotalCHN = TotalCHN + "," + i.CHN;
                        }
                        else
                        {
                            TwoRlist = TwoRlist + "," + i.CHN;
                            TotalCHN = TotalCHN + "," + i.CHN;
                        }
                    }

            if (AppCfg.devicepara.Card2_enable != 0)
                foreach (Card i in AppCfg.devicepara.Cardlist2)
                    if (i.func == 1)
                    {
                        TwoR_num++;
                        if (TwoRlist.Length == 0)
                        {
                            TwoRlist = i.CHN;
                            if (TotalCHN.Length == 0)
                                TotalCHN = i.CHN;
                            else
                                TotalCHN = TotalCHN + "," + i.CHN;
                        }
                        else
                        {
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
            channels = TotalCHN.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            sendmsg(TwoRlist, FourRlist, Templist, TwoR_num, FourR_num, Temp_num);
            #endregion

            _temp = new List<string>();

            try
            {
                StreamWriter write = new StreamWriter(_latestDataFile);
                write.WriteLine("step," + TotalCHN);
                write.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }

        }

        public string resolvcmd(string s1, string s2, string s3, int i1, int i2, int i3)
        {
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


            if (i1 > 0)
            {
                string str = @"SENS:FUNC 'RES',(@*channel*)   
SENS:RES:NPLC 1,(@*channel*)   
SENS:RES:RANG:AUTO ON,(@*channel*)";
                str = str.Replace("*channel*", s1);
                st = st.Replace("*s1*", str);
            }

            if (i2 > 0)
            {
                string str = @"SENS:FUNC 'TEMP',(@*channel*)   
SENS:TEMP:NPLC 1,(@*channel*)   
:TEMP:TRAN TC,(@*channel*)   
:TEMP:TC:TYPE J,(@*channel*)   ";
                str = str.Replace("*channel*", s2);
                st = st.Replace("*s2*", str);
            }

            if (i3 > 0)
            {
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

        public void sendmsg(string s1, string s2, string s3, int i1, int i2, int i3)
        {
            #region

            string st3 = resolvcmd(s1, s2, s3, i1, i2, i3);

            string[] str = st3.Split('\n');
            foreach (string i in str)
            {
                serialPort1.WriteLine(i);
                Thread.Sleep(100);
            }

            Thread.Sleep(100);

            Thread thread;
            //MessageBox.Show(TotalCHN);
            string[] chn = TotalCHN.Split(',');
            enablescan = true;
            thread = new Thread(() => //新开线程，执行接收数据操作
            {
                while (enablescan) //如果标识为true
                {
                    Thread.Sleep(1);
                    try
                    {
                        serialPort1.WriteLine(":READ?");
                        Thread.Sleep(AppCfg.devicepara.Scan_interval);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }
            });
            thread.Start(); //启动线程
            thread.IsBackground = true;

            #endregion
        }

        public byte[] str2ASCII(string xmlStr)
        {
            return Encoding.Default.GetBytes(xmlStr);
        }

        private void btn_stop()
        {
            serialPort1.Close();
            _temp.Clear();
            enablescan = false;
        }

        private void SetupTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            serialPort1.Close();
            while (serialPort1.IsOpen)
            {
            }
            Application.Exit();
        }



        private void SetupTest_Load(object sender, EventArgs e)
        {
            #region //不同设置窗口默认显示

            //EmptyGroupBox.Size = new Size(1250, 855);
            TextGroupbox1.Size = new Size(0, 0);
            TextGroupbox2.Size = new Size(0, 0);
            TextGroupbox3.Size = new Size(0, 0);
            TextGroupbox4.Size = new Size(0, 0);
            skinGroupBox1.Size = new Size(0, 0);
            TestChoiseGroupBox.Size = new Size(969, 581);
            TestChartGroupBox.Size = new Size(0, 0);
            MenuGroupBox.Visible = false;
            TestChoiseGroupBox.Visible = false;
            SoftwareNameLabel.Visible = false;
            TestTime.Text = "";
            _saveParameter = false;
            #endregion

            CheckForIllegalCrossThreadCalls = false; //去掉线程安全
            #region //初始化变量
            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AutoSave"));
            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bak"));
            _latestDataFile = "";
            _latestResultFile = "";
            _heatMeter1 = new HeatMeter("HeatMeter1", 3);
            _heatMeter2 = new HeatMeter("HeatMeter2");
            string sysFilePath = SlnIni.CreateDefaultIni();
            string settingFilePath = SlnIni.CreateDefaultSettingIni();
            SlnIni.LoadHeatMeterInfo(ref _heatMeter1, ref _heatMeter2, settingFilePath, sysFilePath);
            #endregion

            #region //串口设置 
            //_testResultChart.FormClosing += TestResultChart_FormClosing;
            ReadPara();
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

            #region //显示登录窗口
            LoginForm loginForm = new LoginForm();
            loginForm.Show(this);
            #endregion
        }



        private void ChartShow_Timer_Tick(object sender, EventArgs e)
        {
            if (!_testResultChartUpdate) return;
            ShowChart();
            _testResultChartUpdate = false;

        }

        private void label139_Click(object sender, EventArgs e)
        {

        }

        private void Tlable4_12_Click(object sender, EventArgs e)
        {

        }
    }
}
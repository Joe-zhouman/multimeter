using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DataAccess;
using log4net;
using Model;

namespace multimeter {
    public partial class SetupTest : Form {
        #region logger

        private static readonly ILog Log
            = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        private AppCfg _appCfg;
        private string _autoSaveFilePath;
        private bool _convergent;
        private TestDevice _device;
        private List<string> _lastTemp;
        private string _latestDataFile;
        private string _latestOriginFile;
        private string _latestResultFile;
        private TestMethod _method;
        private MultiMeterInfo _multiMeter;
        private string _recvStr;
        private bool _saveParameter;
        private List<string> _temp;
        private readonly Dictionary<string, double> _testResult = new Dictionary<string, double>();
        private bool _testResultChartUpdate;
        public UserType User;


        public SetupTest() {
            InitializeComponent();
        }

        public void TestChooseFormShow_Click(object sender, EventArgs e) {
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

        private void ModifyParameter_Click(object sender, EventArgs e) {
            if (_saveParameter) {
                if (!apply_btm()) return;
                if (User == UserType.NORMAL) NormalTextBoxEnable(false);
                ModifyParameter_Enable(true, true);
                TestChooseFormShow_Enable(true);
                _saveParameter = false;
                ModifyParameterLabel.Text = @"修改参数";
            }
            else {
                if (User == UserType.NORMAL) NormalTextBoxEnable(true);
                ModifyParameter_Enable(true, false);
                TestChooseFormShow_Enable(false);
                _saveParameter = true;
                ModifyParameterLabel.Text = @"确定参数";
            }
        }

        private void TestRun_Click(object sender, EventArgs e) {
            TestTime.Text = "";
            if (serialPort1.IsOpen) {
                btn_stop();
                TestChooseFormShow_Enable(true);
                TestRun_Enable(true);
                //Monitor_Enable(false);
                CurrentTestResult_Enable(true);
                HistoryTestResult_Enable(true);
                SerialPort_Enable(true);
                AdvancedSetting_Enable(true);
                ModifyParameter_Enable(true, true);
                TestRunLabel.Text = @"  运行  ";
                SerialPort_Timer.Enabled = false;
                ChartShow_Timer.Enabled = false;
                TestTime_Timer.Enabled = false;
            }
            else {
                if (_saveParameter) ModifyParameter_Click(sender, e);
                if (!apply_btm()) return;
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
                TestRunLabel.Text = @"  停止  ";
                Monitor_Click(sender, e);
                SerialPort_Timer.Enabled = true;
                ChartShow_Timer.Enabled = true;
                TestTime_Timer.Enabled = true;
            }
        }

        private void Monitor_Click(object sender, EventArgs e) {
            TextGroupbox1.Size = new Size(0, 0);
            TextGroupbox2.Size = new Size(0, 0);
            TextGroupbox3.Size = new Size(0, 0);
            TextGroupbox4.Size = new Size(0, 0);
            TestChartGroupBox.Size = new Size(1250, 855);
        }

        private void SerialPort_Click(object sender, EventArgs e) {
            skinGroupBox1.BringToFront();
            skinGroupBox1.Size = new Size(253, 201);
        }

        private void SerialPortEnsure_Click(object sender, EventArgs e) {
            _appCfg.SerialPortPara.SerialPort = combox_comport.Text;

            _appCfg.SerialPortPara.SerialBaudRate = combox_baudrate.Text;


            try {
                _appCfg.SysPara.SaveInterval.Value = int.Parse(edit_save_interval.Text);
            }
            catch (Exception exception) {
                MessageBox.Show(@"错误的保存频率" + @"
" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

//            try
//            {
//                _appCfg.SysPara.ScanInterval.Value = int.Parse(edit_scan_interval.Text);
//            }
//            catch (Exception exception)
//            {
//                MessageBox.Show(@"错误的采集频率" + @"
//" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return;
//            }
            IniReadAndWrite.WriteBasicPara(_appCfg.SerialPortPara, IniReadAndWrite.IniFilePath);
            skinGroupBox1.Size = new Size(0, 0);
        }

        public void AdvancedSetting_Click(object sender, EventArgs e) {
            var parSetting = new ParaSetting(_appCfg);
            parSetting.Show();
        }

        private void HelpButton_Click(object sender, EventArgs e) {
            Process.Start(@"..\..\HelpDocument\help.pdf");
        }

        //private void TestResultChart_FormClosing(object sender, EventArgs e) {
        //    if (_testResultChart.DialogResult == DialogResult.Cancel) return;
        //    btn_stop();
        //    MessageBox.Show(@"计算已收敛,是否结束计算并显示结果?", @"提示", MessageBoxButtons.YesNo,
        //        MessageBoxIcon.Question);
        //    if (_testResultChart.DialogResult != DialogResult.Yes) return;
        //    CurrentTestResult_Click(this, e);
        //}
        private void SetupTest_Load(object sender, EventArgs e) {
            IniReadAndWrite.CreateDefaultIni();

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
            _appCfg = new AppCfg();

            #endregion

            #region //串口设置 

            //_testResultChart.FormClosing += TestResultChart_FormClosing;
            IniReadAndWrite.ReadPara(ref _appCfg, IniReadAndWrite.IniFilePath);
            edit_scan_interval.Text = _appCfg.SysPara.ScanInterval.Value.ToString();
            edit_save_interval.Text = _appCfg.SysPara.SaveInterval.Value.ToString();

            switch (_appCfg.SerialPortPara.SerialPort) {
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


            switch (_appCfg.SerialPortPara.SerialBaudRate) {
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

            #endregion

            #region //显示登录窗口

            var loginForm = new LoginForm();
            loginForm.Show(this);

            #endregion
        }

        private void ChartShow_Timer_Tick(object sender, EventArgs e) {
            if (!_testResultChartUpdate) return;
            ShowChart();
            _testResultChartUpdate = false;
        }

        #region //串口采集

        private int _count;
        private bool _enableScan;

        #endregion
    }
}
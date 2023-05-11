using DataAccess;
using log4net;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

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
        private List<double[]> _lastTemp;
        private string _latestDataFile;
        private string _latestOriginFile;
        private string _latestResultFile;
        private MultiMeterInfo _multiMeter;
        private bool _saveParameter;
        private List<double[]> _temp;
        private readonly Dictionary<string, double> _testResult = new Dictionary<string, double>();
        private bool _testResultChartUpdate;
        public UserType User;
        public TestMethod _method;


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
                StatusTextBox_AddText(PromptType.INFO, $"[{DateTime.Now:MM-dd-hh:mm:ss}]修改参数成功!");
                AllTextBoxEnable(false);
                ModifyParameter_Enable(true, true);
                TestChooseFormShow_Enable(true);
                _saveParameter = false;
                ModifyParameterLabel.Text = @"修改参数";
            }
            else {
                if (User == UserType.NORMAL) NormalTextBoxEnable(true);
                else AllTextBoxEnable(true);
                ModifyParameter_Enable(true, false);
                TestChooseFormShow_Enable(false);
                _saveParameter = true;
                ModifyParameterLabel.Text = @"确定参数";
            }
            HideChart_Click(sender, e);
            switch (_method) {
                case TestMethod.KAPPA:
                    TestChoosiest1_Click(sender, e);
                    break;
                case TestMethod.ITC:
                    TestChoosiest2_Click(sender, e);
                    break;
                case TestMethod.ITM:
                    TestChoosiest3_Click(sender, e);
                    break;
                case TestMethod.ITMS:
                    TestChoose4_Click(sender, e);
                    break;
                default:
                    break;
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
                StatusTextBox_AddText(PromptType.INFO, $"[{DateTime.Now:MM-dd-hh:mm:ss}]测试结束!");
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
                StatusTextBox_Init();
                TestChooseFormShow_Enable(false);
                TestRun_Enable(false);
                Monitor_Enable(true);
                CurrentTestResult_Enable(false);
                HistoryTestResult_Enable(false);
                SerialPort_Enable(false);
                AdvancedSetting_Enable(false);
                ModifyParameter_Enable(false, false);
                StatusTextBox_AddText(PromptType.INFO, $"[{DateTime.Now:MM-dd-hh:mm:ss}]测试开始!");
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
            SetChartGroupSize();
        }

        private void SetChartGroupSize() {
            //var scaleFactor = Math.Min(this.Width * 0.98 / 1250, (this.Height - MenuGroupBox.Height) * 0.98 / 855);
            //MessageBox.Show($"Width:{Width} | Height:{Height}");
            TestChartGroupBox.Size = new Size(1250, 855);

            //ScaleControls(TestChartGroupBox, scaleFactor);
        }

        private void ScaleControls(Control control, double scaleFactor) {
            // 缩放控件的大小和位置
            control.Width = (int)Math.Round(control.Width * scaleFactor);
            control.Height = (int)Math.Round(control.Height * scaleFactor);
            control.Left = (int)Math.Round(control.Left * scaleFactor);
            control.Top = (int)Math.Round(control.Top * scaleFactor);
            // 如果是 Label 控件，则同时缩放字体大小
            if (control is Label) {
                Label label = (Label)control;
                label.Font = new Font(label.Font.FontFamily, (float)(label.Font.Size * scaleFactor));
            }
            // 缩放控件中的所有子控件
            foreach (Control childControl in control.Controls) {
                ScaleControls(childControl, scaleFactor);
            }


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
            parSetting.Show(this);
        }

        private void HelpButton_Click(object sender, EventArgs e) {
            try {
                Process.Start(@"doc\help.pdf");
            }
            catch (Exception ex) {

                MessageBox.Show(@"无法正确的打开帮助文档，帮助文档可能不存在。");
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
            }

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
            ShowDefaultWindows();
            CheckForIllegalCrossThreadCalls = false; //去掉线程安全
            #region //初始化变量

            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AutoSave"));
            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bak"));
            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "doc"));
            _saveParameter = false;
            _latestDataFile = "";
            _latestResultFile = "";
            _appCfg = new AppCfg();
            _serialPortData = new Queue<string>();
            SerialPort_Timer.Interval = _appCfg.SysPara.ScanInterval.Value / 2;
            ChartShow_Timer.Interval = _appCfg.SysPara.ScanInterval.Value / 2;
            #endregion

            #region //串口设置 

            //_testResultChart.FormClosing += TestResultChart_FormClosing;
            IniReadAndWrite.ReadPara(ref _appCfg, IniReadAndWrite.IniFilePath);
            edit_scan_interval.Text = _appCfg.SysPara.ScanInterval.Value.ToString();
            edit_save_interval.Text = _appCfg.SysPara.SaveInterval.Value.ToString();

            var comIdx = _appCfg.SerialPortPara.SerialPort.Replace("COM", "");
            combox_comport.SelectedIndex = int.Parse(comIdx) - 1;

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

            GroupTextBox();

            #region //显示登录窗口
            var loginForm = new LoginForm();
            loginForm.Show(this);
            #endregion
        }
        /// <summary>
        /// 初始窗口显示
        /// </summary>
        private void ShowDefaultWindows() {
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
        }

        private void ChartShow_Timer_Tick(object sender, EventArgs e) {
            if (!_testResultChartUpdate) return;
            ShowChart();
            _testResultChartUpdate = false;
        }
        private int _count;
        private bool _enableScan;

        private void TestChoose_Leave(object sender, EventArgs e) {
            var groupBox = sender as GroupBox;
            if (groupBox != null) {
                groupBox.BackColor = ColorTranslator.FromHtml("#f0f0f0");
            }
        }
    }
}
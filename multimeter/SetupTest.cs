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
        private TestMethod _method;
        private TestResultChart testResultChart=new TestResultChart();
        private Dictionary<string, double> testResult = new Dictionary<string, double>();


        public SetupTest() {
            InitializeComponent();
        }

        public void TestChooseFormShow_Click(object sender, EventArgs e) {
            TextGroupbox1.Size = new Size(0, 0);
            TextGroupbox2.Size = new Size(0, 0);
            TextGroupbox3.Size = new Size(0, 0);
            TextGroupbox4.Size = new Size(0, 0);
            skinGroupBox1.Size = new Size(0, 0);
            MenuGroupBox.Visible = false;
            TestChoiseGroupBox.BringToFront();
            TestChoiseGroupBox.Visible = true;
        }
        private void TestRun_Click(object sender, EventArgs e) {
            if (serialPort1.IsOpen) {
                TestChooseFormShow_Enable(true);
                TestRun_Enable(true);
                //Monitor_Enable(false);
                CurrentTestResult_Enable(true);
                HistoryTestResult_Enable(true);
                SerialPort_Enable(true);
                AdvancedSetting_Enable(true);
                TestRunLabel.Text = "运行";
                btn_stop();
            }
            else {
                TestChooseFormShow_Enable(false);
                TestRun_Enable(false);
                Monitor_Enable(true);
                CurrentTestResult_Enable(true);
                HistoryTestResult_Enable(true);
                SerialPort_Enable(false);
                AdvancedSetting_Enable(false);
                TestRunLabel.Text = "停止";
                btn_start();
                Monitor_Click(sender, e);
            }
        }
        private void Monitor_Click(object sender, EventArgs e) {
            if (testResultChart.IsAccessible == true)
                testResultChart.Hide();
            else testResultChart.Show();
        }
        private void SerialPort_Click(object sender, EventArgs e) {
            skinGroupBox1.BringToFront();
            skinGroupBox1.Size = new Size(248, 247);
        }

        private void SerialPortEnsure_Click(object sender, EventArgs e) {
            skinGroupBox1.Size = new Size(0, 0);
        }
        public void AdvancedSetting_Click(object sender, EventArgs e) {
            AlphaT0Setting alphaT0Setting = new AlphaT0Setting();
            alphaT0Setting.Show();
        }
        

        
        private void TestResultChart_FormClosing(object sender, EventArgs e) {
            if(testResultChart.DialogResult == DialogResult.Cancel) return;
            btn_stop();
            MessageBox.Show(@"计算已收敛,是否结束计算并显示结果?", @"提示", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (testResultChart.DialogResult != DialogResult.Yes) return;
            CurrentTestResult_Click(this,e);

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
            //MessageBox.Show(TotalCHN);
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
                    if (count % AppCfg.devicepara.Save_interval == 0) {
                        SavetData("AutoSave", listView_main);
                        listView_main.Items.Clear();
                    }
                    //MessageBox.Show(@"数据已收敛", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    List<string> channels = TotalCHN.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    List<double> dataList = recvstr.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(double.Parse).ToList();
                    //Dictionary<string, double> testResult = new Dictionary<string, double>();
                    testResult.Clear();
                    for (int i = 0; i < channels.Count; i++)
                    {
                        testResult.Add(channels[i], dataList[i]);
                    }

                    //timer1.Enabled = true;
                    //timer1.Start();
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

        private void SetupTest_Load(object sender, EventArgs e) {
            #region //不同设置窗口默认显示
                //EmptyGroupBox.Size = new Size(1250, 855);
                TextGroupbox1.Size = new Size(0, 0);
                TextGroupbox2.Size = new Size(0, 0);
                TextGroupbox3.Size = new Size(0, 0);
                TextGroupbox4.Size = new Size(0, 0);
                skinGroupBox1.Size = new Size(0, 0);
                MenuGroupBox.Visible = false;
                TestChoiseGroupBox.Visible = false;
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

                
                testResultChart.FormClosing += TestResultChart_FormClosing;
                ReadPara();
                LoginGroupBox.BringToFront();
                LoginGroupBox.Visible = true;
                comboBox.SelectedItem = "普通用户";

        }

        private void timer1_Tick(object sender, EventArgs e) {
            //testResultChart.ShowChart(testResult);
            //timer1.Stop();
            //timer1.Enabled = false;
        }

    }
}
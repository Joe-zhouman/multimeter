// Administrator
// multimeter
// multimeter
// 2021-01-16-9:39
// *Author*    : Joe, Zhou Man
// *Email*     : man.man.man.man.a@gmail.com
// *Email*     : joe_zhouman@foxmail.com
// *QQ*        : 1592020915
// *Weibo*     : @zhouman_LFC
// *Twitter*   : @zhouman_LFC
// *Website*   : www.joezhouman.com
// *Github*    : https://github.com/Joe-zhouman
// *LeetCode*  : https://leetcode-cn.com/u/joe_zm/

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using BusinessLogic;
using DataAccess;
using Model;

namespace multimeter {
    public partial class SetupTest {
        private Queue<string> _serialPortData;

        private void btn_start() {
            #region //开始串口采集

            //btn_stop.Enabled = true;
            //btn_start.Enabled = false;
            _count = 0;
            _latestDataFile = "";
            _latestResultFile = "";
            _latestOriginFile = "";
            _serialPortData.Clear();
            string fileName = _method + "-TempAutoSave.csv";
            _autoSaveFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AutoSave",
                _method + "-" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss.ffff"));
            try {
                DirectoryInfo di = Directory.CreateDirectory(_autoSaveFilePath);
            }
            catch (Exception ex) {
                MessageBox.Show($@"自动保存文件创建失败,请重试
{ex.Message}", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Error(ex);
                btn_stop();
                return;
            }

            _latestDataFile = Path.Combine(_autoSaveFilePath, fileName);
            _latestOriginFile = Path.Combine(_autoSaveFilePath, _method + "-OriginalDataAutoSave.csv");
            try {
                serialPort1.BaudRate = int.Parse(_appCfg.SerialPortPara.SerialBaudRate);
                serialPort1.PortName = _appCfg.SerialPortPara.SerialPort;
                serialPort1.Parity = (Parity) Enum.Parse(typeof(Parity), _appCfg.SerialPortPara.SerialParity);
                serialPort1.StopBits = (StopBits) Enum.Parse(typeof(StopBits), _appCfg.SerialPortPara.SerialStopBits);
                serialPort1.DataBits = int.Parse(_appCfg.SerialPortPara.SerialDataBits);

                serialPort1.Open();
            }
            catch (Exception ex) {
                Log.Error(ex);
                MessageBox.Show(@"无法打开串口！");
                return;
            }

            serialPort1.DiscardInBuffer(); //丢弃来自串口驱动程序的接收缓冲区的数据

            #endregion

            _multiMeter = new MultiMeterInfo(_appCfg.SerialPortPara);
            SendMsg();
            _temp = new List<double[]>();
            try {
                StreamWriter tempWrite = new StreamWriter(_latestDataFile);
                tempWrite.WriteLine("step," + string.Join(",", _device.Channels));
                tempWrite.Close();

                StreamWriter write = new StreamWriter(_latestOriginFile);
                write.WriteLine("step," + _multiMeter.TotalChn);
                write.Close();
                StatusTextBox_AddText(PromptType.INFO,$@"[{DateTime.Now:MM-dd-hh:mm:ss}]数据保存成果!
测试仪原始数据保存在 {_latestOriginFile}
温度历史数据保存在 {_latestDataFile}");
            }
            catch (Exception ex) {
                StatusTextBox_AddText( PromptType.WARNING,$"[{DateTime.Now:MM-dd-hh:mm:ss}]数据保存失败!");
                Log.Error(ex);
            }

        }

        public void SendMsg() {
            #region

            string st3 = SerialPortOpt.ResolveCmd(_multiMeter);

            string[] str = st3.Split('\n');
            foreach (string i in str) {
                serialPort1.WriteLine(i);
                Thread.Sleep(100);
            }

            Thread.Sleep(100);

            //MessageBox.Show(TotalCHN);
            _enableScan = true;
            Thread thread = new Thread(() => //新开线程，执行接收数据操作
            {
                while (_enableScan) //如果标识为true
                {
                    Thread.Sleep(1);
                    try {
                        serialPort1.WriteLine(":READ?");
                        Thread.Sleep(10);
                        if (serialPort1.BytesToRead != 0) _serialPortData.Enqueue(serialPort1.ReadTo(((char)0x11).ToString()));
                        Thread.Sleep(1500);
                        //Thread.Sleep(_appCfg.SysPara.ScanInterval.Value * _multiMeter.TotalNum);
                    }
                    catch (Exception ex) {
                        Log.Error(ex);
                    }
                }
            });
            thread.Start(); //启动线程
            thread.IsBackground = true;

            #endregion
        }

        private void btn_stop() {
            serialPort1.Close();
            _temp.Clear();
            _enableScan = false;
        }

        private void SetupTest_FormClosing(object sender, FormClosingEventArgs e) {
            serialPort1.Close();
            while (serialPort1.IsOpen) {
            }

            Application.Exit();
        }

        private void SerialPort_Timer_Tick(object sender, EventArgs e) {
            #region

            if (_serialPortData.Count != 0) {
                string str = _serialPortData.Dequeue();

                str = str.Replace((char) 19, (char) 0);
                str = str.Replace((char) 13, (char) 0);
                str = str.Replace((char) 0x11, (char) 0);
                str = str.Replace("\0", "");
                if (str.Length == 0) return;
                double[] dataList;
                try {
                    dataList = str.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                        .Select(double.Parse).ToArray();
                }
#if DEBUG
                catch (FormatException) {
                    StatusTextBox_AddText(PromptType.ERROR,$"[{DateTime.Now:MM-dd-hh:mm:ss}]数据转换失败，请检查串口!");
                    StatusTextBox_AddText(PromptType.ERROR, str);
                    return;
                }
#endif
                catch (Exception ex) {
                    Log.Error(ex);

                    return;
                }

                string[] channels = _multiMeter.Channels;
                if (dataList.Length != channels.Length) {
#if DEBUG
                    StatusTextBox_AddText(PromptType.ERROR,$"[{DateTime.Now:MM-dd-hh:mm:ss}]接收到的数据数目不等于频道数，请检查串口!");
                    StatusTextBox_AddText(PromptType.ERROR, str);
                    StatusTextBox_AddText(PromptType.ERROR, $"{dataList.Length},{channels.Length}");
#endif
                    return;
                }

                _count++;
                string temp = _count.ToString() + ',';
                _testResult.Clear();
                for (int i = 0; i < channels.Length; i++)
                    _testResult.Add(channels[i], dataList[i]);
                try {
                    DeviceOpt.SetTemp(ref _device, _testResult);
                    temp += string.Join(",", _device.Temp);
                }
                catch (ValOutOfRangeException ex) when (ex.Type == ValOutOfRangeType.LESS_THAN) {
                    StatusTextBox_AddText(PromptType.WARNING,
                        $"[{DateTime.Now:MM-dd-hh:mm:ss}]温度小于测试范围({_appCfg.SysPara.TempLb:G4}℃~{_appCfg.SysPara.TempUb:G4}℃)，请检查串口或标定参数!");
#if DEBUG
                    StatusTextBox_AddText(PromptType.WARNING, str);
                    StatusTextBox_AddText(PromptType.WARNING, temp);
#endif
                    return;
                }
                catch (ValOutOfRangeException ex) when (ex.Type == ValOutOfRangeType.GREATER_THAN) {
                    StatusTextBox_AddText( PromptType.WARNING,
                        $"[{DateTime.Now:MM-dd-hh:mm:ss}]温度大于测试范围({_appCfg.SysPara.TempLb:G4}℃~{_appCfg.SysPara.TempUb:G4}℃)，请检查标定参数或减小加热功率!");
#if DEBUG
                    StatusTextBox_AddText(PromptType.WARNING, str);
                    StatusTextBox_AddText(PromptType.WARNING, temp);
#endif
                    return;
                }
#if DEBUG
                catch (ValOutOfRangeException ex) {
                    StatusTextBox_AddText(PromptType.WARNING,
                        $"[{DateTime.Now:MM-dd-hh:mm:ss}]求解错误({_appCfg.SysPara.TempLb:G4}℃~{_appCfg.SysPara.TempUb:G4}℃)，请检查标定参数或减小加热功率!");
                    StatusTextBox_AddText(PromptType.WARNING, str);
                    StatusTextBox_AddText(PromptType.WARNING, temp);
                    return;
                }
#endif
                catch (Exception ex) {
                    Log.Error(ex);
                    return;
                }

                
                try {
                    StreamWriter tempWrite = new StreamWriter(_latestDataFile, true);
                    tempWrite.WriteLine(temp);
                    tempWrite.Close();
                    StreamWriter write = new StreamWriter(_latestOriginFile, true);
                    write.WriteLine(_count.ToString() + ',' + str);
                    write.Close();
                }
                catch (Exception ex) {
                    StatusTextBox_AddText(PromptType.WARNING, $"[{DateTime.Now:MM-dd-hh:mm:ss}]数据保存失败!");
                    Log.Error(ex);
                }

                _temp.Add(_device.Temp.ToArray());


                if (_count % _appCfg.SysPara.SaveInterval.Value == 0 && TempOk()) {
                    if (!_convergent) IsConvergent();
                    _latestResultFile = Path.Combine(_autoSaveFilePath, _method + "-" + _count + ".rst");
                    SaveDataThread t = new SaveDataThread(_latestResultFile, _lastTemp);
                    Thread rstThread = new Thread(t.SaveToData);
                    
                    _temp.Clear();
                    StatusTextBox_AddText(PromptType.INFO, $@"[{DateTime.Now:MM-dd-hh:mm:ss}]开始保存结果数据 {_latestResultFile}!
");
                    rstThread.Start();
                }

                _testResultChartUpdate = true;

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
                //        SaveToData("sss", listView_main);
                //        listView_main.Items.Clear();
                //    }         
                //}

                #endregion
            }
        }

        internal class SaveDataThread
        {
            private string _name;
            private List<double[]> _temp;
            public SaveDataThread(string name, List<double[]> tempList)
            {
                _name = name;
                _temp = new List<double[]>();
                foreach (var temp in tempList)
                {
                    var tempC = new double[temp.Length];
                    for (int i = 0; i < temp.Length; i++)
                    {
                        tempC[i] = temp[i];
                    }
                    _temp.Add(tempC);
                }
            }
            public void SaveToData()
            {
                #region

                File.Copy(IniReadAndWrite.IniFilePath, _name);
                //MessageBox.Show(filePath);
                try
                {
                    for (int i = 0; i < _temp.Count; i++)
                        IniHelper.Write("Data", i.ToString(), string.Join(",", _temp[i]), _name);
                }
                catch
                {
                    //StatusTextBox.AppendText($@"![ERROR][{DateTime.Now:MM-dd-hh:mm:ss}]保存失败！");
                    return;
                }

                //StatusTextBox.AppendText($@"![INFO][{DateTime.Now:MM-dd-hh:mm:ss}]保存成功！");

                #endregion
            }
        } 
        

        private bool TempOk() {
            if (_temp.Count == 0) return false;
            _lastTemp = _temp;
            return true;
        }

        private void IsConvergent() {
            double[] lastTempArray = Solution.AveTemp(_lastTemp);
            double[] currentTempArray = Solution.AveTemp(_temp);
            for (int i = 0; i < _multiMeter.TotalNum; i++)
                if (Math.Abs(1 - lastTempArray[i] / currentTempArray[i]) > _appCfg.SysPara.ConvergentLim) {
                    _convergent = false;
                    return;
                }

            _convergent = true;

            /*    if (ConvergentHolding_Timer.Enabled) return;
                ConvergentHolding_Timer.Enabled = true;
                string countDown = SecToTimeSpan(AppCfg.devicepara.AutoCloseInterval);
                MessageBox.Show($@"所有通道数据已经稳定
    自动停止测试倒计时长{countDown}", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);   */
        }
    }
}
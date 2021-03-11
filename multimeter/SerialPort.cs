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
        private void btn_start() {
            #region //开始串口采集

            //btn_stop.Enabled = true;
            //btn_start.Enabled = false;
            _count = 0;
            _latestDataFile = "";
            _latestResultFile = "";
            _latestOriginFile = "";
            var fileName = _method + "-TempAutoSave.csv";
            _autoSaveFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AutoSave",
                _method + "-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss.ffff"));
            try {
                var di = Directory.CreateDirectory(_autoSaveFilePath);
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
                //btn_start.Enabled = true;
                //btn_stop.Enabled = false;
                return;
            }

            serialPort1.DiscardInBuffer(); //丢弃来自串口驱动程序的接收缓冲区的数据

            #endregion
            _device.SetTempRange(_appCfg.SysPara.TempLb,_appCfg.SysPara.TempUb);
            _multiMeter = new MultiMeterInfo(_appCfg.SerialPortPara);
            SendMsg();
            _temp = new List<string>();
            try {
                var tempWrite = new StreamWriter(_latestDataFile);
                tempWrite.WriteLine("step," + _multiMeter.TotalChn);
                tempWrite.Close();
                var write = new StreamWriter(_latestOriginFile);
                write.WriteLine("step," + _multiMeter.TotalChn);
                write.Close();
            }
            catch (Exception ex) {
                StatusTextBox.Text += $"![WARNING][{DateTime.Now:mm-dd-hh:mm:ss}]数据保存失败!\n";
                Log.Error(ex);
            }
        }

        public void SendMsg() {
            #region

            var st3 = SerialPortOpt.ResolveCmd(_multiMeter);

            var str = st3.Split('\n');
            foreach (var i in str) {
                serialPort1.WriteLine(i);
                Thread.Sleep(100);
            }

            Thread.Sleep(100);

            Thread thread;
            //MessageBox.Show(TotalCHN);
            _enableScan = true;
            thread = new Thread(() => //新开线程，执行接收数据操作
            {
                while (_enableScan) //如果标识为true
                {
                    Thread.Sleep(1);
                    try {
                        serialPort1.WriteLine(":READ?");
                        Thread.Sleep(_appCfg.SysPara.ScanInterval.Value);
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
            while (serialPort1.IsOpen) { }

            Application.Exit();
        }

        private void SerialPort_Timer_Tick(object sender, EventArgs e) {
            #region

            if (serialPort1.BytesToRead != 0) {
                var recStr = serialPort1.ReadTo(((char)0x11).ToString());
                //serialPort1.DiscardInBuffer();  //丢弃来自串口驱动程序的接收缓冲区的数据

                recStr = recStr.Replace((char) 19, (char) 0);
                recStr = recStr.Replace((char) 13, (char) 0);
                recStr = recStr.Replace("\0", "");

                double[] dataList;
                try {
                    dataList = recStr.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                        .Select(double.Parse).ToArray();
                }
                catch (FormatException) {
                    StatusTextBox.Text += $"![ERROR][{DateTime.Now:mm-dd-hh:mm:ss}]数据转换失败，请检查串口!\n";
                    return;
                }
                catch (Exception ex) {
                    Log.Error(ex);
                    return;
                }

                if (dataList.Length != _multiMeter.Channels.Length) {
                    StatusTextBox.Text += $"![ERROR][{DateTime.Now:mm-dd-hh:mm:ss}]接收到的数据数目小于频道数，请检查串口!\n";
                    return;
                }
                _count++;
                var temp = _count.ToString() + ',';
                _testResult.Clear();
                for (var i = 0; i < _multiMeter.Channels.Length; i++)
                    _testResult.Add(_multiMeter.Channels[i], dataList[i]);
                try {
                    DeviceOpt.SetTemp(ref _device, _testResult);
                }
                catch (ValOutOfRangeException ex)when(ex.Type == ValOutOfRangeType.LESS_THAN) {
                    StatusTextBox.Text += $"![ERROR][{DateTime.Now:mm-dd-hh:mm:ss}]温度小于测试范围，请检查串口或标定参数!\n";
                    return;
                }
                catch (ValOutOfRangeException ex) when (ex.Type == ValOutOfRangeType.GREATER_THAN)
                {
                    StatusTextBox.Text += $"![WARNING][{DateTime.Now:mm-dd-hh:mm:ss}]温度大于测试范围，标定参数或加热功率!\n";
                    return;
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    return;
                }
                DeviceOpt.GetTempList(ref temp, _device);
                try {
                    var tempWrite = new StreamWriter(_latestDataFile, true);
                    tempWrite.WriteLine(temp);
                    tempWrite.Close();
                    var write = new StreamWriter(_latestOriginFile, true);
                    write.WriteLine(_count.ToString() + ','+recStr);
                    write.Close();
                }
                catch (Exception ex) {
                    StatusTextBox.Text += $"![WARNING][{DateTime.Now:mm-dd-hh:mm:ss}]数据保存失败!\n";
                    Log.Error(ex);
                }
                _temp.Add(temp);

                if (_count % _appCfg.SysPara.SaveInterval.Value == 0)
                    if (TempOk()) {
                        if (!_convergent) IsConvergent();
                        SaveToData(_method + "-" + _count + ".rst");
                        _temp.Clear();
                    }

                //MessageBox.Show(@"数据已收敛", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //Dictionary<string, double> testResult = new Dictionary<string, double>();

                _testResultChartUpdate = true;
            }

            //if(str.IndexOf((char)13)!=)
            //recvstr = str.Substring(str.IndexOf((char)13), str.Length - str.IndexOf((char)13));



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

        private void SaveToData(string name) {
            #region

            var fileName = name;
            _latestResultFile = Path.Combine(_autoSaveFilePath, fileName);
            File.Copy(IniReadAndWrite.IniFilePath, _latestResultFile);
            //MessageBox.Show(filePath);
            try {
                for (var i = 0; i < _lastTemp.Count; i++)
                    IniHelper.Write("Data", i.ToString(), _lastTemp[i], _latestResultFile);
            }
            catch (Exception ex) {
                Log.Error(ex);
                MessageBox.Show(ex.ToString());
            }

            #endregion
        }

        private bool TempOk() {
            if (_temp.Count == 0) return false;
            _lastTemp = _temp;
            return true;
        }

        private void IsConvergent() {
            var lastTempArray = Solution.AveTemp(_lastTemp.ToArray(), _multiMeter.TotalNum);
            var currentTempArray = Solution.AveTemp(_temp.ToArray(), _multiMeter.TotalNum);
            for (var i = 0; i < _multiMeter.TotalNum; i++)
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
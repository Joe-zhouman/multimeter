﻿// Administrator
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
using System.Runtime.Remoting.Channels;
using System.Threading;
using System.Windows.Forms;
using BusinessLogic;
using DataAccess;
using Model;

namespace multimeter {
    public partial class SetupTest {
        private string _recStr;
        private string _str;
        private Thread _serialPortThread;
        private void SerialPortEnable(bool enable) {
            if (enable) {
                _serialPortThread = new Thread(new ThreadStart(SerialPortRead));
                _serialPortThread.Start();
            }
            else {
                _serialPortThread.Abort();
            }
            
        }  

        private void SerialPortRead() {
            if (serialPort1.BytesToRead != 0) {
                string str = serialPort1.ReadExisting();
            }

        }


         private void btn_start() {
            #region //开始串口采集

            //btn_stop.Enabled = true;
            //btn_start.Enabled = false;
            _count = 0;
            _latestDataFile = "";
            _latestResultFile = "";
            _latestOriginFile = "";
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
                //btn_start.Enabled = true;
                //btn_stop.Enabled = false;
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
                StatusTextBox.Text += $@"![INFO][{DateTime.Now:MM-dd-hh:mm:ss}]数据保存成果!
测试仪原始数据保存在 {_latestOriginFile}
温度历史数据保存在 {_latestDataFile}";
            }
            catch (Exception ex) {
                StatusTextBox.Text += $"![WARNING][{DateTime.Now:MM-dd-hh:mm:ss}]数据保存失败!\n";
                Log.Error(ex);
            }
            SerialPortEnable(true);
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
                        Thread.Sleep(_appCfg.SysPara.ScanInterval.Value*_multiMeter.TotalNum);
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
            SerialPortEnable(false);
        }

        private void SetupTest_FormClosing(object sender, FormClosingEventArgs e) {
            serialPort1.Close();
            while (serialPort1.IsOpen) {
            }

            Application.Exit();
        }

        private void SerialPort_Timer_Tick(object sender, EventArgs e) {
            #region
            if (serialPort1.BytesToRead != 0) {
                string str = serialPort1.ReadExisting();

                if (str.IndexOf((char) 19) != -1)
                    str = str.Substring(str.IndexOf((char) 19), str.Length - str.IndexOf((char) 19));
                if (str.IndexOf((char) 13) == -1) {
                    recStr += str;
                }
                else {
                    str = str.Substring(0, str.IndexOf((char) 13));
                    _recStr += str;
                    if (_recStr.Length > 0) {
                        _recStr = _recStr.Replace((char) 19, (char) 0);
                        _recStr = _recStr.Replace((char) 13, (char) 0);
                        _recStr = _recStr.Replace((char) 0x11, (char) 0);
                        _recStr = _recStr.Replace("\0", "");

                        double[] dataList;
                        try {
                            dataList = _recStr.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                                .Select(double.Parse).ToArray();
                        }
#if DEBUG
                        catch (FormatException) {
                            StatusTextBox.Text += $"![ERROR][{DateTime.Now:MM-dd-hh:mm:ss}]数据转换失败，请检查串口!\n";
                            StatusTextBox.Text += _recStr + "\n";
                            _recStr = "";
                            return;
                        }
#endif
                        catch (Exception ex) {
                            Log.Error(ex);
                            _recStr = "";
                            return;
                        }

                        var channels = _multiMeter.Channels;
                        if (dataList.Length != channels.Length) {
#if DEBUG
                            StatusTextBox.Text += $"![ERROR][{DateTime.Now:MM-dd-hh:mm:ss}]接收到的数据数目小于频道数，请检查串口!\n";
                            StatusTextBox.Text += _recStr + "\n";
#endif
                            _recStr = "";
                            return;
                        }

                        _count++;
                        string temp = _count.ToString() + ',';
                        _testResult.Clear();
                        for (int i = 0; i < channels.Length; i++)
                            _testResult.Add(channels[i], dataList[i]);
                        try {
                            DeviceOpt.SetTemp(ref _device, _testResult);
                        }
                        catch (ValOutOfRangeException ex) when (ex.Type == ValOutOfRangeType.LESS_THAN) {
                            StatusTextBox.Text +=
                                $"![WARNING][{DateTime.Now:MM-dd-hh:mm:ss}]温度小于测试范围({_appCfg.SysPara.TempLb:G4}℃~{_appCfg.SysPara.TempUb:G4}℃)，请检查串口或标定参数!\n";
#if DEBUG
                            StatusTextBox.Text += _recStr + "\n";
                            StatusTextBox.Text += temp + "\n";
#endif
                            _recStr = "";
                            return;
                        }
                        catch (ValOutOfRangeException ex) when (ex.Type == ValOutOfRangeType.GREATER_THAN) {
                            StatusTextBox.Text +=
                                $"![WARNING][{DateTime.Now:MM-dd-hh:mm:ss}]温度大于测试范围({_appCfg.SysPara.TempLb:G4}℃~{_appCfg.SysPara.TempUb:G4}℃)，请检查标定参数或减小加热功率!\n";
#if DEBUG
                            StatusTextBox.Text += _recStr + "\n";
                            StatusTextBox.Text += temp + "\n";
#endif
                            _recStr = "";
                            return;
                        }
#if DEBUG
                        catch (ValOutOfRangeException ex) {
                            StatusTextBox.Text +=
                                $"![WARNING][{DateTime.Now:MM-dd-hh:mm:ss}]求解错误({_appCfg.SysPara.TempLb:G4}℃~{_appCfg.SysPara.TempUb:G4}℃)，请检查标定参数或减小加热功率!\n";
                            StatusTextBox.Text += _recStr + "\n";
                            StatusTextBox.Text += temp + "\n";
                            _recStr = "";
                            return;
                        }
#endif
                        catch (Exception ex) {
                            Log.Error(ex);
                            _recStr = "";
                            return;
                        }

                        temp += string.Join(",", _device.Temp);
                        try {
                            StreamWriter tempWrite = new StreamWriter(_latestDataFile, true);
                            tempWrite.WriteLine(temp);
                            tempWrite.Close();
                            StreamWriter write = new StreamWriter(_latestOriginFile, true);
                            write.WriteLine(_count.ToString() + ',' + _recStr);
                            write.Close();
                        }
                        catch (Exception ex) {
                            StatusTextBox.Text += $"![WARNING][{DateTime.Now:MM-dd-hh:mm:ss}]数据保存失败!\n";
                            Log.Error(ex);
                        }

                        recStr = "";
                        _temp.Add(_device.Temp.ToArray());


                        if (_count % _appCfg.SysPara.SaveInterval.Value == 0 && TempOk()) {
                            if (!_convergent) IsConvergent();
                            _latestResultFile = Path.Combine(_autoSaveFilePath, _method + "-" + _count + ".rst");
                            Thread rstThread = new Thread(() => {
                                    SaveToData(_latestResultFile,_lastTemp);
                                }
                            );
                            _temp.Clear();
                            StatusTextBox.Text += $@"![Info][{DateTime.Now:MM-dd-hh:mm:ss}]开始保存结果数据 {_latestResultFile}!
";
                            rstThread.Start();
                        }

                        _testResultChartUpdate = true;
                    }
                }
                else {
                    _recStr += str;
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
                //        SaveToData("sss", listView_main);
                //        listView_main.Items.Clear();
                //    }         
                //}

                #endregion
            }
        }

        private void SaveToData(string name,List<double[]> temp) {
            #region
            
            File.Copy(IniReadAndWrite.IniFilePath, name);
            //MessageBox.Show(filePath);
            try {
                for (int i = 0; i < temp.Count; i++)
                    IniHelper.Write("Data", i.ToString(), string.Join(",",temp[i]), name);
            }
            catch {
                StatusTextBox.Text += $@"![ERROR][{DateTime.Now:MM-dd-hh:mm:ss}]保存失败！";
                return;
            }
            StatusTextBox.Text += $@"![Info][{DateTime.Now:MM-dd-hh:mm:ss}]保存成功！";
            #endregion
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
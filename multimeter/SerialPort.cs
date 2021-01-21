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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DataProcessor;

namespace multimeter {
    public partial class SetupTest {
        private void SerialPort_Timer_Tick(object sender, EventArgs e)
        {
            #region
            if (serialPort1.BytesToRead != 0)
            {
                string str = serialPort1.ReadExisting();
                //serialPort1.DiscardInBuffer();  //丢弃来自串口驱动程序的接收缓冲区的数据
                //richTextBox1.Text = richTextBox1.Text + str+"\n";

                //byte[] array = System.Text.Encoding.ASCII.GetBytes(str);  //数组array为对应的ASCII数组     
                //string ASCIIstr2 = null;
                //for (int i = 0; i < array.Length; i++)
                //{
                //    int asciicode = (int)(array[i]);
                //    ASCIIstr2 += Convert.ToString(asciicode);//字符串ASCIIstr2 为对应的ASCII字符串
                //}
                //richTextBox2.Text = richTextBox2.Text + ASCIIstr2;


                if (str.IndexOf((char)19) != -1)
                    str = str.Substring(str.IndexOf((char)19), str.Length - str.IndexOf((char)19));
                if (str.IndexOf((char)13) != -1)
                {
                    str = str.Substring(0, str.IndexOf((char)13));
                    _recvstr += str;
                    if (_recvstr.Length > 0)
                    {
                        int firstIdx = _recvstr.IndexOf((char)13);
                        int lastIdx = _recvstr.LastIndexOf((char)13);
                        if (-1 != firstIdx && firstIdx != lastIdx)
                        {
                            _recvstr = _recvstr.Remove(firstIdx, lastIdx - firstIdx + 1);
                        }

                        _recvstr = _recvstr.Replace((char)19, (char)0);
                        _recvstr = _recvstr.Replace((char)13, (char)0);
                        _recvstr = _recvstr.Replace((char)0x11, (char)0);
                        _recvstr = _recvstr.Replace("\0", "");


                        double[] dataList;
                        try
                        {
                            dataList = _recvstr.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).ToArray();
                        }
                        catch (Exception ex)
                        {
                            log.Error(_recvstr + "\n", ex);
                            return;
                        }
                        if (dataList.Length != channels.Length)
                        {
                            return;
                        }

                        count++;
                        string temp = count.ToString() + ',';
                        _testResult.Clear();
                        for (int i = 0; i < channels.Length; i++) _testResult.Add(channels[i], dataList[i]);
                        _heatMeter1.SetTemp(_testResult);
                        temp = _heatMeter1.Temp.Aggregate(temp, (current, d) => current + (d.ToString(CultureInfo.InvariantCulture) + ','));
                        if (_sample1 != null)
                        {
                            _sample1.SetTemp(_testResult);
                            temp = _sample1.Temp.Aggregate(temp, (current, d) => current + (d.ToString(CultureInfo.InvariantCulture) + ','));
                        }
                        if (_sample2 != null)
                        {
                            _sample2.SetTemp(_testResult);
                            temp = _sample2.Temp.Aggregate(temp, (current, d) => current + (d.ToString(CultureInfo.InvariantCulture) + ','));
                        }
                        _heatMeter2.SetTemp(_testResult);
                        temp = _heatMeter2.Temp.Aggregate(temp, (current, d) => current + (d.ToString(CultureInfo.InvariantCulture) + ','));
                        try {
                            StreamWriter write = new StreamWriter(_latestDataFile, true);
                            write.WriteLine(temp);
                            write.Close();
                        }
                        catch (Exception ex) {

                            log.Error(ex);
                        }
                        finally {
                            _recvstr = "";
                        }
                        _temp.Add(temp);

                        if (count % AppCfg.devicepara.Save_interval == 0)
                        {
                            SaveToData(_method.ToString());
                            _temp.Clear();
                        }

                        //MessageBox.Show(@"数据已收敛", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        //Dictionary<string, double> testResult = new Dictionary<string, double>();

                        _testResultChartUpdate = true;

                    }

                    //if(str.IndexOf((char)13)!=)
                    //recvstr = str.Substring(str.IndexOf((char)13), str.Length - str.IndexOf((char)13));
                }
                else
                {
                    _recvstr += str;
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
            }

            #endregion
        }
        private void SaveToData(string name)
        {
            #region
            if (_temp.Count == 0)
                return;
            IsConvergent();
            _lastTemp = _temp;
            string fileName = name + "-" + count + ".rst";
            _latestResultFile = Path.Combine(_autoSaveFilePath, fileName);
            //MessageBox.Show(filePath);
            try
            {
                File.Copy(_tempFilePath, _latestResultFile);
                for (int i = 0; i < _lastTemp.Count; i++)
                {
                    INIHelper.Write("Data", i.ToString(), _lastTemp[i], _latestResultFile);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.ToString());
            }

            #endregion
        }

        private void IsConvergent() {
            if(_lastTemp == null)
            {
                _convergent = false;
                return;
            }
            if (_temp.Count == 0) {
                _convergent = false;
                return;
            }

            if (_lastTemp.Count == 0) {
                _convergent = false;
                return;
            }
            var lastTempArray = Solution.AveTemp(_lastTemp.ToArray(), TotalNum);
            var currentTempArray = Solution.AveTemp(_temp.ToArray(), TotalNum);
            for (int i = 0; i < TotalNum; i++) {
                if (Math.Abs(1 - lastTempArray[i] / currentTempArray[i]) > 1e-3) {
                    _convergent = false;
                    return;
                }
            }
            _convergent = true;
        }
    }
}
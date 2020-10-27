using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCWin;
using CCWin.SkinControl;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;


namespace multimeter
{
    public partial class MainDLG : Skin_VS
    {
        string TotalCHN = "";
        int TotalNum = 0;
        int count = 0;
        bool enablescan = false;
   
        public MainDLG()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;  //去掉线程安全
            CreateDefaultIni();
            ReadPara();
        }

        private void btn_setup_Click(object sender, EventArgs e)
        {
            SetupDLG dlg = new SetupDLG();
            dlg.Show();

        }
        private void CreateDefaultIni()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini");//在当前程序路径创建
            if (INIHelper.CheckPath(filePath))
                return;
            else
            {
                File.Create(filePath).Close();
                INIHelper.Write("Serial", "port", "COM1", filePath);
                INIHelper.Write("Serial", "baudrate", "9600", filePath);
                INIHelper.Write("Serial", "databits", "8", filePath);
                INIHelper.Write("Serial", "stopbites", "1", filePath);
                INIHelper.Write("Serial", "parity", "None", filePath);
                INIHelper.Write("Card1", "enable", "0", filePath);
                INIHelper.Write("Card2", "enable", "0", filePath);
                INIHelper.Write("SYS", "save_interval", "50", filePath);
                INIHelper.Write("SYS", "scan_interval", "2000", filePath);

                for (int i = 0; i < 22; i++)          //每个7700卡22个通道
                {
                    INIHelper.Write((i + 101).ToString(), "name", "", filePath);
                    INIHelper.Write((i + 101).ToString(), "func", "0", filePath);
                }

                for (int i = 0; i < 22; i++)
                {
                    INIHelper.Write((i + 201).ToString(), "name", "", filePath);
                    INIHelper.Write((i + 201).ToString(), "func", "0", filePath);
                }
            }
        }
        private void ReadPara()//读取ini
        {
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

            AppCfg.devicepara.Card1_enable=int.Parse(INIHelper.Read("Card1", "enable", "", filePath));
            AppCfg.devicepara.Card2_enable = int.Parse(INIHelper.Read("Card2", "enable", "", filePath));
            AppCfg.devicepara.Scan_interval = int.Parse(INIHelper.Read("SYS", "scan_interval", "2000", filePath));
            AppCfg.devicepara.Save_interval = int.Parse(INIHelper.Read("SYS", "save_interval", "50", filePath));

        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            btn_stop.Enabled = true;
            btn_start.Enabled = false;
            try
            {

                serialPort1.BaudRate = int.Parse(AppCfg.devicepara.SerialBaudRate);
                serialPort1.PortName = AppCfg.devicepara.SerialPort;
                switch (AppCfg.devicepara.SerialParity)
                {
                    case "None":
                        serialPort1.Parity = System.IO.Ports.Parity.None;
                        break;
                    case "奇校验":
                        serialPort1.Parity = System.IO.Ports.Parity.Odd;
                        break;
                    case "偶校验":
                        serialPort1.Parity = System.IO.Ports.Parity.Even;
                        break;
                    case "Mark":
                        serialPort1.Parity = System.IO.Ports.Parity.Mark;
                        break;
                    case "Space":
                        serialPort1.Parity = System.IO.Ports.Parity.Space;
                        break;
                    default:
                        serialPort1.Parity = System.IO.Ports.Parity.None;
                        break;
                }

                switch (AppCfg.devicepara.SerialStopBits)
                {
                    case "1":
                        serialPort1.StopBits = System.IO.Ports.StopBits.One;
                        break;
                    case "2":
                        serialPort1.StopBits = System.IO.Ports.StopBits.Two;
                        break;
                    case "1.5":
                        serialPort1.StopBits = System.IO.Ports.StopBits.OnePointFive;
                        break;

                    default:
                        serialPort1.Parity = System.IO.Ports.Parity.None;
                        break;
                }

                serialPort1.DataBits = int.Parse(AppCfg.devicepara.SerialDataBits);

                serialPort1.Open();
            }
            catch
            {
                MessageBox.Show("无法打开串口！");
                btn_start.Enabled = true;
                btn_stop.Enabled = false;
                return;
            }

            string TwoRlist = "";
            int TwoR_num = 0;
            if(AppCfg.devicepara.Card1_enable!=0)
            {
                foreach(Card i in AppCfg.devicepara.Cardlist1)
                {
                    if(i.func==1)
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
                }
            }

            if (AppCfg.devicepara.Card2_enable != 0)
            {
                foreach (Card i in AppCfg.devicepara.Cardlist2)
                {
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
                }
            }





            string FourRlist = "";
            int FourR_num = 0;
            if (AppCfg.devicepara.Card1_enable != 0)
            {
                foreach (Card i in AppCfg.devicepara.Cardlist1)
                {
                    if (i.func == 2)
                    {
                        FourR_num++;
                        if (FourRlist.Length == 0)
                        {
                            FourRlist = i.CHN;
                            if (TotalCHN.Length == 0)
                                TotalCHN = i.CHN;
                            else
                                TotalCHN = TotalCHN + "," + i.CHN;

                        }
                        else
                        {
                            FourRlist = FourRlist + "," + i.CHN;
                            TotalCHN = TotalCHN + "," + i.CHN;
                        }
                    }
                }
            }

            if (AppCfg.devicepara.Card2_enable != 0)
            {
                foreach (Card i in AppCfg.devicepara.Cardlist2)
                {
                    if (i.func == 2)
                    {
                        FourR_num++;
                        if (FourRlist.Length == 0)
                        {
                            FourRlist = i.CHN;
                            if (TotalCHN.Length == 0)
                                TotalCHN = i.CHN;
                            else
                                TotalCHN = TotalCHN + "," + i.CHN;
                        }
                        else
                        {
                            FourRlist = FourRlist + "," + i.CHN;
                            TotalCHN = TotalCHN + "," + i.CHN;
                        }
                    }
                }
            }


            string Templist = "";
            int Temp_num = 0;
            if (AppCfg.devicepara.Card1_enable != 0)
            {
                foreach (Card i in AppCfg.devicepara.Cardlist1)
                {
                    if (i.func == 3)
                    {
                        Temp_num++;
                        if (Templist.Length == 0)
                        {
                            Templist = i.CHN;
                            if (TotalCHN.Length == 0)
                                TotalCHN = i.CHN;
                            else
                                TotalCHN = TotalCHN + "," + i.CHN;
                        }
                        else
                        {
                            Templist = Templist + "," + i.CHN;
                            TotalCHN = TotalCHN + "," + i.CHN;
                        }
                    }
                }
            }

            if (AppCfg.devicepara.Card2_enable != 0)
            {
                foreach (Card i in AppCfg.devicepara.Cardlist2)
                {
                    if (i.func == 3)
                    {
                        Temp_num++;
                        if (Templist.Length == 0)
                        {
                            Templist = i.CHN;
                            if (TotalCHN.Length == 0)
                                TotalCHN = i.CHN;
                            else
                                TotalCHN = TotalCHN + "," + i.CHN;
                        }
                        else
                        {
                            Templist = Templist + "," + i.CHN;
                            TotalCHN = TotalCHN + "," + i.CHN;
                        }
                    }
                }
            }

            TotalNum = TwoR_num + FourR_num + Temp_num;

            sendmsg(TwoRlist, FourRlist, Templist, TwoR_num, FourR_num, Temp_num);

            
        }

        public string resolvcmd(string s1, string s2, string s3, int i1, int i2, int i3)
        {
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
            if (num > 0)
            {
                st = st.Replace("*nchannel*", num.ToString());
            }
            st = st.Replace("*allchannel*", TotalCHN);
          
            return st;
           

        }

        public void sendmsg(string s1,string s2,string s3,int i1,int i2,int i3)
        {
            string st3 = resolvcmd(s1, s2, s3, i1, i2, i3);

            string[] str = st3.Split('\n');
            foreach (string i in str)
            {
                serialPort1.WriteLine(i);
                Thread.Sleep(100);
            }
            Thread.Sleep(100);

            Thread thread;

            string[] chn = TotalCHN.Split(',');
            this.listView_main.Clear();
            this.listView_main.Columns.Add("序号", 120);
            for (int i = 0; i < TotalNum; i++)
            {
                this.listView_main.Columns.Add(chn[i], 120);
            }
            enablescan = true;
            thread = new Thread(() =>//新开线程，执行接收数据操作
            {
                while (enablescan)//如果标识为true
                {
                    Thread.Sleep(1);
                    try
                    {
                        serialPort1.WriteLine(":READ?");
                        Thread.Sleep(AppCfg.devicepara.Scan_interval);

                    }
                    catch (Exception ex)
                    {
                        ;
                    }
                }
            });
            thread.Start();//启动线程
            thread.IsBackground = true;

        }


        public  byte[] str2ASCII(String xmlStr)
        {
            return Encoding.Default.GetBytes(xmlStr);
        }



        string recvstr;
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
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


            if (str.IndexOf((char)19) != -1)
            {
                str = str.Substring(str.IndexOf((char)19), str.Length - str.IndexOf((char)19));
            }

            string nextstr = "";
            if(str.IndexOf((char)13)!=-1)
            {
                str = str.Substring(0, str.IndexOf((char)13));
                recvstr = recvstr + str;

                if (recvstr.Length > 0)
                {
                    count++;
                    recvstr = recvstr.Replace((char)19, (char)0);
                    recvstr = recvstr.Replace((char)13, (char)0);
                    recvstr = recvstr.Replace((char)0x11, (char)0);
                    recvstr = recvstr.Replace("\0", "");



                    string tmp = count.ToString() + "," + recvstr;
                    ListViewItem item = new ListViewItem(tmp.Split(','));
                    listView_main.Items.Add(item);
                    LastScan.Text = recvstr;

                    if (count % AppCfg.devicepara.Save_interval == 0)
                    {
                        SavetData("sss", listView_main);
                        listView_main.Items.Clear();
                    }

                }
                //if(str.IndexOf((char)13)!=)
                //recvstr = str.Substring(str.IndexOf((char)13), str.Length - str.IndexOf((char)13));
                recvstr = "";

            }
            else
            {
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
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            btn_start.Enabled = true;
            btn_stop.Enabled = false;
            enablescan = false;
        }


        public void SavetData(string name,ListView listView)
        {
            if (listView.Items.Count == 0)
            {
               // MessageBox.Show("未找到可导入的数据");
                return;
            }
          
            string FileName = Name  + DateTime.Now.ToString("yyyyMMddHHmmssffff")+".csv";

            try
            {
           
                    int size = 1024;
                    int sizeCnt = (int)Math.Ceiling((Double)listView.Items.Count / (Double)2000);
                    StreamWriter write = new StreamWriter(FileName, false, Encoding.Default, size * sizeCnt);
                    //获取listView标题行
                    for (int t = 0; t < listView.Columns.Count; t++)
                    {
                        write.Write(listView.Columns[t].Text + ",");
                    }
                    write.WriteLine();
                    //获取listView数据行
                    for (int lin = 0; lin < listView.Items.Count; lin++)
                    {
                        string Tem = null;
                        for (int k = 0; k < listView.Columns.Count; k++)
                        {
                            string TemString = listView.Items[lin].SubItems[k].Text;
                            Tem += TemString;
                            Tem += ",";
                        }
                        write.WriteLine(Tem);
                    }
                    write.Flush();
                    write.Close();
                
            }
            catch (System.Exception ex)
            {
                            
            }

        }
    }

    internal class AppCfg //全局变量
    {
        public AppCfg()
        {

        }
        internal static ParaInfo devicepara = new ParaInfo();
    }
}

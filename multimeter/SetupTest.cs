using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DataProcessor;
using System.IO;
using CCWin;
using CCWin.SkinControl;
using System.Runtime.InteropServices;
using System.Threading;

namespace multimeter
{
    public partial class SetupTest : Form
    {
        public string[] formData = new string[64];     //设置所有的界面输入框的数据为全局变量，方便存储数据到本地，或者从文件更新数据
        public string TestChoose;                      //测试方法选择标志符
        public string PatternChoose = "normal";       // 软件模式选择，分为“高级模式”和“普通模式”
        ListViewItem m_SelectedItem;
        #region  //串口采集
        string TotalCHN = "";
        int TotalNum = 0;
        int count = 0;
        bool enablescan = false;
        #endregion 

        public SetupTest()
        {
            InitializeComponent();
        }

        private void 导热系数测量_Load(object sender, EventArgs e)
        {
            #region  //不同设置窗口默认显示
            NoneGroupBox.Size = new Size(1531, 966);
            TextGroupbox1.Size = new Size(0, 0);
            TextGroupbox2.Size = new Size(0, 0);
            TextGroupbox3.Size = new Size(0, 0);
            TextGroupbox4.Size = new Size(0, 0);
            ViewGroupBox1.Size = new Size(0, 0);
            ViewGroupBox2.Size = new Size(0, 0);
            ViewGroupBox3.Size = new Size(0, 0);
            ViewGroupBox4.Size = new Size(0, 0);
            ResultGroupBox.Size = new Size(0, 0);
            FileGroupBox.Size = new Size(0, 0);   //“文件”按钮子目录
            chart1.Size = new Size(0, 0);//将曲线表格放在隐藏
            skinGroupBox1.Size = new Size(263, 218);  //普通模式下窗口尺寸
            //skinGroupBox1.Size = new Size(521, 218);  //高级模式下窗口尺寸
            skinGroupBox2.Size = new Size(0, 0);
            TestStop.Enabled = false;
            Monitor.Enabled = false;
            Monitor.Enabled = false;
            TestResult.Enabled = false;
            #endregion

            #region  //串口设置
            CheckForIllegalCrossThreadCalls = false;//去掉线程安全

            edit_scan_interval.Text = AppCfg.devicepara.Scan_interval.ToString();
            edit_save_interval.Text = AppCfg.devicepara.Save_interval.ToString();

            if (AppCfg.devicepara.Card1_enable == 0)
            {
                combox_card1.SelectedIndex = 0;
                listview_card1.Enabled = false;
            }
            else
            {
                combox_card1.SelectedIndex = 1;
                listview_card1.Enabled = true;

            }

            if (AppCfg.devicepara.Card2_enable == 0)
            {
                combox_card2.SelectedIndex = 0;
                listview_card2.Enabled = false;
            }
            else
            {
                combox_card2.SelectedIndex = 1;
                listview_card2.Enabled = true;

            }


            foreach (Card i in AppCfg.devicepara.Cardlist1)
            {
                string func = "";
                switch (i.func)
                {
                    case 1:
                        func = "两线电阻";
                        break;
                    case 2:
                        func = "四线电阻";
                        break;
                    case 3:
                        func = "热电偶";
                        break;
                    default:
                        func = "";
                        break;

                }

                ListViewItem item = new ListViewItem(new string[] { i.CHN, i.name, func });
                listview_card1.Items.Add(item);

            }

            foreach (Card i in AppCfg.devicepara.Cardlist2)
            {
                string func = "";
                switch (i.func)
                {
                    case 1:
                        func = "两线电阻";
                        break;
                    case 2:
                        func = "四线电阻";
                        break;
                    case 3:
                        func = "热电偶";
                        break;
                    default:
                        func = "";
                        break;

                }

                ListViewItem item = new ListViewItem(new string[] { i.CHN, i.name, func });
                listview_card2.Items.Add(item);

            }



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

            #region  //串口采集
            CheckForIllegalCrossThreadCalls = false;
            CreateDefaultIni();
            ReadPara();
            #endregion

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void ChannelTextBox1_10_TextChanged(object sender, EventArgs e)
        {

        }
        private void pictureLabel2_7_Click(object sender, EventArgs e)
        {

        }

        private void LengthTextBox2_7_TextChanged(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void LengthTextBox1_7_TextChanged(object sender, EventArgs e)
        {

        }


        private void groupBox2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            FileGroupBox.BringToFront();            //将“文件”按钮子目录设置为最顶层
            FileGroupBox.Size = new Size(131, 98);   //显示“文件”按钮子目录
            DelayTimer.Enabled = true;              //“文件”按钮子目录隐藏倒计时
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            conf_fileR();
            FileGroupBox.Size = new Size(0, 0);   //隐藏“文件”按钮子目录
        }

        private void SaveFile_Click(object sender, EventArgs e)
        {
            conf_fileW();
            FileGroupBox.Size = new Size(0, 0);   //隐藏“文件”按钮子目录
        }

        private void Pattern_Click(object sender, EventArgs e)
        {
            if (PatternChoose == "normal")
            {
                MessageBox.Show("高级模式已打开");
                PatternChoose = "advanced";
            }
            else
            {
                MessageBox.Show("高级模式已关闭");
                PatternChoose = "normal";
            }

            //以下代码为测试配置文档INI的读写
            //string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys2.ini");//在当前程序路径创建
            //INIHelper.Write("Serial", "baudrate1", "1", filePath);  //写
            //MessageBox.Show(INIHelper.Read("Serial", "baudrate", "", filePath).ToString());  //读取
        }
        private void ExitApplication_Click(object sender, EventArgs e)
        {
            conf_fileW();   //关闭前提示保存配置文件
            Application.Exit();
        }


        //---------------------------------------------------------------子函数区--------------------------------------------------------------------------------------------------------------------------------
        public void conf_fileW()        //保存数据设置
        {
            #region  //保存数据设置
            string[] formData = new string[64]
                {   //test1的数据
                    LengthTextBox1_1.Text,
                    LengthTextBox1_2.Text,
                    LengthTextBox1_3.Text,
                    LengthTextBox1_4.Text,
                    LengthTextBox1_5.Text,
                    LengthTextBox1_6.Text,
                    LengthTextBox1_7.Text,
                    LengthTextBox1_8.Text,
                    LengthTextBox1_9.Text,
                    LengthTextBox1_10.Text,
                    LengthTextBox1_11.Text,
                    K1TextBox1_1.Text,
                    K2TextBox1_2.Text,
                    ForceTextBox1.Text,

                    //test2的数据
                    LengthTextBox2_1.Text,
                    LengthTextBox2_2.Text,
                    LengthTextBox2_3.Text,
                    LengthTextBox2_4.Text,
                    LengthTextBox2_5.Text,
                    LengthTextBox2_6.Text,
                    LengthTextBox2_7.Text,
                    LengthTextBox2_8.Text,
                    LengthTextBox2_9.Text,
                    LengthTextBox2_10.Text,
                    LengthTextBox2_11.Text,
                    LengthTextBox2_12.Text,
                    LengthTextBox2_13.Text,
                    LengthTextBox2_14.Text,
                    K1TextBox2_1.Text,
                    Ks1TextBox2_3.Text,
                    Ks2TextBox2_4.Text,
                    K2TextBox2_2.Text,
                    ForceTextBox2.Text,

                    //test3的数据
                    LengthTextBox3_1.Text,
                    LengthTextBox3_2.Text,
                    LengthTextBox3_3.Text,
                    LengthTextBox3_4.Text,
                    LengthTextBox3_5.Text,
                    LengthTextBox3_6.Text,
                    LengthTextBox3_7.Text,
                    LengthTextBox3_8.Text,
                    K1TextBox3_1.Text,
                    K2TextBox3_1.Text,
                    ForceTextBox3.Text,
                    FilmThickness1.Text,

                    //test4的数据
                    LengthTextBox4_1.Text,
                    LengthTextBox4_2.Text,
                    LengthTextBox4_3.Text,
                    LengthTextBox4_4.Text,
                    LengthTextBox4_5.Text,
                    LengthTextBox4_6.Text,
                    LengthTextBox4_7.Text,
                    LengthTextBox4_8.Text,
                    LengthTextBox4_9.Text,
                    LengthTextBox4_10.Text,
                    LengthTextBox4_10.Text,
                    LengthTextBox4_12.Text,
                    LengthTextBox4_13.Text,
                    LengthTextBox4_14.Text,
                    K1TextBox4_1.Text,
                    Ks1TextBox4_2.Text,
                    Ks2TextBox4_3.Text,
                    K4TextBox4_4.Text,
                    ForceTextBox4.Text
                };
            saveFileDialog1.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                multimeter.test_file conf = new test_file();
                string str = saveFileDialog1.FileName;
                //conf.FileWrite(64, str, formData);
            }
            else MessageBox.Show("配置文件保存失败");
            #endregion
        }


        public void conf_fileR()        //更新数据设置
        {
            /* #region  //更新数据设置
             openFileDialog1.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";
             if (openFileDialog1.ShowDialog() == DialogResult.OK)
             {
                 multimeter.test_file conf = new test_file();
                 string str = openFileDialog1.FileName;
                 //conf.FileRead(64, str, out formData);

                 //test1的数据
                 LengthTextBox1_1.Text = formData[0];
                 LengthTextBox1_2.Text = formData[1];
                 LengthTextBox1_3.Text = formData[2];
                 LengthTextBox1_4.Text = formData[3];
                 LengthTextBox1_5.Text = formData[4];
                 LengthTextBox1_6.Text = formData[5];
                 LengthTextBox1_7.Text = formData[6];
                 LengthTextBox1_8.Text = formData[7];
                 LengthTextBox1_9.Text = formData[8];
                 LengthTextBox1_10.Text = formData[9];
                 LengthTextBox1_11.Text = formData[10];
                 K1TextBox1_1.Text = formData[11];
                 K2TextBox1_2.Text = formData[12];
                 ForceTextBox1.Text = formData[13];

                 //test2的数据
                 LengthTextBox2_1.Text = formData[14];
                 LengthTextBox2_2.Text = formData[15];
                 LengthTextBox2_3.Text = formData[16];
                 LengthTextBox2_4.Text = formData[17];
                 LengthTextBox2_5.Text = formData[18];
                 LengthTextBox2_6.Text = formData[19];
                 LengthTextBox2_7.Text = formData[20];
                 LengthTextBox2_8.Text = formData[21];
                 LengthTextBox2_9.Text = formData[22];
                 LengthTextBox2_10.Text = formData[23];
                 LengthTextBox2_11.Text = formData[24];
                 LengthTextBox2_12.Text = formData[25];
                 LengthTextBox2_13.Text = formData[26];
                 LengthTextBox2_14.Text = formData[27];
                 K1TextBox2_1.Text = formData[28];
                 Ks1TextBox2_3.Text = formData[29];
                 Ks2TextBox2_4.Text = formData[30];
                 K2TextBox2_2.Text = formData[31];
                 ForceTextBox2.Text = formData[32];

                 //test3的数据
                 LengthTextBox3_1.Text = formData[33];
                 LengthTextBox3_2.Text = formData[34];
                 LengthTextBox3_3.Text = formData[35];
                 LengthTextBox3_4.Text = formData[36];
                 LengthTextBox3_5.Text = formData[37];
                 LengthTextBox3_6.Text = formData[38];
                 LengthTextBox3_7.Text = formData[39];
                 LengthTextBox3_8.Text = formData[40];
                 K1TextBox3_1.Text = formData[41];
                 K2TextBox3_1.Text = formData[42];
                 ForceTextBox3.Text = formData[43];
                 FilmThickness1.Text = formData[44];

                 //test4的数据
                 LengthTextBox4_1.Text = formData[45];
                 LengthTextBox4_2.Text = formData[46];
                 LengthTextBox4_3.Text = formData[47];
                 LengthTextBox4_4.Text = formData[48];
                 LengthTextBox4_5.Text = formData[49];
                 LengthTextBox4_6.Text = formData[50];
                 LengthTextBox4_7.Text = formData[51];
                 LengthTextBox4_8.Text = formData[52];
                 LengthTextBox4_9.Text = formData[53];
                 LengthTextBox4_10.Text = formData[54];
                 LengthTextBox4_10.Text = formData[55];
                 LengthTextBox4_12.Text = formData[56];
                 LengthTextBox4_13.Text = formData[57];
                 LengthTextBox4_14.Text = formData[58];
                 K1TextBox4_1.Text = formData[59];
                 Ks1TextBox4_2.Text = formData[60];
                 Ks2TextBox4_3.Text = formData[61];
                 K4TextBox4_4.Text = formData[62];
                 ForceTextBox4.Text = formData[63];
             }
             else MessageBox.Show("配置文件读取失败");
             #endregion  */

            multimeter.test_file conf = new test_file();
            string[][] d1 = new string [20][];
            conf.ReadtData("1.csv", d1);
        }

        public void Test1Run()
        {
            #region //导热系数测量
            double T_u1, T_u2, T_u3, T_u4, T_s1, T_s2, T_s3, T_l1, T_l2, T_l3, T_l4;                                  //定义热电偶温度
            double k1 = double.Parse(K1TextBox1_1.Text); double k2 = double.Parse(K2TextBox1_2.Text);         //定义“流量计”热导率，单位w/((m*k)
            double q_u, q_s, q_l;                                                                                     //定义“流量计”“试件”热流密度
            T_u1 = 80; T_u2 = 70; T_u3 = 60; T_u4 = 50;
            T_l1 = 30; T_l2 = 20; T_l3 = 10; T_l4 = 0;
            T_s1 = 45; T_s2 = 40; T_s3 = 35;

            //以下代码为读取热电偶位置值，单位mm,转换为m
            double L_u1 = 0.001 * double.Parse(LengthTextBox1_1.Text);
            double L_u2 = 0.001 * double.Parse(LengthTextBox1_2.Text);
            double L_u3 = 0.001 * double.Parse(LengthTextBox1_3.Text);
            double L_u4 = 0.001 * double.Parse(LengthTextBox1_4.Text);
            double L_s1 = 0.001 * double.Parse(LengthTextBox1_5.Text);
            double L_s2 = 0.001 * double.Parse(LengthTextBox1_6.Text);
            double L_s3 = 0.001 * double.Parse(LengthTextBox1_7.Text);
            double L_l1 = 0.001 * double.Parse(LengthTextBox1_8.Text);
            double L_l2 = 0.001 * double.Parse(LengthTextBox1_9.Text);
            double L_l3 = 0.001 * double.Parse(LengthTextBox1_10.Text);
            double L_l4 = 0.001 * double.Parse(LengthTextBox1_11.Text);

            //以下代码计算“上流量计”热流密度q_u ，热导率为 k1 ,温度为T_u1, T_u2, T_u3, T_u4 ，热电偶位置为L_u1，L_u2，L_u3，L_u4 ,线性拟合方程为Y_u = b_u * X_u + a_u
            double[] x_u = new double[4] { 0, L_u1, L_u1 + L_u2, L_u1 + L_u2 + L_u3 };
            double[] y_u = new double[4] { T_u1, T_u2, T_u3, T_u4 };
            double a_u, b_u, maxErr_u;
            if (CalcRegress(x_u, y_u, 4, out a_u, out b_u, out maxErr_u) != 0)
            {
                MessageBox.Show("计算出错！");
                return;
            }
            else
            {
               // MessageBox.Show("Y_u = " + b_u.ToString() + " X_u +" + a_u.ToString() + "   maxErr_u=" + maxErr_u.ToString());
                q_u = k1 * Math.Abs(b_u);

            }

            //以下代码计算“下流量计”热流密度q_l ，热导率为 k2 ,温度为T_l1, T_l2, T_l3, T_l4 ，热电偶位置为L_l1，L_l2，L_l3，L_l4 ,线性拟合方程为Y_l = b_l * X_l + a_l
            double[] x_l = new double[4] { 0, L_l2, L_l2 + L_l3, L_l2 + L_l3 + L_l4 };
            double[] y_l = new double[4] { T_l1, T_l2, T_l3, T_l4 };
            double a_l, b_l, maxErr_l;
            if (CalcRegress(x_l, y_l, 4, out a_l, out b_l, out maxErr_l) != 0)
            {
                MessageBox.Show("计算出错！");
                return;
            }
            else
            {
               // MessageBox.Show("Y_l = " + b_l.ToString() + " X_l +" + a_l.ToString() + "   maxErr_l=" + maxErr_l.ToString());
                q_l = k2 * Math.Abs(b_l);
            }

            //以下代码计算“试件”导热系数 k_s ,其中热流密度q_s ，温度为T_s1, T_s2, T_s3，热电偶位置为L_s1，L_s2，L_s3 ,线性拟合方程为Y_s = b_s * X_s + a_s
            q_s = (q_u + q_l) * 0.5;
            double[] x_s = new double[3] { 0, L_s1, L_s1 + L_s2 };
            double[] y_s = new double[3] { T_s1, T_s2, T_s3 };
            double a_s, b_s, maxErr_s, k_s;
            if (CalcRegress(x_s, y_s, 3, out a_s, out b_s, out maxErr_s) != 0)
            {
                MessageBox.Show("计算出错！");
                return;
            }
            else
            {
                //MessageBox.Show("Y_s = " + b_s.ToString() + " X_s +" + a_s.ToString() + "   maxErr_s=" + maxErr_s.ToString());
                k_s = q_s / Math.Abs(b_s);  //求得“试件”导热系数 k_s ，单位为w/(m*k)
               // MessageBox.Show("k_s = " + k_s.ToString() + "w/(m*k)");
            }

            //以下代码为更新监视温度  T_u1, T_u2, T_u3, T_u4, T_s1, T_s2, T_s3, T_l1, T_l2, T_l3, T_l4 
            T_TextBox1_1.Text = T_u1.ToString();
            T_TextBox1_2.Text = T_u2.ToString();
            T_TextBox1_3.Text = T_u3.ToString();
            T_TextBox1_4.Text = T_u4.ToString();
            T_TextBox1_5.Text = T_s1.ToString();
            T_TextBox1_6.Text = T_s2.ToString();
            T_TextBox1_7.Text = T_s3.ToString();
            T_TextBox1_8.Text = T_l1.ToString();
            T_TextBox1_9.Text = T_l2.ToString();
            T_TextBox1_10.Text = T_l3.ToString();
            T_TextBox1_11.Text = T_l4.ToString();
            #endregion
        }

        public void Test2Run()
        {
            #region //固_固接触热阻测量
            double T_u1, T_u2, T_u3, T_u4, T_su1, T_su2, T_su3, T_sl1, T_sl2, T_sl3, T_l1, T_l2, T_l3, T_l4;                                  //定义热电偶温度
            double k1 = double.Parse(K1TextBox2_1.Text); double k2 = double.Parse(K2TextBox2_2.Text);     //定义“流量计”热导率，单位w/((m*k)
            double ks1, ks2 ;                                                                                //定义“试件”热导率，单位w/((m*k)
            double q_u, q_s, q_l;                                                                                     //定义“流量计”“试件”热流密度
            T_u1 = 80; T_u2 = 70; T_u3 = 60; T_u4 = 50;
            T_l1 = 30; T_l2 = 20; T_l3 = 10; T_l4 = 0;
            T_su1 = 45; T_su2 = 44; T_su3 = 43;
            T_sl1 = 40; T_sl2 = 37; T_sl3 = 35;

            //以下代码为读取热电偶位置值，单位mm，转换为m
            double L_u1 = 0.001 * double.Parse(LengthTextBox2_1.Text);
            double L_u2 = 0.001 * double.Parse(LengthTextBox2_2.Text);
            double L_u3 = 0.001 * double.Parse(LengthTextBox2_3.Text);
            double L_u4 = 0.001 * double.Parse(LengthTextBox2_4.Text);
            double L_su1 = 0.001 * double.Parse(LengthTextBox2_5.Text);
            double L_su2 = 0.001 * double.Parse(LengthTextBox2_6.Text);
            double L_su3 = 0.001 * double.Parse(LengthTextBox2_7.Text);
            double L_sl1 = 0.001 * double.Parse(LengthTextBox2_8.Text);
            double L_sl2 = 0.001 * double.Parse(LengthTextBox2_9.Text);
            double L_sl3 = 0.001 * double.Parse(LengthTextBox2_10.Text);
            double L_l1 = 0.001 * double.Parse(LengthTextBox2_11.Text);
            double L_l2 = 0.001 * double.Parse(LengthTextBox2_12.Text);
            double L_l3 = 0.001 * double.Parse(LengthTextBox2_13.Text);
            double L_l4 = 0.001 * double.Parse(LengthTextBox2_14.Text);

            //以下代码计算“上流量计”热流密度q_u ，热导率为 k1 ,温度为T_u1, T_u2, T_u3, T_u4 ，热电偶位置为L_u1，L_u2，L_u3，L_u4 ,线性拟合方程为Y_u = b_u * X_u + a_u
            double[] x_u = new double[4] { 0, L_u1, L_u1 + L_u2, L_u1 + L_u2 + L_u3 };
            double[] y_u = new double[4] { T_u1, T_u2, T_u3, T_u4 };
            double a_u, b_u, maxErr_u;
            if (CalcRegress(x_u, y_u, 4, out a_u, out b_u, out maxErr_u) != 0)
            {
                MessageBox.Show("计算出错！");
                return;
            }
            else
            {
                //MessageBox.Show("Y_u = " + b_u.ToString() + " X_u +" + a_u.ToString() + "   maxErr_u=" + maxErr_u.ToString());
                q_u = k1 * Math.Abs(b_u);

            }

            //以下代码计算“下流量计”热流密度q_l ，热导率为 k2 ,温度为T_l1, T_l2, T_l3, T_l4 ，热电偶位置为L_l1，L_l2，L_l3，L_l4 ,线性拟合方程为Y_l = b_l * X_l + a_l
            double[] x_l = new double[4] { 0, L_l2, L_l2 + L_l3, L_l2 + L_l3 + L_l4 };
            double[] y_l = new double[4] { T_l1, T_l2, T_l3, T_l4 };
            double a_l, b_l, maxErr_l;
            if (CalcRegress(x_l, y_l, 4, out a_l, out b_l, out maxErr_l) != 0)
            {
                MessageBox.Show("计算出错！");
                return;
            }
            else
            {
                //MessageBox.Show("Y_l = " + b_l.ToString() + " X_l +" + a_l.ToString() + "   maxErr_l=" + maxErr_l.ToString());
                q_l = k2 * Math.Abs(b_l);
            }

            //以下代码计算“试件1”接触面温度contactT_u，导热系数 ks1 ,其中热流密度 q_s ，温度为T_su1, T_su2, T_su3 ，热电偶位置为L_su1，L_su2，L_su3 ,线性拟合方程为Y_su = b_su * X_su + a_su
            q_s = (q_u + q_l) * 0.5;
            double[] x_su = new double[3] { 0, L_su1, L_su1 + L_su2 };
            double[] y_su = new double[3] { T_su1, T_su2, T_su3 };
            double a_su, b_su, maxErr_su, contactT_u;
            if (CalcRegress(x_su, y_su, 3, out a_su, out b_su, out maxErr_su) != 0)
            {
                MessageBox.Show("计算出错！");
                return;
            }
            else
            {
                //MessageBox.Show("Y_su = " + b_su.ToString() + " X_su +" + a_su.ToString() + "   maxErr_su=" + maxErr_su.ToString());
                contactT_u = b_su * (L_su1 + L_su2 + L_su3) + a_su;
                ks1 = q_s / Math.Abs(b_su);            //计算“试件1”热导率，单位w/((m*k)
                Ks1TextBox2_3.Text = ks1.ToString();
            }

            //以下代码计算“试件2”接触面温度contactT_l，导热系数 ks2 ,其中热流密度 q_s ，温度为T_sl1, T_sl2, T_sl3 ，热电偶位置为L_sl1，L_sl2，L_sl3 ,线性拟合方程为Y_sl = b_sl * X_sl + a_sl
            double[] x_sl = new double[3] { L_sl1, L_sl1 + L_sl2, L_sl1 + L_sl2 + L_sl3 };
            double[] y_sl = new double[3] { T_sl1, T_sl2, T_sl3 };
            double a_sl, b_sl, maxErr_sl, contactT_l;
            if (CalcRegress(x_sl, y_sl, 3, out a_sl, out b_sl, out maxErr_sl) != 0)
            {
                MessageBox.Show("计算出错！");
                return;
            }
            else
            {
                //MessageBox.Show("Y_sl = " + b_sl.ToString() + " X_sl +" + a_sl.ToString() + "   maxErr_sl=" + maxErr_sl.ToString());
                contactT_l = a_sl;
                ks2 = q_s / Math.Abs(b_sl);             //计算“试件2”热导率，单位w/((m*k)
                Ks2TextBox2_4.Text = ks2.ToString();
            }

            double contactR = Math.Abs(contactT_u - contactT_l) / q_s;                                                   //代码计算试件1、2的接触热阻，单位(m^2 * K)/W
            //MessageBox.Show(contactR.ToString() + "(m^2 * K)/W");

            //以下代码为更新监视温度 T_u1, T_u2, T_u3, T_u4, T_su1, T_su2, T_su3, T_sl1, T_sl2, T_sl3, T_l1, T_l2, T_l3, T_l4
            T_TextBox2_1.Text = T_u1.ToString();
            T_TextBox2_2.Text = T_u2.ToString();
            T_TextBox2_3.Text = T_u3.ToString();
            T_TextBox2_4.Text = T_u4.ToString();
            T_TextBox2_5.Text = T_su1.ToString();
            T_TextBox2_6.Text = T_su2.ToString();
            T_TextBox2_7.Text = T_su3.ToString();
            T_TextBox2_8.Text = T_sl1.ToString();
            T_TextBox2_9.Text = T_sl2.ToString();
            T_TextBox2_10.Text = T_sl3.ToString();
            T_TextBox2_11.Text = T_l1.ToString();
            T_TextBox2_12.Text = T_l2.ToString();
            T_TextBox2_13.Text = T_l3.ToString();
            T_TextBox2_14.Text = T_l4.ToString();


            double[] Tchart_x = new double[6] { 0, L_su1, L_su1 + L_su2,
                                                L_su1 + L_su2 + L_su3 + L_sl1,
                                                L_su1 + L_su2 + L_su3 + L_sl1 + L_sl2,
                                                L_su1 + L_su2 + L_su3 + L_sl1 + L_sl2 + L_sl3 };

            double[] Tchart_y = new double[6] { T_su1, T_su2, T_su3, T_sl1, T_sl2, T_sl3 };
            Test1ViewPoint(Tchart_x, Tchart_y, 6); //绘制温度点

            Test1ViewLine(0, L_su1 + L_su2 + L_su3, a_su, b_su,
                          L_su1 + L_su2 + L_su3, Tchart_x[5], a_sl, b_sl); //绘制拟合曲线  Y_sl = b_sl * X_sl + a_sl
            #endregion
        }
        public void Test3Run()
        {
            #region //热界面材料测量_热流计间
            double T_u1, T_u2, T_u3, T_u4, T_l1, T_l2, T_l3, T_l4;                                  //定义热电偶温度
            double k1 = double.Parse(K1TextBox3_1.Text); double k2 = double.Parse(K2TextBox3_1.Text);     //定义“流量计”热导率，单位w/((m*k)
            double q_u, q_s, q_l;                                                                                     //定义“流量计”“试件”热流密度
            double Thickness_s = 0.000001 * double.Parse(FilmThickness1.Text);       //定义热界面材料厚度，单位um，转换为m
            T_u1 = 80; T_u2 = 70; T_u3 = 60; T_u4 = 50;
            T_l1 = 30; T_l2 = 20; T_l3 = 10; T_l4 = 0;

            //以下代码为读取热电偶位置值，单位mm，转换为m
            double L_u1 = 0.001 * double.Parse(LengthTextBox3_1.Text);
            double L_u2 = 0.001 * double.Parse(LengthTextBox3_2.Text);
            double L_u3 = 0.001 * double.Parse(LengthTextBox3_3.Text);
            double L_u4 = 0.001 * double.Parse(LengthTextBox3_4.Text);
            double L_l1 = 0.001 * double.Parse(LengthTextBox3_5.Text);
            double L_l2 = 0.001 * double.Parse(LengthTextBox3_6.Text);
            double L_l3 = 0.001 * double.Parse(LengthTextBox3_7.Text);
            double L_l4 = 0.001 * double.Parse(LengthTextBox3_8.Text);

            //以下代码计算“上流量计”热流密度q_u ，“热界面材料”上部接触温度为contactT_u，热导率为 k1 ,温度为T_u1, T_u2, T_u3, T_u4 ，热电偶位置为L_u1，L_u2，L_u3，L_u4 ,线性拟合方程为Y_u = b_u * X_u + a_u
            double[] x_u = new double[4] { 0, L_u1, L_u1 + L_u2, L_u1 + L_u2 + L_u3 };
            double[] y_u = new double[4] { T_u1, T_u2, T_u3, T_u4 };
            double a_u, b_u, maxErr_u, contactT_u;
            if (CalcRegress(x_u, y_u, 4, out a_u, out b_u, out maxErr_u) != 0)
            {
                MessageBox.Show("计算出错！");
                return;
            }
            else
            {
                //MessageBox.Show("Y_u = " + b_u.ToString() + " X_u +" + a_u.ToString() + "   maxErr_u=" + maxErr_u.ToString());
                q_u = k1 * Math.Abs(b_u);
                contactT_u = b_u * (L_u1 + L_u2 + L_u3 + L_u4) + a_u;

            }

            //以下代码计算“下流量计”热流密度q_l ，“热界面材料”下部接触温度为contactT_l，热导率为 k2 ,温度为T_l1, T_l2, T_l3, T_l4 ，热电偶位置为L_l1，L_l2，L_l3，L_l4 ,线性拟合方程为Y_l = b_l * X_l + a_l
            double[] x_l = new double[4] { L_l1, L_l1 + L_l2, L_l1 + L_l2 + L_l3, L_l1 + L_l2 + L_l3 + L_l4 };
            double[] y_l = new double[4] { T_l1, T_l2, T_l3, T_l4 };
            double a_l, b_l, maxErr_l, contactT_l;
            if (CalcRegress(x_l, y_l, 4, out a_l, out b_l, out maxErr_l) != 0)
            {
                MessageBox.Show("计算出错！");
                return;
            }
            else
            {
                //MessageBox.Show("Y_l = " + b_l.ToString() + " X_l +" + a_l.ToString() + "   maxErr_l=" + maxErr_l.ToString());
                q_l = k2 * Math.Abs(b_l);
                contactT_l = a_l;
            }

            //以下代码计算“热界面材料”热阻contactR和导热系数 k_s ,其中热流密度q_s
            q_s = (q_u + q_l) * 0.5;
            double contactR = Math.Abs(contactT_u - contactT_l) / q_s;
            double k_s = (Thickness_s * q_s) / Math.Abs(contactT_u - contactT_l);
            //MessageBox.Show("接触热阻=" + contactR.ToString() + "(m^2)K/W  \n" + "导热系数=" + k_s.ToString() + "W/(m*K)");

            //以下代码为更新监视温度  T_u1, T_u2, T_u3, T_u4, T_l1, T_l2, T_l3, T_l4
            T_TextBox3_1.Text = T_u1.ToString();
            T_TextBox3_2.Text = T_u2.ToString();
            T_TextBox3_3.Text = T_u3.ToString();
            T_TextBox3_4.Text = T_u4.ToString();
            T_TextBox3_5.Text = T_l1.ToString();
            T_TextBox3_6.Text = T_l2.ToString();
            T_TextBox3_7.Text = T_l3.ToString();
            T_TextBox3_8.Text = T_l4.ToString();

            double[] Tchart_x = new double[8] { 0, L_u1, L_u1 + L_u2, L_u1 + L_u2 + L_u3, 
                                               L_u1 + L_u2 + L_u3 + L_u4 + Thickness_s + L_l1,
                                               L_u1 + L_u2 + L_u3 + L_u4 + Thickness_s + L_l1 + L_l2, 
                                               L_u1 + L_u2 + L_u3 + L_u4 + Thickness_s + L_l1 + L_l2 + L_l3, 
                                               L_u1 + L_u2 + L_u3 + L_u4 + Thickness_s + L_l1 + L_l2 + L_l3 + L_l4 };

            double[] Tchart_y = new double[8] { T_u1, T_u2, T_u3, T_u4, T_l1, T_l2, T_l3, T_l4 };
            Test1ViewPoint(Tchart_x, Tchart_y, 8); //绘制温度点

            Test1ViewLine(0, L_u1 + L_u2 + L_u3 + L_u4, a_u, b_u,
                                  L_u1 + L_u2 + L_u3 + L_u4 + Thickness_s, Tchart_x[7], a_l, b_l); //绘制拟合曲线  Y_sl = b_sl * X_sl + a_sl

            #endregion
        }
        public void Test4Run()
        {
            #region //热界面材料测量_试件间
            double T_u1, T_u2, T_u3, T_u4, T_su1, T_su2, T_su3, T_sl1, T_sl2, T_sl3, T_l1, T_l2, T_l3, T_l4;                                  //定义热电偶温度
            double k1 = double.Parse(K1TextBox4_1.Text); double k2 = double.Parse(K4TextBox4_4.Text);     //定义“流量计”热导率，单位w/((m*k)
            double ks1 ,ks2 ;                                                                            //定义“试件”热导率，单位w/((m*k)
            double q_u, q_s, q_l;                                                                                     //定义“流量计”“试件”热流密度
            double Thickness_s = 0.000001 * double.Parse(FilmThickness2.Text);       //定义热界面材料厚度，单位um，转换为m
            T_u1 = 80; T_u2 = 70; T_u3 = 60; T_u4 = 50;
            T_l1 = 30; T_l2 = 20; T_l3 = 10; T_l4 = 0;
            T_su1 = 45; T_su2 = 44; T_su3 = 43;
            T_sl1 = 38; T_sl2 = 37; T_sl3 = 35;

            //以下代码为读取热电偶位置值，单位mm，转换为m
            double L_u1 = 0.001 * double.Parse(LengthTextBox4_1.Text);
            double L_u2 = 0.001 * double.Parse(LengthTextBox4_2.Text);
            double L_u3 = 0.001 * double.Parse(LengthTextBox4_3.Text);
            double L_u4 = 0.001 * double.Parse(LengthTextBox4_4.Text);
            double L_su1 = 0.001 * double.Parse(LengthTextBox4_5.Text);
            double L_su2 = 0.001 * double.Parse(LengthTextBox4_6.Text);
            double L_su3 = 0.001 * double.Parse(LengthTextBox4_7.Text);
            double L_sl1 = 0.001 * double.Parse(LengthTextBox4_8.Text);
            double L_sl2 = 0.001 * double.Parse(LengthTextBox4_9.Text);
            double L_sl3 = 0.001 * double.Parse(LengthTextBox4_10.Text);
            double L_l1 = 0.001 * double.Parse(LengthTextBox4_11.Text);
            double L_l2 = 0.001 * double.Parse(LengthTextBox4_12.Text);
            double L_l3 = 0.001 * double.Parse(LengthTextBox4_13.Text);
            double L_l4 = 0.001 * double.Parse(LengthTextBox4_14.Text);

            //以下代码计算“上流量计”热流密度q_u ，热导率为 k1 ,温度为T_u1, T_u2, T_u3, T_u4 ，热电偶位置为L_u1，L_u2，L_u3，L_u4 ,线性拟合方程为Y_u = b_u * X_u + a_u
            double[] x_u = new double[4] { 0, L_u1, L_u1 + L_u2, L_u1 + L_u2 + L_u3 };
            double[] y_u = new double[4] { T_u1, T_u2, T_u3, T_u4 };
            double a_u, b_u, maxErr_u;
            if (CalcRegress(x_u, y_u, 4, out a_u, out b_u, out maxErr_u) != 0)
            {
                MessageBox.Show("计算出错！");
                return;
            }
            else
            {
                //MessageBox.Show("Y_u = " + b_u.ToString() + " X_u +" + a_u.ToString() + "   maxErr_u=" + maxErr_u.ToString());
                q_u = k1 * Math.Abs(b_u);

            }

            //以下代码计算“下流量计”热流密度q_l ，热导率为 k2 ,温度为T_l1, T_l2, T_l3, T_l4 ，热电偶位置为L_l1，L_l2，L_l3，L_l4 ,线性拟合方程为Y_l = b_l * X_l + a_l
            double[] x_l = new double[4] { 0, L_l2, L_l2 + L_l3, L_l2 + L_l3 + L_l4 };
            double[] y_l = new double[4] { T_l1, T_l2, T_l3, T_l4 };
            double a_l, b_l, maxErr_l;
            if (CalcRegress(x_l, y_l, 4, out a_l, out b_l, out maxErr_l) != 0)
            {
                MessageBox.Show("计算出错！");
                return;
            }
            else
            {
                //MessageBox.Show("Y_l = " + b_l.ToString() + " X_l +" + a_l.ToString() + "   maxErr_l=" + maxErr_l.ToString());
                q_l = k2 * Math.Abs(b_l);
            }

            //以下代码计算“试件1”接触面温度contactT_u，导热系数 ks1 ,其中热流密度q_s ，温度为T_su1, T_su2, T_su3 ，热电偶位置为L_su1，L_su2，L_su3 ,线性拟合方程为Y_su = b_su * X_su + a_su
            q_s = (q_u + q_l) * 0.5;
            double[] x_su = new double[3] { 0, L_su1, L_su1 + L_su2 };
            double[] y_su = new double[3] { T_su1, T_su2, T_su3 };
            double a_su, b_su, maxErr_su, contactT_u;
            if (CalcRegress(x_su, y_su, 3, out a_su, out b_su, out maxErr_su) != 0)
            {
                MessageBox.Show("计算出错！");
                return;
            }
            else
            {
                //MessageBox.Show("Y_su = " + b_su.ToString() + " X_su +" + a_su.ToString() + "   maxErr_su=" + maxErr_su.ToString());
                contactT_u = b_su * (L_su1 + L_su2 + L_su3) + a_su;
                ks1 = q_s / Math.Abs(b_su);                                  //计算“试件1”热导率，单位w/((m*k)
                Ks1TextBox4_2.Text = ks1.ToString();
            }

            //以下代码计算“试件2”接触面温度contactT_l，导热系数 ks2 ,其中热流密度q_s ，温度为T_sl1, T_sl2, T_sl3 ，热电偶位置为L_sl1，L_sl2，L_sl3 ,线性拟合方程为Y_sl = b_sl * X_sl + a_sl
            double[] x_sl = new double[3] { L_sl1, L_sl1 + L_sl2, L_sl1 + L_sl2 + L_sl3 };
            double[] y_sl = new double[3] { T_sl1, T_sl2, T_sl3 };
            double a_sl, b_sl, maxErr_sl, contactT_l;
            if (CalcRegress(x_sl, y_sl, 3, out a_sl, out b_sl, out maxErr_sl) != 0)
            {
                MessageBox.Show("计算出错！");
                return;
            }
            else
            {
                //MessageBox.Show("Y_sl = " + b_sl.ToString() + " X_sl +" + a_sl.ToString() + "   maxErr_sl=" + maxErr_sl.ToString());
                contactT_l = a_sl;
                ks2 = q_s / Math.Abs(b_sl);                                  //计算“试件2”热导率，单位w/((m*k)
                Ks2TextBox4_3.Text = ks2.ToString();      
            }

            //以下代码计算“热界面材料_试件间”热阻contactR和导热系数 k_s ,其中热流密度q_s 
            double contactR = Math.Abs(contactT_u - contactT_l) / q_s;                                                   //计算试件1、2的接触热阻，单位(m^2*K)/W 
            double k_s = (Thickness_s * q_s) / (Math.Abs(contactT_u - contactT_l));                     //计算热界面材料导热系数，单位W/(m*K)
            //MessageBox.Show("接触热阻=" + contactR.ToString() + "(m^2*K)/W  \n" + "导热系数=" + k_s.ToString() + "W/(m*K)");

            //以下代码为更新监视温度 T_u1, T_u2, T_u3, T_u4, T_su1, T_su2, T_su3, T_sl1, T_sl2, T_sl3, T_l1, T_l2, T_l3, T_l4
            T_TextBox4_1.Text = T_u1.ToString();
            T_TextBox4_2.Text = T_u2.ToString();
            T_TextBox4_3.Text = T_u3.ToString();
            T_TextBox4_4.Text = T_u4.ToString();
            T_TextBox4_5.Text = T_su1.ToString();
            T_TextBox4_6.Text = T_su2.ToString();
            T_TextBox4_7.Text = T_su3.ToString();
            T_TextBox4_8.Text = T_sl1.ToString();
            T_TextBox4_9.Text = T_sl2.ToString();
            T_TextBox4_10.Text = T_sl3.ToString();
            T_TextBox4_11.Text = T_l1.ToString();
            T_TextBox4_12.Text = T_l2.ToString();
            T_TextBox4_13.Text = T_l3.ToString();
            T_TextBox4_14.Text = T_l4.ToString();

            double[] Tchart_x = new double[6] { 0, L_su1, L_su1 + L_su2, 
                                                L_su1 + L_su2 + L_su3 + Thickness_s + L_sl1,
                                                L_su1 + L_su2 + L_su3 + Thickness_s + L_sl1 + L_sl2, 
                                                L_su1 + L_su2 + L_su3 + Thickness_s + L_sl1 + L_sl2 + L_sl3 };

            double[] Tchart_y = new double[6] { T_su1, T_su2, T_su3, T_sl1, T_sl2, T_sl3 };
            Test1ViewPoint(Tchart_x, Tchart_y, 6); //绘制温度点

            Test1ViewLine(0, L_su1 + L_su2 + L_su3, a_su, b_su,
                          L_su1 + L_su2 + L_su3 + Thickness_s, Tchart_x[5], a_sl, b_sl); //绘制拟合曲线  Y_sl = b_sl * X_sl + a_sl
            #endregion
        }
        private int CalcRegress(double[] x, double[] y, int dataCnt, out double a, out double b, out double maxErr) //最小二乘法线性回归
        {
            #region //拟合函数
            double sumX = 0;
            double sum_y = 0;
            double avgX;
            double avg_y;

            if (dataCnt < 2)
            {
                a = 0;
                b = 0;
                maxErr = 0;
                return -1;
            }

            for (int i = 0; i < dataCnt; i++)
            {
                sumX += x[i];
                sum_y += y[i];
            }

            avgX = sumX / dataCnt;
            avg_y = sum_y / dataCnt;

            double SPxy = 0;
            double SSx = 0;

            for (int i = 0; i < dataCnt; i++)
            {
                SPxy += (x[i] - avgX) * (y[i] - avg_y);
                SSx += (x[i] - avgX) * (x[i] - avgX);
            }

            if (SSx == 0)
            {
                a = 0;
                b = 0;
                maxErr = 0;
                return -1;
            }
            b = SPxy / SSx;
            a = avg_y - b * avgX;
            //下面代码计算最大偏差            
            maxErr = 0;
            for (int i = 0; i < dataCnt; i++)
            {
                double yi = a + b * x[i];
                double absErrYi = Math.Abs(yi - y[i]);

                if (absErrYi > maxErr)
                {
                    maxErr = absErrYi;
                }
            }
            return 0;

            /*拟合函数测试
              double[] x = new double[2]{0,1};
              double[] y = new double[2]{1,3};
              double a1, b1, maxErr;
              int dataCnt = 2;
              if (CalcRegress(x, y, dataCnt, out a1, out b1, out maxErr) != 0)
              {
                  MessageBox.Show("计算出错！");
                  return;
              }
              else
              {
                  MessageBox.Show("y=" + b1.ToString() + "x+" + a1.ToString() + "   maxErr=" + maxErr.ToString());
              }
              */
            #endregion
        }

        private int Test1ViewPoint(double[] x, double[] y, int dataCnt) //绘制温度离散点
        {
            #region  //绘制温度离散点
            Series T_point = chart1.Series[0];
            T_point.Points.Clear();  
            for (int i = 0; i < dataCnt; i++)
            {
                T_point.Points.AddXY(x[i], y[i]);
            }
            return 0;
            #endregion
        }

        private int Test1ViewLine(double x1_1, double x1_2, double a1, double b1,
                                  double x2_1, double x2_2, double a2, double b2) //绘制拟合曲线  Y_sl = b_sl * X_sl + a_sl
        {
            #region  //绘制拟合曲线  Y_sl = b_sl * X_sl + a_sl
            Series T_fittingLine = chart1.Series[1];
            T_fittingLine.Points.Clear();
            for (double i = x1_1 - 0.01; i < x1_2; )
            {
                T_fittingLine.Points.AddXY(i, (b1 * i + a1));
                i += 0.002;
            }
            T_fittingLine.Points.AddXY(x1_2, (b1 * x1_2 + a1));
            for (double i = x2_1; i < x2_2 + 0.01; )
            {
                T_fittingLine.Points.AddXY(i, (b2 * (i - x2_1) + a2));
                i += 0.002;
            }

            // chart1.ChartAreas[0].AxisX.Maximum = x2_2 + 0.02;
            //chart1.ChartAreas[0].AxisX.Interval = (x2_2 + 0.02)/10;
            return 0;
            #endregion
        }

        private void DelayTimer_Tick(object sender, EventArgs e)   //定时器倒计时
        {
            #region  //时钟倒计时
            FileGroupBox.Size = new Size(0, 0);   //隐藏“文件”按钮子目录
            DelayTimer.Enabled = false;
            #endregion
        }

        private double StableTemp(double [] temp, double stableDif, double stableTemp)  //判断温度是否稳定，并求出稳定温度 
        {
            #region  //判断温度是否稳定，并求出稳定温度
            if (Math.Abs(temp.Max() - temp.Min()) < stableDif)
                stableTemp = temp.Average();
            else 
                return -1;
            return stableTemp;
            #endregion
        }

        private int FactorStandView(int chn ,double[] k1, double[] k2)    //显示factordataGridView数据
        {
            #region  //显示factordataGridView数据
            factordataGridView.Rows.Clear();
            for (int i = 0; i < chn; i++)  
            { 
                factordataGridView.Rows.Add(i.ToString(), k1[i].ToString(), k2[i].ToString());
            }
            return 0;
            #endregion
        }
        private int FactorStandUpdate(int chn, double[] k1, double[] k2)  //更新factordataGridView数据
        {
            #region  //更新factordataGridView数据
            for (int i = 0; i < chn; i++)   
            {
                k1[chn] = double.Parse(factordataGridView.Rows[chn].Cells[1].ToolTipText);
                k2[chn] = double.Parse(factordataGridView.Rows[chn].Cells[2].ToolTipText);
            }
            return 0;
            #endregion
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        private void button1_Click(object sender, EventArgs e)
        {
            #region  //选择测试方法1
            TestChoose1.ForeColor = Color.LightSkyBlue;
            TestChoose2.ForeColor = Color.White;
            TestChoose3.ForeColor = Color.White;
            TestChoose4.ForeColor = Color.White;
            TestChoose = "Test1";
            #endregion

        }
        private void TestChoose2_Click(object sender, EventArgs e)
        {
            #region  //选择测试方法2
            TestChoose1.ForeColor = Color.White;
            TestChoose2.ForeColor = Color.LightSkyBlue;
            TestChoose3.ForeColor = Color.White;
            TestChoose4.ForeColor = Color.White;
            TestChoose = "Test2";
            #endregion
        }

        private void TestChoose3_Click(object sender, EventArgs e)
        {
            #region  //选择测试方法3
            TestChoose1.ForeColor = Color.White;
            TestChoose2.ForeColor = Color.White;
            TestChoose3.ForeColor = Color.LightSkyBlue;
            TestChoose4.ForeColor = Color.White;
            TestChoose = "Test3";
            #endregion
        }

        private void TestChoose4_Click(object sender, EventArgs e)
        {
            #region  //选择测试方法4
            TestChoose1.ForeColor = Color.White;
            TestChoose2.ForeColor = Color.White;
            TestChoose3.ForeColor = Color.White;
            TestChoose4.ForeColor = Color.LightSkyBlue;
            TestChoose = "Test4";
            #endregion
        }
        private void SerialPort_Click(object sender, EventArgs e)
        {
            #region //串口设置
            NoneGroupBox.Size = new Size(0, 0);
            TextGroupbox1.Size = new Size(0, 0);
            TextGroupbox2.Size = new Size(0, 0);
            TextGroupbox3.Size = new Size(0, 0);
            TextGroupbox4.Size = new Size(0, 0);
            ViewGroupBox1.Size = new Size(0, 0);
            ViewGroupBox2.Size = new Size(0, 0);
            ViewGroupBox3.Size = new Size(0, 0);
            ViewGroupBox4.Size = new Size(0, 0);
            ResultGroupBox.Size = new Size(0, 0);
            skinGroupBox1.Size = new Size(263, 218);  //普通模式下窗口尺寸
            skinGroupBox2.Size = new Size(911, 966);
            #endregion
        }
        private void ParSetting_Click(object sender, EventArgs e)
        {
            #region   //参数设置窗口打开
            switch (TestChoose)
            {
                case "Test1":
                    {
                        //显示对应设置窗口TEST1
                        NoneGroupBox.Size = new Size(0, 0);
                        TextGroupbox1.Size = new Size(1531, 966);
                        TextGroupbox2.Size = new Size(0, 0);
                        TextGroupbox3.Size = new Size(0, 0);
                        TextGroupbox4.Size = new Size(0, 0);
                        ViewGroupBox1.Size = new Size(0, 0);
                        ViewGroupBox2.Size = new Size(0, 0);
                        ViewGroupBox3.Size = new Size(0, 0);
                        ViewGroupBox4.Size = new Size(0, 0);
                        chart1.Size = new Size(0, 0);
                        ResultGroupBox.Size = new Size(0, 0);
                        skinGroupBox2.Size = new Size(0, 0);
                    }
                    break;
                case "Test2":
                    {
                        //显示对应设置窗口TEST2
                        NoneGroupBox.Size = new Size(0, 0);
                        TextGroupbox1.Size = new Size(0, 0);
                        TextGroupbox2.Size = new Size(1531, 966);
                        TextGroupbox3.Size = new Size(0, 0);
                        TextGroupbox4.Size = new Size(0, 0);
                        ViewGroupBox1.Size = new Size(0, 0);
                        ViewGroupBox2.Size = new Size(0, 0);
                        ViewGroupBox3.Size = new Size(0, 0);
                        ViewGroupBox4.Size = new Size(0, 0);
                        chart1.Size = new Size(0, 0);
                        ResultGroupBox.Size = new Size(0, 0);
                        skinGroupBox2.Size = new Size(0, 0);
                    }
                    break;
                case "Test3":
                    {
                        //显示对应设置窗口TEST3
                        NoneGroupBox.Size = new Size(0, 0);
                        TextGroupbox1.Size = new Size(0, 0);
                        TextGroupbox2.Size = new Size(0, 0);
                        TextGroupbox3.Size = new Size(1531, 966);
                        TextGroupbox4.Size = new Size(0, 0);
                        ViewGroupBox1.Size = new Size(0, 0);
                        ViewGroupBox2.Size = new Size(0, 0);
                        ViewGroupBox3.Size = new Size(0, 0);
                        ViewGroupBox4.Size = new Size(0, 0);
                        chart1.Size = new Size(0, 0);
                        ResultGroupBox.Size = new Size(0, 0);
                        skinGroupBox2.Size = new Size(0, 0);
                    }
                    break;
                case "Test4":
                    {
                        //显示对应设置窗口TEST4
                        NoneGroupBox.Size = new Size(0, 0);
                        TextGroupbox1.Size = new Size(0, 0);
                        TextGroupbox2.Size = new Size(0, 0);
                        TextGroupbox3.Size = new Size(0, 0);
                        TextGroupbox4.Size = new Size(1531, 966);
                        ViewGroupBox1.Size = new Size(0, 0);
                        ViewGroupBox2.Size = new Size(0, 0);
                        ViewGroupBox3.Size = new Size(0, 0);
                        ViewGroupBox4.Size = new Size(0, 0);
                        chart1.Size = new Size(0, 0);
                        ResultGroupBox.Size = new Size(0, 0);
                        skinGroupBox2.Size = new Size(0, 0);
                    }
                    break;
                default:
                    break;

            }
            #endregion
        }

        private void TestRun_Click(object sender, EventArgs e)
        {
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
            Monitor.Enabled = true;
            Monitor.Enabled = true;
            TestResult.Enabled = true;

            switch (TestChoose)
            {
                case "Test1":
                    {
                        Test1Run();
                    }
                    break;
                case "Test2":
                    {
                        Test2Run();
                    }
                    break;
                case "Test3":
                    {
                        Test3Run();
                    }
                    break;
                case "Test4":
                    {
                        Test4Run();
                    }
                    break;
                default:
                    break;
            }
            #endregion
        }

        private void TestStop_Click(object sender, EventArgs e)
        {
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
            Monitor.Enabled = false;
            Monitor.Enabled = false;
            TestResult.Enabled = false;
            #endregion 
        }

        private void Monitor_Click(object sender, EventArgs e)
        {

            #region  //温度监视
            NoneGroupBox.Size = new Size(0, 0);
            TextGroupbox1.Size = new Size(0, 0);
            TextGroupbox2.Size = new Size(0, 0);
            TextGroupbox3.Size = new Size(0, 0);
            TextGroupbox4.Size = new Size(0, 0);
            ViewGroupBox1.Size = new Size(0, 0);
            ViewGroupBox2.Size = new Size(0, 0);
            ViewGroupBox3.Size = new Size(0, 0);
            ViewGroupBox4.Size = new Size(0, 0);
            chart1.Size = new Size(0, 0);  //将曲线表格放在隐藏
            ResultGroupBox.Size = new Size(1531, 966);
            #endregion
        }

        private void TestResult_Click(object sender, EventArgs e)
        {
            #region  //数据结果
            chart1.BringToFront();            //将曲线表格放在最顶层
            chart1.Size = new Size(806,439);
            switch (TestChoose)
            {
                case "Test1":
                    {
                        //显示对应监视窗口TEST1
                        NoneGroupBox.Size = new Size(0, 0);
                        TextGroupbox1.Size = new Size(0, 0);
                        TextGroupbox2.Size = new Size(0, 0);
                        TextGroupbox3.Size = new Size(0, 0);
                        TextGroupbox4.Size = new Size(0, 0);
                        ViewGroupBox1.Size = new Size(1531, 966);
                        ViewGroupBox2.Size = new Size(0, 0);
                        ViewGroupBox3.Size = new Size(0, 0);
                        ViewGroupBox4.Size = new Size(0, 0);
                        chart1.Size = new Size(0, 0);  //将曲线表格放在隐藏
                        ResultGroupBox.Size = new Size(0, 0);
                        skinGroupBox2.Size = new Size(0, 0);
                    }
                    break;
                case "Test2":
                    {
                        //显示对应监视窗口TEST2
                        NoneGroupBox.Size = new Size(0, 0);
                        TextGroupbox1.Size = new Size(0, 0);
                        TextGroupbox2.Size = new Size(0, 0);
                        TextGroupbox3.Size = new Size(0, 0);
                        TextGroupbox4.Size = new Size(0, 0);
                        ViewGroupBox1.Size = new Size(0, 0);
                        ViewGroupBox2.Size = new Size(1531, 966);
                        ViewGroupBox3.Size = new Size(0, 0);
                        ViewGroupBox4.Size = new Size(0, 0);
                        ResultGroupBox.Size = new Size(0, 0);
                        skinGroupBox2.Size = new Size(0, 0);
                    }
                    break;
                case "Test3":
                    {
                        //显示对应监视窗口TEST3
                        NoneGroupBox.Size = new Size(0, 0);
                        TextGroupbox1.Size = new Size(0, 0);
                        TextGroupbox2.Size = new Size(0, 0);
                        TextGroupbox3.Size = new Size(0, 0);
                        TextGroupbox4.Size = new Size(0, 0);
                        ViewGroupBox1.Size = new Size(0, 0);
                        ViewGroupBox2.Size = new Size(0, 0);
                        ViewGroupBox3.Size = new Size(1531, 966);
                        ViewGroupBox4.Size = new Size(0, 0);
                        ResultGroupBox.Size = new Size(0, 0);
                        skinGroupBox2.Size = new Size(0, 0);
                    }
                    break;
                case "Test4":
                    {
                        //显示对应监视窗口TEST4
                        NoneGroupBox.Size = new Size(0, 0);
                        TextGroupbox1.Size = new Size(0, 0);
                        TextGroupbox2.Size = new Size(0, 0);
                        TextGroupbox3.Size = new Size(0, 0);
                        TextGroupbox4.Size = new Size(0, 0);
                        ViewGroupBox1.Size = new Size(0, 0);
                        ViewGroupBox2.Size = new Size(0, 0);
                        ViewGroupBox3.Size = new Size(0, 0);
                        ViewGroupBox4.Size = new Size(1531, 966);
                        ResultGroupBox.Size = new Size(0, 0);
                        skinGroupBox2.Size = new Size(0, 0);
                    }
                    break;
                default:
                    break;

            }
            #endregion
        }

        //---------------------------------------------------------------------------------------串口设置-------------------------------------------------------------------------------------------------
        private void combox_card1_SelectedValueChanged(object sender, EventArgs e)
        {
            #region
            int i = combox_card1.SelectedIndex;
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini");
            INIHelper.Write("Card1", "enable", i.ToString(), filePath);

            AppCfg.devicepara.Card1_enable = i;

            if (AppCfg.devicepara.Card1_enable == 0)
            {
                combox_card1.SelectedIndex = 0;
                listview_card1.Enabled = false;
            }
            else
            {
                combox_card1.SelectedIndex = 1;
                listview_card1.Enabled = true;

            }
            #endregion
        }
        public void ChoiceBox(ListView lst, int box, int combox, MouseEventArgs e)  //把combox放到listview上
        {
            #region
            m_SelectedItem = lst.GetItemAt(e.X, e.Y);//先判断是否有选中行
            if (m_SelectedItem != null)
            {
                //获取选中行的Bounds
                System.Drawing.Rectangle Rect = m_SelectedItem.Bounds;

                int LX5 = 0;
                for (int i = 0; i <= box; i++)
                {
                    LX5 += lst.Columns[i].Width;
                }
                int RX5 = LX5 + lst.Columns[box + 1].Width;

                //修改Rect的范围使其与第二列的单元格的大小相同，为了好看 ，左边缩进了2个单位
                Rect.X += lst.Left + LX5 + 2;
                Rect.Y += lst.Top + 2;
                Rect.Width = lst.Columns[box + 1].Width + 2;

                if (box == 1)
                {
                    if (combox == 1)
                    {
                        this.combox_func.SelectedIndex = -1;
                        this.combox_func.Bounds = Rect;
                        this.combox_func.Text = "";
                        this.combox_func.Visible = true;
                        this.combox_func.BringToFront();
                        this.combox_func.Focus();
                        this.combox_func.DroppedDown = true;
                        return;
                    }
                    if (combox == 2)
                    {
                        this.combox_func2.SelectedIndex = -1;
                        this.combox_func2.Bounds = Rect;
                        this.combox_func2.Text = "";
                        this.combox_func2.Visible = true;
                        this.combox_func2.BringToFront();
                        this.combox_func2.Focus();
                        this.combox_func2.DroppedDown = true;
                        return;
                    }
                }
            }
            #endregion
        }

        private void listview_card1_MouseUp(object sender, MouseEventArgs e)
        {
            #region
            try
            {
                m_SelectedItem = this.listview_card1.GetItemAt(e.X, e.Y);//先判断是否有选中行
                if (m_SelectedItem != null)
                {
                    int LX = 0;
                    int RX = 0;
                    for (int j = 1; j < 5; j++)
                    {
                        LX = 0;
                        RX = 0;
                        for (int i = 0; i <= j; i++)
                        {
                            LX += listview_card1.Columns[i].Width;
                        }
                        RX = LX + listview_card1.Columns[j + 1].Width;

                        if (e.X < RX && e.X > LX)
                        {
                            ChoiceBox(listview_card1, j, 1, e);

                        }

                    }
                }

            }
            catch
            {
            }
            #endregion
        }

        private void combox_func_DropDownClosed(object sender, EventArgs e)
        {
            combox_func.Visible = false;
        }

        private void combox_func_TextChanged(object sender, EventArgs e)
        {
            #region
            if (combox_func.Text.Length != 0)
            {

                m_SelectedItem.SubItems[2].Text = combox_func.Text;


                int i = combox_func.SelectedIndex;
                if (i == 0)
                {
                    m_SelectedItem.SubItems[2].Text = "";
                    AppCfg.devicepara.Card1_enable = 0;
                }
                else
                {
                    m_SelectedItem.SubItems[2].Text = combox_func.Text;
                    AppCfg.devicepara.Card1_enable = 1;
                }
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini");
                INIHelper.Write(m_SelectedItem.SubItems[0].Text, "func", i.ToString(), filePath);


                foreach (Card j in AppCfg.devicepara.Cardlist2)
                {
                    if (j.CHN == m_SelectedItem.SubItems[0].Text)
                        j.func = i;
                }

            }
            combox_func.Visible = false;
            #endregion
        }

        private void combox_card2_SelectedValueChanged(object sender, EventArgs e)
        {
            #region
            int i = combox_card2.SelectedIndex;
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini");
            INIHelper.Write("Card2", "enable", i.ToString(), filePath);

            AppCfg.devicepara.Card2_enable = i;

            if (AppCfg.devicepara.Card2_enable == 0)
            {
                combox_card2.SelectedIndex = 0;
                listview_card2.Enabled = false;
            }
            else
            {
                combox_card2.SelectedIndex = 1;
                listview_card2.Enabled = true;

            }
            #endregion
        }

        private void listview_card2_MouseUp(object sender, MouseEventArgs e)
        {
            #region
            try
            {
                m_SelectedItem = this.listview_card2.GetItemAt(e.X, e.Y);//先判断是否有选中行
                if (m_SelectedItem != null)
                {
                    int LX = 0;
                    int RX = 0;
                    for (int j = 1; j < 5; j++)
                    {
                        LX = 0;
                        RX = 0;
                        for (int i = 0; i <= j; i++)
                        {
                            LX += listview_card2.Columns[i].Width;
                        }
                        RX = LX + listview_card2.Columns[j + 1].Width;

                        if (e.X < RX && e.X > LX)
                        {
                            ChoiceBox(listview_card2, j, 2, e);

                        }

                    }
                }

            }
            catch
            {
            }
            #endregion
        }

        private void combox_func2_DropDownClosed(object sender, EventArgs e)
        {
            combox_func2.Visible = false;
        }

        private void combox_card2_TextChanged(object sender, EventArgs e)
        {
            #region
            if (combox_func2.Text.Length != 0)
            {

                m_SelectedItem.SubItems[2].Text = combox_func2.Text;


                int i = combox_func2.SelectedIndex;
                if (i == 0)
                {
                    m_SelectedItem.SubItems[2].Text = "";
                    AppCfg.devicepara.Card2_enable = 0;
                }
                else
                {
                    m_SelectedItem.SubItems[2].Text = combox_func2.Text;
                    AppCfg.devicepara.Card2_enable = 1;
                }
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini");
                INIHelper.Write(m_SelectedItem.SubItems[0].Text, "func", i.ToString(), filePath);


                foreach (Card j in AppCfg.devicepara.Cardlist2)
                {
                    if (j.CHN == m_SelectedItem.SubItems[0].Text)
                        j.func = i;
                }

            }
            combox_func2.Visible = false;
            #endregion
        }

        private void combox_comport_SelectedValueChanged(object sender, EventArgs e)
        {
            AppCfg.devicepara.SerialPort = combox_comport.Text;
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini");//在当前程序路径创建
            INIHelper.Write("Serial", "port", combox_comport.Text, filePath);
        }

        private void combox_baudrate_SelectedValueChanged(object sender, EventArgs e)
        {
            AppCfg.devicepara.SerialBaudRate = combox_baudrate.Text;
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini");//在当前程序路径创建
            INIHelper.Write("Serial", "baudrate", combox_baudrate.Text, filePath);
        }

        private void combox_databits_SelectedValueChanged(object sender, EventArgs e)
        {
            AppCfg.devicepara.SerialDataBits = combox_databits.Text;
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini");//在当前程序路径创建
            INIHelper.Write("Serial", "databits", combox_databits.Text, filePath);
        }

        private void combox_stopbits_SelectedValueChanged(object sender, EventArgs e)
        {
            AppCfg.devicepara.SerialStopBits = combox_stopbits.Text;
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini");//在当前程序路径创建
            INIHelper.Write("Serial", "stopbites", combox_stopbits.Text, filePath);
        }

        private void combox_parity_SelectedValueChanged(object sender, EventArgs e)
        {
            AppCfg.devicepara.SerialParity = combox_parity.Text;
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini");//在当前程序路径创建
            INIHelper.Write("Serial", "parity", combox_parity.Text, filePath);
        }

        private void edit_scan_interval_TextChanged(object sender, EventArgs e)
        {
            AppCfg.devicepara.Scan_interval = int.Parse(edit_scan_interval.Text);
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini");//在当前程序路径创建
            INIHelper.Write("SYS", "scan_interval", edit_scan_interval.Text, filePath);
        }

        private void edit_save_interval_TextChanged(object sender, EventArgs e)
        {
            AppCfg.devicepara.Save_interval = int.Parse(edit_save_interval.Text);
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini");//在当前程序路径创建
            INIHelper.Write("SYS", "save_interval", edit_save_interval.Text, filePath);
        }
        //--------------------------------------------------------------------------------------------串口采集--------------------------------------------------------------------------------------------------
        private void CreateDefaultIni()
        {
            #region
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
            #endregion
        }
        private void ReadPara()
        {
            #region  //读取ini
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
            #endregion
        }

        private void btn_start()
        {
            #region  //开始串口采集
            //btn_stop.Enabled = true;
            //btn_start.Enabled = false;
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
                //btn_start.Enabled = true;
                //btn_stop.Enabled = false;
                return;
            }

            string TwoRlist = "";
            int TwoR_num = 0;
            if (AppCfg.devicepara.Card1_enable != 0)
            {
                foreach (Card i in AppCfg.devicepara.Cardlist1)
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
            #endregion
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
            if (num > 0)
            {
                st = st.Replace("*nchannel*", num.ToString());
            }
            st = st.Replace("*allchannel*", TotalCHN);
            return st;
            #endregion
        }

         public void sendmsg(string s1,string s2,string s3,int i1,int i2,int i3)
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
            #endregion

        }

        public  byte[] str2ASCII(String xmlStr)
        {
            return Encoding.Default.GetBytes(xmlStr);
        }

        string recvstr;

        private void btn_stop()
        {
            serialPort1.Close();
            //btn_start.Enabled = true;
            //btn_stop.Enabled = false;
            enablescan = false;
        }


        public void SavetData(string name,ListView listView)
        {
            #region
            if (listView.Items.Count == 0)
            {
               // MessageBox.Show("未找到可导入的数据");
                return;
            }
          
            string FileName = Name  + DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss.ffff")+".csv";

            try
            {
           
                    int size = 1024;
                    int sizeCnt = (int)Math.Ceiling((Double)listView.Items.Count / (Double)2000);
                    StreamWriter write = new StreamWriter(FileName, false, Encoding.Default, size * sizeCnt);
                    write.Write;
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
                MessageBox.Show(ex.ToString());
            }
            #endregion
        }

        internal class AppCfg //全局变量
        {
            public AppCfg()
            {

            }
            internal static ParaInfo devicepara = new ParaInfo();
        }

        private void serialPort1_DataReceived_1(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
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


            if (str.IndexOf((char)19) != -1)
            {
                str = str.Substring(str.IndexOf((char)19), str.Length - str.IndexOf((char)19));
            }

            string nextstr = "";
            if (str.IndexOf((char)13) != -1)
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
            #endregion
        }


        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using CCWin;
using CCWin.SkinControl;

namespace multimeter
{
    public partial class SetupDLG : Skin_VS
    {
        ListViewItem m_SelectedItem;
        public SetupDLG()
        {
            InitializeComponent();
        }

        private void SetupDLG_Load(object sender, EventArgs e)
        {
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
        }



        private void combox_card1_SelectedValueChanged(object sender, EventArgs e)
        {
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
                listview_card1.Enabled=true;

            }

        }


        public void ChoiceBox(ListView lst,  int box, int combox, MouseEventArgs e)  //把combox放到listview上
        {
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
                    if(combox==2)
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

        }

        private void listview_card1_MouseUp(object sender, MouseEventArgs e)
        {
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
                            ChoiceBox(listview_card1, j,1, e);

                        }

                    }
                }

            }
            catch
            {
            }
        }

        private void combox_func_DropDownClosed(object sender, EventArgs e)
        {
            combox_func.Visible = false;
        }

        private void combox_func_TextChanged(object sender, EventArgs e)
        {
            
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
        }

        private void combox_card2_SelectedValueChanged(object sender, EventArgs e)
        {
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
        }


        private void listview_card2_MouseUp(object sender, MouseEventArgs e)
        {
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
                            ChoiceBox(listview_card2, j,2, e);

                        }

                    }
                }

            }
            catch
            {
            }
        }

        private void combox_func2_DropDownClosed(object sender, EventArgs e)
        {
            combox_func2.Visible = false;
        }

        private void combox_func2_TextChanged(object sender, EventArgs e)
        {
            if (combox_func2.Text.Length != 0)
            {

                m_SelectedItem.SubItems[2].Text = combox_func2.Text;
                

                int i = combox_func2.SelectedIndex;
                if(i==0)
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
                        j.func = i ;
                }

            }
            combox_func2.Visible = false;
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

        private void skinGroupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multimeter
{

    public class SendCmd
    {
        /// <summary>
        /// 两线电阻
        /// </summary>
        public string  TwoR { get; set; }  
        /// <summary>
        /// 四线电阻
        /// </summary>
        public string FourR { get; set; }

       /// <summary>
       /// 热电偶
       /// </summary>
        public string Temp { get; set; }

        /// <summary>
        /// 多少路两线电阻
        /// </summary>

        public int TwoR_Num { get; set; }
        /// <summary>
        /// 多少路四线电阻
        /// </summary>
        public int FourR_Num { get; set; }
        /// <summary>
        /// 多少路热电偶
        /// </summary>
        public int Temp_Num { get; set; }

        public SendCmd()
       {
            TwoR = "";
            FourR = "";
            Temp = "";
            TwoR_Num = 0;
            FourR_Num = 0;
            Temp_Num = 0;

        }



    }
    public class Card
    {
   
        /// <summary>
        /// 通道名称 101,102.....201,202等
        /// </summary>
        public string CHN { get; set; }
        /// <summary>
        /// 通道别名，随意取，方便记录
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 通道功能
        /// </summary>
        public int func { get; set; } //0代表无功能，1代表2线电阻，2代表4线电阻，3代表热电偶


        /// <summary>
        /// 通道图形显示
        /// </summary>
        public int chart { get; set; }

        public Card()
        {
            CHN = "000";
            name = "";
            func = 0;
            chart = 0;
     

         }
    }
    public class ParaInfo
    {

        /// <summary>
        /// 串口
        /// </summary>
        public string SerialPort { get; set; }

        /// <summary>
        /// 波特率
        /// </summary>
        public string SerialBaudRate  { get; set; }


        /// <summary>
        /// 数据位
        /// </summary>
        public string SerialDataBits { get; set; }

        /// <summary>
        /// 停止位
        /// </summary>
        public string SerialStopBits { get; set; }
        /// <summary>
        /// 校验位
        /// </summary>
        public string SerialParity { get; set; }

        /// <summary>
        /// 要检测的卡槽1的所有通道
        /// </summary>

        public List<Card> Cardlist1 { get; set; }
        /// <summary>
        /// 要检测的卡槽2所有通道
        /// </summary>

        public List<Card> Cardlist2 { get; set; }

        /// <summary>
        /// 卡槽1是否启用
        /// </summary>
        public int Card1_enable { get; set; }

        /// <summary>
        /// 卡槽2是否启用
        /// </summary>
        public int Card2_enable { get; set; }

        /// <summary>
        /// 保存间隔
        /// </summary>

        public int Save_interval { get; set; }

        /// <summary>
        /// 扫描间隔
        /// </summary>

        public int Scan_interval { get; set; }

        /// <summary>
        /// 收敛后自动关闭间隔(Unit : s)
        /// </summary>

        public int AutoCloseInterval { get; set; }


        public double ConvergentLim { get; set; }

        public ParaInfo()
        {
            SerialPort = "COM7";
            SerialBaudRate = "9600";

            SerialDataBits = "8";

            SerialStopBits = "1";
            SerialParity = "None";
            Card1_enable = 0;
            Card2_enable = 0;
            Save_interval = 50;
            Scan_interval = 2000;
            AutoCloseInterval = 1800;
            ConvergentLim = 1e-3;

            Cardlist1 = new List<Card>();

            for(int i=0;i<22;i++)
            {
                Card C = new Card();
                C.CHN = (i + 101).ToString();
                C.func = 0;
                Cardlist1.Add(C);
            }


            Cardlist2 = new List<Card>();

            for (int i = 0; i < 22; i++)
            {
                Card C = new Card();
                C.CHN = (i + 201).ToString();
                C.func = 0;
                Cardlist2.Add(C);
            }

        }



    }
}

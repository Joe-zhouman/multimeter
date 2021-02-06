using System;
using System.Collections.Generic;
namespace Model {
    public class Card {
        public Card() {
            Chn = "000";
            Func = 0;
            Type = ProbeType.NULL;
        }

        /// <summary>
        ///     通道名称 101,102.....201,202等
        /// </summary>
        public string Chn { get; set; }

        /// <summary>
        ///     通道别名，随意取，方便记录
        /// </summary>

        /// <summary>
        ///     通道功能
        /// </summary>
        public int Func { get; set; } //0代表无功能，1代表2线电阻，2代表4线电阻，3代表热电偶

        public ProbeType Type { get; set; }
    }

    public class SerialPortPara {
        public SerialPortPara() {
            SerialPort = "COM7";
            SerialBaudRate = "9600";

            SerialDataBits = "8";
            
            SerialStopBits = "One";
            SerialParity = "None";
            Card1Enable = 0;
            Card2Enable = 1;

            CardList1 = new List<Card>();

            for (var i = 0; i < 28; i++) {
                var c = new Card {Chn = (i + 101).ToString(), Func = 0};
                CardList1.Add(c);
            }


            CardList2 = new List<Card>();

            for (var i = 0; i < 28; i++) {
                var c = new Card {Chn = (i + 201).ToString(), Func = 0};
                CardList2.Add(c);
            }
        }

        /// <summary>
        ///     串口
        /// </summary>
        public string SerialPort { get; set; }

        /// <summary>
        ///     波特率
        /// </summary>
        public string SerialBaudRate { get; set; }


        /// <summary>
        ///     数据位
        /// </summary>
        public string SerialDataBits { get; set; }

        /// <summary>
        ///     停止位
        /// </summary>
        public string SerialStopBits { get; set; }

        /// <summary>
        ///     校验位
        /// </summary>
        public string SerialParity { get; set; }

        /// <summary>
        ///     要检测的卡槽1的所有通道
        /// </summary>

        public List<Card> CardList1 { get; set; }

        /// <summary>
        ///     要检测的卡槽2所有通道
        /// </summary>

        public List<Card> CardList2 { get; set; }

        /// <summary>
        ///     卡槽1是否启用
        /// </summary>
        public int Card1Enable { get; set; }

        /// <summary>
        ///     卡槽2是否启用
        /// </summary>
        public int Card2Enable { get; set; }
    }
}
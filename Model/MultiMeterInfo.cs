// Joe
// 周漫
// 2021012512:54

using System;
using System.Collections.Generic;

namespace Model {
    public class MultiMeterInfo {
        /// <summary>
        ///     四线电阻
        /// </summary>
        public string ResistanceChn;

        /// <summary>
        ///     多少路四线电阻
        /// </summary>
        public int ResistanceNum;

        /// <summary>
        ///     热电偶
        /// </summary>
        public string ThermocoupleChn;

        /// <summary>
        ///     多少路热电偶
        /// </summary>
        public int ThermocoupleNum;

        /// <summary>
        ///     两线电阻
        /// </summary>
        public string VoltageChn;

        /// <summary>
        ///     多少路两线电阻
        /// </summary>
        public int VoltageNum;

        public MultiMeterInfo() {
            VoltageChn = "";
            ResistanceChn = "";
            ThermocoupleChn = "";
            VoltageNum = 0;
            ResistanceNum = 0;
            ThermocoupleNum = 0;
        }

        public MultiMeterInfo(SerialPortPara serialPort) {
            VoltageChn = "";
            ResistanceChn = "";
            ThermocoupleChn = "";
            VoltageNum = 0;
            ResistanceNum = 0;
            ThermocoupleNum = 0;
            if (serialPort.Card1Enable != 0) GetChannels(serialPort.CardList1);

            if (serialPort.Card2Enable != 0) GetChannels(serialPort.CardList2);
        }

        public int TotalNum => VoltageNum + ThermocoupleNum + ResistanceNum;
        public string[] Channels => TotalChn.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);

        public string TotalChn {
            get {
                var temp = VoltageChn;
                if (ThermocoupleChn.Length != 0) temp += "," + ThermocoupleChn;
                if (ResistanceChn.Length != 0) temp += "," + ResistanceChn;
                return temp;
            }
        }

        private void GetChannels(List<Card> cardList) {
            foreach (var card in cardList)
                switch (card.Func) {
                    case 0:
                        break;
                    case 1: {
                        VoltageNum++;
                        AddChn(ref VoltageChn, card.Chn);
                    }
                        break;
                    case 2: {
                        ThermocoupleNum++;
                        AddChn(ref ThermocoupleChn, card.Chn);
                    }
                        break;
                    case 3: {
                        ResistanceNum++;
                        AddChn(ref ResistanceChn, card.Chn);
                    }
                        break;
                }
        }

        private void AddChn(ref string list, string chn) {
            if (list.Length == 0)
                list = chn;
            else
                list += "," + chn;
        }
    }
}
// Joe
// 周漫
// 2021012511:43

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Model;

namespace DataAccess {
    public static partial class IniReadAndWrite {
        public static string IniFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini");

        public static void ReadPara(ref SysPara sys, string filePath) {
            sys.ScanInterval.LowerBound = int.Parse(IniHelper.Read("SYS", "scanIntervalLb", "2000", filePath));
            sys.ScanInterval.UpperBound = int.Parse(IniHelper.Read("SYS", "scanIntervalUb", "5000", filePath));
            sys.ScanInterval.Value = int.Parse(IniHelper.Read("SYS", "scanInterval", "2000", filePath));

            sys.SaveInterval.LowerBound = int.Parse(IniHelper.Read("SYS", "saveInterval", "30", filePath));
            sys.SaveInterval.UpperBound = int.Parse(IniHelper.Read("SYS", "saveInterval", "100", filePath));
            sys.SaveInterval.Value = int.Parse(IniHelper.Read("SYS", "saveInterval", "50", filePath));

            sys.AutoCloseInterval = int.Parse(IniHelper.Read("SYS", "autoSaveInterval", "1500", filePath));
            sys.ConvergentLim = double.Parse(IniHelper.Read("SYS", "convergentLim", "1e-3", filePath));

            sys.AllowedChannels = IniHelper.Read("SYS", "allowedChannel", "", filePath)
                .Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public static void WritePara(SysPara sys, string filePath) {
            IniHelper.Write("SYS", "scanInterval", sys.ScanInterval.Value.ToString(), filePath);
            IniHelper.Write("SYS", "scanIntervalLb", sys.ScanInterval.LowerBound.ToString(), filePath);
            IniHelper.Write("SYS", "scanIntervalUb", sys.ScanInterval.UpperBound.ToString(), filePath);

            IniHelper.Write("SYS", "saveInterval", sys.SaveInterval.Value.ToString(), filePath);
            IniHelper.Write("SYS", "saveIntervalLb", sys.SaveInterval.LowerBound.ToString(), filePath);
            IniHelper.Write("SYS", "saveIntervalUb", sys.SaveInterval.UpperBound.ToString(), filePath);

            IniHelper.Write("SYS", "autoSaveInterval", sys.AutoCloseInterval.ToString(), filePath);
            IniHelper.Write("SYS", "convergentLim", sys.ConvergentLim.ToString(CultureInfo.InvariantCulture), filePath);
            IniHelper.Write("SYS", "allowedChannel",
                sys.AllowedChannels.Aggregate("", (current, s) => current + s + ","), filePath);
        }

        public static void ReadPara(ref SerialPortPara serialPort, string filePath) {
            serialPort.SerialPort = IniHelper.Read("Serial", "port", "COM1", filePath);
            serialPort.SerialBaudRate = IniHelper.Read("Serial", "baudrate", "9600", filePath);
            serialPort.SerialDataBits = IniHelper.Read("Serial", "databits", "8", filePath);

            serialPort.SerialStopBits = IniHelper.Read("Serial", "stopbites", "1", filePath);
            serialPort.SerialParity = IniHelper.Read("Serial", "parity", "none", filePath);
            foreach (var i in serialPort.CardList1) {
                i.Func = int.Parse(IniHelper.Read(i.Chn, "func", "0", filePath));
                i.Type = (ProbeType) int.Parse(IniHelper.Read(i.Chn, "type", "0", filePath));
            }

            foreach (var i in serialPort.CardList2) {
                i.Func = int.Parse(IniHelper.Read(i.Chn, "func", "0", filePath));
                i.Type = (ProbeType) int.Parse(IniHelper.Read(i.Chn, "type", "0", filePath));
            }

            serialPort.Card1Enable = int.Parse(IniHelper.Read("Card1", "enable", "", filePath));
            serialPort.Card2Enable = int.Parse(IniHelper.Read("Card2", "enable", "", filePath));
        }

        public static void WriteBasicPara(SerialPortPara serialPort, string filePath) {
            IniHelper.Write("Serial", "port", serialPort.SerialPort, filePath);
            IniHelper.Write("Serial", "baudrate", serialPort.SerialBaudRate, filePath);
            IniHelper.Write("Serial", "databits", serialPort.SerialDataBits, filePath);

            IniHelper.Write("Serial", "stopbites", serialPort.SerialStopBits, filePath);
            IniHelper.Write("Serial", "parity", serialPort.SerialParity, filePath);
        }

        public static void WriteChannelPara(SerialPortPara serialPort, string filePath) {
            foreach (var i in serialPort.CardList1) {
                IniHelper.Write(i.Chn, "type", $"{(int) i.Type}", filePath);
                IniHelper.Write(i.Chn, "func", i.Func.ToString(), filePath);
            }

            foreach (var i in serialPort.CardList2) {
                IniHelper.Write(i.Chn, "type", $"{(int) i.Type}", filePath);
                IniHelper.Write(i.Chn, "func", i.Func.ToString(), filePath);
            }

            IniHelper.Write("Card1", "enable", serialPort.Card1Enable.ToString(), filePath);
            IniHelper.Write("Card2", "enable", serialPort.Card2Enable.ToString(), filePath);
        }

        public static void ReadPara(ref AppCfg app, string filePath) {
            ReadPara(ref app.SysPara, filePath);
            ReadPara(ref app.SerialPortPara, filePath);
        }

        public static void DeviceToApp(TestDevice device, ref SerialPortPara serialPort) {
            var channelList = device.Channels;
            if (serialPort.Card1Enable == 1)
                foreach (var card in serialPort.CardList1)
                    if (channelList.Contains(card.Chn))
                        card.Func = (int) card.Type;
                    else
                        card.Func = 0;
            if (serialPort.Card2Enable == 1)
                foreach (var card in serialPort.CardList2)
                    if (channelList.Contains(card.Chn))
                        card.Func = (int) card.Type;
                    else
                        card.Func = 0;
        }
    }
}
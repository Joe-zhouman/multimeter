using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Model;

namespace DataAccess {
    public static partial class IniReadAndWrite {

        public static void CreateDefaultIni() {
            #region
            if (File.Exists(IniFilePath)) return;
            File.Create(IniFilePath).Close();
            AppCfg app = new AppCfg();
            TestDevice device = new TestDevice(TestMethod.ITMS);
            WriteBasicPara(app.SerialPortPara,IniFilePath);
            WriteChannelPara(app.SerialPortPara, IniFilePath);
            WritePara(app.SysPara,IniFilePath);
            WriteDevicePara(device,IniFilePath);
            for (int i = 101; i < 123; i++) {
                IniHelper.Write(i.ToString(), "type", $"{(int)ProbeType.NULL}", IniFilePath);
                IniHelper.Write(i.ToString(), "A0", "0.0", IniFilePath);
                IniHelper.Write(i.ToString(), "A1", "0.0", IniFilePath);
                IniHelper.Write(i.ToString(), "A3", "0.0", IniFilePath);
            }

            for (int i = 201; i < 223; i++) {
                IniHelper.Write(i.ToString(), "type", $"{(int)ProbeType.NULL}", IniFilePath);
                IniHelper.Write(i.ToString(), "A0", "0.0", IniFilePath);
                IniHelper.Write(i.ToString(), "A1", "0.0", IniFilePath);
                IniHelper.Write(i.ToString(), "A3", "0.0", IniFilePath);
            }
            #endregion
        }
        //public static string CreateDefaultSettingIni() {
        //    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "setting.ini"); //在当前程序路径创建
        //    if (INIHelper.CheckPath(filePath))
        //        return filePath;
        //    HeatMeter heatMeter1 = new HeatMeter("HeatMeter1",3);
        //    HeatMeter heatMeter2 = new HeatMeter("HeatMeter2");
        //    Sample sample1 = new Sample("Sample1");
        //    Sample sample2 = new Sample("Sample2");
        //    File.Create(filePath).Close();
        //    INIHelper.Write("TestMethod", "method", "", filePath);
        //    INIHelper.Write("Pressure", "force", "0", filePath); //施加压力 Unit kg
        //    heatMeter1.SaveToIni(filePath);
        //    heatMeter2.SaveToIni(filePath);
        //    sample1.SaveToIni(filePath);
        //    sample2.SaveToIni(filePath);
        //    INIHelper.Write("ITM", "thickness", "0", filePath); //热界面材料厚度 Unit um
        //    return filePath;
        //} // 创建 热流计及热电偶属性 初始配置文件
        

        

        //public static void WriteChannelInfo(List<string> channelList) {
        //    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini");
        //    if (channelList.First().First() == '1') {
        //        INIHelper.Write("Card1", "enable", "1", filePath);
        //        INIHelper.Write("Card2", "enable", "0", filePath);
        //        for (int i = 101; i < 122; i++) {
        //            INIHelper.Write(i.ToString(), "func", channelList.Contains(i.ToString()) ? "1" : "0", filePath);
        //        }
        //    } else {
        //        INIHelper.Write("Card1", "enable", "0", filePath);
        //        INIHelper.Write("Card2", "enable", "1", filePath);
        //        for (int i = 201; i < 222; i++) {
        //            INIHelper.Write(i.ToString(), "func", channelList.Contains(i.ToString()) ? "1" : "0", filePath);
        //        }
        //    }
        //}//将频道信息写入系统配置文件
        public static string AutoSaveIni(TestMethod method) {
            var iniFileName = method.ToString().ToLower();
            string iniFilePath =
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, iniFileName + ".ini");
            string autoSaveFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AutoSave",
                iniFileName + "-" + DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss") + ".ini");

            File.Copy(iniFilePath, autoSaveFilePath);
            return autoSaveFilePath;
        }//自动保存当前的测试配置文件
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using multimeter;

namespace DataProcessor {
    public static class SlnIni {
        public static string CreateDefaultKappaIni() {
            string filePath = "";
            return filePath;
        }
        public static string CreateDefaultItcIni() {
            string filePath = "";
            return filePath;
        }
        public static string CreateDefaultItmIni() {
            string filePath = "";
            return filePath;
        }
        public static string CreateDefaultItmsIni() {
            string filePath = "";
            return filePath;
        }

        public static string CreateDefaultIni() {
            #region

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini"); //在当前程序路径创建
            if (INIHelper.CheckPath(filePath)) { return filePath; }
                

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

            for (int i = 101; i < 123; i++) //每个7700卡22个通道
            {
                string t = i.ToString();
                INIHelper.Write(t, "name", "", filePath);
                INIHelper.Write(t, "func", "0", filePath);
                INIHelper.Write(t, "A0", "0.0", filePath); // 热电偶标定系数 alpha Unit ℃/V
                INIHelper.Write(t, "A1", "0.0", filePath); //热电偶标定系数 beta Unit ℃
                INIHelper.Write(t, "A3", "0.0", filePath); //热电偶标定系数 theta Unit ℃
            }

            for (int i = 201; i < 223; i++) {
                string t = i.ToString();
                INIHelper.Write(t, "name", "", filePath);
                INIHelper.Write(t, "func", "0", filePath);
                INIHelper.Write(t, "A0", "0.0", filePath); // 热电偶标定系数 alpha Unit ℃/V
                INIHelper.Write(t, "A1", "0.0", filePath); //热电偶标定系数 beta Unit ℃
                INIHelper.Write(t, "A3", "0.0", filePath); //热电偶标定系数 theta Unit ℃
            }

            #endregion
            return filePath;
        }
        public static string CreateDefaultSettingIni() {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "setting.ini"); //在当前程序路径创建
            if (INIHelper.CheckPath(filePath))
                return filePath;
            HeatMeter heatMeter1 = new HeatMeter("HeatMeter1");
            HeatMeter heatMeter2 = new HeatMeter("HeatMeter2");
            Sample sample1 = new Sample("Sample1");
            Sample sample2 = new Sample("Sample2");
            File.Create(filePath).Close();
            INIHelper.Write("TestMethod", "method", "", filePath);
            INIHelper.Write("Pressure", "force", "0", filePath); //施加压力 Unit kg
            heatMeter1.SaveToIni(filePath);
            heatMeter2.SaveToIni(filePath);
            sample1.SaveToIni(filePath);
            sample2.SaveToIni(filePath);
            INIHelper.Write("ITM", "thickness", "0", filePath); //热界面材料厚度 Unit um
            return filePath;
        } // 创建 热流计及热电偶属性 初始配置文件
        

        public static void LoadHeatMeterInfo(ref HeatMeter heatMeter1,ref HeatMeter heatMeter2,string settingFilePath, string sysFilePath) {
            
            heatMeter1.ReadFromIni(settingFilePath);
            heatMeter1.LoadTempPara(sysFilePath);
            heatMeter2.ReadFromIni(settingFilePath);
            heatMeter2.LoadTempPara(sysFilePath);
        } // 读取 热流计 属性设置

        public static void LoadKappaInfo(ref Sample sample1, out string force,string settingFilePath, string sysFilePath) {
            sample1.ReadFromIni(settingFilePath);
            sample1.LoadTempPara(sysFilePath);
            force = INIHelper.Read("Pressure", "force", "0", settingFilePath);

        } // 读取 试件热导率测量 属性设置

        public static void LoadItcInfo(ref Sample sample1, ref Sample sample2, out string force, string settingFilePath, string sysFilePath) {
            sample1.ReadFromIni(settingFilePath);
            sample1.LoadTempPara(sysFilePath);
            sample2.ReadFromIni(settingFilePath);
            sample2.LoadTempPara(sysFilePath);
            force = INIHelper.Read("Pressure", "force", "0", settingFilePath);

        } // 读取 试件间接触热阻测量 属性设置

        public static void LoadItmInfo(out string force, out string thickness,string settingFilePath) {
            thickness = INIHelper.Read("ITM", "thickness", "0", settingFilePath);
            force = INIHelper.Read("Pressure", "force", "0", settingFilePath);
        } // 读取 热流计间 热界面材料 热导率测量 属性设置

        public static void LoadItmsInfo(ref Sample sample1, ref Sample sample2, out string force, out string thickness, string settingFilePath, string sysFilePath) {
            sample1.ReadFromIni(settingFilePath);
            sample1.LoadTempPara(sysFilePath);
            sample2.ReadFromIni(settingFilePath);
            sample2.LoadTempPara(sysFilePath);
            thickness = INIHelper.Read("ITM", "thickness", "0", settingFilePath);
            force = INIHelper.Read("Pressure", "force", "0", settingFilePath);

        } // 读取 试件间 热界面材料 热导率测量 属性设置

        public static void SaveHeatMeterInfo(HeatMeter heatMeter1, HeatMeter heatMeter2, string filePath) {
            heatMeter1.SaveToIni(filePath);
            heatMeter2.SaveToIni(filePath);
        } // 保存 热流计 属性设置

        public static void SaveKappaInfo(Sample sample1, string force, string filePath) {
            sample1.SaveToIni(filePath);
            INIHelper.Write("Pressure", "force", force, filePath);

        } // 保存 试件热导率测量 属性设置

        public static void SaveItcInfo(Sample sample1, Sample sample2, string force, string filePath) {
            sample1.SaveToIni(filePath);
            sample2.SaveToIni(filePath);
            INIHelper.Write("Pressure", "force", force, filePath);

        } // 保存 试件间接触热阻测量 属性设置

        public static void SaveItmInfo(string force, string thickness, string filePath) {
            INIHelper.Write("ITM", "thickness", thickness, filePath);
            INIHelper.Write("Pressure", "force", force, filePath);
        } // 保存 热流计间 热界面材料 热导率测量 属性设置

        public static void SaveItmsInfo(Sample sample1, Sample sample2, string force, string thickness, string filePath) {
            sample1.SaveToIni(filePath);
            sample2.SaveToIni(filePath);
            INIHelper.Write("ITM", "thickness", thickness, filePath);
            INIHelper.Write("Pressure", "force", force, filePath);

        } // 保存 试件间 热界面材料 热导率测量 属性设置

        public static void WriteChannelInfo(List<string> channelList) {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini");
            if (channelList.First().First() == '1') {
                INIHelper.Write("Card1", "enable", "1", filePath);
                INIHelper.Write("Card2", "enable", "0", filePath);
                for (int i = 101; i < 122; i++) {
                    INIHelper.Write(i.ToString(), "func", channelList.Contains(i.ToString()) ? "1" : "0", filePath);
                }
            } else {
                INIHelper.Write("Card1", "enable", "0", filePath);
                INIHelper.Write("Card2", "enable", "1", filePath);
                for (int i = 201; i < 222; i++) {
                    INIHelper.Write(i.ToString(), "func", channelList.Contains(i.ToString()) ? "1" : "0", filePath);
                }
            }
        }//将频道信息写入系统配置文件
        public static string AutoSaveIni(TestMethod method) {
            var iniFileName = method.ToString().ToLower();
            string iniFilePath =
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, iniFileName + ".ini");
            string autoSaveFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AutoSave",
                iniFileName + "-" + DateTime.Now.ToString("yyyy-MM-dd-HH") + ".ini");
            while (File.Exists(autoSaveFilePath)) {
                DialogResult results = MessageBox.Show(@"该文件已存在,是否覆盖?", @"提示", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (results == DialogResult.Yes) {
                    File.Delete(autoSaveFilePath);
                } else {
                    int insertIdx = autoSaveFilePath.LastIndexOf(".ini", StringComparison.Ordinal);
                    autoSaveFilePath = autoSaveFilePath.Insert(insertIdx, "(1)");
                }
            }

            File.Copy(iniFilePath, autoSaveFilePath);
            return autoSaveFilePath;
        }//自动保存当前的测试配置文件
    }
}

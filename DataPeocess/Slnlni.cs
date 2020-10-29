using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DataProcessor {
    public static class SlnIni {
        public static void CreateDefaultIni() {
            #region

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini"); //在当前程序路径创建
            if (INIHelper.CheckPath(filePath))
                return;

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

            for (int i = 0; i < 22; i++) //每个7700卡22个通道
            {
                INIHelper.Write((i + 101).ToString(), "name", "", filePath);
                INIHelper.Write((i + 101).ToString(), "func", "0", filePath);
            }

            for (int i = 0; i < 22; i++) {
                INIHelper.Write((i + 201).ToString(), "name", "", filePath);
                INIHelper.Write((i + 201).ToString(), "func", "0", filePath);
            }

            #endregion
        }
        public static string CreateDefaultSlnIni() {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sln.ini"); //在当前程序路径创建
            if (INIHelper.CheckPath(filePath))
                return filePath;
            HeatMeter heatMeter1 = new HeatMeter("HeatMeter1");
            HeatMeter heatMeter2 = new HeatMeter("HeatMeter2");
            File.Create(filePath).Close();
            heatMeter1.SaveToIni(filePath);
            heatMeter2.SaveToIni(filePath);
            //热电偶温度 T = alpha * U + T0
            //卡1 22个通道
            for (int i = 0; i < 22; i++) {
                INIHelper.Write((i + 101).ToString(), "alpha", "0.0", filePath); // 热电偶标定系数 alpha Unit ℃/V
                INIHelper.Write((i + 101).ToString(), "T0", "0.0", filePath); //热电偶标定系数 T0 Unit ℃
            }

            //卡2 22个通道
            for (int i = 0; i < 22; i++) {
                INIHelper.Write((i + 201).ToString(), "alpha", "", filePath); // 热电偶标定系数 alpha Unit ℃/V
                INIHelper.Write((i + 201).ToString(), "T0", "0.0", filePath); //热电偶标定系数 T0 Unit ℃
            }

            return filePath;
        } // 创建 热流计及热电偶属性 初始配置文件

        public static string CreateDefaultKappaIni() {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "kappa.ini"); //在当前程序路径创建
            if (INIHelper.CheckPath(filePath))
                return filePath;
            File.Create(filePath).Close();
            INIHelper.Write("Pressure", "force", "0", filePath); //施加压力 Unit kg

            Sample sample = new Sample("Sample1");
            sample.SaveToIni(filePath);

            return filePath;
        } // 创建 试件热导率测量 初始配置文件

        public static string CreateDefaultItcIni() {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "itc.ini"); //在当前程序路径创建
            if (INIHelper.CheckPath(filePath))
                return filePath;
            File.Create(filePath).Close();

            INIHelper.Write("Pressure", "force", "0", filePath); //施加压力 Unit kg

            Sample sample1 = new Sample("Sample1");
            sample1.SaveToIni(filePath);
            Sample sample2 = new Sample("Sample2");
            sample2.SaveToIni(filePath);

            return filePath;
        } // 创建 接触热阻测量 初始配置文件

        public static string CreateDefaultItmIni() {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "itm.ini"); //在当前程序路径创建
            if (INIHelper.CheckPath(filePath))
                return filePath;
            File.Create(filePath).Close();

            INIHelper.Write("Pressure", "force", "0", filePath); //施加压力 Unit kg
            INIHelper.Write("ITM", "thickness", "0", filePath); //热界面材料厚度 Unit um
            
            return filePath;
        } // 创建 热流计间 热界面材料热导率测量 初始配置文件

        public static string CreateDefaultItmsIni() {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "itms.ini"); //在当前程序路径创建
            if (INIHelper.CheckPath(filePath))
                return filePath;
            File.Create(filePath).Close();

            INIHelper.Write("Pressure", "force", "0", filePath); //施加压力 Unit kg
            INIHelper.Write("ITM", "thickness", "0", filePath); //热界面材料厚度 Unit um

            Sample sample1 = new Sample("Sample1");
            sample1.SaveToIni(filePath);
            Sample sample2 = new Sample("Sample2");
            sample2.SaveToIni(filePath);

            return filePath;
        } // 创建 试件间 热界面材料热导率测量 初始配置文件

        public static void LoadHeatMeterInfo(ref HeatMeter heatMeter1,ref HeatMeter heatMeter2,string filePath) {
            
            heatMeter1.ReadFromIni(filePath);
            heatMeter2.ReadFromIni(filePath);
        } // 读取 热流计 属性设置

        public static void LoadKappaInfo(ref Sample sample1, out string force,string filePath) {
            sample1.ReadFromIni(filePath);
            force = INIHelper.Read("Pressure", "force", "0", filePath);

        } // 读取 试件热导率测量 属性设置

        public static void LoadItcInfo(ref Sample sample1, ref Sample sample2, out string force, string filePath) {
            sample1.ReadFromIni(filePath);
            sample2.ReadFromIni(filePath);
            force = INIHelper.Read("Pressure", "force", "0", filePath);

        } // 读取 试件间接触热阻测量 属性设置

        public static void LoadItmInfo(out string force, out string thickness,string filePath) {
            thickness = INIHelper.Read("ITM", "thickness", "0", filePath);
            force = INIHelper.Read("Pressure", "force", "0", filePath);
        } // 读取 热流计间 热界面材料 热导率测量 属性设置

        public static void LoadItmsInfo(ref Sample sample1, ref Sample sample2, out string force, out string thickness, string filePath) {
            sample1.ReadFromIni(filePath);
            sample2.ReadFromIni(filePath);
            thickness = INIHelper.Read("ITM", "thickness", "0", filePath);
            force = INIHelper.Read("Pressure", "force", "0", filePath);

        } // 读取 试件间 热界面材料 热导率测量 属性设置

        public static void SaveHeatMeterInfo(HeatMeter heatMeter1, HeatMeter heatMeter2, string filePath) {
            string bakFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bak", "sln.ini.",
                DateTime.Now.ToString("yyyy-MM-dd"), ".bak");
            File.Copy(filePath, bakFilePath);
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
        public static bool AutoSaveIni(string testChoose) {
            string iniFileName;
            switch (testChoose) {
                case "Test1":
                    iniFileName = "kappa";
                    break;
                case "Test2":
                    iniFileName = "itc";
                    break;
                case "Test3":
                    iniFileName = "itm";
                    break;
                case "Test4":
                    iniFileName = "itms";
                    break;
                default:

                    return false;
            }

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
            return true;
        }//自动保存当前的测试配置文件
    }
}

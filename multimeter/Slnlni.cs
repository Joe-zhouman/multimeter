using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace multimeter {
    public static class SlnIni {
        public static void CreateDefaultSlnIni() {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sln.ini"); //在当前程序路径创建
            if (INIHelper.CheckPath(filePath))
                return;
            File.Create(filePath).Close();
            INIHelper.Write("UpperHeatMeter", "kappa", "0", filePath); //上热流计热导率 Unit W/(mK)
            INIHelper.Write("UpperHeatMeter", "diameter", "0", filePath); //上热流计直径 Unit mm

            INIHelper.Write("LowerHeatMeter", "kappa", "0", filePath); //下热流计热导率 Unit W/(mK)
            INIHelper.Write("LowerHeatMeter", "diameter", "0", filePath); //下热流计直径 Unit mm

            for (int i = 0; i < 4; i++) {
                INIHelper.Write($"UpperHeatMeter{i}", "channel", "000", filePath); // 测温点通道
                INIHelper.Write($"UpperHeatMeter{i}", "position", "0.00", filePath); //测温点位置 Unit mm
            }

            for (int i = 0; i < 4; i++) {
                INIHelper.Write($"LowerHeatMeter{i}", "channel", "000", filePath); // 测温点通道
                INIHelper.Write($"LowerHeatMeter{i}", "position", "0.00", filePath); //测温点位置 Unit mm
            }

            //热电偶温度 T = alpha * U + T0
            //卡1 22个通道
            for (int i = 0; i < 22; i++) {
                INIHelper.Write((i + 101).ToString(), "alpha", "", filePath); // 热电偶标定系数 alpha Unit ℃/V
                INIHelper.Write((i + 101).ToString(), "T0", "0", filePath); //热电偶标定系数 T0 Unit ℃
            }

            //卡2 22个通道
            for (int i = 0; i < 22; i++) {
                INIHelper.Write((i + 101).ToString(), "alpha", "", filePath); // 热电偶标定系数 alpha Unit ℃/V
                INIHelper.Write((i + 101).ToString(), "T0", "0", filePath); //热电偶标定系数 T0 Unit ℃
            }
        } // 创建 热流计及热电偶属性 初始配置文件

        public static void CreateDefaultKappaIni() {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "kappa.ini"); //在当前程序路径创建
            if (INIHelper.CheckPath(filePath))
                return;
            File.Create(filePath).Close();
            INIHelper.Write("Sample", "diameter", "0", filePath); //下热流计直径 Unit mm
            INIHelper.Write("Pressure", "force", "0", filePath); //施加压力 Unit kg

            for (int i = 0; i < 3; i++) {
                INIHelper.Write($"Sample{i}", "channel", "000", filePath); // 测温点通道
                INIHelper.Write($"Sample{i}", "position", "0.00", filePath); //测温点位置 Unit mm
            }
        } // 创建 试件热导率测量 初始配置文件

        public static void CreateDefaultItcIni() {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "itc.ini"); //在当前程序路径创建
            if (INIHelper.CheckPath(filePath))
                return;
            File.Create(filePath).Close();
            INIHelper.Write("UpperSample", "diameter", "0", filePath); //下热流计直径 Unit mm
            INIHelper.Write("LowerSample", "diameter", "0", filePath); //下热流计直径 Unit mm

            INIHelper.Write("Pressure", "force", "0", filePath); //施加压力 Unit kg

            for (int i = 0; i < 3; i++) {
                INIHelper.Write($"UpperSample{i}", "channel", "000", filePath); // 测温点通道
                INIHelper.Write($"UpperSample{i}", "position", "0.00", filePath); //测温点位置 Unit mm
            }

            for (int i = 0; i < 3; i++) {
                INIHelper.Write($"LowerSample{i}", "channel", "000", filePath); // 测温点通道
                INIHelper.Write($"LowerSample{i}", "position", "0.00", filePath); //测温点位置 Unit mm
            }
        } // 创建 接触热阻测量 初始配置文件

        public static void CreateDefaultItmIni() {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "itm.ini"); //在当前程序路径创建
            if (INIHelper.CheckPath(filePath))
                return;
            File.Create(filePath).Close();
            INIHelper.Write("ITM", "thickness", "0", filePath); //热界面材料厚度 Unit um
            INIHelper.Write("Pressure", "force", "0", filePath); //施加压力 Unit kg
        } // 创建 热流计间 热界面材料热导率测量 初始配置文件

        public static void CreateDefaultItmsIni() {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "itms.ini"); //在当前程序路径创建
            if (INIHelper.CheckPath(filePath))
                return;
            File.Create(filePath).Close();
            INIHelper.Write("ITM", "thickness", "0", filePath); //热界面材料厚度 Unit um
            INIHelper.Write("Pressure", "force", "0", filePath); //施加压力 Unit kg
            INIHelper.Write("UpperSample", "diameter", "0", filePath); //下热流计直径 Unit mm
            INIHelper.Write("LowerSample", "diameter", "0", filePath); //下热流计直径 Unit mm

            for (int i = 0; i < 3; i++) {
                INIHelper.Write($"UpperSample{i}", "channel", "000", filePath); // 测温点通道
                INIHelper.Write($"UpperSample{i}", "position", "0.00", filePath); //测温点位置 Unit mm
            }

            for (int i = 0; i < 3; i++) {
                INIHelper.Write($"LowerSample{i}", "channel", "000", filePath); // 测温点通道
                INIHelper.Write($"LowerSample{i}", "position", "0.00", filePath); //测温点位置 Unit mm
            }
        } // 创建 试件间 热界面材料热导率测量 初始配置文件

        public static void LoadHeatMeterInfo(List<TextBox> heatMeterPositionBoxes, List<TextBox> heatMeterChannelBoxes,
            List<TextBox> heatMeterKappaBoxes, List<TextBox> heatMeterDiameterBoxes) {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sln.ini");
            for (int i = 0; i < 4; i++) {
                heatMeterChannelBoxes[i].Text = INIHelper.Read($"UpperHeatMeter{i}", "channel", "000", filePath);
                heatMeterChannelBoxes[i + 4].Text = INIHelper.Read($"LowerHeatMeter{i}", "channel", "000", filePath);
                heatMeterPositionBoxes[i].Text = INIHelper.Read($"UpperHeatMeter{i}", "position", "0.00", filePath);
                heatMeterPositionBoxes[i + 4].Text = INIHelper.Read($"LowerHeatMeter{i}", "position", "0.00", filePath);
            }

            heatMeterKappaBoxes[0].Text =
                INIHelper.Read("UpperHeatMeter", "kappa", "0", filePath); //上热流计热导率 Unit W/(mK)
            heatMeterDiameterBoxes[0].Text =
                INIHelper.Read("UpperHeatMeter", "diameter", "0", filePath); //上热流计直径 Unit mm

            heatMeterKappaBoxes[1].Text =
                INIHelper.Read("LowerHeatMeter", "kappa", "0", filePath); //下热流计热导率 Unit W/(mK)
            heatMeterDiameterBoxes[1].Text =
                INIHelper.Read("LowerHeatMeter", "diameter", "0", filePath); //下热流计直径 Unit mm
        } // 读取 热流计 属性设置

        public static void LoadKappaInfo(List<TextBox> samplePositionBoxes, List<TextBox> sampleChannelBoxes,
            TextBox forceBox, TextBox diameterBox) {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "kappa.ini");
            for (int i = 0; i < 3; i++) {
                sampleChannelBoxes[i].Text = INIHelper.Read($"Sample{i}", "channel", "000", filePath);
                samplePositionBoxes[i].Text = INIHelper.Read($"Sample{i}", "position", "0.00", filePath);
            }
            diameterBox.Text = INIHelper.Read("Sample", "diameter", "0", filePath);
            forceBox.Text = INIHelper.Read("Pressure", "force", "0", filePath);

        } // 读取 试件热导率测量 属性设置

        public static void LoadItcInfo(List<TextBox> samplePositionBoxes, List<TextBox> sampleChannelBoxes,
            TextBox forceBox,List<TextBox> diameterBox) {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "itc.ini");
            for (int i = 0; i < 3; i++) {
                sampleChannelBoxes[i].Text = INIHelper.Read($"UpperSample{i}", "channel", "000", filePath);
                samplePositionBoxes[i].Text = INIHelper.Read($"UpperSample{i}", "position", "0.00", filePath);
                sampleChannelBoxes[i + 3].Text = INIHelper.Read($"LowerSample{i}", "channel", "000", filePath);
                samplePositionBoxes[i + 3].Text = INIHelper.Read($"LowerSample{i}", "position", "0.00", filePath);
            }
            diameterBox[0].Text = INIHelper.Read("UpperSample", "diameter", "0", filePath); //下热流计直径 Unit mm
            diameterBox[1].Text = INIHelper.Read("LowerSample", "diameter", "0", filePath);
            forceBox.Text = INIHelper.Read("Pressure", "force", "0", filePath);

        } // 读取 试件间接触热阻测量 属性设置

        public static void LoadItmInfo(TextBox forceBox, TextBox thicknessBox) {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "itm.ini");
            thicknessBox.Text = INIHelper.Read("ITM", "thickness", "0", filePath);
            forceBox.Text = INIHelper.Read("Pressure", "force", "0", filePath);
        } // 读取 热流计间 热界面材料 热导率测量 属性设置

        public static void LoadItcsInfo(List<TextBox> samplePositionBoxes, List<TextBox> sampleChannelBoxes,
            TextBox forceBox, TextBox thicknessBox) {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "itcs.ini");
            for (int i = 0; i < 3; i++) {
                sampleChannelBoxes[i].Text = INIHelper.Read($"UpperSample{i}", "channel", "000", filePath);
                samplePositionBoxes[i].Text = INIHelper.Read($"UpperSample{i}", "position", "0.00", filePath);
                sampleChannelBoxes[i + 3].Text = INIHelper.Read($"LowerSample{i}", "channel", "000", filePath);
                samplePositionBoxes[i + 3].Text = INIHelper.Read($"LowerSample{i}", "position", "0.00", filePath);
            }

            thicknessBox.Text = INIHelper.Read("ITM", "thickness", "0", filePath);
            forceBox.Text = INIHelper.Read("Pressure", "force", "0", filePath);

        } // 读取 试件间 热界面材料 热导率测量 属性设置

        public static void SaveHeatMeterInfo(List<TextBox> heatMeterPositionBoxes, List<TextBox> heatMeterChannelBoxes,
            List<TextBox> heatMeterKappaBoxes, List<TextBox> heatMeterDiameterBoxes) {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sln.ini");
            string bakFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bak\\", "sln.ini.",
                DateTime.Now.ToString("yyyyMMdd"), ".bak");
            File.Copy(filePath, bakFilePath);
            for (int i = 0; i < 4; i++) {
                INIHelper.Write($"UpperHeatMeter{i}", "channel", heatMeterChannelBoxes[i].Text, filePath);
                INIHelper.Write($"LowerHeatMeter{i}", "channel", heatMeterChannelBoxes[i + 4].Text, filePath);
                INIHelper.Write($"UpperHeatMeter{i}", "position", heatMeterPositionBoxes[i].Text, filePath);
                INIHelper.Write($"LowerHeatMeter{i}", "position", heatMeterPositionBoxes[i + 4].Text, filePath);
            }

            INIHelper.Write("UpperHeatMeter", "kappa", heatMeterKappaBoxes[0].Text, filePath); //上热流计热导率 Unit W/(mK)
            INIHelper.Write("UpperHeatMeter", "diameter", heatMeterDiameterBoxes[0].Text, filePath); //上热流计直径 Unit mm

            INIHelper.Write("LowerHeatMeter", "kappa", heatMeterKappaBoxes[1].Text, filePath); //下热流计热导率 Unit W/(mK)
            INIHelper.Write("LowerHeatMeter", "diameter", heatMeterDiameterBoxes[1].Text, filePath); //下热流计直径 Unit mm
        } // 保存 热流计 属性设置

        public static void SaveKappaInfo(List<TextBox> samplePositionBoxes, List<TextBox> sampleChannelBoxes,
            TextBox forceBox, TextBox diameterBox) {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "kappa.ini");
            for (int i = 0; i < 3; i++) {
                INIHelper.Write($"Sample{i}", "channel", sampleChannelBoxes[i].Text, filePath);
                INIHelper.Write($"Sample{i}", "position", samplePositionBoxes[i].Text, filePath);
            }
            INIHelper.Write("Sample", "diameter", diameterBox.Text, filePath);
            INIHelper.Write("Pressure", "force", forceBox.Text, filePath);

        } // 保存 试件热导率测量 属性设置

        public static void SaveItcInfo(List<TextBox> samplePositionBoxes, List<TextBox> sampleChannelBoxes,
            TextBox forceBox, List<TextBox> diameterBox) {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "itc.ini");
            for (int i = 0; i < 3; i++) {
                INIHelper.Write($"UpperSample{i}", "channel", sampleChannelBoxes[i].Text, filePath);
                INIHelper.Write($"UpperSample{i}", "position", samplePositionBoxes[i].Text, filePath);
                INIHelper.Write($"LowerSample{i}", "channel", sampleChannelBoxes[i + 3].Text, filePath);
                INIHelper.Write($"LowerSample{i}", "position", samplePositionBoxes[i + 3].Text, filePath);
            }
            INIHelper.Write("UpperSample", "diameter", diameterBox[0].Text, filePath); //下热流计直径 Unit mm
            INIHelper.Write("LowerSample", "diameter", diameterBox[1].Text, filePath);
            INIHelper.Write("Pressure", "force", forceBox.Text, filePath);

        } // 保存 试件间接触热阻测量 属性设置

        public static void SaveItmInfo(TextBox forceBox, TextBox thicknessBox) {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "itm.ini");
            INIHelper.Write("ITM", "thickness", thicknessBox.Text, filePath);
            INIHelper.Write("Pressure", "force", forceBox.Text, filePath);
        } // 保存 热流计间 热界面材料 热导率测量 属性设置

        public static void SaveItcsInfo(List<TextBox> samplePositionBoxes, List<TextBox> sampleChannelBoxes,
            TextBox forceBox, TextBox thicknessBox) {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "itcs.ini");
            for (int i = 0; i < 3; i++) {
                INIHelper.Write($"UpperSample{i}", "channel", sampleChannelBoxes[i].Text, filePath);
                INIHelper.Write($"UpperSample{i}", "position", samplePositionBoxes[i].Text, filePath);
                INIHelper.Write($"LowerSample{i}", "channel", sampleChannelBoxes[i + 3].Text, filePath);
                INIHelper.Write($"LowerSample{i}", "position", samplePositionBoxes[i + 3].Text, filePath);
            }

            INIHelper.Write("ITM", "thickness", thicknessBox.Text, filePath);
            INIHelper.Write("Pressure", "force", forceBox.Text, filePath);

        } // 保存 试件间 热界面材料 热导率测量 属性设置
    }
}

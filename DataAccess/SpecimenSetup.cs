﻿// Joe
// 周漫
// 2021012416:55

using Model;
using Model.Probe;
using System;
using System.Globalization;

namespace DataAccess {
    public static partial class IniReadAndWrite {
        /// <summary>
        /// 从读取试件的channel和position信息.如果试件为热流计,还会读取热导率
        /// </summary>
        /// <param name="specimen">试件</param>
        /// <param name="filePath">配置文件路径</param>
        public static void ReadBasicPara(ref Specimen specimen, string filePath) {
            if (specimen.SpecimenType == SpecimenType.HEAT_METER)
                specimen.Kappa = IniHelper.Read(specimen.Name, "kappa", "10.0", filePath);
            for (int i = 0; i < specimen.TestPoint; i++) {
                specimen.Channel[i] = IniHelper.Read($"{specimen.Name}.{i}", "channel", "*", filePath);
                specimen.Position[i] = IniHelper.Read($"{specimen.Name}.{i}", "position", "*", filePath);

                specimen.Area = IniHelper.Read(specimen.Name, "area", "10.0", filePath);
            }
        }
        /// <summary>
        /// 把试件的channel和position信息写入配置文件.如果试件为热流计,还会写入热导率信息
        /// </summary>
        /// <param name="specimen">试件</param>
        /// <param name="filePath">配置文件路径</param>
        public static void WriteBasicPara(Specimen specimen, string filePath) {
            if (specimen.SpecimenType == SpecimenType.HEAT_METER)
                IniHelper.Write(specimen.Name, "kappa", specimen.Kappa, filePath);
            IniHelper.Write(specimen.Name, "area", specimen.Area, filePath);
            for (int i = 0; i < specimen.TestPoint; i++) {
                IniHelper.Write($"{specimen.Name}.{i}", "channel", specimen.Channel[i], filePath);
                IniHelper.Write($"{specimen.Name}.{i}", "position",
                    specimen.Channel[i] == "*" ? "*" : specimen.Position[i], filePath);
            }
        }
        /// <summary>
        /// 从配置文件读取tim的厚度和面积信息
        /// </summary>
        /// <param name="itm">TIM</param>
        /// <param name="filePath">配置文件路径</param>
        public static void ReadBasicPara(ref Tim itm, string filePath) {
            itm.Thickness = IniHelper.Read("ITM", "thickness", "10.0", filePath);
            itm.Area = IniHelper.Read("ITM", "area", "0.0", filePath);
        }
        /// <summary>
        /// 将tim的厚度和面积信息
        /// </summary>
        /// <param name="itm">TIM</param>
        /// <param name="filePath">配置文件路径</param>
        public static void WriteBasicPara(Tim itm, string filePath) {
            IniHelper.Write("ITM", "thickness", itm.Thickness, filePath);
            IniHelper.Write("ITM", "area", itm.Area, filePath);
        }
        /// <summary>
        /// 从配置文件读取探头的参数
        /// </summary>
        /// <param name="probe">探头</param>
        /// <param name="channel">探头所在频道</param>
        /// <param name="filePath">配置文件路径</param>
        public static void ReadTempPara(ref ProbeBase probe, string channel, string filePath) {
            if (probe is null) return;
            for (int i = 0; i < probe.Paras.Length; i++)
                probe.Paras[i] = double.Parse(IniHelper.Read(channel, $"A{i}", "10.0", filePath));
            probe.Init();
            probe.TempLb = double.Parse(IniHelper.Read("SYS", "tempLb", "0.0", filePath));
            probe.TempUb = double.Parse(IniHelper.Read("SYS", "tempUb", "120.0", filePath));
        }
        /// <summary>
        /// 把探头参数写入配置文件
        /// </summary>
        /// <param name="probe">探头</param>
        /// <param name="channel">探头所在频道</param>
        /// <param name="filePath">配置文件路径</param>
        public static void WriteTempPara(ProbeBase probe, string channel, string filePath) {
            for (int i = 0; i < probe.Paras.Length; i++)
                IniHelper.Write(channel, $"A{i}", probe.Paras[i].ToString(CultureInfo.InvariantCulture), filePath);
        }
        /// <summary>
        /// 读取试件的所有探头参数
        /// </summary>
        /// <param name="specimen">试件</param>
        /// <param name="filePath">配置文件路径</param>
        public static void ReadTempPara(ref Specimen specimen, string filePath) {
            for (int i = 0; i < specimen.TestPoint; i++) {
                specimen.Probes[i] = ProbeFactory.Create((ProbeType)Enum.Parse(typeof(ProbeType),
                    IniHelper.Read(specimen.Channel[i], "type", "NULL", filePath)));
                ReadTempPara(ref specimen.Probes[i], specimen.Channel[i], filePath);
            }
        }
        /// <summary>
        /// 从配置文件读取测试方法
        /// </summary>
        /// <param name="filePath">配置文件路径</param>
        /// <returns>测试方法字符串</returns>
        public static string ReadTestMethod(string filePath) {
            return IniHelper.Read("SYS", "method", "", filePath);
        }
        //public static void WriteTempPara(Specimen specimen, string filePath) {
        //    for (var i = 0; i < specimen.TestPoint; i++) {
        //        if(specimen.Channel[i]=="0") continue;
        //        var type = specimen.Probes[i].GetType();
        //        switch (type) { }

        //        (type == ) {
        //            IniHelper.Write(channel, "type", ProbeType.THERMISTOR.ToString(), filePath);
        //            var specimenThermistor = specimen.Probes[i] as Voltage;
        //            WriteTempPara(specimenThermistor, specimen.Channel[i], filePath);
        //        }
        //        else if (type == typeof(Thermistor)) {
        //            var specimenThermistor = specimen.Probes[i] as Thermistor;
        //            WriteTempPara(specimenThermistor, specimen.Channel[i], filePath);
        //        }
        //    }

        //}
        /// <summary>
        /// 读取测试装置所有试件的基本信息
        /// </summary>
        /// <param name="testDevice">测试装置</param>
        /// <param name="filePath">配置文件路径</param>
        public static void ReadDevicePara(ref TestDevice testDevice, string filePath) {
            ReadBasicPara(ref testDevice.HeatMeter1, filePath);
            ReadBasicPara(ref testDevice.HeatMeter2, filePath);
            if (testDevice.Sample1 != null) ReadBasicPara(ref testDevice.Sample1, filePath);
            if (testDevice.Sample2 != null) ReadBasicPara(ref testDevice.Sample2, filePath);

            if (testDevice.Itm != null) ReadBasicPara(ref testDevice.Itm, filePath);
            testDevice.Force = IniHelper.Read("Pressure", "force", "0", filePath);
        }
        /// <summary>
        /// 写入测试装置所有试件的基本信息
        /// </summary>
        /// <param name="testDevice">测试装置</param>
        /// <param name="filePath">配置文件路径</param>
        public static void WriteDevicePara(TestDevice testDevice, string filePath) {
            WriteBasicPara(testDevice.HeatMeter1, filePath);
            WriteBasicPara(testDevice.HeatMeter2, filePath);
            if (testDevice.Sample1 != null) WriteBasicPara(testDevice.Sample1, filePath);
            if (testDevice.Sample2 != null) WriteBasicPara(testDevice.Sample2, filePath);

            if (testDevice.Itm != null) WriteBasicPara(testDevice.Itm, filePath);
            IniHelper.Write("Pressure", "force", testDevice.Force, filePath);
            IniHelper.Write("SYS", "method", testDevice.Method.ToString(), filePath);
        }
        /// <summary>
        /// 读取测试装置所有试件的探头参数
        /// </summary>
        /// <param name="testDevice">测试装置</param>
        /// <param name="filePath">配置文件路径</param>
        public static void ReadTempPara(ref TestDevice testDevice, string filePath) {
            ReadTempPara(ref testDevice.HeatMeter1, filePath);
            ReadTempPara(ref testDevice.HeatMeter2, filePath);
            if (testDevice.Sample1 != null) ReadTempPara(ref testDevice.Sample1, filePath);
            if (testDevice.Sample2 != null) ReadTempPara(ref testDevice.Sample2, filePath);
        }

        //public static void WriteTempPara(TestDevice testDevice, string filePath) {
        //    WriteTempPara(testDevice.HeatMeter1, filePath);
        //    WriteTempPara(testDevice.HeatMeter2, filePath);
        //    if (testDevice.Sample1 != null)
        //    {
        //        WriteTempPara(testDevice.Sample1, filePath);
        //    }
        //    if (testDevice.Sample2 != null)
        //    {
        //        WriteTempPara(testDevice.Sample2, filePath);
        //    }
        //}
    }
}
﻿// Joe
// 周漫
// 2021012416:55

using System;
using System.Globalization;
using Model;

namespace DataAccess {
    public static partial class IniReadAndWrite {
        public static void ReadBasicPara(ref Specimen specimen, string filePath) {
            if (specimen.SpecimenType == SpecimenType.HEAT_METER)
                specimen.Kappa = IniHelper.Read(specimen.Name, "kappa", "10.0", filePath);
            for (var i = 0; i < specimen.TestPoint; i++) {
                specimen.Channel[i] = IniHelper.Read($"{specimen.Name}.{i}", "channel", "0", filePath);
                specimen.Position[i] = IniHelper.Read($"{specimen.Name}.{i}", "position", "0", filePath);

                specimen.Area = IniHelper.Read(specimen.Name, "area", "10.0", filePath);
            }
        }

        public static void WriteBasicPara(Specimen specimen, string filePath) {
            if (specimen.SpecimenType == SpecimenType.HEAT_METER)
                IniHelper.Write(specimen.Name, "kappa", specimen.Kappa, filePath);
            IniHelper.Write(specimen.Name, "area", specimen.Area, filePath);
            for (var i = 0; i < specimen.TestPoint; i++) {
                IniHelper.Write($"{specimen.Name}.{i}", "channel", specimen.Channel[i], filePath);
                IniHelper.Write($"{specimen.Name}.{i}", "position",
                    specimen.Channel[i] == "0" ? "0.0" : specimen.Position[i], filePath);
            }
        }

        public static void ReadBasicPara(ref Itm itm, string filePath) {
            itm.Thickness = IniHelper.Read("ITM", "thickness", "10.0", filePath);
            itm.Area = IniHelper.Read("ITM", "area", "0.0", filePath);
        }

        public static void WriteBasicPara(Itm itm, string filePath) {
            IniHelper.Write("ITM", "thickness", itm.Thickness, filePath);
            IniHelper.Write("ITM", "area", itm.Area, filePath);
        }

        public static void ReadTempPara(ref Probe probe, string channel, string filePath) {
            for (var i = 0; i < probe.Paras.Length; i++)
                probe.Paras[i] = double.Parse(IniHelper.Read(channel, $"A{i}", "10.0", filePath));
        }

        public static void WriteTempPara(Probe probe, string channel, string filePath) {
            for (var i = 0; i < probe.Paras.Length; i++)
                IniHelper.Write(channel, $"A{i}", probe.Paras[i].ToString(CultureInfo.InvariantCulture), filePath);
        }

        public static void ReadTempPara(ref Specimen specimen, string filePath) {
            for (var i = 0; i < specimen.TestPoint; i++)
                if (specimen.Channel[i] != "0") {
                    switch ((ProbeType) Enum.Parse(typeof(ProbeType),
                        IniHelper.Read(specimen.Channel[i], "type", "NULL", filePath))) {
                        case ProbeType.VOLTAGE: {
                            specimen.Probes[i] = new Voltage();
                        }
                            break;
                        case ProbeType.THERMOCOUPLE: {
                            specimen.Probes[i] = new Thermocouple();
                        }
                            break;
                        case ProbeType.THERMISTOR: {
                            specimen.Probes[i] = new Thermistor();
                        }
                            break;
                        default:
                            throw new NumOutOfRangeException("未知的探头类型");
                    }

                    ReadTempPara(ref specimen.Probes[i], specimen.Channel[i], filePath);
                }
                else {
                    specimen.Probes[i] = null;
                }
        }

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

        public static void ReadDevicePara(ref TestDevice testDevice, string filePath) {
            ReadBasicPara(ref testDevice.HeatMeter1, filePath);
            ReadBasicPara(ref testDevice.HeatMeter2, filePath);
            if (testDevice.Sample1 != null) ReadBasicPara(ref testDevice.Sample1, filePath);
            if (testDevice.Sample2 != null) ReadBasicPara(ref testDevice.Sample2, filePath);

            if (testDevice.Itm != null) ReadBasicPara(ref testDevice.Itm, filePath);
            testDevice.Force = IniHelper.Read("Pressure", "force", "0", filePath);
        }

        public static void WriteDevicePara(TestDevice testDevice, string filePath) {
            WriteBasicPara(testDevice.HeatMeter1, filePath);
            WriteBasicPara(testDevice.HeatMeter2, filePath);
            if (testDevice.Sample1 != null) WriteBasicPara(testDevice.Sample1, filePath);
            if (testDevice.Sample2 != null) WriteBasicPara(testDevice.Sample2, filePath);

            if (testDevice.Itm != null) WriteBasicPara(testDevice.Itm, filePath);
            IniHelper.Write("Pressure", "force", testDevice.Force, filePath);
            IniHelper.Write("SYS", "method", testDevice.Method.ToString(), filePath);
        }

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
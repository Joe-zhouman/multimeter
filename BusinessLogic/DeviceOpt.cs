// Joe
// 周漫
// 2021012514:48

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Model;

namespace BusinessLogic {
    public static class DeviceOpt {
        public static void SetTemp(ref Specimen specimen, Dictionary<string, double> testResult) {
            if (specimen == null) return;
            for (var i = 0; i < specimen.TestPoint; i++)
                if (specimen.Channel[i] != "0")
                    specimen.Probes[i].SetTemp(testResult[specimen.Channel[i]]);
        }

        public static void ReadTemp(ref Specimen specimen, Dictionary<string, double> testResult) {
            if (specimen == null) return;
            for (var i = 0; i < specimen.TestPoint; i++)
                if (specimen.Channel[i] != "0")
                    specimen.Probes[i].Temp = testResult[specimen.Channel[i]];
        }

        public static void SetTemp(ref TestDevice device, Dictionary<string, double> testResult) {
            SetTemp(ref device.HeatMeter1, testResult);
            SetTemp(ref device.HeatMeter2, testResult);
            SetTemp(ref device.Sample1, testResult);
            SetTemp(ref device.Sample2, testResult);
        }

        public static void ReadTemp(ref TestDevice device, Dictionary<string, double> testResult) {
            ReadTemp(ref device.HeatMeter1, testResult);
            ReadTemp(ref device.HeatMeter2, testResult);
            ReadTemp(ref device.Sample1, testResult);
            ReadTemp(ref device.Sample2, testResult);
        }

        public static void GetTempList(ref string temp, Specimen specimen) {
            if (specimen == null) return;
            temp = specimen.Probes.Where(thermistor => thermistor != null).Aggregate(temp,
                (current, thermistor) => current + thermistor.Temp.ToString(CultureInfo.InvariantCulture) + ",");
        }

    }
}
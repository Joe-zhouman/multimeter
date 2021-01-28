// Joe
// 周漫
// 2021012419:35

using System;
using System.Linq;

namespace Model {
    public class ThermistorProbe : Probe
    {
        public string A0 { get; set; }
        public string A1 { get; set; }
        public string A3 { get; set; }

        public ThermistorProbe() {
            A0 = "0.0";
            A1 = "0.0";
            A3 = "0.0";
            Temp = 0;
        }

        public override void SetTemp(double resistance) {
            double tempVar = Math.Log(resistance);
            Temp = Math.Round(
                -273.15 + 1 / (double.Parse(A0) + double.Parse(A1) * tempVar +
                               double.Parse(A3) * Math.Pow(tempVar, 3)), 4);
        }
    }
}
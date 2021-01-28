// Joe
// 周漫
// 2021012419:33

using System;

namespace Model {
    public class VoltageProbe : Probe {
        public string Alpha { get; set; }
        public string Beta { get; set; }

        public VoltageProbe() {
            Alpha = "0.0";
            Beta = "0.0";
            Temp = 0.0;
        }
        public override void SetTemp(double voltage) {
            Temp = Math.Round(double.Parse(Alpha)  * voltage + double.Parse(Beta), 4);
        }
    }
}
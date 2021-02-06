// Joe
// 周漫
// 2021012419:33

using System;

namespace Model {
    public class Voltage : Probe {
        public Voltage() {
            Paras = new[] {0.0, 0.0};
            Temp = 0.0;
        }
        public override void SetTemp(double voltage) {
            Temp = Math.Round(Paras[0] +Paras[1] * voltage, 4);
        }
    }
}
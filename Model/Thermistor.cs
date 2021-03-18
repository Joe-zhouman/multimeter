// Joe
// 周漫
// 2021012419:35

using System;

namespace Model {
    public class Thermistor : Probe {
        public Thermistor() {
            Paras = new[] {0.0, 0.0, 0.0};
            Temp = 0;
        }

        public override void SetTemp(double resistance) {
            var tempVar = Math.Log(resistance);
            Temp = 
                -273.15 + 1 / (Paras[0] + Paras[1] * tempVar +
                               Paras[2] * Math.Pow(tempVar, 3));
        }

        public override void Init() {
            return;
        }
    }
}
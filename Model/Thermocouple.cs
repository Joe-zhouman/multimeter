// Joe
// 周漫
// 2021012712:09

using System;

namespace Model {
    public class Thermocouple:Probe {
        
        public Thermocouple()
        {
            Paras = new[] { 0.0, 0.0, 0.0 };
            Temp = 0;
        }

        public override void SetTemp(double voltage) {
            Temp = Paras[0] + Paras[1] * voltage + Paras[2] * voltage * voltage;
        }
    }
}
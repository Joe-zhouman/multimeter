// Joe
// 周漫
// 2021012419:33

using System;

namespace Model {
    public class Voltage : Probe {
        public Voltage() {
            Paras = new[] {0.0, 0.0, 0.0, 0.0};
            Temp = 0.0;
        }

        public override void SetTemp(double voltage) {
            Paras[0] = Paras[0] - voltage;
            Equation.GetRoot(Paras[3], Paras[2], Paras[1], Paras[0], out var root);
            
                Temp = Math.Round(root, 4);
            
        }
    }
}
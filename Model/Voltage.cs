// Joe
// 周漫
// 2021012419:33

using System;
using System.ComponentModel.Design;
using SolveEquation;

namespace Model {
    public class Voltage : Probe {
        public Voltage() {
            Paras = new[] {0.0, 0.0, 0.0, 0.0 };
            Temp = 0.0;
        }
        public override void SetTemp(double voltage) {
            string[] root;
            Paras[0] = Paras[0] - voltage;
            Equation.GetRoot(Paras, out root);
            for (int i = 0; i < 3; i++) {
                if (double.Parse(root[i])>0 && !root[i].Contains("i")) 
                    Temp = Math.Round(double.Parse(root[i]), 4);
            }
            //Temp = Math.Round(Paras[0] +Paras[1] * voltage, 4);
        }
    }
}
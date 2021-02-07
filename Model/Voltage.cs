// Joe
// 周漫
// 2021012419:33

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace Model {
    public class Voltage : Probe {
        public Voltage() {
            Paras = new[] {0.0, 0.0, 0.0, 0.0 };
            Temp = 0.0;
        }
        public override void SetTemp(double voltage) {
            Paras[0] = Paras[0] - voltage;
            SolveEquation.GetRoot(Paras[3], Paras[2], Paras[1], Paras[0], out var root);
            if (double.IsNaN(root)) {
                
            } //数据异常，请检查测温探头和标定参数
            else {
                Temp = Math.Round(root, 4);
            }
        }
    }
}
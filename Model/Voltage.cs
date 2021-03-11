// Joe
// 周漫
// 2021012419:33

using System;

namespace Model {
    public class Voltage : Probe {
        private CubeRootPara _cubeRootPara;

        public Voltage() {
            Paras = new[] {0.0, 0.0, 0.0, 0.0};
            Temp = 10.0;
        }

        public override void SetTemp(double voltage) {
            Equation.GetRoot(_cubeRootPara, (Paras[0] - voltage)/Paras[3], out var root);
            for (int i = 0; i < 3; i++) {
                try {
                    Temp = root[i];
                }
                catch (ValOutOfRangeException) {
                    continue;
                }
                return;
            }

            if (root[0]>TempUb) { 
                throw new ValOutOfRangeException(ValOutOfRangeType.GREATER_THAN);
            }
            throw new ValOutOfRangeException(ValOutOfRangeType.LESS_THAN);

        }

        public override void Init() {
            _cubeRootPara = new CubeRootPara(Paras[3], Paras[2], Paras[1]);
        }
    }

    public struct CubeRootPara{
        public double Q3 { get; private set; }
        public double R0 { get; private set; }
        public double SqrtQ { get; private set; }
        public double SqrtQ3 { get; private set; }
        public double Shift { get; private set; }

        public CubeRootPara(double p3, double p2, double p1) {
            double a1 = p1 / p3;
            double a2 = p2 / p3;
            double q = (3 * a1 - a2 * a2) / 9.0;
            Q3 = q * q * q;
            R0 = (9.0 * a2 * a1 - 2 * a2 * a2 * a2) / 54.0;
            Shift = -a2 / 3d;
            if (q<=0) {
                SqrtQ = Math.Sqrt(-q);
                SqrtQ3 = Math.Sqrt(-Q3);
            }
            else {
                SqrtQ = 0;
                SqrtQ3 = 0;
            }
        }
    }
}
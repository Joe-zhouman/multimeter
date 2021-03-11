using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Model {
    public static class Equation {
        /// <summary>
        ///     盛金公式求解一元三次方程
        /// </summary>
        /// <param name="para"></param>
        /// <param name="a0"></param>
        /// <param name="x">结果为null则没有实根</param>

        public static void GetRoot(CubeRootPara para,double a0, out double[] x) {
            double R = para.R0-a0/2.0;
            
            var D = para.Q3 + R * R;

            x = new double[3];
            x[1] = double.NaN;
            x[2] = double.NaN;

            if (D >= 0) {
                // when D >= 0, use eqn (54)-(56) where S and T are real
                var sqrtD = Math.Pow(D, 0.5);
                var S = GetCubeRoot(R + sqrtD);
                var T = GetCubeRoot(R - sqrtD);
                x[0] = para.Shift + (S + T);
                if (D == 0) x[1] = para.Shift - S;
            }
            else {
                // 3 real roots, use eqn (70)-(73) to calculate the real roots
                var theta = Math.Acos(R / para.SqrtQ3);
                x[0] = 2d * para.SqrtQ * Math.Cos(theta / 3.0) + para.Shift;
                x[1] = 2d * para.SqrtQ * Math.Cos((theta + 2.0 * Math.PI) / 3d) + para.Shift;
                x[2] = 2d * para.SqrtQ * Math.Cos((theta - 2.0 * Math.PI) / 3d) + para.Shift;
            }
            Array.Sort(x);
        }
        public static double GetCubeRoot(double value) {
            return Math.Pow(Math.Abs(value), 1d / 3d) * Math.Sign(value);
        }
    }
}

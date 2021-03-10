using System;
using System.Runtime.Serialization;

namespace Model {
    public static class Equation {
        /// <summary>
        ///     盛金公式求解一元三次方程
        /// </summary>
        /// <param name="a">三次系数</param>
        /// <param name="b">二次系数</param>
        /// <param name="c">一次系数</param>
        /// <param name="d">常系数</param>
        /// <param name="x">结果为null则没有实根</param>
        public static void GetRoot(double a, double b, double c, double d, out double[] x) {
            double Q, R;
            var a0 = d / a;
            var a1 = c / a;
            var a2 = b / a;
            QR(a2, a1, a0, out Q, out R);

            var Q3 = Q * Q * Q;
            var D = Q3 + R * R;
            var shift = -a2 / 3d;

            x = new double[3];
            x[1] = double.NaN;
            x[2] = double.NaN;

            if (D >= 0) {
                // when D >= 0, use eqn (54)-(56) where S and T are real
                var sqrtD = Math.Pow(D, 0.5);
                var S = GetCubeRoot(R + sqrtD);
                var T = GetCubeRoot(R - sqrtD);
                x[0] = shift + (S + T);
                if (D == 0) x[1] = shift - S;
            }
            else {
                // 3 real roots, use eqn (70)-(73) to calculate the real roots
                var theta = Math.Acos(R / Math.Sqrt(-Q3));
                x[0] = 2d * Math.Sqrt(-Q) * Math.Cos(theta / 3.0) + shift;
                x[1] = 2d * Math.Sqrt(-Q) * Math.Cos((theta + 2.0 * Math.PI) / 3d) + shift;
                x[2] = 2d * Math.Sqrt(-Q) * Math.Cos((theta - 2.0 * Math.PI) / 3d) + shift;
            }
            
        }

        private static void QR(double a2, double a1, double a0, out double Q, out double R) {
            Q = (3 * a1 - a2 * a2) / 9.0;
            R = (9.0 * a2 * a1 - 27 * a0 - 2 * a2 * a2 * a2) / 54.0;
        }

        public static double GetCubeRoot(double value) {
            return Math.Pow(Math.Abs(value), 1d / 3d) * Math.Sign(value);
        }
    }

    
    }
}
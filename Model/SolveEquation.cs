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
        /// <param name="root">结果为null则没有实根</param>
        public static void GetRoot(double a, double b, double c, double d, out double root) {
            double Q, R;
            var a0 = d / a;
            var a1 = c / a;
            var a2 = b / a;
            QR(a2, a1, a0, out Q, out R);

            var Q3 = Q * Q * Q;
            var D = Q3 + R * R;
            var shift = -a2 / 3d;

            double x1;
            var x2 = double.NaN;
            var x3 = double.NaN;

            if (D >= 0) {
                // when D >= 0, use eqn (54)-(56) where S and T are real
                var sqrtD = Math.Pow(D, 0.5);
                var S = GetCubeRoot(R + sqrtD);
                var T = GetCubeRoot(R - sqrtD);
                x1 = shift + (S + T);
                if (D == 0) x2 = shift - S;
            }
            else {
                // 3 real roots, use eqn (70)-(73) to calculate the real roots
                var theta = Math.Acos(R / Math.Sqrt(-Q3));
                x1 = 2d * Math.Sqrt(-Q) * Math.Cos(theta / 3.0) + shift;
                x2 = 2d * Math.Sqrt(-Q) * Math.Cos((theta + 2.0 * Math.PI) / 3d) + shift;
                x3 = 2d * Math.Sqrt(-Q) * Math.Cos((theta - 2.0 * Math.PI) / 3d) + shift;
            }

            var rootMax = Math.Max(Math.Max(x1, x2), x3);
            if (rootMax > 0) root = rootMax; //> 0 ? rootMax : double.NaN;
            throw new NoRootException("方程无符合要求的实根！");
        }

        private static void QR(double a2, double a1, double a0, out double Q, out double R) {
            Q = (3 * a1 - a2 * a2) / 9.0;
            R = (9.0 * a2 * a1 - 27 * a0 - 2 * a2 * a2 * a2) / 54.0;
        }

        public static double GetCubeRoot(double value) {
            return Math.Pow(Math.Abs(value), 1d / 3d) * Math.Sign(value);
        }
    }

    [Serializable]
    public class NoRootException : Exception {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public NoRootException() { }
        public NoRootException(string message) : base(message) { }
        public NoRootException(string message, Exception inner) : base(message, inner) { }

        protected NoRootException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
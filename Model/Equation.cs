
using System;
using System.Collections.Generic;
using System.Numerics;


namespace SolveEquation
{
    public partial class Equation
    {
        public static double A;
        public static double B;
        public static double C;
        public static double Delta;
        public static double X1 , X2, X3;
        public static double DeltaQuadratic;

        /// <summary>
        /// 盛金公式求解一元三次方程
        /// </summary>
        /// <param name="a">三次系数</param>
        /// <param name="b">二次系数</param>
        /// <param name="c">一次系数</param>
        /// <param name="d">常系数</param>
        /// <param name="root">结果为null则没有实根</param>
        public static void GetRoot(double a, double b, double c, double d, out double root) {
            X1 = 0; X2 = 0; X3 = 0;
            A = b * b - 3 * a * c;
            B = b * c - 9 * a * d;
            C = c * c - 3 * b * d;
            Delta = B * B - 4 * A * C;
            if (d == 0) {
                X3 = 0;
                SolveQuadraticEquation(a, b, c);
            }
            else if (A == 0 && B == 0) {
                X1 = -b / (3 * a);
            } //方程有一个三重实根
            else if (Delta > 0) {
                double y1 = (A * b + 3 * a * (-B + Math.Sqrt(Delta)) / 2);
                double y2 = (A * b + 3 * a * (-B - Math.Sqrt(Delta)) / 2);
                X1 = (-b - (GetCubeRoot(y1) + GetCubeRoot(y2))) / (3 * a);
            }  //方程有一个实根和一对共轭虚根
            else if (Delta == 0) {
                var k = B / A;
                X1 = -b / a + k;
                X2 = X3 = -k / 2;
            } //方程有一个实根，其中有一个两重根
            else {
                var T = (2 * A * b - 3 * a * B) / (2 * Math.Sqrt(A * A * A));
                var angle = Math.Acos(T) / 3;
                X1 = (-b - 2 * Math.Sqrt(A) * Math.Cos(angle)) / (3 * a);
                X2 = ((-b + Math.Sqrt(A) * (Math.Cos(angle) + Math.Sqrt(3) * Math.Sin(angle))) / (3 * a));
                X3 = ((-b + Math.Sqrt(A) * (Math.Cos(angle) - Math.Sqrt(3) * Math.Sin(angle))) / (3 * a));
            }  //方程有三个不相等的实根 

            var rootMax = Math.Max(Math.Max(X1, X2), X3);
            root = (rootMax > 0) ? rootMax : double.NaN;
        }

        public static void SolveQuadraticEquation(double a, double b, double c) {
            DeltaQuadratic = b * b - 4 * a * c;
            if (DeltaQuadratic == 0) {
                X1 = X2 = -b / (2 * a);
            }
            else if (DeltaQuadratic > 0) {
                X1 = -b + Math.Sqrt(DeltaQuadratic) / (2 * a);
                X2 = -b - Math.Sqrt(DeltaQuadratic) / (2 * a);
            }
        }

        public static double GetCubeRoot(double value) {
            if (value < 0)
                return (double) (-Math.Pow(-value, 1f / 3));
            else if (value == 0)
                return 0;
            else
                return (double) (Math.Pow(value, 1f / 3));
        }



    }
}


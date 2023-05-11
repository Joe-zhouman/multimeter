// Joe
// 周漫
// 2021012419:33

using System;

namespace Model.Probe {
    public class CubicPolyProbe : ProbeBase {
        private CubeRootPara _cubeRootPara;

        public CubicPolyProbe() {
            Paras = new[] { 0.0, 0.0, 0.0, 0.0 };
        }

        public override void SetTemp(double para) {
            Equation.GetRoot(_cubeRootPara, (Paras[0] - para) / Paras[3], out var root);
            for (int i = 0; i < 3; i++) {
                try {
                    Temp = root[i];
                }
                catch (ValOutOfRangeException) {
                    continue;
                }
                return;
            }

            if (root[2] > TempUb) {
                throw new ValOutOfRangeException(ValOutOfRangeType.GREATER_THAN);
            }
            throw new ValOutOfRangeException(ValOutOfRangeType.LESS_THAN);

        }
        /// <summary>
        /// 初始化求解三次方程所需的参数.
        /// </summary>
        /// 运行过程中从头计算较费时间.
        /// 注意到一旦探头参数确定,求解过程中很多中间参数已经确定,因此先将一些中间参数计算出来.
        /// 本质上是用空间换时间
        public override void Init() {
            _cubeRootPara = new CubeRootPara(Paras[3], Paras[2], Paras[1]);
        }
    }
    /// <summary>
    /// 求解三次方程所需的参数
    /// </summary>
    public readonly struct CubeRootPara {
        public double Q3 { get; }
        public double R0 { get; }
        public double SqrtQ { get; }
        public double SqrtQ3 { get; }
        public double Shift { get; }

        public CubeRootPara(double p3, double p2, double p1) {
            double a1 = p1 / p3;
            double a2 = p2 / p3;
            double q = (3 * a1 - a2 * a2) / 9.0;
            Q3 = q * q * q;
            R0 = (9.0 * a2 * a1 - 2 * a2 * a2 * a2) / 54.0;
            Shift = -a2 / 3d;
            if (q <= 0) {
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
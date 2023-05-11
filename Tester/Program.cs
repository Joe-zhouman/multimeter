using Model.Probe;
using System;

namespace Tester {
    internal class Program {
        private static void Main(string[] args) {
            ProbeBase x = new CubicPolyResistanceProbe();
            x.TempLb = 0;
            x.TempUb = 700;
            x.Paras[0] = 99.0;
            x.Paras[1] = 0.39;
            x.Paras[2] = -9e-5;
            x.Paras[3] = 3.45e-8;
            x.Init();
            x.SetTemp(151);
            Console.WriteLine($"{x.Temp}");
            //x.Temp = double.NaN;
            //Console.WriteLine($"{double.NaN==double.NaN}");
        }

    }
}



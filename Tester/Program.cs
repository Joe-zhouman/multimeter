using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using log4net.Core;
using SolveEquation;
using System.Numerics;
using Complex = System.Numerics.Complex;
using Model;
namespace Tester {
    internal class Program {
        private static void Main(string[] args) {
            Probe x = new CubicPolyResistanceProbe();
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



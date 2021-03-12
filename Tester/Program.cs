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
            Probe x = new Voltage();
            x.TempLb = 0;
            x.TempUb = 100;
            //x.Temp = double.NaN;
            Console.WriteLine($"{double.NaN==double.NaN}");
        }

    }
}



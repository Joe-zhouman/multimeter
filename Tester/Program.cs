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
namespace Tester {
    internal class Program {
        private static void Main(string[] args) {
            double[] paras = {1.0, 1.0, 1.0, 1.0};
            
            Stopwatch sw2 = new Stopwatch();
            sw2.Start();
            Model.Equation.GetRoot(1,1,1,1,out double root);
            sw2.Stop();
            TimeSpan ts2 = sw2.Elapsed;
            Console.WriteLine(root);
            Console.WriteLine(ts2);
        }

    }
}



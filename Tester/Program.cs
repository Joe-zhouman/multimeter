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
using DataAccess;
namespace Tester {
    internal class Program {
        private static void Main(string[] args) {
            var result =
                GetUpdateInfo.Get("https://api.github.com/repos/Joe-zhouman/multimeter-public/releases/latest");
            Console.WriteLine(result);
        }

    }
}



using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DataProcessor;
using multimeter;
using log4net;
using log4net.Core;

namespace Tester {
    internal class Program {
        private static void Main(string[] args) {
            ILog logger = LogManager.GetLogger("TesterLogger");
            double[] a = {1, 2};
            for (int i = 0; i < 10; i++) {
                try {
                    Console.WriteLine(a[i]);
                }
                catch (Exception exception) {
                    logger.Error("test",exception);
                }
                
            }
        }
    }

    enum MyEnum {
        Red,Black
    }
}
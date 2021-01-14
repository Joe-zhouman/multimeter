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
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += MyHandler;
            ILog logger = LogManager.GetLogger("TesterLogger");
            double[] a = {1, 2};
            for (int i = 0; i < 3; i++) {
                Console.WriteLine(a[i]);
            }
        }
        private static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            ILog logger = LogManager.GetLogger("Unexcepted Exception");
            logger.Fatal(e);
        }
    }

    enum MyEnum {
        Red,Black
    }
}
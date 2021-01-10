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
            ILog logger = LogManager.GetLogger("MultimeterLog");
            for (int i = 0; i < 10; i++) {
                logger.Error("test");
            }
        }
    }

    enum MyEnum {
        Red,Black
    }
}
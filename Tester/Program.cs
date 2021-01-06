using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DataProcessor;
using multimeter;

namespace Tester {
    internal class Program {
        private static void Main(string[] args) {
            List<int> x = new List<int>() {1, 1, 1, 1, 1};
            var cumSum = x.Select((_, i) => x.Take(i+1).Sum()).ToList();
            foreach (int i in cumSum) {
                Console.WriteLine(i.ToString());

            }

            Console.WriteLine(TestMethod.ITC.ToString());
        }
    }

    enum MyEnum {
        Red,Black
    }
}
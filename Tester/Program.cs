using System;
using System.Data;
using DataProcessor;

namespace Tester {
    internal class Program {
        private static void Main(string[] args) {
            var dt = new DataTable();
            var e = Solution.ReadCsvFile(ref dt,
                @"C:\Users\Joe\source\Joe-zhouman\multimeter\multimeter\bin\Debug\AutoSave\AutoSave-2020-10-29-21-27-09.2515.csv");
            if (e == null) {
                foreach (DataRow row in dt.Rows) {
                    foreach (DataColumn column in dt.Columns)
                        Console.Write("\t{0}", row[column].GetType());

                    Console.WriteLine();
                }
            } else {
                throw e;
            }

            var t = Solution.CalAve(dt);
            foreach (var d in t) {
                Console.WriteLine("{0}:{1}",d.Key,d.Value);
            }

        }
    }

    enum MyEnum {
        Red,Black
    }
}
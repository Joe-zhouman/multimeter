// Joe
// 周漫
// 2020110210:34

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DataProcessor {
    public static class Solution {
        public static Exception ReadCsvFile(ref DataTable channelTable, string fileName) {
            try {

                string strLine;
                StreamReader sw = new StreamReader(fileName, System.Text.Encoding.Default);
                if (null != (strLine = sw.ReadLine())) {
                    var aryLine = strLine.Split(new[]{','},StringSplitOptions.RemoveEmptyEntries);
                    foreach (var t in aryLine) {
                        var tempCol = new DataColumn("ch" + t) {DataType = Type.GetType("System.Double")};
                        channelTable.Columns.Add(tempCol);
                    }
                }
                while (null!=(strLine = sw.ReadLine())) {
                    var aryLine = strLine.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var tempRow = channelTable.NewRow();
                    for (int i = 0; i < aryLine.Length; i++) {
                        tempRow[i] = double.Parse(aryLine[i]);
                    }
                    channelTable.Rows.Add(tempRow);
                }

                return null;

            } catch (Exception e) {

                return e;
            }
        }//读取CSV文件

        public static Dictionary<string,double> CalAve(DataTable channelTable) {
            var testResult = new Dictionary<string, double>();
            var count = channelTable.Columns[0].ColumnName;
            testResult.Add("TestMethod",double.Parse(count.Last().ToString()));
            string filter = "true";
            if (channelTable.Rows.Count > 50) {
                filter = count + " > " + (channelTable.Rows.Count - 50);
            }
            for (int i = 1; i < channelTable.Columns.Count; i++) {
                string expression = "Avg(" + channelTable.Columns[i].ColumnName + ")";
                double temp = (double)channelTable.Compute(expression, filter);
                testResult.Add(channelTable.Columns[i].ColumnName.TrimStart('c','h'),temp);
            }
            return testResult;
        }//求平均值

        public static bool LinearFit(List<double> x, List<double> y, ref double k, ref double b,ref double err) {
            if (x.Count != y.Count) {
                return false;
            }

            if (x.Count < 2) {
                return false;
            }
            var aveX = x.Average();
            var aveY = y.Average();
            List<double> xDotY = new List<double>();
            List<double> xSquare = new List<double>();
            for (int i = 0; i < x.Count; i++) {
                xDotY[i] = x[i] * y[i];
                xSquare[i] = x[i] * x[i];
            }

            try {
                k = (xDotY.Average() - aveX * aveY) / (xSquare.Average() - aveX * aveX);
                b = aveY - k * aveX;
                return true;
            }
            catch (Exception ) {
                return false;
            }
        }
    }
}
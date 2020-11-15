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

        public static bool LinearFit(List<double> x, List<double> y, ref double k, ref double b) {
            if (x.Count != y.Count) {
                return false;
            }

            if (x.Count < 2) {
                return false;
            }
            var aveX = x.Average();
            var aveY = y.Average();

            List<double> xDotY = x.Select(t => t * t).ToList();
            List<double> xSquare = x.Select((t,i)=>t*y[i]).ToList();
            

            try {
                k = (xDotY.Average() - aveX * aveY) / (xSquare.Average() - aveX * aveX);
                b = aveY - k * aveX;
                var stdX = GetStd(x,aveX);
                var stdY = GetStd(y, aveY);
                var err=x.Select((t, i) => (t - aveX) * (y[i] - aveY)).ToList().Sum()/x.Count/stdX/stdY;
                
                return err < 0.7;
            }
            catch (Exception ) {
                return false;
            }
        }//线性拟合

        public static bool GetResults(HeatMeter heatMeter1, HeatMeter heatMeter2,ref Sample sample1,ref Sample sample2,ref double itc) {
            double heatFlow1 = 0.0;
            double heatFlow2 = 0.0;
            double[] k = new double[4];
            double[] b = new double[4];
            if(true != GetHeatFlow(heatMeter1,ref heatFlow1,ref k[0],ref b[0]))
            {
                return false;
            }
            if(true != GetHeatFlow(heatMeter2, ref heatFlow2, ref k[3], ref b[3]))
            {
                return false;
            }
            if(Math.Abs(1-heatFlow1/heatFlow1) > 0.2)
            {
                return false;
            }
            heatFlow1 = (heatFlow1 + heatFlow2) / 2;
            var samplePosition1 = sample1.Position.Select(double.Parse).Reverse().ToList();
            var sampleLength1 = samplePosition1.Select((_, i) => samplePosition1.Take(i + 1).Sum()).ToList();
            if (true != LinearFit(samplePosition1, sample1.Temp.ToList(), ref k[1], ref b[1]))
            {
                return false;
            }
            sample1.Kappa = (heatFlow1 / Math.PI / Math.Pow(double.Parse(sample1.Diameter), 2) / k[1]).ToString();

            var samplePosition2 = sample2.Position.Select(double.Parse).ToList();
            var sampleLength2 = samplePosition2.Select((_, i) => samplePosition2.Take(i + 1).Sum()).ToList();
            if (true != LinearFit(samplePosition1, sample1.Temp.ToList(), ref k[2], ref b[2]))
            {
                return false;
            }
            sample2.Kappa = (heatFlow1 / Math.PI / Math.Pow(double.Parse(sample1.Diameter), 2) / k[2]).ToString();
            itc = (b[1] - b[2]) / heatFlow1 * 1000;
            return true;
        }

        public static bool GetHeatFlow(HeatMeter heatMeter,ref double heatFlow,ref double k,ref double b) {
            var numPosition = heatMeter.Position.Select(double.Parse).ToList();
            var p = numPosition.Select((_, i) => numPosition.Take(i + 1).Sum()).ToList();
            if (true == LinearFit(numPosition,heatMeter.Temp.ToList(),ref k,ref b)) {
                heatFlow = double.Parse(heatMeter.Kappa) * Math.PI * Math.Pow(double.Parse(heatMeter.Diameter), 2) * k;
            }
            return false;
            
        } 

        public static double GetStd(List<double> x, double aveX) {
            var sum = x.Sum(d => (d - aveX));
            return Math.Sqrt(sum / x.Count);
        }
    }
}
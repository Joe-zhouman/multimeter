﻿// Joe
// 周漫
// 2020110210:34

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DataProcessor {
    public static class Solution {
        public static Exception ReadData(ref Dictionary<string, double> testResult, string[] channelList,
            string filePath) {
            var dataPoints = int.Parse(INIHelper.Read("Data", "save_interval", "0", filePath));
            try {
                var temp = new string[dataPoints];
                for (var i = 0; i < dataPoints; i++) temp[i] = INIHelper.Read("Data", i.ToString(), "", filePath);

                var aveTemp = AveTemp(temp, channelList.Length);
                for (var i = 0; i < channelList.Length; i++) testResult.Add(channelList[i], Math.Round(aveTemp[i], 2));
                return null;
            }
            catch (Exception e) {
                return e;
            }
        }

        public static double[] AveTemp(string[] temp, int numChannel) {
            var aveTemp = new double[numChannel];
            foreach (var tempList in temp.Select(t =>
                t.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).ToArray()))
                for (var j = 0; j < numChannel; j++)
                    aveTemp[j] += tempList[j + 1];
            for (var i = 0; i < numChannel; i++) aveTemp[i] /= temp.Length;
            return aveTemp;
        }
        ///// <summary>
        /////     读取程序自动保存的CSV文件,并保存至DataTable中
        ///// </summary>
        ///// <param name="channelTable">要保存dataTable,引用类型</param>
        ///// <param name="fileName">CSV文件名</param>
        ///// <returns>若出现异常,返回异常,否则,返回null</returns>
        //public static Exception ReadCsvFile(ref DataTable channelTable, string fileName) {
        //    try {
        //        string strLine;
        //        StreamReader sw = new StreamReader(fileName, Encoding.Default);
        //        if (null != (strLine = sw.ReadLine())) {
        //            string[] aryLine = strLine.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
        //            foreach (string t in aryLine) {
        //                DataColumn tempCol = new DataColumn("ch" + t) {DataType = Type.GetType("System.Double")};
        //                channelTable.Columns.Add(tempCol);
        //            }
        //        }

        //        while (null != (strLine = sw.ReadLine())) {
        //            string[] aryLine = strLine.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
        //            DataRow tempRow = channelTable.NewRow();
        //            for (int i = 0; i < aryLine.Length; i++) tempRow[i] = double.Parse(aryLine[i]);
        //            channelTable.Rows.Add(tempRow);
        //        }

        //        return null;
        //    }
        //    catch (Exception e) {
        //        return e;
        //    }
        //}

        ///// <summary>
        /////     计算各通道结果的平均值
        ///// </summary>
        ///// <param name="channelTable">数据所在的DataTable</param>
        ///// <returns>字典{频道名,平均值},并包含文件对于的测试方法</returns>
        //public static Dictionary<string, double> CalAve(DataTable channelTable) {
        //    Dictionary<string, double> testResult = new Dictionary<string, double>();
        //    string count = channelTable.Columns[0].ColumnName;
        //    testResult.Add("TestMethod", double.Parse(count.Last().ToString()));
        //    string filter = "true";
        //    if (channelTable.Rows.Count > 50) filter = count + " > " + (channelTable.Rows.Count - 50);
        //    for (int i = 1; i < channelTable.Columns.Count; i++) {
        //        string expression = "Avg(" + channelTable.Columns[i].ColumnName + ")";
        //        double temp = (double) channelTable.Compute(expression, filter);
        //        testResult.Add(channelTable.Columns[i].ColumnName.TrimStart('c', 'h'), temp);
        //    }

        //    return testResult;
        //} //求平均值

        /// <summary>
        ///     线性拟合
        /// </summary>
        /// <param name="x">x数据</param>
        /// <param name="y">y数据</param>
        /// <param name="k">拟合直线斜率,引用类型</param>
        /// <param name="b">拟合参数截距,引用类型</param>
        /// <returns>拟合成功,返回true;拟合失败或拟合误差过大,返回false</returns>
        public static bool LinearFit(double[] x, double[] y, ref double k, ref double b) {
            if (x.Length != y.Length) return false;
            if (x.Length < 3) return false;

            var aveX = x.Average();
            var aveY = y.Average();

            var xSquare = x.Select(t => t * t).ToArray();
            var xDotY = x.Select((t, i) => t * y[i]).ToArray();

            var err = 0.0;
            try {
                k = (xDotY.Average() - aveX * aveY) / (xSquare.Average() - aveX * aveX);
                b = aveY - k * aveX;
                var stdX = GetStd(x, aveX);
                var stdY = GetStd(y, aveY);
                err = x.Select((t, i) => (t - aveX) * (y[i] - aveY)).ToArray().Sum() / x.Length / stdX / stdY;
            }
            catch (Exception) {
                // ignored
            }

            return err > 0.7;
        } //线性拟合

        /// <summary>
        ///     计算试件热导率
        /// </summary>
        /// <param name="heatMeter1"></param>
        /// <param name="heatMeter2"></param>
        /// <param name="sample1"></param>
        /// <returns></returns>
        public static bool GetResults(HeatMeter heatMeter1, HeatMeter heatMeter2, ref Sample sample1) {
            var k = new double[2];
            var b = new double[2];
            var accurate = GetHeatFlow(heatMeter1, heatMeter2, out var heatFlow, ref k, ref b);
            if (!LinearFitUpper(sample1, ref k[0], ref b[0])) accurate = false;
            sample1.Kappa =
                (heatFlow / double.Parse(sample1.Area) / k[0]).ToString("0.000e+0", CultureInfo
                    .InvariantCulture);
            return accurate;
        }

        /// <summary>
        ///     计算试件间及热界面材料接触热阻
        /// </summary>
        /// <param name="heatMeter1"></param>
        /// <param name="heatMeter2"></param>
        /// <param name="sample1"></param>
        /// <param name="sample2"></param>
        /// <param name="itc"></param>
        /// <returns></returns>
        public static bool GetResults(HeatMeter heatMeter1, HeatMeter heatMeter2, ref Sample sample1,
            ref Sample sample2, out double itc) {
            var k = new double[2];
            var b = new double[2];
            var accurate = GetHeatFlow(heatMeter1, heatMeter2, out var heatFlow, ref k, ref b);
            if (!LinearFitUpper(sample1, ref k[0], ref b[0])) accurate = false;
            sample1.Kappa =
                (heatFlow / double.Parse(sample1.Area) / k[0]).ToString("0.000e+0", CultureInfo
                    .InvariantCulture);

            if (!LinearFitLower(sample2, ref k[1], ref b[1])) accurate = false;
            sample2.Kappa =
                (heatFlow / double.Parse(sample2.Area) / k[1]).ToString("0.000e+0", CultureInfo
                    .InvariantCulture);
            itc = (b[0] - b[1]) / heatFlow * (double.Parse(sample1.Area) + double.Parse(sample2.Area)) * 500;
            return accurate;
        } //接触热阻计算


        /// <summary>
        ///     计算热流计间热界面材料接触热阻
        /// </summary>
        /// <param name="heatMeter1"></param>
        /// <param name="heatMeter2"></param>
        /// <param name="itc"></param>
        /// <returns></returns>
        public static bool GetResults(HeatMeter heatMeter1, HeatMeter heatMeter2, out double itc) {
            var k = new double[2];
            var b = new double[2];
            var accurate = GetHeatFlow(heatMeter1, heatMeter2, out var heatFlow, ref k, ref b);
            itc = (b[0] - b[1]) / heatFlow * (double.Parse(heatMeter1.Area) + double.Parse(heatMeter2.Area)) * 500;
            return accurate;
        }

        /// <summary>
        ///     计算热流计热流密度
        /// </summary>
        /// <param name="heatMeter1"></param>
        /// <param name="heatMeter2"></param>
        /// <param name="heatFlow"></param>
        /// <param name="k"></param>
        /// <param name="b"></param>
        /// 上热流计,3个测温点
        /// 下热流计,4个测温点
        /// 热流密度
        /// <returns></returns>
        private static bool GetHeatFlow(HeatMeter heatMeter1, HeatMeter heatMeter2, out double heatFlow, ref double[] k,
            ref double[] b) {
            var accurate = LinearFitUpper(heatMeter1, ref k[0], ref b[0]);

            var heatFlow1 = double.Parse(heatMeter1.Kappa) *
                            double.Parse(heatMeter1.Area) * k[0];
            if (!LinearFitLower(heatMeter2, ref k[1], ref b[1])) accurate = false;
            var heatFlow2 = double.Parse(heatMeter1.Kappa) *
                            double.Parse(heatMeter2.Area) * k[1];
            if (Math.Abs(1 - heatFlow1 / heatFlow2) > 0.4) accurate = false;
            heatFlow = (heatFlow1 + heatFlow2) / 2;
            return accurate;
        }

        private static bool LinearFitLower(Specimen specimen, ref double k, ref double b) {
            var numPosition = specimen.Position.Select(i => double.Parse(i) * -1).ToArray();
            for (var i = 1; i < specimen.TestPoint; i++) numPosition[i] += numPosition[i - 1];

            return LinearFit(numPosition, specimen.Temp.ToArray(), ref k, ref b);
        }

        private static bool LinearFitUpper(Specimen specimen, ref double k, ref double b) {
            var numPosition = specimen.Position.Select(double.Parse).ToArray();
            for (var i = specimen.TestPoint - 2; i >= 0; i--) numPosition[i] += numPosition[i + 1];
            return LinearFit(numPosition, specimen.Temp, ref k, ref b);
        }

        private static double GetStd(double[] x, double aveX) {
            var sum = x.Sum(d => Math.Pow(d - aveX, 2));
            return Math.Sqrt(sum / x.Length);
        }
    }
}
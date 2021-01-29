// Joe
// 周漫
// 2020110210:34

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using DataAccess;
using Model;

namespace BusinessLogic {
    public static class Solution {
        public static void GetTestResult(ref Dictionary<string, double> testResult, string filePath,
            string[] channelList) {
            var aveTemp = AveTemp(ReadFile.ReadData(filePath), channelList.Length);
            for (var i = 0; i < channelList.Length; i++) testResult.Add(channelList[i], Math.Round(aveTemp[i], 2));
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
        public static string LinearFit(double[] x, double[] y, ref double k, ref double b) {
            if (x.Length != y.Length) return @"位置信息和温度不对应";
            if (x.Length < 3) return @"拟合数据过少,拟合结果不可靠";

            var aveX = x.Average();
            var aveY = y.Average();

            var xSquare = x.Select(t => t * t).ToArray();
            var xDotY = x.Select((t, i) => t * y[i]).ToArray();


            k = (xDotY.Average() - aveX * aveY) / (xSquare.Average() - aveX * aveX);
            b = aveY - k * aveX;
            var stdX = GetStd(x, aveX);
            var stdY = GetStd(y, aveY);
            var err = x.Select((t, i) => (t - aveX) * (y[i] - aveY)).ToArray().Sum() / x.Length / stdX / stdY;
            return err > 0.7 ? "" : @"拟合误差过大,请检查位置及温度数据";
        } //线性拟合

        /// <summary>
        ///     计算试件热导率
        /// </summary>
        /// <param name="heatMeter1"></param>
        /// <param name="heatMeter2"></param>
        /// <param name="sample1"></param>
        /// <returns></returns>
        public static string GetResults(ref TestDevice device) {
            var k = new double[2];
            var b = new double[2];
            var area = (double.Parse(device.HeatMeter1.Area)+ double.Parse(device.HeatMeter1.Area))/2;
            var errInfo = GetHeatFlow(device, out double heatFlow,ref k,ref b);
            if (device.Sample1 != null) {
                var errInfoSample1 = LinearFitUpper(device.Sample1, ref k[0], ref b[0]);
                device.Sample1.Kappa =
                    (heatFlow / double.Parse(device.Sample1.Area) / k[0]).ToString("0.000e+0", CultureInfo
                        .InvariantCulture);
                if (errInfoSample1 != "") {
                    errInfo += errInfoSample1 + "（试件1）\n";
                }
            }
            if (device.Sample2 != null)
            {
                var errInfoSample2 = LinearFitLower(device.Sample2, ref k[1], ref b[2]);
                device.Sample2.Kappa =
                    (heatFlow / double.Parse(device.Sample2.Area) / k[0]).ToString("0.000e+0", CultureInfo
                        .InvariantCulture);
                if (errInfoSample2 != "")
                {
                    errInfo += errInfoSample2 + "（试件1）\n";
                }
                area = (double.Parse(device.Sample1.Area) + double.Parse(device.Sample2.Area)) / 2;
            }

            device.Itc = (b[0] - b[1]) / heatFlow * area * 1000;
            if (device.Itm!=null) {
                device.Itm.Kappa = double.Parse(device.Itm.Thickness) / device.Itc;
            }

            return errInfo;
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
        private static string GetHeatFlow(TestDevice device, out double heatFlow, ref double[] k,
            ref double[] b) {
            var errInfo = LinearFitUpper(device.HeatMeter1, ref k[0], ref b[0]);
            if (errInfo!="") {
                errInfo += "（上热流计）\n";
            }
            var heatFlow1 = double.Parse(device.HeatMeter1.Kappa) *
                            double.Parse(device.HeatMeter1.Area) * k[0];
            var errInfo2 = LinearFitLower(device.HeatMeter2, ref k[1], ref b[1]);
            if (errInfo != "") {
                errInfo += errInfo2 + "（上热流计）\n";
            }
            var heatFlow2 = double.Parse(device.HeatMeter2.Kappa) *
                            double.Parse(device.HeatMeter2.Area) * k[1];
            if (Math.Abs(1 - heatFlow1 / heatFlow2) > 0.4) {
                errInfo += @"上下热流计热流相差过大";
            }
            heatFlow = (heatFlow1 + heatFlow2) / 2;
            return errInfo;
        }

        private static string LinearFitLower(Specimen specimen, ref double k, ref double b) {
            var numPosition = specimen.Position.Where(i=>i!="0").Select(i => double.Parse(i) * -1).ToArray();
            for (var i = 1; i < numPosition.Length; i++) numPosition[i] += numPosition[i - 1];

            return LinearFit(numPosition, specimen.Temp.ToArray(), ref k, ref b);
        }

        private static string LinearFitUpper(Specimen specimen, ref double k, ref double b) {
            var numPosition = specimen.Position.Where(i => i != "0").Select(double.Parse).ToArray();
            for (var i = numPosition.Length - 2; i >= 0; i--) numPosition[i] += numPosition[i + 1];
            return LinearFit(numPosition, specimen.Temp.ToArray(), ref k, ref b);
        }

        private static double GetStd(double[] x, double aveX) {
            var sum = x.Sum(d => Math.Pow(d - aveX, 2));
            return Math.Sqrt(sum / x.Length);
        }
    }
}
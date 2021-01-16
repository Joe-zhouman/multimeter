// Joe
// 周漫
// 2020110210:34

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace DataProcessor {
    public static class Solution {
        public static Exception ReadData(ref Dictionary<string, double>testResult,string[] channelList, string filePath) {
            int dataPoints = int.Parse(INIHelper.Read("Data", "save_interval", "0", filePath));
            try {
                string[] temp = new string[dataPoints];
                for (int i = 0; i < dataPoints; i++) {
                    temp[i] = INIHelper.Read("Data", i.ToString(), "", filePath);
                }

                double[] aveTemp = AveTemp(temp, channelList.Length);
                for (int i = 0; i < channelList.Length; i++) {
                    testResult.Add(channelList[i],Math.Round( aveTemp[i],2));
                }   
                return null;
            }
            catch (Exception e) {
                return e;
            }
        }

        public static double[] AveTemp(string[] temp,int numChannel) {
            double[] aveTemp = new double[numChannel];
            foreach (double[] tempList in temp.Select(t => t.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).ToArray())) {
                for (int j = 0; j < numChannel; j++)
                {
                    aveTemp[j] += tempList[j + 1];
                }
            }
            for (int i = 0; i < numChannel; i++) {
                aveTemp[i] /= temp.Length;
            }
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

            if (x.Length < 2) return false;
            double aveX = x.Average();
            double aveY = y.Average();

            double[] xDotY = x.Select(t => t * t).ToArray();
            double[] xSquare = x.Select((t, i) => t * y[i]).ToArray();


            try {
                k = (xDotY.Average() - aveX * aveY) / (xSquare.Average() - aveX * aveX);
                b = aveY - k * aveX;
                double stdX = GetStd(x, aveX);
                double stdY = GetStd(y, aveY);
                double err = x.Select((t, i) => (t - aveX) * (y[i] - aveY)).ToArray().Sum() / x.Length / stdX / stdY;

                return err < 0.7;
            }
            catch (Exception) {
                return false;
            }
        } //线性拟合

        /// <summary>
        ///     计算试件热导率
        /// </summary>
        /// <param name="heatMeter1"></param>
        /// <param name="heatMeter2"></param>
        /// <param name="sample1"></param>
        /// <returns></returns>
        public static bool GetResults(HeatMeter heatMeter1, HeatMeter heatMeter2, ref Sample sample1) {
            double heatFlow = 0.0;
            double[] k = new double[2];
            double[] b = new double[2];
            if (!GetHeatFlow(heatMeter1, heatMeter2, ref heatFlow, ref k, ref b)) return false;
            double[] samplePosition1 = sample1.Position.Select(double.Parse).ToArray();
            double[] sampleLength1 = samplePosition1.Select((_, i) => samplePosition1.Take(i + 1).Sum()).ToArray();
            if (true != LinearFit(sampleLength1, sample1.Temp, ref k[0], ref b[0])) return false;
            sample1.Kappa =
                (heatFlow / double.Parse(sample1.Area) / k[0]).ToString(CultureInfo
                    .InvariantCulture);
            return true;
        }

        /// <summary>
        ///     计算试件间接触热阻
        /// </summary>
        /// <param name="heatMeter1"></param>
        /// <param name="heatMeter2"></param>
        /// <param name="sample1"></param>
        /// <param name="sample2"></param>
        /// <param name="itc"></param>
        /// <returns></returns>
        public static bool GetResults(HeatMeter heatMeter1, HeatMeter heatMeter2, ref Sample sample1,
            ref Sample sample2, ref double itc) {
            double heatFlow = 0.0;
            double[] k = new double[2];
            double[] b = new double[2];
            if (!GetHeatFlow(heatMeter1, heatMeter2, ref heatFlow, ref k, ref b)) return false;
            double[] samplePosition1 = sample1.Position.Select(double.Parse).Reverse().ToArray();
            double[] sampleLength1 = samplePosition1.Select((_, i) => samplePosition1.Take(i + 1).Sum()).ToArray();
            if (true != LinearFit(sampleLength1, sample1.Temp, ref k[0], ref b[0])) return false;
            sample1.Kappa =
                (heatFlow / double.Parse(sample1.Area) / k[0]).ToString(CultureInfo
                    .InvariantCulture);

            double[] samplePosition2 = sample2.Position.Select(double.Parse).ToArray();
            double[] sampleLength2 = samplePosition2.Select((_, i) => samplePosition2.Take(i + 1).Sum()).ToArray();
            if (true != LinearFit(sampleLength2, sample1.Temp, ref k[1], ref b[1])) return false;
            sample2.Kappa =
                (heatFlow / double.Parse(sample1.Area) / k[1]).ToString(CultureInfo
                    .InvariantCulture);
            itc = (b[0] - b[1]) / heatFlow * 1000;
            return true;
        } //接触热阻计算

        /// <summary>
        ///     计算试件间热界面材料热导率
        /// </summary>
        /// <param name="heatMeter1"></param>
        /// <param name="heatMeter2"></param>
        /// <param name="sample1"></param>
        /// <param name="sample2"></param>
        /// <param name="thickness"></param>
        /// <param name="itmKappa"></param>
        /// <returns></returns>
        public static bool GetResults(HeatMeter heatMeter1, HeatMeter heatMeter2, ref Sample sample1,
            ref Sample sample2, double thickness, ref double itmKappa) {
            double itc = 0.0;
            if (!GetResults(heatMeter1, heatMeter2, ref sample1, ref sample2, ref itc)) return false;
            itmKappa = thickness / itc;
            return true;
        }

        /// <summary>
        ///     计算热流计间热界面材料热导率
        /// </summary>
        /// <param name="heatMeter1"></param>
        /// <param name="heatMeter2"></param>
        /// <param name="thickness"></param>
        /// <param name="itmKappa"></param>
        /// <returns></returns>
        public static bool GetResults(HeatMeter heatMeter1, HeatMeter heatMeter2, double thickness,
            ref double itmKappa) {
            double heatFlow = 0.0;
            double[] k = new double[2];
            double[] b = new double[2];
            if (!GetHeatFlow(heatMeter1, heatMeter2, ref heatFlow, ref k, ref b)) return false;
            itmKappa = thickness / (b[0] - b[1]) * heatFlow / 1000;
            return true;
        }

        /// <summary>
        ///     计算热流计热流密度
        /// </summary>
        /// <param name="heatMeter1"></param>
        /// 上热流计,四个测温点
        /// <param name="heatMeter2"></param>
        /// 下热流计,三个测温点
        /// <param name="heatFlow"></param>
        /// 热流密度
        /// <param name="k"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static bool GetHeatFlow(HeatMeter heatMeter1, HeatMeter heatMeter2, ref double heatFlow, ref double[] k,
            ref double[] b) {
            double[] numPosition = heatMeter1.Position.Select(double.Parse).ToArray();
            double[] p = numPosition.Select((_, i) => numPosition.Take(i + 1).Sum()).ToArray();

            if (!LinearFit(p, heatMeter1.Temp, ref k[0], ref b[0])) return false;
            double heatFlow1 = double.Parse(heatMeter1.Kappa) * Math.PI *
                               double.Parse(heatMeter1.Area) * k[0];
            numPosition = heatMeter2.Position.Select(double.Parse).ToArray();
            p = numPosition.Select((_, i) => numPosition.Take(i + 1).Sum()).ToArray();
            if (!LinearFit(p, heatMeter2.Temp.ToArray(), ref k[1], ref b[1])) return false;
            double heatFlow2 = double.Parse(heatMeter1.Kappa) * Math.PI *
                               double.Parse(heatMeter1.Area) * k[1];
            if (Math.Abs(1 - heatFlow1 / heatFlow1) > 0.2) return false;
            heatFlow = (heatFlow1 + heatFlow2) / 2;
            return true;
        }

        private static double GetStd(double[] x, double aveX) {
            double sum = x.Sum(d => d - aveX);
            return Math.Sqrt(sum / x.Length);
        }
    }
}
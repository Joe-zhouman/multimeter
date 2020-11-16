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
        /// <summary>
        /// 读取程序自动保存的CSV文件,并保存至DataTable中
        /// </summary>
        /// <param name="channelTable">要保存dataTable,引用类型</param>
        /// <param name="fileName">CSV文件名</param>
        /// <returns>若出现异常,返回异常,否则,返回null</returns>
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
        }

        /// <summary>
        /// 计算各通道结果的平均值
        /// </summary>
        /// <param name="channelTable">数据所在的DataTable</param>
        /// <returns>字典{频道名,平均值},并包含文件对于的测试方法</returns>
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

        /// <summary>
        /// 线性拟合
        /// </summary>
        /// <param name="x">x数据</param>
        /// <param name="y">y数据</param>
        /// <param name="k">拟合直线斜率,引用类型</param>
        /// <param name="b">拟合参数截距,引用类型</param>
        /// <returns>拟合成功,返回true;拟合失败或拟合误差过大,返回false</returns>
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
        
        /// <summary>
        /// 计算试件热导率
        /// </summary>
        /// <param name="heatMeter1"></param>
        /// <param name="heatMeter2"></param>
        /// <param name="sample1"></param>
        /// <returns></returns>
        public static bool GetResults(HeatMeter heatMeter1, HeatMeter heatMeter2, ref Sample sample1)
        {
            double heatFlow1 = 0.0;
            double heatFlow2 = 0.0;
            double k = 0.0;
            double b = 0.0;
            if (true != GetHeatFlow(heatMeter1, ref heatFlow1,ref k, ref b))
            {
                return false;
            }
            if (true != GetHeatFlow(heatMeter2, ref heatFlow2,ref k, ref b))
            {
                return false;
            }
            if (Math.Abs(1 - heatFlow1 / heatFlow1) > 0.2)
            {
                return false;
            }
            heatFlow1 = (heatFlow1 + heatFlow2) / 2;
            var samplePosition1 = sample1.Position.Select(double.Parse).Reverse().ToList();
            var sampleLength1 = samplePosition1.Select((_, i) => samplePosition1.Take(i + 1).Sum()).ToList();
            if (true != LinearFit(samplePosition1, sample1.Temp.ToList(), ref k, ref b))
            {
                return false;
            }
            sample1.Kappa = (heatFlow1 / Math.PI / Math.Pow(double.Parse(sample1.Diameter), 2) / k).ToString();
            return true;
        }
        /// <summary>
        /// 计算试件间接触热阻
        /// </summary>
        /// <param name="heatMeter1"></param>
        /// <param name="heatMeter2"></param>
        /// <param name="sample1"></param>
        /// <param name="sample2"></param>
        /// <param name="itc"></param>
        /// <returns></returns>
        public static bool GetResults(HeatMeter heatMeter1, HeatMeter heatMeter2,ref Sample sample1,ref Sample sample2,ref double itc) {
            double heatFlow1 = 0.0;
            double heatFlow2 = 0.0;
            double[] k = new double[2];
            double[] b = new double[2];
            if(true != GetHeatFlow(heatMeter1,ref heatFlow1,ref k[0],ref b[0]))
            {
                return false;
            }
            if(true != GetHeatFlow(heatMeter2, ref heatFlow2,ref k[1],ref k[1]))
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
            if (true != LinearFit(samplePosition1, sample1.Temp.ToList(), ref k[0], ref b[0]))
            {
                return false;
            }
            sample1.Kappa = (heatFlow1 / Math.PI / Math.Pow(double.Parse(sample1.Diameter), 2) / k[0]).ToString();

            var samplePosition2 = sample2.Position.Select(double.Parse).ToList();
            var sampleLength2 = samplePosition2.Select((_, i) => samplePosition2.Take(i + 1).Sum()).ToList();
            if (true != LinearFit(samplePosition1, sample1.Temp.ToList(), ref k[1], ref b[1]))
            {
                return false;
            }
            sample2.Kappa = (heatFlow1 / Math.PI / Math.Pow(double.Parse(sample1.Diameter), 2) / k[1]).ToString();
            itc = (b[0] - b[1]) / heatFlow1 * 1000;
            return true;
        }//接触热阻计算
        /// <summary>
        /// 计算试件间热界面材料热导率
        /// </summary>
        /// <param name="heatMeter1"></param>
        /// <param name="heatMeter2"></param>
        /// <param name="sample1"></param>
        /// <param name="sample2"></param>
        /// <param name="thickness"></param>
        /// <param name="itmKappa"></param>
        /// <returns></returns>
        public static bool GetResults(HeatMeter heatMeter1, HeatMeter heatMeter2, ref Sample sample1, ref Sample sample2, double thickness, ref double itmKappa)
        {
            double itc = 0.0;
            if (true != GetResults(heatMeter1, heatMeter2, ref sample1, ref sample2, ref itc))
            {
                return false;
            }
            itmKappa = thickness / itc;
            return true;
        }
        /// <summary>
        /// 计算热流计间热界面材料热导率
        /// </summary>
        /// <param name="heatMeter1"></param>
        /// <param name="heatMeter2"></param>
        /// <param name="thickness"></param>
        /// <param name="itmKappa"></param>
        /// <returns></returns>
        public static bool GetResults(HeatMeter heatMeter1, HeatMeter heatMeter2, double thickness, ref double itmKappa)
        {
            double heatFlow1 = 0.0;
            double heatFlow2 = 0.0;
            double[] k = new double[2];
            double[] b = new double[2];
            if (true != GetHeatFlow(heatMeter1, ref heatFlow1, ref k[0], ref b[0]))
            {
                return false;
            }
            if (true != GetHeatFlow(heatMeter2, ref heatFlow2, ref k[1], ref k[1]))
            {
                return false;
            }
            if (Math.Abs(1 - heatFlow1 / heatFlow1) > 0.2)
            {
                return false;
            }
            heatFlow1 = (heatFlow1 + heatFlow2) / 2;
            itmKappa = thickness / (b[0] - b[1]) * heatFlow1 / 1000;
            return true;
        }
        /// <summary>
        /// 计算热流计热流密度
        /// </summary>
        /// <param name="heatMeter"></param>
        /// <param name="heatFlow"></param>
        /// <param name="k"></param>
        /// <param name="b"></param>
        /// <returns></returns>
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace DataProcessor
{
    public static class DataMonitor
    {
        public static string StableTemp(string[,] TOrigin0, double stableDif, string state) {

            double[] max = new double[TOrigin0.GetLength(1)];
            double[] min = new double[TOrigin0.GetLength(1)];
            for (int j = 0; j < TOrigin0.GetLength(1); j++)
                for (int i = 0; i < TOrigin0.GetLength(0); i++){
                    max[j] = Math.Max(max[j], double.Parse(TOrigin0[i, j]));
                    min[j] = Math.Min(min[j], double.Parse(TOrigin0[i, j]));
                }

            state = "stable";
            for (int j = 0; j < TOrigin0.GetLength(1); j++){
                if ((max[j] - min[j]) > stableDif) state = "unstable";
            }
            return state;
        }//判断温度是否稳定


    }
}

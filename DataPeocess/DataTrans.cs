using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multimeter
{
    public static class DataTrans
    {
        public static double[] Test1DataTrans(string [,] TOrigin0, double [] alpha, double [] T0,
                                   double[] T){
            double[] t = new double[TOrigin0.GetLength(1)];
            for(int j=0;j< TOrigin0.GetLength(1); j++)
                for (int i = 0; i < TOrigin0.GetLength(0); i++)
                    t[j] = t[j] + double.Parse(TOrigin0[i, j]);

            for (int j = 0; j < TOrigin0.GetLength(1); j++)
                T[j] = alpha[j]*(t[j] / TOrigin0.GetLength(0))+ T0[j];
            return T;
        }//热电偶电压值转为温度



    }
}

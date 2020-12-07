using System;
using System.Collections.Generic;
using System.Linq;

namespace DataProcessor {
    /// <summary>
    /// 测试件基类
    /// </summary>
    public class Specimen {
        protected Specimen(string name,int testPoint) {
            Name = name;
            TestPoint = testPoint;

            Kappa = "10.0";
            Diameter = "10.0";
            
            Channel = Enumerable.Repeat("201",TestPoint).ToArray();
            Position = Enumerable.Repeat("1", TestPoint).ToArray();
            Alpha = Enumerable.Repeat("1", TestPoint).ToArray();
            Beta = Enumerable.Repeat("1", TestPoint).ToArray();
            Theta = Enumerable.Repeat("1", TestPoint).ToArray();
            Temp = Enumerable.Repeat(0.0, TestPoint).ToArray();
        }

        public string Name { get; set; }
        public string Kappa { get; set; }
        public string Diameter { get; set; }
        public string[] Channel { get; set; }
        public string[] Position { get; set; }
        public string[] Alpha { get; set; }
        public string[] Beta { get; set; }
        public string[] Theta { get; set; }
        public double[] Temp { get; set; }
        public int TestPoint { get; set; }

        public void ReadFromIni(string filePath) {
            for (var i = 0; i < TestPoint; i++)
            {
                Channel[i] = INIHelper.Read($"{Name}.{i}", "channel", "201", filePath);
                Position[i] = INIHelper.Read($"{Name}.{i}", "position", "10.0", filePath);
            }

            Kappa = INIHelper.Read(Name, "kappa", "10.0", filePath);
            Diameter = INIHelper.Read(Name, "diameter", "10.0", filePath);
        }

        public virtual void SaveToIni(string filePath) {
            INIHelper.Write(Name, "diameter", Diameter, filePath);
            for (var i = 0; i < TestPoint; i++)
            {
                INIHelper.Write($"{Name}.{i}", "channel", Channel[i], filePath);
                INIHelper.Write($"{Name}.{i}", "position", Position[i], filePath);
            }
        }
        public void LoadTempPara(string filePath) {
            for (int i = 0; i < TestPoint; i++) {
                Alpha[i] = INIHelper.Read(Channel[i], "alpha", "10.0", filePath);
                Beta[i] = INIHelper.Read(Channel[i], "beta", "10.0", filePath);
                Theta[i] = INIHelper.Read(Channel[i], "theta", "10.0", filePath);
            }
        }

        /// <summary>
        /// 电压转换为温度
        /// T = -273.15 + 1 / (alpha + beta * ln(x)+theta * ln(x)^3)
        /// </summary>
        /// <param name="testResult"></param>
        public void SetTemp(Dictionary<string, double> testResult) {
            for (int i = 0; i < TestPoint; i++) {
                double tempVar = Math.Log(testResult[Channel[i]]);
                Temp[i] = Math.Round(
                    -273.15 + 1 / (double.Parse(Alpha[i]) + double.Parse(Beta[i]) * tempVar +
                                   double.Parse(Theta[i]) * Math.Pow(tempVar, 3)), 2);
            }
        }
        //public void SetTemp(Dictionary<string, double> testResult) {
        //    for (int i = 0; i < TestPoint; i++)
        //        Temp[i] = Math.Round(double.Parse(Alpha[i]) * testResult[Channel[i]] + double.Parse(T0[i]), 2);
        //}
    }
}
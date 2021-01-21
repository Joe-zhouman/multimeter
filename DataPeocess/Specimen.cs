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
            Area = "10.0";
            
            Channel = Enumerable.Repeat("201",TestPoint).ToArray();
            Position = Enumerable.Repeat("1", TestPoint).ToArray();
            A0 = Enumerable.Repeat("1", TestPoint).ToArray();
            A1 = Enumerable.Repeat("1", TestPoint).ToArray();
            A3 = Enumerable.Repeat("1", TestPoint).ToArray();
            Temp = Enumerable.Repeat(0.0, TestPoint).ToArray();
        }

        public string Name { get; set; }
        public string Kappa { get; set; }
        public string Area { get; set; }
        public string[] Channel { get; set; }
        public string[] Position { get; set; }
        public string[] A0 { get; set; }
        public string[] A1 { get; set; }
        public string[] A3 { get; set; }
        public double[] Temp { get; set; }
        public int TestPoint { get; set; }

        public void ReadFromIni(string filePath) {
            for (var i = 0; i < TestPoint; i++)
            {
                Channel[i] = INIHelper.Read($"{Name}.{i}", "channel", "201", filePath);
                Position[i] = INIHelper.Read($"{Name}.{i}", "position", "10.0", filePath);
            }

            Kappa = INIHelper.Read(Name, "kappa", "10.0", filePath);
            Area = INIHelper.Read(Name, "area", "10.0", filePath);
        }

        public virtual void SaveToIni(string filePath) {
            INIHelper.Write(Name, "area", Area, filePath);
            for (var i = 0; i < TestPoint; i++)
            {
                INIHelper.Write($"{Name}.{i}", "channel", Channel[i], filePath);
                INIHelper.Write($"{Name}.{i}", "position", Position[i], filePath);
            }
        }
        public void LoadTempPara(string filePath) {
            for (int i = 0; i < TestPoint; i++) {
                A0[i] = INIHelper.Read(Channel[i], "A0", "10.0", filePath);
                A1[i] = INIHelper.Read(Channel[i], "A1", "10.0", filePath);
                A3[i] = INIHelper.Read(Channel[i], "A3", "10.0", filePath);
            }
        }
        public void WriteTempPara(string filePath)
        {
            for (int i = 0; i < TestPoint; i++)
            {
                INIHelper.Write(Channel[i], "A0", A0[i], filePath);
                INIHelper.Write(Channel[i], "A1", A1[i], filePath);
                INIHelper.Write(Channel[i], "A3", A3[i], filePath);
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
                    -273.15 + 1 / (double.Parse(A0[i]) + double.Parse(A1[i]) * tempVar +
                                   double.Parse(A3[i]) * Math.Pow(tempVar, 3)), 2);
            }
        }
        public void ReadTemp(Dictionary<string, double> testResult)
        {
            for (int i = 0; i < TestPoint; i++)
            {
               
                Temp[i] = testResult[Channel[i]];
            }
        }
        //public void SetTemp(Dictionary<string, double> testResult) {
        //    for (int i = 0; i < TestPoint; i++)
        //        Temp[i] = Math.Round(double.Parse(Alpha[i]) * testResult[Channel[i]] + double.Parse(T0[i]), 2);
        //}
    }
}
using System;
using System.Collections.Generic;

namespace DataProcessor {
    public class Specimen {
        protected Specimen(string name) {
            Name = name;
            Kappa = "10.0";
            Diameter = "10.0";
            Channel = new string[TestPoint];
            Position = new string[TestPoint];
            Alpha = new string[TestPoint];
            T0 = new string[TestPoint];
            Temp = new double[TestPoint];
            for (int i = 0; i < TestPoint; i++)
            {
                Channel[i] = "201";
                Position[i] = "10.0";
                Alpha[i] = "1.0";
                T0[i] = "1.0";
                Temp[i] = 0.0;
            }
        }

        public string Name { get; set; }
        public string Kappa { get; set; }
        public string Diameter { get; set; }
        public string[] Channel { get; set; }
        public string[] Position { get; set; }
        public string[] Alpha { get; set; }
        public string[] T0 { get; set; }
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
                T0[i] = INIHelper.Read(Channel[i], "T0", "10.0", filePath);
            }
        }

        public void SetTemp(Dictionary<string, double> testResult) {
            for (int i = 0; i < TestPoint; i++)
                Temp[i] = Math.Round(double.Parse(Alpha[i]) * testResult[Channel[i]] + double.Parse(T0[i]), 2);
        }
    }
}
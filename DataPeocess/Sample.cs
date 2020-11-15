using System;
using System.Collections.Generic;

namespace DataProcessor {
    public class Sample {
        public Sample(string name) {
            Name = name;
            Kappa = "10.0";
            Diameter = "10.0";
            Channel = new[] {"201", "201", "201"};
            Position = new[] {"10.0", "10.0", "10.0"};
            Alpha = new[] {"10.0", "10.0", "10.0"};
            T0 = new[] {"10.0", "10.0", "10.0"};
            Temp = new[] {0.0, 0.0, 0.0};
        }

        public string Name { get; set; }
        public string Kappa { get; set; }
        public string Diameter { get; set; }
        public string[] Channel { get; set; }
        public string[] Position { get; set; }
        public string[] Alpha { get; set; }
        public string[] T0 { get; set; }
        public double[] Temp { get; private set; }

        public void ReadFromIni(string filePath) {
            for (var i = 0; i < 3; i++) {
                Channel[i] = INIHelper.Read($"{Name}.{i}", "channel", "201", filePath);
                Position[i] = INIHelper.Read($"{Name}.{i}", "position", "10.0", filePath);
            }

            Kappa = INIHelper.Read(Name, "kappa", "10.0", filePath);
            Diameter = INIHelper.Read(Name, "diameter", "10.0", filePath);
        }

        public void SaveToIni(string filePath) {
            INIHelper.Write(Name, "diameter", Diameter, filePath);
            for (var i = 0; i < 3; i++) {
                INIHelper.Write($"{Name}.{i}", "channel", Channel[i], filePath);
                INIHelper.Write($"{Name}.{i}", "position", Position[i], filePath);
            }
        }

        public void LoadTempPara(string filePath) {
            for (var i = 0; i < 3; i++) {
                Alpha[i] = INIHelper.Read(Channel[i], "alpha", "10.0", filePath);
                T0[i] = INIHelper.Read(Channel[i], "T0", "10.0", filePath);
            }
        }

        public void SetTemp(Dictionary<string, double> testResult) {
            for (int i = 0; i < 3; i++) {
                Temp[i] = Math.Round(double.Parse(Alpha[i]) * testResult[Channel[i]] + double.Parse(T0[i]),2);
            }
        }
    }
}
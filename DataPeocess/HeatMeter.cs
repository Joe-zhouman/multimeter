using System;
using System.Collections.Generic;
using System.IO;


namespace DataProcessor {
    public class HeatMeter {
        public string Name { get; set; }
        public string Kappa { get; set; }
        public string Diameter { get; set; }
        public string[] Channel { get; set; }
        public string[] Position { get; set; }
        public string[] Alpha { get; set; }
        public string[] T0 { get; set; }

        public HeatMeter() {
            Name = "";
            Kappa = "0";
            Diameter = "0";
            Channel = new[] {"000", "000", "000", "000"};
            Position = new[] {"0.0", "0.0", "0.0", "0.0"};
            Alpha = new[] { "0.0", "0.0", "0.0", "0.0"};
            T0 = new[] { "0.0", "0.0", "0.0", "0.0"};
        }

        public void ReadFromIni(string filePath) {
            for (int i = 0; i < 4; i++) {
           
                    Channel[i] = INIHelper.Read($"{Name}.{i}", "channel", "000", filePath);
                    Position[i] = INIHelper.Read($"{Name}.{i}", "position", "0.0", filePath);
                    Alpha[i] = INIHelper.Read(Channel[i], "alpha", "0.0", filePath);
                    T0[i] = INIHelper.Read(Channel[i], "T0", "0.0", filePath);
                
               
            }
            Kappa = INIHelper.Read(Name, "kappa", "0", filePath);
            Diameter = INIHelper.Read(Name, "diameter", "0", filePath);
        }

        public void SaveToIni(string filePath) {
            for (int i = 0; i < 4; i++) {

                INIHelper.Write($"{Name}.{i}", "channel", Channel[i], filePath);
                INIHelper.Write($"{Name}.{i}", "position", Position[i], filePath);
                INIHelper.Write(Channel[i], "alpha", Alpha[i], filePath);
                INIHelper.Write(Channel[i], "T0", T0[i], filePath);


            }
            INIHelper.Write(Name, "kappa", Kappa, filePath);
            INIHelper.Write(Name, "diameter", Diameter, filePath);
        }
        
    }
}


namespace DataProcessor {
    public class Sample {
        public string Name { get; set; }
        public string Kappa { get; set; }
        public string Diameter { get; set; }
        public string[] Channel { get; set; }
        public string[] Position { get; set; }
        public string[] Alpha { get; set; }
        public string[] T0 { get; set; }

        public Sample(string name) {
            Name = name;
            Kappa = "0";
            Diameter = "0";
            Channel = new[] { "201", "201", "201" };
            Position = new[] { "0.0", "0.0", "0.0"};
            Alpha = new[] { "0.0", "0.0", "0.0"};
            T0 = new[] { "0.0", "0.0", "0.0"};
        }

        public void ReadFromIni(string filePath) {
            for (int i = 0; i < 3; i++) {

                Channel[i] = INIHelper.Read($"{Name}.{i}", "channel", "201", filePath);
                Position[i] = INIHelper.Read($"{Name}.{i}", "position", "0.0", filePath);
            }
            Kappa = INIHelper.Read(Name, "kappa", "0", filePath);
            Diameter = INIHelper.Read(Name, "diameter", "0", filePath);
        }

        public void SaveToIni(string filePath) {
            INIHelper.Write(Name, "diameter", Diameter, filePath);
            for (int i = 0; i < 3; i++) {
                INIHelper.Write($"{Name}.{i}", "channel", Channel[i], filePath);
                INIHelper.Write($"{Name}.{i}", "position", Position[i], filePath);
            }
            
        }
        public void LoadTempPara(string filePath) {
            for (int i = 0; i < 3; i++) {
                Alpha[i] = INIHelper.Read(Channel[i], "alpha", "0.0", filePath);
                T0[i] = INIHelper.Read(Channel[i], "T0", "0.0", filePath);
            }
        }
    }
}

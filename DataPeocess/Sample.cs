// Administrator
// multimeter
// DataProcessor
// 2020-10-27-17:19
// *Author*    : Joe, Zhou Man
// *Email*     : man.man.man.man.a@gmail.com
// *Email*     : joe_zhouman@foxmail.com
// *QQ*        : 1592020915
// *Weibo*     : @zhouman_LFC
// *Twitter*   : @zhouman_LFC
// *Website*   : www.joezhouman.com
// *Github*    : https://github.com/Joe-zhouman
// *LeetCode*  : https://leetcode-cn.com/u/joe_zm/

namespace DataProcessor {
    public class Sample {
        public string Name { get; set; }
        public string Kappa { get; set; }
        public string Diameter { get; set; }
        public string[] Channel { get; set; }
        public string[] Position { get; set; }
        public string[] Alpha { get; set; }
        public string[] T0 { get; set; }

        public Sample() {
            Name = "";
            Kappa = "0";
            Diameter = "0";
            Channel = new[] { "000", "000", "000"};
            Position = new[] { "0.0", "0.0", "0.0"};
            Alpha = new[] { "0.0", "0.0", "0.0"};
            T0 = new[] { "0.0", "0.0", "0.0"};
        }

        public void ReadFromIni(string filePath) {
            for (int i = 0; i < 3; i++) {

                Channel[i] = INIHelper.Read($"{Name}.{i}", "channel", "000", filePath);
                Position[i] = INIHelper.Read($"{Name}.{i}", "position", "0.0", filePath);
                Alpha[i] = INIHelper.Read(Channel[i], "alpha", "0.0", filePath);
                T0[i] = INIHelper.Read(Channel[i], "T0", "0.0", filePath);


            }
            Kappa = INIHelper.Read(Name, "kappa", "0", filePath);
            Diameter = INIHelper.Read(Name, "diameter", "0", filePath);
        }

        public void SaveToIni(string filePath) {
            for (int i = 0; i < 3; i++) {

                INIHelper.Write($"{Name}.{i}", "channel", Channel[i], filePath);
                INIHelper.Write($"{Name}.{i}", "position", Position[i], filePath);
                INIHelper.Write(Channel[i], "alpha", Alpha[i], filePath);
                INIHelper.Write(Channel[i], "T0", T0[i], filePath);


            }
            INIHelper.Write(Name, "diameter", Diameter, filePath);
        }

    }
}
}
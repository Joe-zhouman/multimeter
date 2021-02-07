// Joe
// 周漫
// 2021012419:06

namespace Model {
    public abstract class Probe {
        public double Temp { get; set; }
        public double[] Paras { get; set; }
        public abstract void SetTemp(double value);
    }
}
// Joe
// 周漫
// 2021012419:06

namespace Model {
    public abstract class Probe {
        private double _temp;
        public double TempLb { get; set; }

        public double TempUb { get; set; }

        public double Temp { get=>_temp;
            set {
                if (value > TempUb || value < TempLb) {
                    throw new ValOutOfRangeException();
                }

                _temp = value;
            }
        }
        public double[] Paras { get; set; }
        public abstract void SetTemp(double value);
    }
}
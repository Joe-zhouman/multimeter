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
                if(value == double.NaN)
                {
                    throw new ValOutOfRangeException();
                }
                if (value > TempUb) {
                    throw new ValOutOfRangeException(ValOutOfRangeType.GREATER_THAN);
                }
                if (value < TempLb) {
                    throw new ValOutOfRangeException(ValOutOfRangeType.LESS_THAN);
                }
                _temp = value;
            }
        }
        public double[] Paras { get; set; }
        public abstract void SetTemp(double value);
        public abstract void Init();
    }
}
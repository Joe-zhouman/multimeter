// Joe
// 周漫
// 2021012419:06

namespace Model {
    //TODO 用工厂模式重写
    public abstract class Probe {
        private double _temp;
        public double TempLb { get; set; }

        public double TempUb { get; set; }

        public virtual double Temp { get=>_temp;
            set {
                if(double.IsNaN(value)|| value < TempLb)
                {
                    _temp = TempLb;
                }
                if (value > TempUb) {
                    _temp = TempUb;
                }
                _temp = value;
            }
        }
        public double[] Paras { get; set; }
        public abstract void SetTemp(double value);
        public abstract void Init();
    }
}
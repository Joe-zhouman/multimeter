// Joe
// 周漫
// 2021012712:09

namespace Model {
    public class Thermocouple:Probe {
        public override void SetTemp(double temp) {
            Paras = null;
            Temp = temp;
        }
    }
}
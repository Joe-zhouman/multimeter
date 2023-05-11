// Joe
// 周漫
// 2021012712:09

namespace Model.Probe {
    public class Thermocouple : ProbeBase {
        public Thermocouple() {
            Paras = new[] { 0.0, 0.0 };
            Temp = 0;
        }

        public override void SetTemp(double voltage) {
            Temp = Paras[0] + Paras[1] * voltage;
        }

        public override void Init() {
            return;
        }
    }
}
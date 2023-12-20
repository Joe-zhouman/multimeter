namespace Model.Probe {
    public class CubicPolyResistanceProbe : ProbeBase {
        public CubicPolyResistanceProbe() {
            Paras = new[] { 0.0, 0.0, 0.0, 0.0 };
            Temp = 0;
        }

        public override void SetTemp(double resistance) {
            Temp = Paras[0] + Paras[1] * resistance + Paras[2] * resistance * resistance + Paras[3] * resistance * resistance * resistance;
        }

        public override void Init() {
            return;
        }
    }
}

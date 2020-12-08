namespace DataProcessor {
    public class HeatMeter : Specimen {
        public HeatMeter(string name,int testPoint = 4) : base(name, testPoint) {
        }

        public override void SaveToIni(string filePath) {
            INIHelper.Write(Name, "kappa", Kappa, filePath);
            base.SaveToIni(filePath);
        }
    }
}
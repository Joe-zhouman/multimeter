using System;
using System.Collections.Generic;

namespace DataProcessor {
    public class HeatMeter : Specimen {
        public HeatMeter(string name) : base(name) {
            TestPoint = 4;
        }
        public override void SaveToIni(string filePath) {
            INIHelper.Write(Name, "kappa", Kappa, filePath);
            base.SaveToIni(filePath);
        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace DataProcessor {
    public class Sample : Specimen {
        public Sample(string name):base(name) {
            TestPoint = 3;
        }
    }
}
using System.Collections.Generic;
using System.Linq;

namespace Model {
    /// <summary>
    ///     测试件基类
    /// </summary>
    public class Specimen {
        public Probe.ProbeBase[] Probes;

        public Specimen(string name, SpecimenType specimenType, int testPoint = 4) {
            Name = name;
            SpecimenType = specimenType;
            TestPoint = testPoint;
            Kappa = "10.0";
            Area = "10.0";
            Channel = new string[TestPoint];
            Position = new string[TestPoint];
            Probes = new Probe.ProbeBase[TestPoint];
            for (var i = 0; i < TestPoint; i++) {
                Channel[i] = "*";
                Position[i] = "*";
                Probes[i] = null;
            }
        }

        public void SetTempRange(double lb, double ub) {
            foreach (Probe.ProbeBase probe in Probes) {
                if (probe == null) continue;
                probe.TempLb = lb;
                probe.TempUb = ub;
            }
        }
        public string Name { get; }
        public string Kappa { get; set; }
        public string Area { get; set; }
        public string[] Channel { get; set; }
        public string[] Position { get; set; }
        public int TestPoint { get; set; }
        public SpecimenType SpecimenType { get; set; }

        public List<double> Temp =>
            (from thermistor in Probes where thermistor != null select thermistor.Temp).ToList();

    }
}
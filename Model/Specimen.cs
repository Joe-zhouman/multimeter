using System.Collections.Generic;
using System.Linq;

namespace Model {
    /// <summary>
    ///     测试件基类
    /// </summary>
    public class Specimen {
        public Specimen(string name, SpecimenType specimenType, int testPoint = 4) {
            Name = name;
            SpecimenType = specimenType;
            TestPoint = testPoint;
            Kappa = "10.0";
            Area = "10.0";
            Channel = new string[TestPoint];
            Position = new string[TestPoint];
            Probes = new Probe[TestPoint];
            for (int i = 0; i < TestPoint; i++) {
                Channel[i] = "201";
                Position[i] = "0.0";
                Probes[i] = null;
            }
        }
        public string Name { get; private set; }
        public string Kappa { get; set; }
        public string Area { get; set; }
        public string[] Channel { get; set; }
        public string[] Position { get; set; }
        public Probe[] Probes;
        public int TestPoint { get; set; }
        public SpecimenType SpecimenType { get; set; }

        public List<double> Temp => (from thermistor in Probes where thermistor != null select thermistor.Temp).ToList();
    }
}
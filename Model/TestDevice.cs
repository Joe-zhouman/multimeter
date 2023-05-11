// Joe
// 周漫
// 2021012418:40

using System;
using System.Collections.Generic;
using System.Linq;

namespace Model {
    public class TestDevice {
        public Specimen HeatMeter1;
        public Specimen HeatMeter2;
        public Tim Itm;
        public Specimen Sample1;
        public Specimen Sample2;

        public TestDevice(TestMethod testMethod) {
            Method = testMethod;
            HeatMeter1 = new Specimen("HeatMeter1", SpecimenType.HEAT_METER);
            HeatMeter2 = new Specimen("HeatMeter2", SpecimenType.HEAT_METER);
            switch (testMethod) {
                case TestMethod.KAPPA: {
                    Sample1 = new Specimen("Sample1", SpecimenType.SAMPLE);
                    Sample2 = null;
                }
                    break;
                case TestMethod.ITC: {
                    Sample1 = new Specimen("Sample1", SpecimenType.SAMPLE);

                    Sample2 = new Specimen("Sample2", SpecimenType.SAMPLE);
                }
                    break;
                case TestMethod.ITM: {
                    Sample1 = null;
                    Sample2 = null;
                    Itm = new Tim();
                }
                    break;
                case TestMethod.ITMS: {
                    Sample1 = new Specimen("Sample1", SpecimenType.SAMPLE);
                    Sample2 = new Specimen("Sample2", SpecimenType.SAMPLE);
                    Itm = new Tim();
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(testMethod), testMethod, null);
            }
        }

        public TestMethod Method { get; }

        public List<string> Channels {
            get {
                List<string> channels = new List<string>();
                AddRange(ref channels, HeatMeter1);
                AddRange(ref channels, Sample1);
                AddRange(ref channels, Sample2);
                AddRange(ref channels, HeatMeter2);
                return channels;
            }
        }

        public List<double> Temp {
            get {
                List<double> t = new List<double>();
                if (HeatMeter1 != null)
                    t.AddRange(HeatMeter1.Temp);
                if (Sample1 != null)
                    t.AddRange(Sample1.Temp);
                if (Sample2 != null)
                    t.AddRange(Sample2.Temp);
                if (HeatMeter2 != null)
                    t.AddRange(HeatMeter2.Temp);
                return t;
            }
        }

        public string Force { get; set; }
        public double Itc { get; set; }

        private void AddRange(ref List<string> channels, Specimen specimen) {
            if (specimen == null) return;
            channels.AddRange(specimen.Channel.Where(channel => channel != "*"));
        }
    }
}
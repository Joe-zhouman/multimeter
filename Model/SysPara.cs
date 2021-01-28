// Joe
// 周漫
// 2021012419:20

using System.Collections.Generic;

namespace Model {
    public class SysPara {
        public List<string> AllowedChannels { get; set; }
        public SysPara() {
            SaveInterval = new RangeValue {
                LowerBound = 30, UpperBound = 100,Value = 50
            };
            ScanInterval = new RangeValue {
                
                LowerBound = 2000,
                UpperBound = 5000,
                Value = 2000
            };
            ;
            AutoCloseInterval = 1800;
            ConvergentLim = 1e-3;
            AllowedChannels = new List<string>() {"0"};
            for (int i = 201; i < 217; i++) {
                AllowedChannels.Add(i.ToString());
            }
        }

        /// <summary>
        ///     收敛后自动关闭间隔(Unit : s)
        /// </summary>
        public int AutoCloseInterval { get; set; }

        public double ConvergentLim { get; set; }

        /// <summary>
        ///     保存间隔
        /// </summary>
        public RangeValue SaveInterval { get; set; }

        /// <summary>
        ///     扫描间隔
        /// </summary>
        public RangeValue ScanInterval { get; set; }
    }
}
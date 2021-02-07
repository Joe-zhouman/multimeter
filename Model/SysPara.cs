// Joe
// 周漫
// 2021012419:20

using System.Collections.Generic;

namespace Model {
    public class SysPara {
        public SysPara() {
            SaveInterval = new RangeValue<int>(50, 30, 100);
            ScanInterval = new RangeValue<int>(2000, 2000, 5000);
            AutoCloseInterval = 1800;
            ConvergentLim = 1e-3;
            AllowedChannels = new List<string> {"0"};
            for (var i = 201; i < 226; i++) AllowedChannels.Add(i.ToString());
        }

        public List<string> AllowedChannels { get; set; }

        /// <summary>
        ///     收敛后自动关闭间隔(Unit : s)
        /// </summary>
        public int AutoCloseInterval { get; set; }

        public double ConvergentLim { get; set; }

        /// <summary>
        ///     保存间隔
        /// </summary>
        public RangeValue<int> SaveInterval { get; set; }

        /// <summary>
        ///     扫描间隔
        /// </summary>
        public RangeValue<int> ScanInterval { get; set; }
    }
}
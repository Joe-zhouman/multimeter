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
            TempLb = 0.0;
            TempUb = 120.0;
        }
        /// <summary>
        /// 允许使用的频道列表
        /// </summary>
        public List<string> AllowedChannels { get; set; }

        /// <summary>
        /// 收敛后自动关闭间隔(Unit : s)
        /// </summary>
        public int AutoCloseInterval { get; set; }
        /// <summary>
        /// 收敛容差
        /// </summary>
        public double ConvergentLim { get; set; }

        /// <summary>
        ///     保存间隔
        /// </summary>
        public RangeValue<int> SaveInterval { get; set; }

        /// <summary>
        ///     扫描间隔
        /// </summary>
        public RangeValue<int> ScanInterval { get; set; }
        /// <summary>
        /// 温度测试上限
        /// </summary>
        public double TempUb { get; set; }
        /// <summary>
        /// 温度测试下限
        /// </summary>
        public double TempLb { get; set; }
    }
}
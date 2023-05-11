// Joe
// 周漫
// 2021012514:48

using Model;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BusinessLogic {
    public static class DeviceOpt {
        /// <summary>
        /// 利用采集得到的原始数据计算温度
        /// </summary>
        /// <param name="specimen">待计算的试件</param>
        /// <param name="testResult">数据采集结果</param>
        public static void CalTemp(ref Specimen specimen, Dictionary<string, double> testResult) {
            if (specimen == null) return;
            for (var i = 0; i < specimen.TestPoint; i++)
                if (specimen.Channel[i] != "*")
                    specimen.Probes[i].SetTemp(testResult[specimen.Channel[i]]);
        }
        /// <summary>
        /// 从保存的温度数据设置温度
        /// </summary>
        /// <param name="specimen">待设置的试件</param>
        /// <param name="tempRecord">读取的结果</param>
        public static void ReadTemp(ref Specimen specimen, Dictionary<string, double> tempRecord) {
            if (specimen == null) return;
            for (var i = 0; i < specimen.TestPoint; i++)
                if (specimen.Channel[i] != "*")
                    specimen.Probes[i].Temp = tempRecord[specimen.Channel[i]];
        }
        /// <summary>
        /// 利用采集得到的原始数据计算温度
        /// </summary>
        /// <param name="device">待计算的实验设备</param>
        /// <param name="testResult">数据采集结果</param>
        public static void CalTemp(ref TestDevice device, Dictionary<string, double> testResult) {
            CalTemp(ref device.HeatMeter1, testResult);
            CalTemp(ref device.HeatMeter2, testResult);
            CalTemp(ref device.Sample1, testResult);
            CalTemp(ref device.Sample2, testResult);
        }
        /// <summary>
        /// 从保存的温度数据设置温度
        /// </summary>
        /// <param name="device">待设置的实验设备</param>
        /// <param name="tempRecord">读取的结果</param>
        public static void ReadTemp(ref TestDevice device, Dictionary<string, double> tempRecord) {
            ReadTemp(ref device.HeatMeter1, tempRecord);
            ReadTemp(ref device.HeatMeter2, tempRecord);
            ReadTemp(ref device.Sample1, tempRecord);
            ReadTemp(ref device.Sample2, tempRecord);
        }
        /// <summary>
        /// 获取试件的温度列表,并组成由','拼接的字符串
        /// </summary>
        /// <param name="tempListString">输出的温度列表字符串</param>
        /// <param name="specimen">待处理试件</param>
        public static void GetTempList(ref string tempListString, Specimen specimen) {
            if (specimen == null) return;
            tempListString = specimen.Probes.Where(probe => probe != null).Aggregate(tempListString,
                (current, thermistor) => current + thermistor.Temp.ToString(CultureInfo.InvariantCulture) + ",");
        }

    }
}
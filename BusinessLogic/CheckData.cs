using Model;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic {
    /// <summary>
    /// 检查数据类型的工具类
    /// </summary>
    public static class CheckData {
        /// <summary>
        /// 检查文本框内是否是一个正确的浮点数(包括*)
        /// </summary>
        /// <param name="doubleStr"></param>
        /// <returns></returns>
        public static bool CheckDouble(string doubleStr) {
            if (doubleStr == "*") return true;
            if (!double.TryParse(doubleStr, out var result)) return false;
            return result >= 0;
        }
        /// <summary>
        /// 检查两个值是否相差大于50%
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Compare(string a, string b) {
            double.TryParse(a, out var numA);
            double.TryParse(b, out var numB);
            return System.Math.Abs(1 - numA / numB) < 0.5;
        }
        /// <summary>
        /// 查看列表里是否有相同元素
        /// </summary>
        /// <typeparam name="T">列表元素类型</typeparam>
        /// <param name="list">列表</param>
        /// <returns>有相同元素返回true;否则为false</returns>
        public static bool HasSameElem<T>(IReadOnlyCollection<T> list) {
            if (list == null)
                return false;
            for (var i = 0; i < list.Count() - 1; i++)
                if (list.ElementAt(i).Equals(list.ElementAt(i + 1)))
                    return true;
            return false;
        }
        /// <summary>
        /// 检查channel列表了是否有相同元素
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public static bool CheckChannelList(TestDevice device) {
            return !HasSameElem(device.Channels);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Model;

namespace BusinessLogic {
    public static class CheckData {
        public static bool CheckDouble(string doubleStr) {
            if (doubleStr == "*") return true;
            if (!double.TryParse(doubleStr, out var result)) return false;
            return result >= 0;
        }
        public static bool Compare(string a, string b) { 
            double.TryParse(a,out var numA);
            double.TryParse(b,out var numB);
            return System.Math.Abs(1 - numA / numB) < 0.5;
        }
        public static bool HasSameElem<T>(IReadOnlyCollection<T> list) {
            if (list == null)
                return false;
            for (var i = 0; i < list.Count() - 1; i++)
                if (list.ElementAt(i).Equals(list.ElementAt(i + 1)))
                    return true;
            return false;
        }

        public static bool CheckChannelList(TestDevice device) {
            return !HasSameElem(device.Channels);
        }
    }
}
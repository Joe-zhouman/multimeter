using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;

namespace DataProcessor {
    public static class CheckData {
        public static bool CheckDoubleList(List<string> doubleList) {
            foreach (string num in doubleList) {
                try {
                    if (double.Parse(num) <= 0)
                        return false;
                } catch (Exception) {
                    return false;
                }
            }
            return true;
        }
        public static bool HasSameElem<T>(IReadOnlyCollection<T> list) {
            if (list == null)
                return false;
            for (int i = 0; i < list.Count() - 1; i++) {
                if (list.ElementAt(i).Equals(list.ElementAt(i + 1))) {
                    return true;
                }
            }
            return false;
        }
        public static bool CheckChannelList(List<string> channelList) {
            List<int> chnIntList = new List<int>();
            foreach (string chn in channelList) {
                try {
                    chnIntList.Add(Convert.ToInt16(chn));
                } catch (Exception) {
                    return false;
                }
            }
            chnIntList.Sort();
            if (chnIntList.First() >= 101 && chnIntList.Last() <= 122) {
                return !CheckData.HasSameElem(chnIntList);
            }

            if (chnIntList.First() >= 201 && chnIntList.Last() <= 222) {
                return !CheckData.HasSameElem(chnIntList);
            }
            return true;
        }
        public static int CheckTextChange(string text) {
            int num;
            try {
                num = int.Parse(text);
            } catch {
                return -1;
            }

            return num > 0 ? num : -1;
        }
    }

}
// Administrator
// multimeter
// DataAccess
// 2021-03-29-21:53
// *Author*    : Joe, Zhou Man
// *Email*     : man.man.man.man.a@gmail.com
// *Email*     : joe_zhouman@foxmail.com
// *QQ*        : 1592020915
// *Weibo*     : @zhouman_LFC
// *Twitter*   : @zhouman_LFC
// *Website*   : www.joezhouman.com
// *Github*    : https://github.com/Joe-zhouman
// *LeetCode*  : https://leetcode-cn.com/u/joe_zm/

using System;
using System.IO;
using System.Linq.Expressions;
using System.Net;

namespace DataAccess {
    public static class GetUpdateInfo {
        private static string _url;
        //bug GET request get 403 response
        static GetUpdateInfo() {
            _url = IniHelper.Read("sys", "requestUrl", "", IniReadAndWrite.IniFilePath);
        }

        public static string Get(string url) {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            try
            {
                //获取内容
                using (StreamReader reader = new StreamReader(stream ?? throw new NullReferenceException()))
                {
                    result = reader.ReadToEnd();
                }
            }
            finally {
                if (stream != null) stream.Close();
            }
            return result;

        }
    }
}
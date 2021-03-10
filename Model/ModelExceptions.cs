// Administrator
// multimeter
// Model
// 2021-03-10-10:28
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
using System.Runtime.Serialization;

namespace Model {
    [Serializable]
    public class NoRootException : Exception {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public NoRootException() {
        }

        public NoRootException(string message) : base(message) {
        }

        public NoRootException(string message, Exception inner) : base(message, inner) {
        }

        protected NoRootException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) {
        }
    }
    [Serializable]
    public class ValOutOfRangeException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ValOutOfRangeException() { }

        public ValOutOfRangeException(string message) : base(message) { }

        public ValOutOfRangeException(string message, Exception inner) : base(message, inner) { }

        protected ValOutOfRangeException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
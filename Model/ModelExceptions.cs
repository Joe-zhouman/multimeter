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
    public class ValOutOfRangeException : Exception {
        public ValOutOfRangeException() {
            Type = ValOutOfRangeType.OUT_OF_RANGE;
        }

        public ValOutOfRangeException(ValOutOfRangeType type) {
            Type = type;
        }

        public ValOutOfRangeException(string message) : base(message) {
            Type = ValOutOfRangeType.OUT_OF_RANGE;
        }

        public ValOutOfRangeException(string message, Exception inner) : base(message, inner) {
            Type = ValOutOfRangeType.OUT_OF_RANGE;
        }

        protected ValOutOfRangeException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) {
        }

        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //
        public ValOutOfRangeType Type { get; private set; }
    }

    public enum ValOutOfRangeType {
        OUT_OF_RANGE = 0,
        GREATER_THAN,
        LESS_THAN
    }
}
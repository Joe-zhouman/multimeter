// Joe
// 周漫
// 2021012612:47

using System;
using System.Runtime.Serialization;

namespace Model {
    public class RangeValue {
        private int _value;
        public RangeValue() {
            LowerBound = 0;
            UpperBound = 0;
            Value = 0;
        }

        public int Value {
            get=>_value;
            set {
                if (Value < LowerBound || Value > UpperBound)
                    throw new NumOutOfRangeException($@"数值不在范围内,应在{LowerBound}~{UpperBound}之间");
                _value = value;
            }
        }

        public int LowerBound { get; set; }
        public int UpperBound { get; set; }
    }
    [Serializable]
    public class NumOutOfRangeException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public NumOutOfRangeException() { }

        public NumOutOfRangeException(string message) : base(message) { }

        public NumOutOfRangeException(string message, Exception inner) : base(message, inner) { }

        protected NumOutOfRangeException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
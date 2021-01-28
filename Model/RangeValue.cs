// Joe
// 周漫
// 2021012612:47

using System;
using System.Runtime.Serialization;

namespace Model {
    public class RangeValue<T>where T:IComparable
    {
        private T _value;
        public RangeValue(T value,T lowerBound,T upperBound) { 
            LowerBound = lowerBound;
            UpperBound = upperBound;
            Value = value;
        }

        public T Value {
            get=>_value;
            set {
                if (value.CompareTo(LowerBound) < 0  || value.CompareTo(UpperBound)>0)
                    throw new NumOutOfRangeException($@"数值不在范围内,应在{LowerBound}~{UpperBound}之间");
                _value = value;
            }
        }

        public T LowerBound { get; set; }
        public T UpperBound { get; set; }
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
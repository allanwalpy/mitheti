using System;

namespace Mitheti.Core
{
    public class TimeInterval : IComparable<DateTime>, IEquatable<DateTime>
    {
        public static readonly TimeInterval All = new TimeInterval(); 
            
        public DateTime Begin { get; }
        public DateTime End { get; }

        public TimeInterval(DateTime begin, DateTime end)
        {
            Begin = begin;
            End = end;
        }

        public TimeInterval(DateTime border, bool isEnd = false)
            : this(
                begin: isEnd ? DateTime.MinValue : border, 
                end: isEnd ? border : DateTime.MaxValue)
        { }
        
        private TimeInterval()
            : this(DateTime.MinValue, DateTime.MaxValue)
        { }

        public bool Equals(DateTime timePoint)
            => (Begin <= timePoint) && (End >= timePoint);

        public int CompareTo(DateTime timePoint)
        {
            if (timePoint < Begin)
            {
                return 1;
            }

            if (timePoint > End)
            {
                return -1;
            }

            return 0;
        }
    }
}
using System;

namespace Mitheti.Core
{
    public struct TimePeriod : IComparable<DateTime>, IEquatable<DateTime>, IEquatable<TimePeriod>
    {
        public static readonly TimePeriod All = new TimePeriod { Begin = DateTime.MinValue, End = DateTime.MaxValue };

        public DateTime Begin { get; set; }
        public DateTime End { get; set; }

        public int CompareTo(DateTime time) => time < Begin ? 1 : (time > End ? -1 : 0);
        public bool Equals(DateTime timePoint) => (Begin <= timePoint) && (End >= timePoint);
        public bool Equals(TimePeriod other) => Begin == other.Begin && End == other.End;
    }
}

using System;

namespace Mitheti.Core.Services
{
    public class TopAppInfo : IEquatable<TopAppInfo>, IComparable<TopAppInfo>
    {
        public const int NullDuration = -1;
        public static readonly TopAppInfo Empty = new TopAppInfo {AppName = null, TotalDuration = 0};

        public string AppName { get; set; }
        public int TotalDuration { get; set; }

        public bool Equals(TopAppInfo other)
            => other != null && AppName == other.AppName && TotalDuration == other.TotalDuration;

        //? simplifing: `TotalDuration.CompareTo((other?.TotalDuration ?? NullDuration));`;
        public int CompareTo(TopAppInfo other)
            => (other?.TotalDuration ?? NullDuration) - TotalDuration;
    }
}
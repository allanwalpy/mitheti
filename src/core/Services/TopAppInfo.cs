using System;

namespace Mitheti.Core.Services
{
    public class TopAppInfo : IEquatable<TopAppInfo>, IComparable<TopAppInfo>
    {
        public string AppName { get; set; }
        public int Duration { get; set; }

        public bool Equals(TopAppInfo other) => AppName == other?.AppName;

        public int CompareTo(TopAppInfo other) => other.Duration - Duration;
    }
}
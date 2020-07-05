using System;

using Mitheti.Core.Database;

namespace Mitheti.Web.Models
{
    public class SearchFilter
    {
        public static readonly DateTime TimeBeginNull = DateTime.MinValue;
        public static readonly DateTime TimeEndNull = DateTime.MaxValue;

        public FilterEqual<string> AppName { get; set; }

        public FilterEqual<int> Hour { get; set; }
        public FilterEqual<int> Day { get; set; }
        public FilterEqual<int> Month { get; set; }
        public FilterEqual<int> Year { get; set; }
        public FilterEqual<int> DayOfWeek { get; set; }

        public DateTime TimeBegin { get; set; }
        public DateTime TimeEnd { get; set; }

        public bool Apply(AppTimeModel item)
        {
            return (AppName?.Apply(item.AppName) ?? true)
                && (Hour?.Apply(item.Hour) ?? true)
                && (Day?.Apply(item.Day) ?? true)
                && (Month?.Apply(item.Month) ?? true)
                && (Year?.Apply(item.Year) ?? true)
                && (DayOfWeek?.Apply(item.DayOfWeek) ?? true)
                && (TimeBegin <= item.Time)
                && (TimeEnd >= item.Time);
        }
    }
}

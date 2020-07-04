using System;

using Mitheti.Core.Database;

namespace Mitheti.Web.Models
{
    public class SearchFilter
    {
        public static readonly DateTime TimeBeginNull = DateTime.MinValue;
        public static readonly DateTime TimeEndNull = DateTime.MaxValue;

        public FilterEqual<string> AppName { get; }

        public FilterEqual<int> Hour { get; }
        public FilterEqual<int> Day { get; }
        public FilterEqual<int> Month { get; }
        public FilterEqual<int> Year { get; }
        public FilterEqual<int> DayOfWeek { get; }

        public DateTime TimeBegin { get; }
        public DateTime TimeEnd { get; }

        public bool Apply(AppTimeModel item)
        {
                return AppName.Apply(item.AppName)
                    && Hour.Apply(item.Hour)
                    && Day.Apply(item.Day)
                    && Month.Apply(item.Month)
                    && Year.Apply(item.Year)
                    && DayOfWeek.Apply(item.DayOfWeek)
                    && (TimeBegin <= item.Time)
                    && (TimeEnd >= item.Time);
        }
    }
}

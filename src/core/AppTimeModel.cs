using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mitheti.Core
{
    [Table(TableName)]
    public class AppTimeModel
    {
        public const string TableName = "appTime_source_v1";
        public const int AppNameMaxLength = 255;

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(AppNameMaxLength)]
        public string AppName { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public DateTime Time { get; set; }

        public int Hour => Time.Hour;

        public int Day => Time.Day;

        public int Month => Time.Month;

        public int Year => Time.Year;

        //TODO:FIXME: use defined enum?;
        public int DayOfWeek => (int)Time.DayOfWeek;

        public AppTimeModel()
        {   }

        public AppTimeModel(string processName, int duration, DateTime timestamp)
        {
            AppName = processName;
            Duration = duration;
            Time = timestamp;
        }

        //TODO:FIXME:relevant only for values obtained from database;
        public bool IsSameTimeSpan(AppTimeModel other)
        {
            return (this.AppName == other.AppName)
                && (this.Hour    == other.Hour)
                && (this.Day     == other.Day)
                && (this.Month   == other.Month)
                && (this.Year    == other.Year);
        }

        //TODO:FIXME: tmp solution for by Day statistic;
        public bool IsSameTimeSpanDay(AppTimeModel other)
        {
            return (this.AppName == other.AppName)
                && (this.Day     == other.Day)
                && (this.Month   == other.Month)
                && (this.Year    == other.Year);
        }

        public override string ToString() => $"{AppName}://{Duration}?at={Time.ToString()};";
    }
}

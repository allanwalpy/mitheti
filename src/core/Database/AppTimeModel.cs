using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mitheti.Core.Database
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

        public int Hour => this.Time.Hour;

        public int Day => this.Time.Day;

        public int Month => this.Time.Month;

        public int Year => this.Time.Year;

        //FIXME: use defined enum?;
        public int DayOfWeek => (int)this.Time.DayOfWeek;

        public AppTimeModel()
        {   }

        public AppTimeModel(string processName, int duration, DateTime timestamp)
        {
            this.AppName = processName;
            this.Duration = duration;
            this.Time = timestamp;
        }

        //FIXME:relevant only for values obtained from database;
        public bool IsSameTimeSpan(AppTimeModel other)
        {
            return (this.AppName == other.AppName)
                && (this.Hour    == other.Hour)
                && (this.Day     == other.Day)
                && (this.Month   == other.Month)
                && (this.Year    == other.Year);
        }

        //FIXME: tmp solution for by Day statistic;
        public bool IsSameTimeSpanDay(AppTimeModel other)
        {
            return (this.AppName == other.AppName)
                && (this.Day == other.Day)
                && (this.Month == other.Month)
                && (this.Year == other.Year);
        }

        public override string ToString() => $"{AppName}://{Duration}?at={Time.ToString()};";
    }
}

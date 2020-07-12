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

        [Key] public int Id { get; set; }

        private string _appName;

        [MaxLength(AppNameMaxLength)]
        [Required]
        public string AppName
        {
            get => _appName;
            set => _appName = value.Length > AppNameMaxLength ? value.Substring(0, AppNameMaxLength - 1) : value;
        }

        [Required] public int Duration { get; set; }

        [Required] public DateTime Time { get; set; }

        public bool IsSameTimeSpan(AppTimeModel other)
            => (AppName == other.AppName) && (GetCutedToHoursTime() == other.GetCutedToHoursTime());

        private DateTime GetCutedToHoursTime() => new DateTime(Time.Year, Time.Month, Time.Day, Time.Hour, 0, 0);

        public override string ToString() => $"{AppName}://{Duration}?at={Time.ToString()};";
    }
}
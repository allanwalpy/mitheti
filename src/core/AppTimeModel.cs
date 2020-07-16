using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Mitheti.Core
{
    [Table(TableName)]
    public class AppTimeModel : IEquatable<AppTimeModel>, IEquatable<string>, IEquatable<DateTime>
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
            set => _appName = value.Length > AppNameMaxLength ? value.Substring(0, AppNameMaxLength) : value;
        }

        [Required] public int Duration { get; set; }

        [Required] public DateTime Time { get; set; }

        public bool Equals(string otherAppName) => AppName == otherAppName;

        public bool Equals(DateTime other)
            => Time.Year == other.Year && Time.Month == other.Month && Time.Day == other.Day && Time.Hour == other.Hour;

        public bool Equals(AppTimeModel other) => Equals(other?.AppName) && Equals(other?.Time);

        public override string ToString() => $"{AppName}://{Duration}?at={Time.ToString(CultureInfo.CurrentCulture)};";
    }
}
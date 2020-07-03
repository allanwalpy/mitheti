using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mitheti.Core.Database
{
    [Table(TableName)]
    public class AppTimeModel
    {
        public const string TableName = "appTime_source_v2";
        public const int AppNameMaxLength = 255;
        public const string TimeColumnName = nameof(Time);

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(AppNameMaxLength)]
        public string AppName { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Hour { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Day { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Month { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Year { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DayOfWeek { get; set; }

        public override string ToString()
            => $"{AppName}://{Duration}?at={Time.ToString()};";

        //FIXME:relevant only for values obtained from database;
        public bool IsSameTimeSpan(AppTimeModel other)
        {
            return (this.AppName == other.AppName)
                && (this.Hour    == other.Hour)
                && (this.Day     == other.Day)
                && (this.Month   == other.Month)
                && (this.Year    == other.Year);
        }
    }
}

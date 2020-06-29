using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mitheti.Core.Database
{
    [Table(TableName)]
    public class AppTimeSpanModel
    {
        public const string TableName = "AppTime_Source_v1";
        public const int NameMaxLength = 255;  //? see https://www.google.com/search?q=windows+max+process+name+length;
        public const string TimeColumnName = nameof(Time);

        // TODO: set exact length;
        [Key]
        public string Guid { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string AppName { get; set; }

        [Required]
        public int TimeSpan { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int Hour => Time.Hour;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int Day => Time.Day;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int Month => Time.Month;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int Year => Time.Year;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int DayOfWeek => (int)Time.DayOfWeek;
    }
}

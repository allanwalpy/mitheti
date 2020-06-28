using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mitheti.Core.Database
{
    [Table(TableName)]
    public class AppTimeSpanModel
    {
        public const string TableName = "AppTimeSpanSource";
        public const int NameMaxLength = 260;  //? see https://www.google.com/search?q=windows+max+process+name+length;
        public const string TimeColumnName = nameof(Time);

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string AppName { get; set; }

        [Required]
        public int TimeSpan { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int Hour { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int Day { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int Month { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int Year { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int DayOfWeek { get; set; }
    }
}

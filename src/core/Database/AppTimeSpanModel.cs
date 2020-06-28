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
        public int Id { get; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string AppName { get; }

        [Required]
        public uint TimeSpan { get; }

        [Required]
        public DateTime Time { get; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int Hour { get; }

        public int Day { get; }

        public int Month { get; }

        public int Year { get; }

        public int DayOfWeek { get; }
    }
}

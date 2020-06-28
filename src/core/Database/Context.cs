using System;
using Microsoft.EntityFrameworkCore;

using Mitheti.Core.Helper;

namespace Mitheti.Core.Database
{
    public class Context : DbContext
    {
        public DbSet<AppTimeSpanModel> AppTimeSpans { get; set; }

        public Context(DbContextOptions options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            this.ConfigureAppTimeSpans(builder);
        }

        private void ConfigureAppTimeSpans(ModelBuilder builder)
        {
            const string hourCode = "hh";
            const string dayCode = "dd";
            const string monthCode = "mm";
            const string yearCode = "year";
            const string weekdayCode = "dw";

            string template = $"datepart({{0}}, [{AppTimeSpanModel.TimeColumnName}])";

            builder.Entity<AppTimeSpanModel>()
                .Property(x => x.Hour)
                .HasComputedColumnSql(template.Format(hourCode));

            builder.Entity<AppTimeSpanModel>()
                .Property(x => x.Day)
                .HasComputedColumnSql(template.Format(dayCode));

            builder.Entity<AppTimeSpanModel>()
                .Property(x => x.Month)
                .HasComputedColumnSql(template.Format(monthCode));

            builder.Entity<AppTimeSpanModel>()
                .Property(x => x.Year)
                .HasComputedColumnSql(template.Format(yearCode));

            builder.Entity<AppTimeSpanModel>()
                .Property(x => x.DayOfWeek)
                .HasComputedColumnSql(template.Format(weekdayCode));
        }
    }
}

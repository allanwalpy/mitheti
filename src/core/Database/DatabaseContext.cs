using Microsoft.EntityFrameworkCore;

namespace Mitheti.Core.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<AppTimeModel> AppTimes { get; set; }

        public DatabaseContext(DbContextOptions options)
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
            const string time = AppTimeModel.TimeColumnName;

            builder.Entity<AppTimeModel>()
                .HasIndex(x => x.Id)
                .IsUnique();

            builder.Entity<AppTimeModel>()
                .Property(x => x.Hour)
                .HasComputedColumnSql($"hour({time})");

            builder.Entity<AppTimeModel>()
                .Property(x => x.Day)
                .HasComputedColumnSql($"day({time})");

            builder.Entity<AppTimeModel>()
                .Property(x => x.Month)
                .HasComputedColumnSql($"month({time})");

            builder.Entity<AppTimeModel>()
                .Property(x => x.Year)
                .HasComputedColumnSql($"year({time})");

            builder.Entity<AppTimeModel>()
                .Property(x => x.DayOfWeek)
                .HasComputedColumnSql($"dayofweek({time})");
        }
    }
}

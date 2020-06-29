using Microsoft.EntityFrameworkCore;

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
            const string time = AppTimeSpanModel.TimeColumnName;

            builder.Entity<AppTimeSpanModel>()
                .HasIndex(x => x.Id)
                .IsUnique();

            builder.Entity<AppTimeSpanModel>()
                .Property(x => x.Hour)
                .HasComputedColumnSql($"hour({time})");

            builder.Entity<AppTimeSpanModel>()
                .Property(x => x.Day)
                .HasComputedColumnSql($"day({time})");

            builder.Entity<AppTimeSpanModel>()
                .Property(x => x.Month)
                .HasComputedColumnSql($"month({time})");

            builder.Entity<AppTimeSpanModel>()
                .Property(x => x.Year)
                .HasComputedColumnSql($"year({time})");

            builder.Entity<AppTimeSpanModel>()
                .Property(x => x.DayOfWeek)
                .HasComputedColumnSql($"dayofweek({time})");
        }
    }
}

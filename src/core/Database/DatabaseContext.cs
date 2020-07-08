using Microsoft.EntityFrameworkCore;

namespace Mitheti.Core.Database
{
    public class DatabaseContext : DbContext
    {
        public const string DatabaseFilename = "database.db";

        public DbSet<AppTimeModel> AppTimes { get; set; }

        public DatabaseContext(DbContextOptions options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer($"Filename={DatabaseFilename}");

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            this.ConfigureAppTimeSpans(builder);
        }

        private void ConfigureAppTimeSpans(ModelBuilder builder)
        {
            builder.Entity<AppTimeModel>()
                .HasIndex(x => x.Id)
                .IsUnique();
        }
    }
}

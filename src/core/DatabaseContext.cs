using Microsoft.EntityFrameworkCore;

namespace Mitheti.Core
{
    public sealed class DatabaseContext : DbContext
    {
        public const string DatabaseFilename = "database.db";

        public DbSet<AppTimeModel> AppTimes { get; set; }

        public DatabaseContext()
            : base(new DbContextOptionsBuilder().Options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DatabaseFilename};");

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppTimeModel>()
                .HasIndex(x => x.Id)
                .IsUnique();
        }
    }
}
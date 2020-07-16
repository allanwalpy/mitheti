using Microsoft.EntityFrameworkCore;

namespace Mitheti.Core
{
    public sealed class DatabaseContext : DbContext
    {
        public const string DatabaseFilename =
#if DEBUG
            "../../../../database.hide.db";
#else
        "database.db";
#endif

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
    }
}
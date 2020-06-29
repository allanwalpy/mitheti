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
            builder.Entity<AppTimeSpanModel>()
                .Property(x => x.Guid)
                .HasValueGenerator(typeof(GuidGenerator))
                .ValueGeneratedOnAdd()
                .HasDefaultValue(GuidGenerator.DefaultGuid);
        }
    }
}

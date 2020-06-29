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
    }
}

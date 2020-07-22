using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mitheti.Core.Services
{
    public class DatabaseService : IDatabaseService
    {
        public DatabaseContext GetContext() => new DatabaseContext();

        public async Task AddAsync(IEnumerable<AppTimeModel> list)
        {
            await using var context = GetContext();
            await context.AddRangeAsync(list);
            await context.SaveChangesAsync();
        }

        public async Task ClearAsync(TimePeriod period)
        {
            await using var context = GetContext();

            var forDeletion = context.AppTimes.Where(
                item => item.Time > period.Begin && item.Time < period.End);
            if (!forDeletion.Any())
            {
                return;
            }

            context.AppTimes.RemoveRange(forDeletion);
            await context.SaveChangesAsync();
        }
    }
}

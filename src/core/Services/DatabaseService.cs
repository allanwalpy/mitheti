using System.Collections.Generic;
using System.Linq;

namespace Mitheti.Core.Services
{
    public class DatabaseService : IDatabaseService
    {
        public DatabaseContext GetContext() => new DatabaseContext();

        public void Add(IEnumerable<AppTimeModel> list)
        {
            using var context = GetContext();
            context.AddRange(list);
            context.SaveChanges();
        }

        public void Clear(TimePeriod period)
        {
            using var context = GetContext();

            var forDeletion = context.AppTimes.Where(
                item => item.Time > period.Begin && item.Time < period.End);
            if (!forDeletion.Any())
            {
                return;
            }

            context.AppTimes.RemoveRange(forDeletion);
            context.SaveChanges();
        }
    }
}

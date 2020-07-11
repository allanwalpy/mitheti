using System;
using System.Linq;

namespace Mitheti.Core.Services
{
    public class ClearDatabaseService : IClearDatabaseService
    {
        public void Clear(DateTime laterThen, DateTime beforeThen)
        {
            using (var context = new DatabaseContext())
            {
                var forDeletion =  context.AppTimes.Where(item => item.Time > laterThen && item.Time < beforeThen);
                if (!forDeletion.Any())
                {
                    return;
                }

                context.AppTimes.RemoveRange(forDeletion);
                context.SaveChanges();
            }
        }

        public void Clear(DateTime beforeThen)
            => Clear(DateTime.MinValue, beforeThen);
    }
}

using System;
using System.Linq;

namespace Mitheti.Core.Services
{
    public class ClearDatabaseService : IClearDatabaseService
    {
        private readonly IDatabaseService _database;
        
        public ClearDatabaseService(IDatabaseService database)
        {
            _database = database;
        }

        public void Clear(DateTime laterThen, DateTime beforeThen)
        {
            using var context = _database.GetContext();

            var forDeletion = context.AppTimes.Where(item => item.Time > laterThen && item.Time < beforeThen);
            if (!forDeletion.Any())
            {
                return;
            }

            context.AppTimes.RemoveRange(forDeletion);
            context.SaveChanges();
        }

        public void Clear(DateTime beforeThen) => Clear(DateTime.MinValue, beforeThen);
    }
}
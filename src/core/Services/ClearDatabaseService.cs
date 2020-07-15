using System;
using System.Linq;

namespace Mitheti.Core.Services
{
    public class ClearDatabaseService : IClearDatabaseService
    {
        private readonly IDatabaseService _database;

        // думаю нет смысла разделять код работающий с базой данных
        // я бы обьединил
        // разделение уместно еслибы они использовались где-то отдельно
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

        // мне смущает такой Clear лучше переименовать
        public void Clear(DateTime beforeThen) => Clear(DateTime.MinValue, beforeThen);
    }
}

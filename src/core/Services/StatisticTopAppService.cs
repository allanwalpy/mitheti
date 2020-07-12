using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Mitheti.Core.Services
{
    public class StatisticTopAppService : IStatisticTopAppService
    {
        private readonly IDatabaseService _database;

        public StatisticTopAppService(IDatabaseService database)
        {
            _database = database;
        }

        public List<TopAppInfo> Get(int topSize, TimeInterval interval)
        {
            using var context = _database.GetContext();
            var top = new Dictionary<string, int>();

            context.AppTimes
                .Where(item => interval.Equals(item.Time))
                .ForEachAsync((item) => AddAppInfoToTopAppInfo(item, top)).Wait();

            var result = top.ToList()
                .ConvertAll(item => new TopAppInfo {AppName = item.Key, TotalDuration = item.Value});

            result.RemoveRange(topSize, result.Count - topSize);

            return result;
        }

        private static void AddAppInfoToTopAppInfo(AppTimeModel appInfo, Dictionary<string, int> topAppInfo)
        {
            if (topAppInfo.Keys.Contains(appInfo.AppName))
            {
                topAppInfo[appInfo.AppName] += appInfo.Duration;
            }
            else
            {
                topAppInfo.Add(appInfo.AppName, appInfo.Duration);
            }
        }
    }
}
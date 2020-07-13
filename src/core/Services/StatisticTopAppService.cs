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

            if (interval.Equals(TimeInterval.All))
            {
                // TODO: check if works;
                context.AppTimes.ForEachAsync(item => AddAppInfoToTopAppInfo(item, top)).Wait();
            }
            else
            {
                context.AppTimes.Where(interval.Equals).ToList()
                    .ForEach((item) => AddAppInfoToTopAppInfo(item, top));
            }

            var result = top.ToList()
                .ConvertAll(item => new TopAppInfo {AppName = item.Key, TotalDuration = item.Value});

            result.Sort();
            Resize(result, topSize, TopAppInfo.Empty);

            return result;
        }

        public List<TopAppInfo> Get(int topSize) => Get(topSize, TimeInterval.All);

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

        // TODO: extension?;
        private static void Resize<T>(List<T> list, int length, T surrogate = default(T))
        {
            if (list.Count < length)
            {
                list.AddRange(Enumerable.Repeat(surrogate, length - list.Count));
            }

            if (list.Count > length)
            {
                list.RemoveRange(length, list.Count - length);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Mitheti.Core.Services
{
    public class StatisticDatabaseService : IStatisticDatabaseService
    {
        public static readonly int DaysOfWeekCount = Enum.GetValues(typeof(DayOfWeek)).Length;

        private readonly IDatabaseService _database;

        public StatisticDatabaseService(IDatabaseService database)
        {
            _database = database;
        }

        public async Task<(Dictionary<DayOfWeek, int> duration, Dictionary<DayOfWeek, double> percentage)>
            GetStatisticByDayOfWeek(TimePeriod period)
        {
            var duration = await GetDurationByDayOfWeek(period);
            var percentage = GetPercentagesFromDurationByDayOfWeek(duration);

            return (duration, percentage);
        }

        private async Task<Dictionary<DayOfWeek, int>> GetDurationByDayOfWeek(TimePeriod period)
        {
            var result = GetEmptyDictionaryByDayOfWeek(0);

            await using var context = _database.GetContext();
            var data = context.AppTimes.WhereTimePeriod(period);

            //? `ForEachAsync` is preferred to `Parallel.ForEach` for databases;
            //? see https://medium.com/@alex.puiu/parallel-foreach-async-in-c-36756f8ebe62 ;
            data.ForEachAsync(item => result[item.Time.DayOfWeek] += item.Duration).Wait();

            return result;
        }

        private static Dictionary<DayOfWeek, double> GetPercentagesFromDurationByDayOfWeek(
            Dictionary<DayOfWeek, int> durations)
        {
            var result = GetEmptyDictionaryByDayOfWeek(0.0);
            var summary = durations.Sum(item => item.Value);
            if (summary == 0)
            {
                //? avoiding 0/0 resulting in NaN;
                summary = 1;
            }

            foreach (var (key, value) in durations)
            {
                result[key] = (double) value / summary;
            }

            return result;
        }

        private static Dictionary<DayOfWeek, T> GetEmptyDictionaryByDayOfWeek<T>(T defaultValue)
        {
            var result = new Dictionary<DayOfWeek, T>();
            for (var i = 0; i < DaysOfWeekCount; i++)
            {
                result[(DayOfWeek) i] = defaultValue;
            }

            return result;
        }

        public async Task<(List<TopAppInfo> durations, Dictionary<string, double> percentages)>
            GetStatisticByAppName(int maximumApps, TimePeriod period)
        {
            var durations = await GetDurationByAppName(period);
            var percentage = GetPercentagesFromDurationByAppName(durations, maximumApps);

            TruncateList(durations, maximumApps);
            return (durations, percentage);
        }


        private async Task<List<TopAppInfo>> GetDurationByAppName(TimePeriod period)
        {
            await using var context = _database.GetContext();
            var data = context.AppTimes.WhereTimePeriod(period);

            var result = new List<TopAppInfo>();
            data.ForEachAsync(item => AddTime(item, result)).Wait();

            result.Sort();
            return result;
        }

        private static void AddTime(AppTimeModel item, List<TopAppInfo> result)
        {
            var founded = result.Find(value => value.AppName == item.AppName);

            if (founded == null)
            {
                result.Add(new TopAppInfo {AppName = item.AppName, Duration = item.Duration});
            }
            else
            {
                founded.Duration += item.Duration;
            }
        }

        private static void TruncateList<T>(List<T> list, int max)
        {
            if (max < 0)
            {
                return;
            }

            if (list.Count > max)
            {
                list.RemoveRange(max, list.Count - max);
            }
        }

        private static Dictionary<string, double> GetPercentagesFromDurationByAppName(List<TopAppInfo> data, int maximumApps)
        {
            var result = new Dictionary<string, double>();
            if (data.Count == 0)
            {
                //? avoid exception on `data.Sum`;
                return result;
            }

            var summary = data.Sum(item => item.Duration);
            if (summary == 0)
            {
                summary = 1;
            }

            var size = Math.Min(data.Count, maximumApps);
            for (var i = 0; i < size; i++)
            {
                result.Add(data[i].AppName, (double) data[i].Duration / summary);
            }
            return result;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
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

        public void GetStatisticByDayOfWeek(TimePeriod period, out Dictionary<DayOfWeek, int> duration, out Dictionary<DayOfWeek, double> percentages)
        {
            duration = GetDurationByDayOfWeek(period);
            percentages = GetPercentagesFromDurationByDayOfWeek(duration);
        }

        private Dictionary<DayOfWeek, int> GetDurationByDayOfWeek(TimePeriod period)
        {
            var result = GetEmptyDictionaryByDayOfWeek<int>(0);

            using var context = _database.GetContext();
            var data = context.AppTimes.WhereTimePeriod(period);

            //? `ForEachAsync` is preferred to `Parallel.ForEach` for databases;
            //? see https://medium.com/@alex.puiu/parallel-foreach-async-in-c-36756f8ebe62 ;
            data.ForEachAsync(item => result[item.Time.DayOfWeek] += item.Duration).Wait();

            return result;
        }

        private static Dictionary<DayOfWeek, double> GetPercentagesFromDurationByDayOfWeek(Dictionary<DayOfWeek, int> durations)
        {
            var result = GetEmptyDictionaryByDayOfWeek<double>(0.0);
            var max = durations.Aggregate((aggregated, item) => item.Value > aggregated.Value ? item : aggregated).Value;
            if (max == 0)
            {
                //? avoiding 0/0 resulting in NaN;
                max = 1;
            }

            foreach (var (key, value) in durations)
            {
                result[key] = (double)value / max;
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

        public void GetStatisticByAppName(int maximumApps, TimePeriod period, out List<TopAppInfo> durations, out Dictionary<string, double> percentages)
        {
            durations = GetDurationByAppName(period);
            percentages = GetPercentagesFromDurationByAppName(durations);

            TruncateList(durations, maximumApps);
        }


        private List<TopAppInfo> GetDurationByAppName(TimePeriod period)
        {
            using var context = _database.GetContext();
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
                result.Add(new TopAppInfo { AppName = item.AppName, Duration = item.Duration});
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

        private static Dictionary<string, double> GetPercentagesFromDurationByAppName(List<TopAppInfo> data)
        {
            var result = new Dictionary<string, double>();
            if (data.Count == 0)
            {
                //? avoid exception on `data.Aggregate`;
                return result;
            }

            // TODO: fix percentage calculation;
            var max = data.Aggregate((r, item) => item.Duration > r.Duration ? item : r).Duration;
            if (max == 0)
            {
                max = 1;
            }

            data.ForEach(item => result.Add(item.AppName, (double)item.Duration / max));
            return result;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mitheti.Core.Services
{
    public interface IStatisticDatabaseService
    {
        Task<(Dictionary<DayOfWeek, int> duration, Dictionary<DayOfWeek, double> percentage)>
            GetStatisticByDayOfWeek(TimePeriod period);

        Task<(List<TopAppInfo> durations, Dictionary<string, double> percentages)>
            GetStatisticByAppName(int maximumApps, TimePeriod period);
    }
}
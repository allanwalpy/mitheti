using System;
using System.Collections.Generic;

namespace Mitheti.Core.Services
{
    public interface IStatisticDatabaseService
    {
        void GetStatisticByDayOfWeek(TimePeriod period,
            out Dictionary<DayOfWeek, int> duration, out Dictionary<DayOfWeek, double> percentages);

        void GetStatisticByAppName(int maximumApps, TimePeriod period,
            out List<TopAppInfo> durations, out Dictionary<string, double> percentages);
    }
}
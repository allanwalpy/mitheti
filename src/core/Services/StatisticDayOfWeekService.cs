using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Mitheti.Core.Services
{
    public class StatisticDayOfWeekService : IStatisticDayOfWeekService
    {
        private const int DaysOfWeekCount = 7;
        
        public List<int> GetTotal() => GetList();

        public List<double> GetPercentage()
        {
            var result = Enumerable.Repeat((double)0, DaysOfWeekCount).ToList();
            var list = GetList();
            var max = list.Max();
            if (max == 0)
            {
                //? avoiding 0/0 resulting in NaN;
                max = 1;
            }

            for (var i = 0; i < DaysOfWeekCount; i++)
            {
                result[i] = (double)list[i] / max;
            }

            return result;
        }
        
        private static List<int> GetList()
        {
            
            using var context = new DatabaseContext();
            var result =  Enumerable.Repeat(0, DaysOfWeekCount).ToList();
            
            context.AppTimes
                .ForEachAsync(timeSpan => result[(int)timeSpan.Time.DayOfWeek] += timeSpan.Duration)
                .Wait();

            return result;
        }
    }
}
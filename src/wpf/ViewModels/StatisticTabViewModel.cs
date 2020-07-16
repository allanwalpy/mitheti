using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using Mitheti.Core;
using Mitheti.Core.Services;
using Mitheti.Wpf.Services;

namespace Mitheti.Wpf.ViewModels
{
    public class StatisticTabViewModel
    {
        public const int TopAppsCount = 10;
        private static readonly int DaysOfWeeksCount = StatisticDatabaseService.DaysOfWeekCount;

        private readonly IStatisticDatabaseService _statisticDatabase;

        public Dictionary<string, string> Localization { get; }
        public List<string> DayOfWeekString { get; private set; }
        public List<string> TopAppsString { get; private set; }

        public StatisticTabViewModel(ILocalizationService localization, IStatisticDatabaseService statisticDatabase)
        {
            Localization = localization.Data;
            _statisticDatabase=statisticDatabase;

            PopulateWithData();
        }

        public void PopulateWithData()
        {
            DayOfWeekString = GetDayOfWeekString();
            TopAppsString = GetTopAppsString();
        }

        private List<string> GetDayOfWeekString()
        {
            _statisticDatabase.GetStatisticByDayOfWeek(TimePeriod.All,
                out var durations, out var percentages );

            var durationsString = new List<string>();
            var percentagesString = new List<string>();

            for (var i = 0; i < DaysOfWeeksCount; i++)
            {
                var dayOfWeek = (DayOfWeek) i;

                durationsString.Add(TimeSpanAsLocalizedString(durations[dayOfWeek]));
                percentagesString.Add(PercentageAsLocalizedString(percentages[dayOfWeek]));
            }

            var result = new List<string>();
            for (var i = 0; i < DaysOfWeeksCount; i++)
            {
                result.Add(string.Format(Localization[$"Window:Statistic:DayOfWeek:Item"],
                    Localization[$"Window:Statistic:DayOfWeek:Name:{i}"],
                    durationsString[i], percentagesString[i]));
            }

            return result;
        }

        private string PercentageAsLocalizedString(double value)
            => string.Format(Localization[$"Window:Statistic:DayOfWeek:Percentage:Item"],
                value.ToString(format: Localization["Formats:Percentage"]));

        private string TimeSpanAsLocalizedString(int milliseconds)
        {
            var timeSpan = TimeSpan.FromMilliseconds(milliseconds);
            // TODO: fix format, deleting zeroes, except 0 seconds;
            return string.Format(Localization[$"Window:Statistic:DayOfWeek:Total:Item"],
                timeSpan.TotalDays, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }

        private List<string> GetTopAppsString()
        {
            _statisticDatabase.GetStatisticByAppName(TopAppsCount, TimePeriod.All,
                out var durations, out var percentages);

            var result = new List<string>();

            foreach (var item in durations)
            {
                var timeSpan = TimeSpan.FromSeconds(item.Duration);

                var timeSpanString = string.Format(Localization[$"Window:Statistic:TopApp:Item:Time"],
                    timeSpan.TotalDays, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
                var percentString = string.Format(Localization["Formats:Percentage"], percentages[item.AppName]);
                var appNameString = string.Format(Localization[$"Window:Statistic:TopApp:Item:AppName"], item.AppName);

                result.Add(string.Format(Localization[$"Window:Statistic:TopApp:Item"],
                        appNameString, timeSpanString, percentString));
            }

            for (var i = result.Count; i < TopAppsCount; i++)
            {
                result.Add(Localization[$"Window:Statistic:TopApp:Item:Null"]);
            }


            return result;
        }
    }
}
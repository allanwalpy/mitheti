using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualBasic;
using Mitheti.Core;
using Mitheti.Core.Services;
using Mitheti.Wpf.Models;
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
        public ObservableCollection<TopAppModel> TopAppsData { get; private set; }

        public StatisticTabViewModel(ILocalizationService localization, IStatisticDatabaseService statisticDatabase)
        {
            Localization = localization.Data;
            _statisticDatabase = statisticDatabase;

            PopulateWithData();
        }

        private void PopulateWithData()
        {
            DayOfWeekString = GetDayOfWeekString();
            TopAppsData = GetTopAppsDataView();
        }

        private List<string> GetDayOfWeekString()
        {
            _statisticDatabase.GetStatisticByDayOfWeek(TimePeriod.All,
                out var durations, out var percentages);

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

        private ObservableCollection<TopAppModel> GetTopAppsDataView()
        {
            _statisticDatabase.GetStatisticByAppName(TopAppsCount, TimePeriod.All,
                out var durations, out var percentages);

            var result = new ObservableCollection<TopAppModel>();

            foreach (var item in durations)
            {
                var percent = percentages[item.AppName];

                result.Add(new TopAppModel
                {
                    AppName = item.AppName,
                    Duration = string.Format(Localization["Window:Statistic:TopApp:Item:Time"], TimeSpan.FromMilliseconds(item.Duration)),
                    PercentageString = percent.ToString(Localization["Formats:Percentage"]),
                    Percentage = (int)(percent * 1000)
                });
            }

            return result;
        }
    }
}
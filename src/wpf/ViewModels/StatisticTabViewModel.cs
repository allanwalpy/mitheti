using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Mitheti.Core;
using Mitheti.Core.Services;
using Mitheti.Wpf.Models;
using Mitheti.Wpf.Services;

namespace Mitheti.Wpf.ViewModels
{
    public class StatisticTabViewModel
    {
        // TODO: add selector for value;
        public const int TopAppsCount = 10;
        private const string ConfigKey = "Window:Statistic";

        private readonly IStatisticDatabaseService _statisticDatabase;
        private readonly ILocalizationService _localization;

        public Dictionary<string, string> Localization { get; }
        public ObservableCollection<TopAppModel> TopAppsData { get; }
        public ObservableCollection<DayOfWeekModel> DayOfWeekData { get; }

        public StatisticTabViewModel(ILocalizationService localization, IStatisticDatabaseService statisticDatabase)
        {
            Localization = localization.Data;
            _localization = localization;
            _statisticDatabase = statisticDatabase;

            DayOfWeekData = new ObservableCollection<DayOfWeekModel>();;
            TopAppsData = new ObservableCollection<TopAppModel>();

            UpdateInfo().ConfigureAwait(false);
        }

        public async Task UpdateInfo()
        {
            var updateDayOfWeek = PopulateDayOfWeek();
            var updateTopApps = PopulateTopApps();
            await Task.WhenAll(updateDayOfWeek, updateTopApps);
        }

        private async Task PopulateDayOfWeek()
        {
            var (durations, percentages)
                = await _statisticDatabase.GetStatisticByDayOfWeek(TimePeriod.All);

            var dayOfWeekOrder = _localization.Config.GetList<int>($"{ConfigKey}:DayOfWeek:Order");
            var dayOfWeekNames = _localization.Config.GetList<string>($"{ConfigKey}:DayOfWeek:Name");

            if (DayOfWeekData.Count > 0)
            {
                DayOfWeekData.Clear();
            }

            foreach (var dayOfWeekNumber in dayOfWeekOrder)
            {
                var dayOfWeek = (DayOfWeek)dayOfWeekNumber;
                var duration = TimeSpan.FromMilliseconds(durations[dayOfWeek]);
                var percent = percentages[dayOfWeek];

                DayOfWeekData.Add(new DayOfWeekModel
                    {
                        DayOfWeek = dayOfWeekNames[dayOfWeekNumber],
                        Duration = string.Format(Localization[$"{ConfigKey}:DayOfWeek:Item:Time"], duration),
                        Percentage = (int)(percent * 1000),
                        PercentageString = percent.ToString(Localization["Formats:Percentage"]),
                    }
                );
            }
        }

        private async Task PopulateTopApps()
        {
            var (durations, percentages) =
                await _statisticDatabase.GetStatisticByAppName(TopAppsCount, TimePeriod.All);

            if (TopAppsData.Count > 0)
            {
                TopAppsData.Clear();
            }

            foreach (var item in durations)
            {
                var percent = percentages[item.AppName];

                TopAppsData.Add(new TopAppModel
                {
                    AppName = item.AppName,
                    Duration = string.Format(Localization[$"{ConfigKey}:TopApp:Item:Time"],
                        TimeSpan.FromMilliseconds(item.Duration)),
                    PercentageString = percent.ToString(Localization["Formats:Percentage"]),
                    Percentage = (int) (percent * 1000)
                });
            }
        }


    }
}
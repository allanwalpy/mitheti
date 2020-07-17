using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Mitheti.Core;
using Mitheti.Core.Services;
using Mitheti.Wpf.Models;
using Mitheti.Wpf.Services;

namespace Mitheti.Wpf.ViewModels
{
    public class StatisticTabViewModel
    {
        public const int TopAppsCount = 10;

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

            DayOfWeekData = GetDayOfWeekString();
            TopAppsData = GetTopAppsDataView();
        }

        private ObservableCollection<DayOfWeekModel> GetDayOfWeekString()
        {
            _statisticDatabase.GetStatisticByDayOfWeek(TimePeriod.All,
                out var durations, out var percentages);

            // TODO: compact "Window:Statistic:DayOfWeek/TopApp" to consts;
            var dayOfWeekOrder = _localization.Config.GetList<int>("Window:Statistic:DayOfWeek:Order");
            var dayOfWeekNames = _localization.Config.GetList<string>("Window:Statistic:DayOfWeek:Name");

            var result = new ObservableCollection<DayOfWeekModel>();

            foreach (var dayOfWeekNumber in dayOfWeekOrder)
            {
                var dayOfWeek = (DayOfWeek)dayOfWeekNumber;
                var duration = TimeSpan.FromMilliseconds(durations[dayOfWeek]);
                var percent = percentages[dayOfWeek];

                result.Add(new DayOfWeekModel
                    {
                        DayOfWeek = dayOfWeekNames[dayOfWeekNumber],
                        Duration = string.Format(Localization["Window:Statistic:DayOfWeek:Item:Time"], duration),
                        Percentage = (int)(percent * 1000),
                        PercentageString = percent.ToString(Localization["Formats:Percentage"]),
                    }
                );
            }

            return result;
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
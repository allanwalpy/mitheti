using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Mitheti.Core;
using Mitheti.Core.Services;
using Mitheti.Wpf.Models.Statistic;
using Rasyidf.Localization;

namespace Mitheti.Wpf.ViewModels
{
    public class StatisticTabViewModel
    {
        // TODO: add selector for value;
        public const int TopAppsCount = 10;

        private readonly IStatisticDatabaseService _statisticDatabase;

        public ObservableCollection<TopAppItemModel> TopAppsData { get; }
        public ObservableCollection<DayOfWeekItemModel> DayOfWeekData { get; }

        public StatisticTabViewModel(IStatisticDatabaseService statisticDatabase)
        {
            _statisticDatabase = statisticDatabase;

            DayOfWeekData = new ObservableCollection<DayOfWeekItemModel>();
            TopAppsData = new ObservableCollection<TopAppItemModel>();

            PopulateDayOfWeek().ConfigureAwait(false);
            PopulateTopApps().ConfigureAwait(false);
        }

        public async Task UpdateInfo() => await Task.WhenAll(PopulateDayOfWeek(), PopulateTopApps());

        private async Task PopulateDayOfWeek()
        {
            var (durations, percentages)
                = await _statisticDatabase.GetStatisticByDayOfWeek(TimePeriod.All);

            var dayOfWeekOrder = "Window:Statistic:DayOfWeek:Order".TranslateAsIntList();
            var dayOfWeekNames = "Window:Statistic:DayOfWeek:Name".TranslateAsList();

            for (var i = 0; i < dayOfWeekOrder.Count; i++)
            {
                var dayOfWeekNumber = dayOfWeekOrder[i];
                var dayOfWeek = (DayOfWeek) dayOfWeekNumber;
                var duration = TimeSpan.FromMilliseconds(durations[dayOfWeek]);
                var percent = percentages[dayOfWeek];

                var item = new DayOfWeekItemModel
                {
                    DayOfWeek = dayOfWeekNames[dayOfWeekNumber],
                    Duration = duration.HumanizeForStatistic(Extensions.LanguageCodeConfigKey.Translate()),
                    Percent = new PercentModel
                    {
                        Value = percent,
                        ValueString = percent.ToString("Formats:Percentage".Translate())
                    }
                };

                if (DayOfWeekData.Count <= i)
                {
                    DayOfWeekData.Add(item);
                }
                else
                {
                    DayOfWeekData[i] = item;
                }
            }
        }

        private async Task PopulateTopApps()
        {
            var (durations, percentages) =
                await _statisticDatabase.GetStatisticByAppName(TopAppsCount, TimePeriod.All);

            for (var i = 0; i < durations.Count; i++)
            {
                var item = durations[i];
                var duration = TimeSpan.FromMilliseconds(item.Duration);
                var percent = percentages[item.AppName];

                var info = new TopAppItemModel
                {
                    AppName = item.AppName,
                    Duration = duration.HumanizeForStatistic(Extensions.LanguageCodeConfigKey.Translate()),
                    Percent = new PercentModel
                    {
                        Value = percent,
                        ValueString = percent.ToString("Formats:Percentage".Translate())
                    }
                };

                if (TopAppsData.Count <= i)
                {
                    TopAppsData.Add(info);
                }
                else
                {
                    TopAppsData[i] = info;
                }
            }
        }
    }
}
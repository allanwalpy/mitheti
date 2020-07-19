using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Humanizer;
using Humanizer.Localisation;
using Mitheti.Core;
using Mitheti.Core.Services;
using Mitheti.Wpf.Models;
using Mitheti.Wpf.Services;
using Mitheti.Wpf.Views;

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

            DayOfWeekData = new ObservableCollection<DayOfWeekModel>();
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
                var dayOfWeek = (DayOfWeek) dayOfWeekNumber;
                var duration = TimeSpan.FromMilliseconds(durations[dayOfWeek]);
                var percent = percentages[dayOfWeek];

                DayOfWeekData.Add(new DayOfWeekModel
                    {
                        DayOfWeek = dayOfWeekNames[dayOfWeekNumber],
                        Duration = HumanizeTimeSpan(duration),
                        Percentage = (int) (percent * 1000),
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
                    Duration = HumanizeTimeSpan(TimeSpan.FromMilliseconds(item.Duration)),
                    PercentageString = percent.ToString(Localization["Formats:Percentage"]),
                    Percentage = (int) (percent * 1000)
                });
            }
        }

        public async Task Initialize(StatisticTab tab)
        {
            var plot = new ScottPlot.Plot(400, 400);
            var daysOfWeek = _localization.Config.GetList<int>($"{ConfigKey}:DayOfWeek:Order");
            var (duration, percentages) = await _statisticDatabase.GetStatisticByDayOfWeek(TimePeriod.All);
            var xValues = Enumerable.Range(0, daysOfWeek.Count).ToList().ConvertAll<double>(item => (double)item);
            var yValues = new List<double>();
            var labels = new List<string>();
            for (var i = 0; i < daysOfWeek.Count; i++)
            {
                var index = daysOfWeek[i];
                yValues.Add(percentages[(DayOfWeek)index] * 100);
                labels.Add(Localization[$"{ConfigKey}:DayOfWeek:Name:{index}"]);
            }

            plot.PlotBar(xValues.ToArray(), yValues.ToArray());

            //plot.Axis();
            plot.Grid(enable: true, color: System.Drawing.Color.Indigo, enableHorizontal: true, enableVertical: false);

            plot.XTicks(xValues.ToArray(), labels.ToArray());

            plot.SaveFig("tmp.png", true);
            //TODO: change to different library;
            var image = new Image {Source = new BitmapImage(new Uri("tmp.png", UriKind.Relative))};
            tab.DayOfWeekGraph.Content = image;
        }

        private string HumanizeTimeSpan(TimeSpan timeSpan) => timeSpan.Humanize(
            precision: 3,
            countEmptyUnits: true,
            culture: new CultureInfo(Localization["Language:Code"]),
            minUnit: TimeUnit.Second,
            maxUnit: TimeUnit.Day,
            toWords: false);
    }
}
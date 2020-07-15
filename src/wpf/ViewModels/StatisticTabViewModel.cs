using System;
using System.Collections.Generic;
using Mitheti.Core.Services;
using Mitheti.Wpf.Services;

namespace Mitheti.Wpf.ViewModels
{
    public class StatisticTabViewModel : BaseViewModel
    {
        private const int TopAppsCount = 10;

        private readonly IStatisticDayOfWeekService _databaseDayOfWeek;
        private readonly IStatisticTopAppService _databaseTopApp;

        public List<string> DayOfWeekString { get; private set; }
        public List<string> TopAppsString { get; private set; }

        public StatisticTabViewModel(ILocalizationService localization, IStatisticDayOfWeekService databaseDayOfWeek,
            IStatisticTopAppService databaseTopApp)
            : base(localization)
        {
            _databaseDayOfWeek = databaseDayOfWeek;
            _databaseTopApp = databaseTopApp;
            
            PopulateWithData();
        }

        public void PopulateWithData()
        {
            DayOfWeekString = GetDayOfWeekString();
            TopAppsString = GetTopAppsString();
        }

        private List<string> GetDayOfWeekString()
        {
            var totalString = _databaseDayOfWeek.GetTotal()
                .ConvertAll(TimeSpanAsLocalizedString);

            var percentageString = _databaseDayOfWeek.GetPercentage()
                .ConvertAll(PercentageAsLocalizedString);

            const int daysOfWeek = StatisticDayOfWeekService.DaysOfWeekCount;
            var result = new List<string>();
            for (var i = 0; i < daysOfWeek; i++)
            {
                result.Add(string.Format(Localization[$"Window:Statistic:DayOfWeek:Item"],
                    Localization[$"Window:Statistic:DayOfWeek:Name:{i}"],
                    totalString[i], percentageString[i]));
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
            var data = _databaseTopApp.Get(TopAppsCount);

            return data.ConvertAll(
                (info) => string.Format(Localization[$"Window:Statistic:TopApp:Item"],
                    string.Format(Localization[$"Window:Statistic:TopApp:Item:AppName"],
                        info.AppName ?? Localization[$"Window:Statistic:TopApp:Item:AppName:Null"]),
                    string.Format(Localization[$"Window:Statistic:TopApp:Item:Time"],
                        //TODO:FIXME;
                        TimeSpan.FromMilliseconds(info.TotalDuration).TotalDays,
                        TimeSpan.FromMilliseconds(info.TotalDuration).Hours,
                        TimeSpan.FromMilliseconds(info.TotalDuration).Minutes,
                        TimeSpan.FromMilliseconds(info.TotalDuration).Seconds
                    )));
        }
    }
}
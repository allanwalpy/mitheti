using System;
using System.Collections.Generic;
using System.Linq;
using Mitheti.Core.Services;
using Mitheti.Wpf.Services;

namespace Mitheti.Wpf.ViewModel
{
    public class StatisticWindowViewModel : BaseViewModel
    {
        private const int TopAppsCount = 10;

        private readonly IStatisticDayOfWeekService _databaseDayOfWeek;
        private readonly IStatisticTopAppService _databaseTopApp;

        public List<string> DayOfWeekString { get; }
        public List<string> TopAppsString { get; }

        public StatisticWindowViewModel(ILocalizationService localization, IStatisticDayOfWeekService databaseDayOfWeek, IStatisticTopAppService databaseTopApp)
            : base(localization)
        {
            _databaseDayOfWeek = databaseDayOfWeek;
            _databaseTopApp = databaseTopApp;

            DayOfWeekString = Enumerable.Repeat(String.Empty, StatisticDayOfWeekService.DaysOfWeekCount).ToList();
            SetDayOfWeekString();

            TopAppsString = GetTopAppsString();
        }

        private void SetDayOfWeekString()
        {
            var totalString = _databaseDayOfWeek.GetTotal()
                .ConvertAll(TimeSpanAsLocalizedString);

            var percentageString = _databaseDayOfWeek.GetPercentage()
                .ConvertAll(PercentageAsLocalizedString);

            for (var i = 0; i < 7; i++)
            {
                DayOfWeekString[i] = string.Format(Localization[$"{nameof(StatisticWindow)}:DayOfWeek:Item"],
                    Localization[$"{nameof(StatisticWindow)}:DayOfWeek:Name:{i}"],
                    totalString[i], percentageString[i]);
            }
        }

        private string PercentageAsLocalizedString(double value)
            => string.Format(Localization[$"{nameof(StatisticWindow)}:DayOfWeek:Percentage:Item"],
                value.ToString(format: Localization["Formats:Percentage"]));

        private string TimeSpanAsLocalizedString(int milliseconds)
        {
            var timeSpan = TimeSpan.FromMilliseconds(milliseconds);
            // TODO: fix format, deleting zeroes, except 0 seconds;
            return string.Format(Localization[$"{nameof(StatisticWindow)}:DayOfWeek:Total:Item"],
                timeSpan.TotalDays, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }

        private List<string> GetTopAppsString()
        {
            var data = _databaseTopApp.Get(TopAppsCount);

            return data.ConvertAll<string>(
                (info) => string.Format(Localization[$"{nameof(StatisticWindow)}:TopApp:Item"],
                    string.Format(Localization[$"{nameof(StatisticWindow)}:TopApp:Item:AppName"], 
                        info.AppName ?? Localization[$"{nameof(StatisticWindow)}:TopApp:Item:AppName:Null"]),
                    string.Format(Localization[$"{nameof(StatisticWindow)}:TopApp:Item:Time"],
                        TimeSpan.FromMilliseconds(info.TotalDuration).TotalDays,
                        TimeSpan.FromMilliseconds(info.TotalDuration).Hours,
                        TimeSpan.FromMilliseconds(info.TotalDuration).Minutes,
                        TimeSpan.FromMilliseconds(info.TotalDuration).Seconds
                    )));
        }
    }
}
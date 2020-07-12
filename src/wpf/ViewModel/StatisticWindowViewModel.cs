using System;
using System.Collections.Generic;
using System.Linq;
using Mitheti.Core;
using Mitheti.Core.Services;
using Mitheti.Wpf.Services;

namespace Mitheti.Wpf.ViewModel
{
    public class StatisticWindowViewModel : BaseViewModel
    {
        private readonly IStatisticDayOfWeekService _database;

        public List<string> DayOfWeekString { get; }

        public StatisticWindowViewModel(ILocalizationService localization, IStatisticDayOfWeekService database)
            : base(localization)
        {
            _database = database;

            DayOfWeekString = Enumerable.Repeat(String.Empty, 7).ToList();
            SetDayOfWeekString();
        }

        private void SetDayOfWeekString()
        {
            var totalString = _database.GetTotal(TimeInterval.All)
                .ConvertAll(TimeSpanAsLocalizedString);
                
            var percentageString = _database.GetPercentage(TimeInterval.All)
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
            var timeSpan = new TimeSpan(milliseconds);
            // TODO: fix format, deleting zeroes, except 0 seconds;
            return string.Format(Localization[$"{nameof(StatisticWindow)}:DayOfWeek:Total:Item"],
                timeSpan.TotalDays, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }
    }
}
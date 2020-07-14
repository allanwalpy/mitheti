using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Mitheti.Core.Services;
using Mitheti.Wpf.Services;

namespace Mitheti.Wpf.ViewModel
{
    public class MainWindowViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private const int TopAppsCount = 10;
        
        private readonly IStatisticDayOfWeekService _databaseDayOfWeek;
        private readonly IStatisticTopAppService _databaseTopApp;
        private readonly IWatcherControlService _watcherControl;

        public event PropertyChangedEventHandler PropertyChanged;
        
        public List<string> DayOfWeekString { get; }
        public List<string> TopAppsString { get; }
        public string IsLaunchedString => Localization[$"Window:Main:StatusLabel:{_watcherControl.IsLaunched}"];

        public MainWindowViewModel(ILocalizationService localization, IWatcherControlService watcherControl, IStatisticDayOfWeekService databaseDayOfWeek, IStatisticTopAppService databaseTopApp)
            : base(localization)
        {
            _databaseDayOfWeek = databaseDayOfWeek;
            _databaseTopApp = databaseTopApp;
            _watcherControl = watcherControl;
            
            _watcherControl.StatusChanged += (sender, args) => { OnPropertyChanged(nameof(IsLaunchedString)); };
            
            DayOfWeekString = Enumerable.Repeat(string.Empty, StatisticDayOfWeekService.DaysOfWeekCount).ToList();
            SetDayOfWeekString();

            TopAppsString = GetTopAppsString();
        }

        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        
        private void SetDayOfWeekString()
        {
            var totalString = _databaseDayOfWeek.GetTotal()
                .ConvertAll(TimeSpanAsLocalizedString);

            var percentageString = _databaseDayOfWeek.GetPercentage()
                .ConvertAll(PercentageAsLocalizedString);

            for (var i = 0; i < 7; i++)
            {
                DayOfWeekString[i] = string.Format(Localization[$"Window:Statistic:DayOfWeek:Item"],
                    Localization[$"Window:Statistic:DayOfWeek:Name:{i}"],
                    totalString[i], percentageString[i]);
            }
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
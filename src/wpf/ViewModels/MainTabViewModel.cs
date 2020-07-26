using System.Collections.Generic;
using System.ComponentModel;
using Mitheti.Core.Services;
using Mitheti.Wpf.Services;

namespace Mitheti.Wpf.ViewModels
{
    public class MainTabViewModel : INotifyPropertyChanged
    {
        private readonly IWatcherControlService _watcherControl;

        public event PropertyChangedEventHandler PropertyChanged;

        public Dictionary<string, string> Localization { get; }

        public bool IsLaunched => _watcherControl.IsLaunched;
        public string IsLaunchedString => Localization[$"Window:Main:StatusLabel:{IsLaunched}"];

        public MainTabViewModel(ILocalizationService localization, IWatcherControlService watcherControl)
        {
            Localization = localization.Data;

            _watcherControl = watcherControl;
            //? for singleton lifetime reference see https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1#singleton ;
            _watcherControl.WatcherStatusChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, WatcherStatusEventArgs args)
        {
            OnPropertyChangedProperty(nameof(IsLaunched));
            OnPropertyChangedProperty(nameof(IsLaunchedString));
        }

        private void OnPropertyChangedProperty(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

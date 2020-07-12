using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Extensions.Configuration;
using Mitheti.Core.Services;
using Mitheti.Wpf.Services;

namespace Mitheti.Wpf.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly IWatcherControlService _watcherControl;

        public event PropertyChangedEventHandler PropertyChanged;

        public Dictionary<string, string> Localization { get; }
        public string IsLaunchedString => Localization[$"MainWindow.StatusLabel.{_watcherControl.IsLaunched}"];

        public MainWindowViewModel(ILocalizationService localization, IWatcherControlService watcherControl)
        {
            Localization = localization.Data;

            _watcherControl = watcherControl;
            _watcherControl.StatusChanged += (sender, args) => { OnPropertyChanged(nameof(IsLaunchedString)); };
        }

        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
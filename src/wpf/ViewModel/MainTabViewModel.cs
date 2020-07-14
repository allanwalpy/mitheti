using System.ComponentModel;
using Mitheti.Core.Services;
using Mitheti.Wpf.Services;

namespace Mitheti.Wpf.ViewModel
{
    public class MainTabViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private readonly IWatcherControlService _watcherControl;

        public event PropertyChangedEventHandler PropertyChanged;

        public string IsLaunchedString => Localization[$"Window:Main:StatusLabel:{_watcherControl.IsLaunched}"];

        public MainTabViewModel(ILocalizationService localization, IWatcherControlService watcherControl)
            : base(localization)
        {
            _watcherControl = watcherControl;
            _watcherControl.StatusChanged += (sender, args) => { OnPropertyChanged(nameof(IsLaunchedString)); };
        }

        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
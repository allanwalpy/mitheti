using System.ComponentModel;
using Mitheti.Core.Services;

namespace Mitheti.Wpf.ViewModels
{
    public class MainTabViewModel : INotifyPropertyChanged
    {
        private readonly IWatcherControlService _watcherControl;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsLaunched => _watcherControl.IsLaunched;
        public string IsLaunchedString => $"Window:Main:StatusLabel:{IsLaunched}".Translate();

        public MainTabViewModel(IWatcherControlService watcherControl)
        {
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

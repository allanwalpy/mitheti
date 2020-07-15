using System.ComponentModel;
using Mitheti.Core.Services;
using Mitheti.Wpf.Services;

namespace Mitheti.Wpf.ViewModels
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

            // опасно такие подписки делать, IWatcherControlService может в контейнере IServiceCollection поменяться а эта подписка останется висеть
            // нужно прогуглить эту ситуцию, что с евентами делает IServiceCollection с хостом
            // может он там переподписывается и тогда здесь норм
            _watcherControl.StatusChanged += (sender, args) => { OnPropertyChanged(nameof(IsLaunchedString)); };
        }

        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

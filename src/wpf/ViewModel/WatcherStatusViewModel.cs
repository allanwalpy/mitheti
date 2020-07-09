using System;
using System.ComponentModel;
using Mitheti.Wpf.Services;

namespace Mitheti.Wpf.ViewModel
{
    public class WatcherStatusViewModel : INotifyPropertyChanged
    {
        private readonly IWatcherControlService _watcherControlService;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsLaunched
        {
            get => _watcherControlService.IsLaunched;
        }

        public WatcherStatusViewModel(IWatcherControlService watcherControlService)
        {
            _watcherControlService = watcherControlService;
            _watcherControlService.StatusChanged += (sender, args) =>
            {
                OnPropertyChanged(nameof(IsLaunched));
            };
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
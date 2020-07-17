using System.Threading.Tasks;
using System.Windows;
using Mitheti.Core.Services;
using Mitheti.Wpf.Services;
using Mitheti.Wpf.ViewModels;

namespace Mitheti.Wpf.Views
{
    public partial class MainTab
    {
        private readonly ILocalizationService _localization;
        private readonly IWatcherControlService _watcherControl;
        private MainWindow _window;

        public MainTab(MainTabViewModel viewModel,  ILocalizationService localization, IWatcherControlService watcherControl)
        {
            _localization = localization;
            _watcherControl = watcherControl;
            watcherControl.WatcherStatusChanged += OnStatusChanged;

            DataContext = viewModel;
            InitializeComponent();

            SetLauncherButton(false, false);
        }

        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            _window = (MainWindow) Window.GetWindow(this);
        }

        private void OnLauncherClick(object sender, RoutedEventArgs args)
        {
            var isLaunched = _watcherControl.IsLaunched;

            SetLauncherButton(isLaunched, true);

            if (isLaunched)
            {
                Task.Run(_watcherControl.Stop).ConfigureAwait(false);
            }
            else
            {
                Task.Run(_watcherControl.Start).ConfigureAwait(false);
            }
        }

        private void OnStatusChanged(object sender, WatcherStatusEventArgs args)
            => SetLauncherButtonDispatched(args.IsLaunched, false);

        private void SetLauncherButtonDispatched(bool isLaunched, bool isChanging)
            => Dispatcher.Invoke(() => SetLauncherButton(isLaunched, isChanging));

        private void SetLauncherButton(bool isLaunched, bool isChanging)
        {
            LauncherButton.Content = _localization[$"Window:Main:Button:Launcher:{isLaunched}:{isChanging}"];
            LauncherButton.IsEnabled = !isChanging;
        }

        private void OnExitClick(object sender, RoutedEventArgs args)
        {
            _watcherControl.WatcherStatusChanged -= OnStatusChanged;
            _window.Exit();
        }
    }
}

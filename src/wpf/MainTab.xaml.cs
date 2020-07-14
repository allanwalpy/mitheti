using System.Threading.Tasks;
using System.Windows;
using Mitheti.Core.Services;
using Mitheti.Wpf.Services;
using Mitheti.Wpf.ViewModel;

namespace Mitheti.Wpf
{
    public partial class MainTab
    {
        private readonly IWatcherControlService _watcherControl;
        private MainWindow _window;

        public MainTab(ILocalizationService localization, IWatcherControlService watcherControl)
        {
            _watcherControl = watcherControl;
            
            DataContext = new MainTabViewModel(localization, watcherControl);
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            _window = (MainWindow) Window.GetWindow(this);
        }

        private void OnStartClick(object sender, RoutedEventArgs args) =>
            Task.Run(_watcherControl.Start).ConfigureAwait(false);

        private void OnStopClick(object sender, RoutedEventArgs args) =>
            Task.Run(_watcherControl.Stop).ConfigureAwait(false);

        private void OnExitClick(object sender, RoutedEventArgs args) => _window.Exit();
    }
}
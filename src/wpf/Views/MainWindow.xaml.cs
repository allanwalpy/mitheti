using System.ComponentModel;
using Mitheti.Wpf.Services;
using Mitheti.Wpf.ViewModels;

namespace Mitheti.Wpf.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly ITrayManagerService _tray;

        public MainWindow(ILocalizationService localization, MainWindowViewModel viewModel, ITrayManagerService tray)
        {
            DataContext = viewModel;
            InitializeComponent();
            viewModel.SetTabs(TabControl);

            Title = localization[$"Window:Title"];
            Closing += HideWindow;

            _tray = tray;
            _tray.Initialize(this);
        }

        private void HideWindow(object sender, CancelEventArgs args)
        {
            args.Cancel = true;
            Hide();
        }

        internal void Exit()
        {
            _tray.Dispose();

            Closing -= HideWindow;
            Close();
        }
    }
}
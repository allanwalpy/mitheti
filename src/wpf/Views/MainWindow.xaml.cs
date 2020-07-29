using System;
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
            _tray.WindowShowing += OnShow;
            _tray.WindowExiting += OnExit;
        }

        private void HideWindow(object sender, CancelEventArgs args)
        {
            args.Cancel = true;
            Hide();
        }

        private void OnShow(object sender, EventArgs args)
        {
            Show();
            Exit();
        }

        private void OnExit(object sender, EventArgs args) => Exit();

        internal void Exit()
        {
            _tray.WindowShowing -= OnShow;
            _tray.WindowExiting -= OnExit;
            _tray.Dispose();

            Closing -= HideWindow;
            Close();
        }
    }
}
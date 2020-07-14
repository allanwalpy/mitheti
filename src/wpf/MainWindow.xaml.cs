using System;
using System.ComponentModel;
using System.Windows;
using Forms = System.Windows.Forms;
using Mitheti.Core.Services;
using Mitheti.Wpf.Services;
using Mitheti.Wpf.ViewModel;

namespace Mitheti.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly IWatcherControlService _watcherControl;
        private readonly ILocalizationService _localization;
        private readonly Forms.NotifyIcon _trayIcon;

        public MainWindow(ILocalizationService localization, IWatcherControlService watcherControl,
            IStatisticDayOfWeekService dayOfWeek, IStatisticTopAppService topApp)
        {
            _localization = localization;
            _watcherControl = watcherControl;

            DataContext = new MainWindowViewModel(localization, watcherControl, dayOfWeek, topApp);

            InitializeComponent();

            _trayIcon = new Forms.NotifyIcon();
            ConfigureTray();

            Title = _localization[$"Window:Title"];
            Closing += HideWindow;
        }

        private void ConfigureTray()
        {
            //TODO: make separate class configuration for tray?;
            _trayIcon.MouseClick += OnTrayIconClick;
            _trayIcon.Icon = new System.Drawing.Icon("./Resources/trayIcon.ico");
            _trayIcon.Visible = true;

            _trayIcon.ContextMenuStrip = new Forms.ContextMenuStrip();
            _trayIcon.ContextMenuStrip.Items.Add(_localization["Tray:Options:Show"]).Click += OnTrayClickShow;
            _trayIcon.ContextMenuStrip.Items.Add(_localization["Tray:Options:Close"]).Click += OnTrayClickExit;
        }

        private void OnTrayIconClick(object? sender, Forms.MouseEventArgs args)
        {
            var isLeftClick = ((args.Button & Forms.MouseButtons.Left) != 0);
            if (isLeftClick)
            {
                OnTrayClickShow(sender, args);
            }
        }

        private void OnTrayClickShow(object? sender, EventArgs args)
        {
            Activate();
            Show();
        }

        private void OnTrayClickExit(object sender, EventArgs args) => Exit();

        private void HideWindow(object sender, CancelEventArgs args)
        {
            args.Cancel = true;
            Hide();
        }

        private void OnStartClick(object sender, RoutedEventArgs args) =>
            _watcherControl.StartAsync().ConfigureAwait(false);

        private void OnStopClick(object sender, RoutedEventArgs args) =>
            _watcherControl.StopAsync().ConfigureAwait(false);

        private void OnExitClick(object sender, RoutedEventArgs args) => Exit();

        private void Exit()
        {
            _trayIcon.MouseClick -= OnTrayIconClick;
            _trayIcon.ContextMenuStrip.Dispose();
            _trayIcon.Dispose();

            Closing -= HideWindow;
            Close();
        }
    }
}
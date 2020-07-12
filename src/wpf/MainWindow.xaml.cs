using System;
using System.Collections.Generic;
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
        private readonly Dictionary<string, string> _localization;
        private readonly Forms.NotifyIcon _trayIcon;

        public MainWindow(ILocalizationService localization, IWatcherControlService watcherControl)
        {
            _localization = localization.Data;
            _watcherControl = watcherControl;

            DataContext = new MainWindowViewModel(localization, watcherControl);

            InitializeComponent();

            _trayIcon = new Forms.NotifyIcon();
            ConfigureTray();

            Title = _localization["MainWindow:Title"];
            Closing += HideWindow;
        }

        private void ConfigureTray()
        {
            //TODO: make separate class configuration for trayIcon?;
            _trayIcon.MouseClick += OnTrayIconClick;
            _trayIcon.Icon = new System.Drawing.Icon("./Resources/trayIcon.ico");
            _trayIcon.Visible = true;

            _trayIcon.ContextMenuStrip = new Forms.ContextMenuStrip();
            _trayIcon.ContextMenuStrip.Items.Add(_localization["Tray:Option:Show"]).Click += OnTrayClickShow;
            _trayIcon.ContextMenuStrip.Items.Add(_localization["Tray:Option:Close"]).Click += OnTrayClickExit;
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

        private void OnTrayClickExit(object sender, EventArgs args)
        {
            _trayIcon.MouseClick -= OnTrayIconClick;
            _trayIcon.ContextMenuStrip.Dispose();
            _trayIcon.Dispose();

            Closing -= HideWindow;
            Close();
        }

        private void OnStartClick(object sender, RoutedEventArgs args) => _watcherControl.Start();

        private void OnStopClick(object sender, RoutedEventArgs args) => _watcherControl.StopAsync().Wait();

        private void OnStatisticClick(object sender, RoutedEventArgs e)
        {
            StatisticWindow window = new StatisticWindow();
            window.Show();
        }

        private void HideWindow(object sender, CancelEventArgs args)
        {
            args.Cancel = true;
            Hide();
        }
    }
}

using System;
using System.ComponentModel;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Forms = System.Windows.Forms;

using Mitheti.Wpf.Services; 

namespace Mitheti.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IConfiguration _config;
        private readonly IWatcherControlService _watcherControl;
        private Forms.NotifyIcon _trayIcon;
        
        public MainWindow(IConfiguration config, IWatcherControlService watcherControl)
        {
            _config = config;
            _watcherControl = watcherControl;
            
            InitializeComponent();

            SetTrayIcon();
            Closing += HideWindow;
        }

        private void SetTrayIcon()
        {
            //TODO: make separate class configuration for trayIcon?; 
            _trayIcon = new Forms.NotifyIcon();
            
            _trayIcon.MouseClick += OnTrayIconClick;
            _trayIcon.DoubleClick += RevealWindow;
            _trayIcon.Icon = new System.Drawing.Icon("./Resources/trayIcon.ico");
            _trayIcon.Visible = true;

            _trayIcon.ContextMenuStrip = new Forms.ContextMenuStrip();
            _trayIcon.ContextMenuStrip.Items.Add("Show").Click += RevealWindow;
            _trayIcon.ContextMenuStrip.Items.Add("Exit").Click += ExitApp;
        }
        
        private void OnTrayIconClick(object sender, Forms.MouseEventArgs args)
        {
            bool isLeftClick = ((args.Button & Forms.MouseButtons.Left) != 0);
            if (isLeftClick)
            {
                RevealWindow(sender, args);
            }
        }

        private void RevealWindow(object sender, EventArgs args)
        {
            Activate();
            Show();
        }

        private void ExitApp(object sender, EventArgs args)
        {
            _trayIcon.MouseClick -= OnTrayIconClick;
            _trayIcon.DoubleClick -= RevealWindow;
            
            _trayIcon.Dispose();
            _trayIcon = null;

            Closing -= HideWindow;
            Close();
        }

        private void OnStartClick(object sender, RoutedEventArgs args)
        {
            _watcherControl.Start();
        }

        private void OnStopClick(object sender, RoutedEventArgs args)
        {
            _watcherControl.StopAsync().Wait();
        }

        private void OnStatisticClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HideWindow(object sender, CancelEventArgs args)
        {
            args.Cancel = true;
            Hide();
        }
    }
}

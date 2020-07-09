using System;
using System.ComponentModel;
using System.Windows;

using Forms = System.Windows.Forms;

namespace Mitheti.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Forms.NotifyIcon _trayIcon;
        
        public MainWindow()
        {
            InitializeComponent();

            SetTrayIcon();

            Closing += HideWindow;
            
            Show();
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

        private void StartClick(object sender, RoutedEventArgs args)
        {
            throw new NotImplementedException();
        }

        private void StopClick(object sender, RoutedEventArgs args)
        {
            throw new NotImplementedException();
        }

        private void StatisticClick(object sender, RoutedEventArgs e)
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

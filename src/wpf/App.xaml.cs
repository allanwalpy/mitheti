using System;
using System.ComponentModel;
using System.Windows;

using Forms = System.Windows.Forms;

namespace Mitheti.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Forms.NotifyIcon _notifyIcon;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow = new MainWindow();
            MainWindow.Closing += this.CloseWindow;

            _notifyIcon = new Forms.NotifyIcon();
            this.SetNotifyIcon();

            MainWindow.Show();
        }

        private void SetNotifyIcon()
        {
            _notifyIcon.DoubleClick += this.ShowWindow;
            //FIXME: conflicts with context menu on rigth click;
            _notifyIcon.Click += this.ShowWindow;
            _notifyIcon.Icon = new System.Drawing.Icon("./Resources/trayIcon.ico");
            _notifyIcon.Visible = true;

            _notifyIcon.ContextMenuStrip = new Forms.ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("Show").Click += this.ShowWindow;
            _notifyIcon.ContextMenuStrip.Items.Add("Exit").Click += this.ExitApp;
        }

        private void ShowWindow(object sender, EventArgs args)
        {
            MainWindow.Activate();
            MainWindow.Show();
        }

        private void ExitApp(object sender, EventArgs args)
        {
            MainWindow.Closing -= this.CloseWindow;
            MainWindow.Close();

            _notifyIcon.Dispose();
            _notifyIcon = null;
        }

        private void CloseWindow(object sender, CancelEventArgs cancelArgs)
        {
            cancelArgs.Cancel = true;
            MainWindow.Hide();
        }
    }
}

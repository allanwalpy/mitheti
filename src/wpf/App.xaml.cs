using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;

using Forms = System.Windows.Forms;

namespace Mitheti.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public const string AppId = "fbffa2ce-2f82-4945-84b1-9d9ba04dc90c";

        private Mutex _instanceMutex;
        private Forms.NotifyIcon _notifyIcon;

        protected override void OnStartup(StartupEventArgs e)
        {
            this.ShutdownIfMoreThanOneInstance();

            base.OnStartup(e);

            MainWindow = new MainWindow();
            MainWindow.Closing += this.CloseWindow;

            this.SetNotifyIcon();

            MainWindow.Show();
        }

        private void ShutdownIfMoreThanOneInstance()
        {
            bool isCreatedNew;
            _instanceMutex = new Mutex(true, $"Global\\{AppId}", out isCreatedNew);

            if (isCreatedNew)
            {
                return;
            }

            _instanceMutex = null;
            Application.Current.Shutdown();
        }

        private void SetNotifyIcon()
        {
            _notifyIcon = new Forms.NotifyIcon();

            _notifyIcon.MouseClick += this.OnTrayIconClick;
            _notifyIcon.Icon = new System.Drawing.Icon("./Resources/trayIcon.ico");
            _notifyIcon.Visible = true;

            _notifyIcon.ContextMenuStrip = new Forms.ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("Show").Click += this.ShowWindow;
            _notifyIcon.ContextMenuStrip.Items.Add("Exit").Click += this.ExitApp;
        }

        private void OnTrayIconClick(object sender, Forms.MouseEventArgs args)
        {
            bool isLeftClick = ((args.Button & Forms.MouseButtons.Left) != 0);
            if (isLeftClick)
            {
                this.ShowWindow(sender, args);
            }
        }

        private void ShowWindow(object? sender, EventArgs args)
        {
            MainWindow.Activate();
            MainWindow.Show();
        }

        private void ExitApp(object sender, EventArgs args)
        {
            _notifyIcon.Click -= this.ShowWindow;
            _notifyIcon.DoubleClick -= this.ShowWindow;

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

        protected override void OnExit(ExitEventArgs args)
        {
            _instanceMutex?.ReleaseMutex();

            base.OnExit(args);
        }
    }
}

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
        private bool _isExiting = false;
        private Forms.NotifyIcon _notifyIcon;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow = new MainWindow();
            MainWindow.Closing += CloseWindow;

            _notifyIcon = new Forms.NotifyIcon();
            this.SetNotifyIcon();

            MainWindow.Show();
        }

        private void SetNotifyIcon()
        {
            _notifyIcon.DoubleClick += (sender, args) => ShowWindow();
            //FIXME: conflicts with context menu on rigth click;
            _notifyIcon.Click += (sender, args) => ShowWindow();
            _notifyIcon.Icon = new System.Drawing.Icon("./Resources/trayIcon.ico");
            _notifyIcon.Visible = true;

            _notifyIcon.ContextMenuStrip = new Forms.ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("Show").Click += (sender, args) => ShowWindow();
            _notifyIcon.ContextMenuStrip.Items.Add("Exit").Click += (sender, args) => ExitApp();
        }

        private void ShowWindow()
        {
            MainWindow.Activate();
            MainWindow.Show();
        }

        private void ExitApp()
        {
            _isExiting = true;

            MainWindow.Close();

            _notifyIcon.Dispose();
            _notifyIcon = null;
        }

        private void CloseWindow(object sender, CancelEventArgs cancelArgs)
        {
            if (!_isExiting)
            {
                cancelArgs.Cancel = true;
                MainWindow.Hide();
            }
        }

    }
}

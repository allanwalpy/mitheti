using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using Microsoft.Extensions.Hosting;

using Forms = System.Windows.Forms;

using WebProgram = Mitheti.Web.Program;

namespace Mitheti.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public const string AppId = "fbffa2ce-2f82-4945-84b1-9d9ba04dc90c";
        private const int WaitUntilShutdownSeconds = 5;

        private IHost _host;
        private IHost _webHost;
        
        private Mutex _instanceMutex;
        private Forms.NotifyIcon _notifyIcon;

        public App()
        {
            _host = new HostBuilder().Build();
        }

        private async void StartupApp(object sender, StartupEventArgs args)
        {
            this.ShutdownIfLaunched();

            await _host.RunAsync();

            MainWindow = new MainWindow();
            MainWindow.Closing += this.CloseWindow;

            this.SetNotifyIcon();

            MainWindow.Show();
            
            _webHost = WebProgram.CreateHostBuilder(new string[0]).Build();
            //TODO:FIXME: await and ConfigureAwait(false)?; 
            await _webHost.RunAsync().ConfigureAwait(false);
        }
        
        private async void ExitApp(object sender, ExitEventArgs args)
        {
            using (_host)
            {
                await _host.StopAsync(TimeSpan.FromSeconds(WaitUntilShutdownSeconds));
            }

            _instanceMutex?.ReleaseMutex();
        }

        private void ShutdownIfLaunched()
        {
            _instanceMutex = new Mutex(true, $"Global\\{AppId}", out var isCreatedNew);

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
            _notifyIcon.ContextMenuStrip.Items.Add("Exit").Click += this.OnExitApp;
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

        private void OnExitApp(object sender, EventArgs args)
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

        
    }
}

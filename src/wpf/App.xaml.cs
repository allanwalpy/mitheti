using System.Threading;
using System.Windows;

namespace Mitheti.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public const string AppId = "fbffa2ce-2f82-4945-84b1-9d9ba04dc90c";

        private readonly HostControl _hostControl;
        private readonly Mutex _instanceMutex;

        public App()
        {
            _instanceMutex = new Mutex(true, $"Global\\{AppId}", out var isCreatedNew);

            if (!isCreatedNew)
            {
                _instanceMutex = null;
                return;
            }

            _hostControl = new HostControl();
        }

        private async void StartupApp(object sender, StartupEventArgs args)
        {
            await _hostControl.StartAsync();

            MainWindow = _hostControl.GetService<MainWindow>();
            MainWindow.Show();
        }

        private async void ExitApp(object sender, ExitEventArgs args)
        {
            await _hostControl.StopAsync();
            _instanceMutex?.ReleaseMutex();
        }
    }
}

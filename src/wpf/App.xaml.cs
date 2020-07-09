using System.Threading;
using System.Windows;
using Mitheti.Wpf.Services;

namespace Mitheti.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public const string AppId = "fbffa2ce-2f82-4945-84b1-9d9ba04dc90c";
        
        private IHostControlService _hostControl;
        private Mutex? _instanceMutex;

        public App()
        {
            _hostControl = new HostControlService();
        }

        private async void StartupApp(object sender, StartupEventArgs args)
        {
            ShutdownIfLaunched();
            await _hostControl.StopAsync();
        }
        
        private async void ExitApp(object sender, ExitEventArgs args)
        {
            await _hostControl.StopAsync();
            _instanceMutex?.ReleaseMutex();
        }

        private void ShutdownIfLaunched()
        {
            _instanceMutex = new Mutex(true, $"Global\\{AppId}", out var isCreatedNew);

            if (!isCreatedNew)
            {
                _instanceMutex = null;
                Application.Current.Shutdown();
            }
        }
    }
}

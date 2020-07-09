using System;
using System.Threading;
using System.Windows;
using Microsoft.Extensions.Hosting;

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
        
        private Mutex? _instanceMutex;

        public App()
        {
            _host = Extensions.CreateAppHost();
        }

        private async void StartupApp(object sender, StartupEventArgs args)
        {
            this.ShutdownIfLaunched();

            await _host.RunAsync();
            
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

            if (!isCreatedNew)
            {
                _instanceMutex = null;
                Application.Current.Shutdown();
            }
        }
    }
}

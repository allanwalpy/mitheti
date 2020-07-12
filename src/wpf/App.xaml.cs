using System;
using System.Threading;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mitheti.Core.Services;
using Mitheti.Wpf.Services;

namespace Mitheti.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public const string AppId = "fbffa2ce-2f82-4945-84b1-9d9ba04dc90c";
        public const int ExitCodeAlreadyLaunched = 101; //? see https://docs.microsoft.com/en-us/windows/win32/debug/system-error-codes--0-499- ;
        public const string LocalizationFile = "localization.json";
        private const int WaitForStopSeconds = 5;

        public readonly IHost Host;
        private readonly Mutex _instanceMutex;

        public App()
        {
            _instanceMutex = new Mutex(true, $"Global\\{AppId}", out var isCreatedNew);

            if (!isCreatedNew)
            {
                _instanceMutex = null;
                Application.Current.Shutdown(ExitCodeAlreadyLaunched);
            }

            Host = GetDefaultHost();
        }

        private async void StartupApp(object sender, StartupEventArgs args)
        {
            MainWindow = Host.GetService<MainWindow>();
            MainWindow.Show();

            await Host.StartAsync();
        }

        private async void ExitApp(object sender, ExitEventArgs args)
        {
            await Host.StopAsync(TimeSpan.FromSeconds(WaitForStopSeconds));

            _instanceMutex?.ReleaseMutex();
        }

        private IHost GetDefaultHost()
        {
            return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(new string [0])
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddCoreConfiguration();
                    config.AddJsonFile(LocalizationFile, false, false);
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    services.AddCoreServices();
                    services.AddSingleton(this);
                    services.AddSingleton<ILocalizationService, LocalizationService>();
                    services.AddSingleton<MainWindow>();
                })
                .Build();
        }
    }
}

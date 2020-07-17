using System;
using System.Threading;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mitheti.Core.Services;
using Mitheti.Wpf.Services;
using Mitheti.Wpf.ViewModels;
using Mitheti.Wpf.Views;

namespace Mitheti.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public const string AppId = "fbffa2ce-2f82-4945-84b1-9d9ba04dc90c";
        public const string LocalizationFile = "localization.json";
        public const int StopWait = 5000;

        //? see https://docs.microsoft.com/en-us/windows/win32/debug/system-error-codes--0-499- ;
        public const int ExitCodeAlreadyLaunched = 101;

        private readonly IHost _host;
        private readonly Mutex _instanceMutex;

        public App()
        {
            _instanceMutex = new Mutex(true, $"Global\\{AppId}", out var isCreatedNew);

            if (!isCreatedNew)
            {
                _instanceMutex = null;
                Application.Current.Shutdown(ExitCodeAlreadyLaunched);
            }

            _host = GetDefaultHost();
        }

        private async void OnStartup(object sender, StartupEventArgs args)
        {
            //TODO: use GetService everywhere or manually always create;
            MainWindow = _host.Services.GetService<MainWindow>();
            MainWindow.Show();

            await _host.StartAsync();
        }

        private async void OnExit(object sender, ExitEventArgs args)
        {
            using (_host)
            {
                await _host.StopAsync(TimeSpan.FromMilliseconds(StopWait));
            }

            _instanceMutex?.ReleaseMutex();
        }

        private IHost GetDefaultHost()
        {
            return Host.CreateDefaultBuilder(new string [0])
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddCoreConfiguration();
                    config.AddJsonFile(LocalizationFile, false, false);
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    services.AddCoreServices();
                    services.AddSingleton<ILocalizationService, LocalizationService>();

                    services.AddTransient<MainWindowViewModel>();
                    services.AddTransient<MainTabViewModel>();
                    services.AddTransient<StatisticTabViewModel>();
                    services.AddTransient<AboutTabViewModel>();

                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<MainTab>();
                    services.AddSingleton<StatisticTab>();
                    services.AddSingleton<AboutTab>();
                })
                .Build();
        }
    }
}

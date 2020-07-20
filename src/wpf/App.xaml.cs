using System.Threading;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

        //? see https://docs.microsoft.com/en-us/windows/win32/debug/system-error-codes--0-499- ;
        public const int ExitCodeAlreadyLaunched = 101;

        public ServiceProvider Container { get; private set; }

        private readonly Mutex _instanceMutex;

        public App()
        {
            _instanceMutex = new Mutex(true, $"Global\\{AppId}", out var isCreatedNew);

            if (!isCreatedNew)
            {
                _instanceMutex = null;
                Application.Current.Shutdown(ExitCodeAlreadyLaunched);
            }
        }

        private void OnStartup(object sender, StartupEventArgs args)
        {
            ConfigureServices();

            MainWindow = Container.GetRequiredService<MainWindow>();
            MainWindow.Show();
        }

        private void OnExit(object sender, ExitEventArgs args)
        {
            Container.Dispose();

            _instanceMutex?.ReleaseMutex();
        }

        private void ConfigureServices()
        {
            var services = new ServiceCollection();

            //TODO: add log files;
            services.AddLogging();
            services.AddCoreServices();

            var config = new ConfigurationBuilder();
            config.AddCoreConfiguration();
            config.AddJsonFile(LocalizationFile, false, false);
            services.AddSingleton<IConfiguration>(config.Build());

            services.AddSingleton<ILocalizationService, LocalizationService>();

            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<MainTabViewModel>();
            services.AddSingleton<StatisticTabViewModel>();
            services.AddSingleton<AboutTabViewModel>();

            services.AddSingleton<MainTab>();
            services.AddSingleton<StatisticTab>();
            services.AddSingleton<AboutTab>();
            services.AddSingleton<MainWindow>();

            services.AddSingleton<ITrayManagerService, TrayManagerService>();
            services.AddSingleton<App>(this);

            Container = services.BuildServiceProvider();
        }
    }
}
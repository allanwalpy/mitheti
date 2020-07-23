using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mitheti.Core.Services;
using Mitheti.Wpf.Services;
using Mitheti.Wpf.ViewModels;
using Mitheti.Wpf.Views;
using NLog.Extensions.Logging;

namespace Mitheti.Wpf
{
    //TODO: configure repository with travis/codacy or other CI or so;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public const string AppId = "fbffa2ce-2f82-4945-84b1-9d9ba04dc90c";
        public const string LocalizationFile = "localization.json";
        public const string LoggingConfigFile = "logging.json";
        public const string LoggingSectionKey = "NLog";

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

            ClearDatabase().ConfigureAwait(false);
        }

        private async Task ClearDatabase() => await Container.GetService<ISizeLimitDatabaseService>().LimitDatabase();

        private void OnExit(object sender, ExitEventArgs args)
        {
            Container.Dispose();

            _instanceMutex?.ReleaseMutex();
        }

        private void ConfigureServices()
        {
            var services = new ServiceCollection();
            var config = GetConfig();

            services.AddLogging(builder => SetLogging(builder, config));
            services.AddSingleton(config);
            services.AddCoreServices();

            //? main services;
            services.AddSingleton<ISettingsManager, SettingsManager>();
            services.AddSingleton<ILocalizationService, LocalizationService>();

            //? viewModels;
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<MainTabViewModel>();
            services.AddSingleton<StatisticTabViewModel>();
            services.AddSingleton<SettingTabViewModel>();
            services.AddSingleton<AboutTabViewModel>();

            //? views;
            services.AddSingleton<MainTab>();
            services.AddSingleton<StatisticTab>();
            services.AddSingleton<AboutTab>();
            services.AddSingleton<SettingTab>();
            services.AddSingleton<MainWindow>();

            //? views services;
            services.AddSingleton<ITrayManagerService, TrayManagerService>();
            services.AddSingleton(this);

            Container = services.BuildServiceProvider();
        }

        private static ILoggingBuilder SetLogging(ILoggingBuilder builder, IConfiguration config)
            => builder.ClearProviders()
                .SetMinimumLevel(LogLevel.Trace)
                .AddNLog(new NLogLoggingConfiguration(config.GetSection(LoggingSectionKey)));

        private static IConfiguration GetConfig()
            => new ConfigurationBuilder()
                .AddCoreConfiguration()
                .AddJsonFile(LocalizationFile, false, false)
                .AddJsonFile(LoggingConfigFile, false, false)
                .Build();
    }
}
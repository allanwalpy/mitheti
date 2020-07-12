using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mitheti.Core.Services;
using Mitheti.Wpf.Services;

namespace Mitheti.Wpf
{
    public class HostControl
    {
        public const string LocalizationFile = "localization.json";
        public const string WpfConfigFile = "config.wpf.json";
        private const int WaitForStopSeconds = 5;

        private IHost _host;

        public async Task StartAsync()
        {
            _host = GetDefaultHost();
            await _host.StartAsync();
        }

        public async Task StopAsync()
        {
            using (_host)
            {
                await _host.StopAsync(TimeSpan.FromSeconds(WaitForStopSeconds));
            }
        }

        public T GetService<T>() where T : class => _host.Services.GetService<T>();

        private static IHost GetDefaultHost()
        {
            return Host.CreateDefaultBuilder(new string [0])
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddCoreConfiguration();
                    config.AddJsonFile(LocalizationFile, false, false);
                    config.AddJsonFile(WpfConfigFile, true, false);
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    services.AddCoreServices();
                    services.AddSingleton<ILocalizationService, LocalizationService>();
                    services.AddSingleton<MainWindow>();
                })
                .Build();
        }
    }
}

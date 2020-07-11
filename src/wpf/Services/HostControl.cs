using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mitheti.Core.Services;
using Mitheti.Wpf.Localization;

namespace Mitheti.Wpf.Services
{
    public class HostControl
    {
        private const int WaitForStopSeconds = 5;
        
        private IHost? _host;
        
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

        public T? GetService<T>() where T : class
            => _host?.Services.GetService<T>();

        private IHost GetDefaultHost()
        {
            return Host.CreateDefaultBuilder(new string [0])
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddCoreConfiguration();
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    services.AddCoreServices();
                    services.AddJsonLocalization();
                    services.AddSingleton<IWatcherControlService, WatcherControlService>();
                    services.AddSingleton<MainWindow>();
                })
                .Build();
        }
    }
}
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

using Mitheti.Core.Services;

namespace Mitheti.Wpf.Services
{
    public class HostControlService : IHostControlService
    {
        private const int WaitForStopSeconds = 5;
        
        private IHost _host;

        public async Task StartAsync()
        {
            CreateAppHost();
            await _host.StopAsync();
        }

        public async Task StopAsync()
        {
            using (_host)
            {
                await _host.StopAsync(TimeSpan.FromSeconds(WaitForStopSeconds));
            }
        }

        private void CreateAppHost()
        {
            _host = Host.CreateDefaultBuilder(new string [0])
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddCoreConfiguration();
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    services.AddCoreServices();
                })
                .Build();   
        }
    }
}
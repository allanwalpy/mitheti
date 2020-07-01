using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Mitheti.Core.Extensions;
using Mitheti.Core.Database;
using Mitheti.Core.Watcher;

//TODO: add logging;

namespace Mitheti.Core
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config
                        .AddCoreConfigFiles()
                        .AddEnvironmentVariables()
                        .AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IConnectionService, ConnectionService>();
                    services.AddSingleton<ISavingService, SavingService>();
                    services.AddHostedService<WatcherService>();
                });

    }
}

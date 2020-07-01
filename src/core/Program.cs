using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Mitheti.Core.Extensions;
using Mitheti.Core.Database;
using Mitheti.Core.Watcher;

namespace Mitheti.Core
{
    public static class Program
    {
        public const string ModuleName = "core";

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddCoreConfigFiles()
                        .AddEnvironmentVariables()
                        .AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IConnectionService, ConnectionService>();
                    services.AddSingleton<ISavingService, SavingService>();
                    services.AddSingleton<WatcherService>();
                    services.AddHostedService<Worker>();
                });

    }
}

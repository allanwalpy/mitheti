using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Mitheti.Core.Database;
using Mitheti.Core.Watcher;

namespace Mitheti.Wpf
{
    public static class Extensions
    {
        public const string ConfigFile = "config.json";
        
        public static IHost CreateAppHost()
            => Host.CreateDefaultBuilder(new string [0])
                .ConfigureAppConfiguration((hotingContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile(ConfigFile, false, false);
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    services.AddSingleton<IConnectionService, ConnectionService>();
                    services.AddSingleton<ISavingService, SavingService>();
                    services.AddSingleton<IFilterApp, FilterApp>();
                    services.AddSingleton<IWatcherService, WatcherService>();
                })
                .Build();
    }
}
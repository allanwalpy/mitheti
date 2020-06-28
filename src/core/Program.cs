using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

//TODO: add logging;

namespace Mitheti.Core
{
    public static class Program
    {
        public const string ConfigFile = "setting.core.json";
        public const string DatabaseConfigFile = "setting.database.secret.json";

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
                        .AddJsonFile(ConfigFile, optional: false, reloadOnChange: false)
                        .AddJsonFile(DatabaseConfigFile, optional: false, reloadOnChange: false)
                        .AddEnvironmentVariables()
                        .AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<Database.ConnectionService>();
                    services.AddHostedService<Watcher.Worker>();
                });
    }
}

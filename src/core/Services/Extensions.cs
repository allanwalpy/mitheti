using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mitheti.Core.Services
{
    public static class Extensions
    {
        public const string ConfigFile = "config.json";

        public static Task ThrowNoExceptionOnCancelled(this Task configuredTask)
        {
            return configuredTask.ContinueWith((task) =>
            {
                if (!task.IsCanceled && task.Exception != null)
                {
                    throw task.Exception;
                }
            });
        }

        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddSingleton<ISavingService, SavingService>();
            services.AddSingleton<IFilterApp, FilterApp>();
            services.AddSingleton<IWatcherService, WatcherService>();

            return services;
        }

        public static IConfigurationBuilder AddCoreConfiguration(this IConfigurationBuilder config)
            => config.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(ConfigFile, false, false);
    }
}

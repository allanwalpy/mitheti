using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Mitheti.Core.Services
{
    public static class Extensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.TryAddSingleton<IAddToDatabaseService, AddToDatabaseService>();
            services.TryAddSingleton<ISavingService, SavingService>();
            services.TryAddSingleton<IClearDatabaseService, ClearDatabaseService>();
            services.TryAddSingleton<IWatcherService, WatcherService>();
            services.TryAddSingleton<IWatcherControlService, WatcherControlService>();

            return services;
        }

        public static IConfigurationBuilder AddCoreConfiguration(this IConfigurationBuilder config,
            bool isOptional = true)
        {
            const string configFile = "config.json";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), configFile);
            return config.AddJsonFile(filePath, isOptional, false);
        }

        internal static Task ThrowNoExceptionOnCancelled(this Task configuredTask)
        {
            return configuredTask.ContinueWith((task) =>
            {
                if (!task.IsCanceled && task.Exception != null)
                {
                    throw task.Exception;
                }
            });
        }
    }
}
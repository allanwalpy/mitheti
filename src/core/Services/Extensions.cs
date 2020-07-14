using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Mitheti.Core.Services
{
    public static class Extensions
    {
        public const string ConfigFile = "config.json";

        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.TryAddSingleton<IDatabaseService, DatabaseService>();
            services.TryAddSingleton<IAddToDatabaseService, AddToDatabaseService>();
            services.TryAddSingleton<ISavingService, SavingService>();
            services.TryAddSingleton<IClearDatabaseService, ClearDatabaseService>();
            services.TryAddSingleton<IWatcherService, WatcherService>();
            services.TryAddSingleton<IWatcherControlService, WatcherControlService>();
            services.TryAddSingleton<IStatisticDayOfWeekService, StatisticDayOfWeekService>();
            services.TryAddSingleton<IStatisticTopAppService, StatisticTopAppService>();

            return services;
        }

        public static IConfigurationBuilder AddCoreConfiguration(this IConfigurationBuilder config,
            bool isOptional = true)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), ConfigFile);
            return config.AddJsonFile(filePath, isOptional, false);
        }

        internal static Task ThrowNoExceptionOnCancelled(this Task configuredTask)
        {
            return configuredTask.ContinueWith(ThrowNoExceptionOnCancelledContinueWith);
        }

        private static void ThrowNoExceptionOnCancelledContinueWith(Task task)
        {
            if (!task.IsCanceled && task.Exception != null)
            {
                throw task.Exception;
            }

            if (task.IsCanceled)
            {
                //TODO:FIXME: is legit?;
                task.Dispose();
            }
        }
    }
}
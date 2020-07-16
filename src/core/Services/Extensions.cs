using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            services.TryAddSingleton<IAddFilterService, AddFilterService>();
            services.TryAddSingleton<ISavingService, SavingService>();
            services.TryAddSingleton<IWatcherService, WatcherService>();
            services.TryAddSingleton<IWatcherControlService, WatcherControlService>();
            services.TryAddSingleton<IStatisticDatabaseService, StatisticDatabaseService>();

            return services;
        }

        public static IConfigurationBuilder AddCoreConfiguration(this IConfigurationBuilder config,
            bool isOptional = true)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), ConfigFile);
            return config.AddJsonFile(filePath, isOptional, false);
        }

        internal static void WaitCancelled(this Task task, int timeoutMilliseconds)
            => Task.WaitAny(new[] {task}, timeoutMilliseconds);

        //? on `"someKey": []`, `GetSection(key).Get<string[]>()` returns `null`, not `string[0]`; `GetValue<T[]>` not working for array/list;
        public static List<T> GetList<T>(this IConfigurationSection config, string key, List<T> defaultValue = null)
            => config.GetSection(key)?.Get<T[]>()?.ToList() ?? defaultValue;

        public static List<T> GetList<T>(this IConfiguration config, string key, List<T> defaultValue = null)
            => config.GetSection(key)?.Get<T[]>()?.ToList() ?? defaultValue;

        public static IQueryable<AppTimeModel> WhereTimePeriod(this IQueryable<AppTimeModel> query, TimePeriod period)
            => period.Equals(TimePeriod.All) ? query : query.Where(item => period.Equals(item.Time));
    }
}

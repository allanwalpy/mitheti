using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

using Mitheti.Core.Database;

namespace Mitheti.Core.Extensions
{
    public static class Helper
    {
        public const string ConfigFile = "setting.core.json";
        public const string DatabaseConfigFile = "setting.database.secret.json";
        public const string AppListConfigFile = "setting.applist.json";
        public const string LocalizationConfigFileTemplate = "localization.{0}.json";

        public const string AppListConfigKey = "applist";

        /// <summary>
        /// Converts boolean value to int
        /// <para> 0 = false; 1 = true; </para>
        /// </summary>
        public static int ToInt(this bool value)
            => value == false ? 0 : 1;

        /// <summary>
        /// Parse <see cref="Process" /> to <see cref="AppTimeModel" /> specifying <paramref name="duration" /> millisecods apptime;
        /// </summary>
        public static AppTimeModel ToDatabaseModel(this Process info, int duration)
            => (info == null) ? null : new AppTimeModel
                {
                    AppName = info.ProcessName?.ToLower(),
                    Duration = duration,
                    Time = DateTime.Now
                };

        /// <summary>
        /// Adds default json config files for <see cref="Mitheti.Core" /> to <see cref="IConfigurationBuilder" />
        /// </summary>
        public static IConfigurationBuilder AddCoreConfigFiles(this IConfigurationBuilder config)
            => config
                .AddJsonFile(DatabaseConfigFile, optional: false, reloadOnChange: false)
                .AddJsonFile(AppListConfigFile, optional: false, reloadOnChange: false)
                .AddJsonFile(ConfigFile, optional: false, reloadOnChange: false);

        /// <summary>
        /// Attempts to bind the configuration instance of type <typeparamref name="T" />[] and converting it to List
        /// </summary>
        public static List<T> GetList<T>(this IConfiguration config, string key)
            => config.GetSection(key).Get<T[]>()?.ToList();


        /// <summary>
        /// Add time spend to app, add item with <paramref name="data" /> if nonthing recorded in <paramref name="list" />
        /// </summary>
        public static void AddAppTime(this List<AppTimeModel> list, AppTimeModel data)
        {
            var previousRecords = list.Where((item) =>
                (item.AppName == data.AppName)
                && (item.Hour == data.Hour)
                && (item.Day == data.Day)
                && (item.Month == data.Month)
                && (item.Year == data.Year));

            if (previousRecords.Any())
            {
                previousRecords.First().Duration += data.Duration;
            }
            else
            {
                list.Add(data);
            }
        }

        /// <summary>
        /// Runs <see cref="String.ToLower()" /> for every string in collections;
        /// </summary>
        public static List<string> ToLowerAll(this List<string> list)
        {
            if (list == null || !list.Any())
            {
                return list;
            }

            for (int i = 0; i < list.Count; i++)
            {
                list[i] = list[i]?.ToLower();
            }

            return list;
        }

        public static string Format(this string template, params object[] args)
            => String.Format(template, args: args);
    }
}

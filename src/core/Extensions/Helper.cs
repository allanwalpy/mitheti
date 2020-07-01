using System;
using System.Collections.Generic;
using System.IO;
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

        public const string AppListConfigKey = "applist";

        public static int ToInt(this bool value)
            => value == false ? 0 : 1;

        public static AppTimeModel ToDatabaseModel(this Process info, int duration)
            => info == null
                ? null
                : new AppTimeModel
                {
                    AppName = info.ProcessName?.ToLower(),
                    TimeSpan = duration,
                    Time = DateTime.Now
                };

        public static IConfigurationBuilder AddCoreConfigFiles(this IConfigurationBuilder config)
            => config
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(DatabaseConfigFile, optional: false, reloadOnChange: false)
                .AddJsonFile(AppListConfigFile, optional: false, reloadOnChange: false)
                .AddJsonFile(ConfigFile, optional: false, reloadOnChange: false);

        public static List<T> GetList<T>(this IConfiguration config, string key)
            => config.GetSection(key).Get<T[]>()?.ToList();
        }
}

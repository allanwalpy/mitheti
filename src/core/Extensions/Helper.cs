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
        /// Attempts to bind the configuration instance of type <typeparamref name="T" />[] and converting it to List
        /// </summary>
        public static List<T> GetList<T>(this IConfiguration config, string key)
            => config.GetSection(key).Get<T[]>()?.ToList();
    }
}

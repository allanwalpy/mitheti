using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

using Mitheti.Core.Database;

namespace Mitheti.Core.Watcher
{
    public class WatcherService : IWatcherService
    {
        public const string DelayConfigKey = "service:delay";
        public const string AppListConfigKey = "applist";

        private readonly ISavingService _database;

        private readonly int _delay;
        private readonly List<string> _appList;

        public WatcherService(IConfiguration config, ISavingService database)
        {
            _database = database;

            _delay = config.GetValue<int>(DelayConfigKey);
            _appList = GetList<string>(config, AppListConfigKey);
        }

        public List<T> GetList<T>(IConfiguration config, string key)
            => config.GetSection(key).Get<T[]>()?.ToList();

        public async Task Run(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_delay, stoppingToken);

                var processName = WinApiAdapter.GetFocusedWindowInfo()?.ProcessName;
                if (processName == null || !_appList.Contains(processName))
                {
                    continue;
                }

                _database.Add(new AppTimeModel(processName, _delay, DateTime.Now));
            }
        }
    }
}

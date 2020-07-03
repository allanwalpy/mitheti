using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Mitheti.Core.Database;
using Mitheti.Core.Extensions;

namespace Mitheti.Core.Watcher
{
    public class WatcherService : IWatcherService
    {
        public const string DelayConfigKey = "service:delay";

        private readonly ILogger<WatcherService> _logger;
        private readonly ISavingService _database;

        private readonly int _delay;
        private readonly List<string> _appList;

        public WatcherService(ILogger<WatcherService> logger, IConfiguration config, ISavingService database)
        {
            _logger = logger;
            _database = database;

            _delay = config.GetValue<int>(DelayConfigKey);
            _appList = config.GetList<string>(Helper.AppListConfigKey).ToLowerAll();
        }

        public async Task Run(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(WatcherService)} is started;");

            while (!stoppingToken.IsCancellationRequested)
            {
                var result = WinApiAdapter.GetFocusedWindowInfo().ToDatabaseModel(_delay);

                _logger.LogTrace("adding app time {0}", result?.ToString() ?? "null object");

                if (result != null
                    && _appList.Contains(result.AppName))
                {
                    _database.AddRecordedTime(result);
                }

                await Task.Delay(_delay, stoppingToken);
            }
        }
    }
}

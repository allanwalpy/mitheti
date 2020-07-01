using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Mitheti.Core.Database;
using Mitheti.Core.Extensions;

namespace Mitheti.Core.Watcher
{
    public class WatcherService : BackgroundService
    {
        public const string DelayConfigKey = "service:delay";

        private readonly ILogger _logger;
        private readonly ISavingService _database;

        private readonly int _delay;
        private readonly List<string> _appList;

        public WatcherService(ILogger logger, IConfiguration config, ISavingService database)
        {
            _database = database;

            _delay = config.GetValue<int>(DelayConfigKey);
            _appList = config.GetList<string>(Helper.AppListConfigKey);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // TODO: add localization;
            _logger.LogInformation($"{nameof(WatcherService)} is started;");

            while (!stoppingToken.IsCancellationRequested)
            {
                var result = WinApiAdapter.GetFocusedWindowInfo().ToDatabaseModel(_delay);

                // TODO: add class for filtering;
                if (_appList.Contains(result.AppName.ToLower()))
                {
                    _database.AddRecordedTime(result);
                }

                // TODO: make timer;
                await Task.Delay(_delay, stoppingToken);
            }
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Mitheti.Core.Services
{
    public class WatcherService : IWatcherService
    {
        public const string DelayConfigKey = "service:delay";
        public const int DefaultDelay = 250;

        private readonly ILogger<WatcherService> _logger;
        private readonly ISavingService _database;
        private readonly int _delay;

        public WatcherService(ILogger<WatcherService> logger, IConfiguration config, ISavingService database)
        {
            _logger = logger;
            _database = database;
            _delay = config.GetValue(DelayConfigKey, DefaultDelay);
        }

        public async Task RunAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var processName = WinApiAdapter.GetFocusedWindowInfo()?.ProcessName;
                _logger.LogTrace(processName == null ? "detected no process [null reference]" : $"detected process with name {processName}");
                if (processName != null)
                {
                    _database.Save(processName, _delay, DateTime.Now);
                }

                await Task.Delay(_delay, token);
            }
        }
    }
}

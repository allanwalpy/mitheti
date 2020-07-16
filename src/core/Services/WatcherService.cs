using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Mitheti.Core.Services
{
    public class WatcherService : IWatcherService
    {
        public const string DelayConfigKey = "service:delay";
        public const int DefaultDelay = 250;

        private readonly ISavingService _database;
        private readonly int _delay;

        public WatcherService(IConfiguration config, ISavingService database)
        {
            _database = database;
            _delay = config.GetValue(DelayConfigKey, DefaultDelay);
        }

        public async Task RunAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var processName = WinApiAdapter.GetFocusedWindowInfo()?.ProcessName;
                if (processName != null)
                {
                    _database.Save(processName, _delay, DateTime.UtcNow);
                }

                await Task.Delay(_delay, token);
            }
        }
    }
}

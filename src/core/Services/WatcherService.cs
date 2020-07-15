using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Mitheti.Core.Services
{
    public class WatcherService : IWatcherService
    {
        public const string DelayConfigKey = "service:delay";
        public const int DefaultDelay = 250;

        private readonly IAddToDatabaseService _database;
        private readonly int _delay;

        public WatcherService(IConfiguration config, IAddToDatabaseService database)
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
                    _database.Add(processName, _delay);
                }

                await Task.Delay(_delay, token);
            }
        }
    }
}

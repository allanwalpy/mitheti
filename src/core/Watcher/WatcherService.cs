using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Mitheti.Core.Watcher
{
    public class WatcherService : IWatcherService
    {
        public const string DelayConfigKey = "service:delay";

        private readonly IFilterApp _filter;
        private readonly int _delay;

        public WatcherService(IConfiguration config, IFilterApp filter)
        {
            _filter = filter;

            _delay = config.GetValue<int>(DelayConfigKey);
        }

        public async Task Run(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var processName = WinApiAdapter.GetFocusedWindowInfo()?.ProcessName;

                _filter.Add(processName, _delay);

                await Task.Delay(_delay, stoppingToken).ThrowNoExceptionOnCancelled();
            }
        }
    }
}

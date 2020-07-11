using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

using Mitheti.Core.Services;

namespace Mitheti.Worker
{
    public class Worker : BackgroundService
    {
        //readonly
        private IWatcherService _watcher;

        public Worker(IWatcherService watcher)
        {
            _watcher = watcher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _watcher.Run(stoppingToken);
        }
    }
}

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Mitheti.Core.Services;

namespace Mitheti.Worker
{
    public class Worker : BackgroundService
    {
        private readonly IWatcherService _watcher;

        public Worker(IWatcherService watcher)
        {
            _watcher = watcher;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
            => _watcher.RunAsync(stoppingToken);
    }
}
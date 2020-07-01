using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

using Mitheti.Core.Extensions;
using Mitheti.Core.Watcher;

namespace Mitheti.Web.Services
{
    public class LauncherService : ILauncherService
    {
        private readonly object _lock = new object();
        private IWatcherService _watcher;
        private Task _watcherTask;
        private CancellationTokenSource _stoppingTokenSource;

        public bool IsRunning { get; private set; }
        public bool IsProcessing { get; private set; }

        public LauncherServiceState State
            => (LauncherServiceState)(this.IsRunning.ToInt() * 1 + this.IsProcessing.ToInt() * 2);

        public LauncherService(IConfiguration config, IWatcherService watcher)
        {
            _stoppingTokenSource = new CancellationTokenSource();
            _watcher = watcher;
            _watcherTask = null;

            this.IsRunning = false;
            this.IsProcessing = false;
        }

        public void StartAsync()
        {
            lock (_lock)
            {
                this.IsProcessing = true;

                Task.Run(() =>
                {
                    _watcherTask = _watcher.Run(_stoppingTokenSource.Token);

                    this.IsRunning = true;
                    this.IsProcessing = false;
                });
            }
        }

        public void StopAsync()
        {
            lock (_lock)
            {
                this.IsProcessing = true;

                Task.Run(async () =>
                {
                    _stoppingTokenSource.Cancel();
                    await Task.Run(() =>
                    {
                        while (!_watcherTask.IsCompleted)
                        {
                            Thread.Sleep(100);
                        }
                    });

                    this.IsRunning = false;
                    this.IsProcessing = false;
                });
            }
        }
    }
}

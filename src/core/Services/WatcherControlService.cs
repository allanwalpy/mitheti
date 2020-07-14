using System.Threading;
using System.Threading.Tasks;

namespace Mitheti.Core.Services
{
    public class WatcherControlService : IWatcherControlService
    {
        public const int StopWait = 500;

        private readonly IWatcherService _watcher;
        private CancellationTokenSource _tokenSource;
        private Task _watcherTask;

        public event StatusChangedHandler StatusChanged;

        public bool IsLaunched { get; private set; }

        public WatcherControlService(IWatcherService watcher)
        {
            _watcher = watcher;
            _tokenSource = null;
            IsLaunched = false;

            UpdateStatus();
        }

        public async Task StartAsync()
        {
            if (IsLaunched)
            {
                return;
            }

            IsLaunched = true;

            _tokenSource = new CancellationTokenSource();
            _watcherTask = _watcher.RunAsync(_tokenSource.Token);

            UpdateStatus();
        }

        public async Task StopAsync()
        {
            if (!IsLaunched)
            {
                return;
            }

            IsLaunched = false;

            _tokenSource.Cancel();
            Task.WhenAll(_watcherTask).Wait(StopWait);

            _tokenSource.Dispose();
            _tokenSource = null;
            _watcherTask = null;
            UpdateStatus();
        }

        private void UpdateStatus() => StatusChanged?.Invoke(this, new WatcherStatusEventArgs(IsLaunched));
    }
}
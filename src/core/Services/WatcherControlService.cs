using System.Threading;
using System.Threading.Tasks;

namespace Mitheti.Core.Services
{
    public class WatcherControlService : IWatcherControlService
    {
        private readonly IWatcherService _watcher;
        private CancellationTokenSource _tokenSource;
        private Task _watcherTask;

        public bool IsLaunched { get; private set; }

        public event StatusChangedHandler StatusChanged;

        public WatcherControlService(IWatcherService watcher)
        {
            _watcher = watcher;
            _tokenSource = null;

            IsLaunched = false;
            UpdateStatus();
        }

        public void Start()
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
            await Task.WhenAny(_watcherTask);

            _tokenSource.Dispose();
            _tokenSource = null;
            _watcherTask = null;
            UpdateStatus();
        }

        private void UpdateStatus() => StatusChanged?.Invoke(this, new WatcherStatusEventArgs(IsLaunched));
    }
}

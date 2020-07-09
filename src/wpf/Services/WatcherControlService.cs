using System.Threading;
using System.Threading.Tasks;

using Mitheti.Core.Services;

namespace Mitheti.Wpf.Services
{
    public class WatcherControlService : IWatcherControlService
    {
        private readonly IWatcherService _watcher;
        private CancellationTokenSource? _tokenSource;
        private Task? _watcherTask;

        public bool IsLaunched { get; private set; }
        
        public event StatusChangedHandler StatusChanged;

        public WatcherControlService(IWatcherService watcher)
        {
            _watcher = watcher;
            _tokenSource = null;

            UpdateStatus(false);
        }

        public void Start()
        {
            if (IsLaunched)
            {
                return;
            }

            _tokenSource = new CancellationTokenSource();
            _watcherTask = _watcher.Run(_tokenSource.Token);

            UpdateStatus(true);
        }
        
        public async Task StopAsync()
        {
            if (!IsLaunched)
            {
                return;
            }

            _tokenSource.Cancel();
            await Task.WhenAny(_watcherTask);

            _tokenSource = null;
            _watcherTask = null;
            UpdateStatus(false);
        }

        private void UpdateStatus(bool isLaunched)
        {
            this.IsLaunched = isLaunched;
            StatusChanged?.Invoke(this, new WatcherStatusEventArgs(IsLaunched));
        }
    }
}
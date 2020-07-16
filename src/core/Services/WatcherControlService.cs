using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mitheti.Core.Services
{
    public class WatcherControlService : IWatcherControlService, IDisposable
    {
        public const int StopWait = 500;

        private readonly IWatcherService _watcher;
        private CancellationTokenSource _tokenSource;
        private Task _watcherTask;

        public event WatcherStatusChangedHandler WatcherStatusChanged;

        public bool IsLaunched { get; private set; }

        public WatcherControlService(IWatcherService watcher)
        {
            _watcher = watcher;
            _tokenSource = null;

            //? initial state, before `Start()` or `Stop()` called;
            IsLaunched = false;
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

        public void Stop()
        {
            if (!IsLaunched)
            {
                return;
            }

            IsLaunched = false;

            _tokenSource.Cancel();
            _watcherTask.WaitCancelled(StopWait);

            _tokenSource.Dispose();
            _tokenSource = null;
            _watcherTask = null;
            UpdateStatus();
        }

        private void UpdateStatus() => WatcherStatusChanged?.Invoke(this, new WatcherStatusEventArgs(IsLaunched));

        public void Dispose() => Stop();
    }
}

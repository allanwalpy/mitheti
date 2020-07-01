using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

using Mitheti.Core.Extensions;
using Mitheti.Core.Service;
using Mitheti.Core.Watcher;

namespace Mitheti.Web.Service
{
    // TODO: more elegant;
    // TODO: add blocking!!!;
    public class LauncherService : ILauncherService
    {
        private WatcherService _worker;
        public bool IsRunning { get; private set; }
        public bool IsProcessing { get; private set; }
        public LauncherServiceState State
            => (LauncherServiceState)(this.IsRunning.ToInt() * 1 + this.IsProcessing.ToInt() * 2);

        public LauncherService(IConfiguration config, ISavingService database)
        {
            _worker = new WatcherService(config, database);

            this.IsRunning = false;
            this.IsProcessing = false;
        }

        public Task StartAsync()
        {
            if (IsProcessing)
            {
                return null;
            }

            this.IsProcessing = true;

            return Task.Run(async () =>
            {
                await _worker.StartAsync();
                this.IsRunning = true;
                this.IsProcessing = false;
            });


        }

        public Task StopAsync()
        {
            if (this.IsProcessing)
            {
                return null;
            }

            this.IsProcessing = true;

            return Task.Run(async () =>
            {
                await _coreHost.StopAsync();
                this.IsRunning = false;
                this.IsProcessing = false;
            });
        }
    }
}

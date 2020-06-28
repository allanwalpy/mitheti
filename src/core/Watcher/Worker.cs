using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Mitheti.Core.Database;

namespace Mitheti.Core.Watcher
{
    public class Worker : BackgroundService
    {
        public const string DelayConfigKey = "service:delay";

        private readonly IConfiguration _config;
        private readonly ISavingService _database;

        private readonly int _delay;

        public Worker(IConfiguration config, ISavingService database)
        {
            this._config = config;
            this._database = database;

            this._delay = this._config.GetValue<int>(DelayConfigKey);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = WinApiAdapter.GetFocusedWindowInfo();

                this._database.AddRecordedTime(result);

                await Task.Delay(this._delay, stoppingToken);
            }
        }

        public override void Dispose()
        {
            //TODO: implement flushing changes or smth; i dunno;
        }
    }
}

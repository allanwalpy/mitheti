using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Mitheti.Core.Database;
using Mitheti.Core.Extensions;

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
            _config = config;
            _database = database;

            _delay = _config.GetValue<int>(DelayConfigKey);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = WinApiAdapter.GetFocusedWindowInfo().ToDatabaseModel(_delay);

                _database.AddRecordedTime(result);

                await Task.Delay(_delay, stoppingToken);
            }
        }

        public override void Dispose()
        {
            //TODO: implement flushing changes or smth; i dunno;
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Mitheti.Core.Watcher
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _config;

        private readonly int _delay;

        public Worker(ILogger<Worker> logger, IConfiguration config)
        {
            this._logger = logger;
            this._config = config;

            this._delay = this._config.GetValue<int>("service:delay");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = WinApiAdapter.GetFocusedWindowInfo();
                _logger.LogInformation(this._config["log:message:iteration"], DateTimeOffset.Now, result);
                await Task.Delay(this._delay, stoppingToken);
            }
        }

        public override void Dispose()
        {
            //TODO: implement flushing changes or smth; i dunno;
        }
    }
}

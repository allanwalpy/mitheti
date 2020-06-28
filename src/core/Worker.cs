using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Mitheti.Core
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _config;

        public Worker(ILogger<Worker> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //TODO:replace with actuall worker stuff you know;
                _logger.LogInformation($"{_config["Log:Message:Iteration"]}: {DateTimeOffset.Now}");
                await Task.Delay(1000, stoppingToken);
            }
        }

        public override void Dispose()
        {
            //TODO:implement flushing changes or smth; i dunno;
        }
    }
}

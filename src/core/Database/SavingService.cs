using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Mitheti.Core.Extensions;

namespace Mitheti.Core.Database
{
    public class SavingService : ISavingService, IDisposable
    {
        public const string RecordDelayConfigKey = "database:delay";

        private readonly ILogger<SavingService> _logger;
        private IConnectionService _databaseService;
        private CancellationTokenSource _stopFlashingToken;

        private readonly object _lock = new object();
        private List<AppTimeModel> _accumulated = new List<AppTimeModel>();
        private Task _flashingTask;

        public SavingService(ILogger<SavingService> logger, IConfiguration config, IConnectionService databaseService)
        {
            _logger = logger;
            _databaseService = databaseService;

            var delay = TimeSpan.FromMinutes(config.GetValue<int>(RecordDelayConfigKey)).Milliseconds;
            _stopFlashingToken = new CancellationTokenSource();

            _flashingTask = Task.Run(() => this.FlashingTask(_stopFlashingToken.Token, delay));

            _logger.LogDebug($"setted task for {nameof(SavingService)} with delay of {delay} milliseconds");
        }

        public void AddRecordedTime(AppTimeModel data)
        {
            lock (_lock)
            {
                this._accumulated.AddAppTime(data);
            }
        }

        private async void FlashingTask(CancellationToken stoppingToken, int delay)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                this.FlashingToDatabase(null);
                await Task.Delay(delay, stoppingToken);
            }
        }

        private void FlashingToDatabase(object state)
        {
            lock (_lock)
            {
                _logger.LogTrace($"flashing accumulated etries of {_accumulated.Count} items to database;");

                if (!_accumulated.Any())
                {
                    return;
                }

                this.SaveRecordsToContext();

                _accumulated.Clear();
            }
        }

        private void SaveRecordsToContext()
        {
            using (var context = _databaseService.Context)
            {
                context.AddRange(_accumulated);
                context.SaveChanges();
            }
        }

        public void Dispose()
        {
            _logger.LogDebug($"disposing of {nameof(SavingService)}");

            //? stop flashing to database task;
            _stopFlashingToken.Cancel();
            _flashingTask.Wait();
            _flashingTask.Dispose();

            //? flash to database that is left;
            this.FlashingToDatabase(null);
        }
    }
}

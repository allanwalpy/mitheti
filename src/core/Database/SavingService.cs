using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Mitheti.Core.Database
{
    public class SavingService : ISavingService, IDisposable
    {
        public const string RecordDelayConfigKey = "database:delay";
        public const int MillisecondsInMinute = 1000 * 60;

        private readonly ILogger<SavingService> _logger;
        private IConnectionService _databaseService;
        private CancellationTokenSource _stopSavingToken;

        private readonly object _lock = new object();
        private List<AppTimeModel> _records = new List<AppTimeModel>();
        private Task _savingTask;

        public SavingService(ILogger<SavingService> logger, IConfiguration config, IConnectionService databaseService)
        {
            _logger = logger;
            _databaseService = databaseService;

            var delay = config.GetValue<int>(RecordDelayConfigKey) * MillisecondsInMinute;
            _stopSavingToken = new CancellationTokenSource();

            _savingTask = Task.Run(() => this.SavingTask(_stopSavingToken.Token, delay));

            _logger.LogDebug($"setted task for {nameof(SavingService)} with delay of {delay} milliseconds");
        }

        public void Add(AppTimeModel data)
        {
            lock (_lock)
            {
                var sameRecords = _records.Where(data.IsSameTimeSpan);

                if (sameRecords.Any())
                {
                    sameRecords.First().Duration += data.Duration;
                    return;
                }
                _records.Add(data);
            }
        }

        private async void SavingTask(CancellationToken stoppingToken, int delay)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                this.SaveToDatabase();
                await Task.Delay(delay, stoppingToken);
            }
        }

        private void SaveToDatabase()
        {
            lock (_lock)
            {
                _logger.LogTrace($"flashing accumulated etries of {_records.Count} items to database;");

                if (!_records.Any())
                {
                    return;
                }

                this.SaveToDatabaseContext();
            }
        }

        private void SaveToDatabaseContext()
        {
            using (var context = _databaseService.Context)
            {
                context.AddRange(_records);
                context.SaveChanges();
            }

            _records.Clear();
        }

        public async void Dispose()
        {
            _logger.LogDebug($"disposing of {nameof(SavingService)}");

            //? stop flashing to database task;
            _stopSavingToken.Cancel();
            await _savingTask;
            _savingTask.Dispose();
            _stopSavingToken.Dispose();

            //? flash to database leftovers;
            this.SaveToDatabase();
        }
    }
}

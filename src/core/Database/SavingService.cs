using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Mitheti.Core.Database
{
    public class SavingService : ISavingService, IDisposable
    {
        public const string RecordDelayConfigKey = "database:delay";
        public const int MillisecondsInMinute = 1000 * 60;

        private IConnectionService _databaseService;
        private CancellationTokenSource _stopSavingToken;

        private readonly object _lock = new object();
        private List<AppTimeModel> _records = new List<AppTimeModel>();
        private Task _savingTask;

        public SavingService(IConfiguration config, IConnectionService databaseService)
        {
            _databaseService = databaseService;

            var delayMinutes = config.GetValue<int>(RecordDelayConfigKey);
            _stopSavingToken = new CancellationTokenSource();
            _savingTask = Task.Run(() => this.SavingTask(_stopSavingToken.Token, delayMinutes * MillisecondsInMinute));
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

        private async Task SavingTask(CancellationToken stoppingToken, int delay)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(delay, stoppingToken).ContinueWith(task => { });

                this.SaveToDatabase();
            }
        }

        private void SaveToDatabase()
        {
            lock (_lock)
            {
                if (_records.Any())
                {
                    this.SaveToDatabaseContext();
                }
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
            //? stop saving to database task;
            _stopSavingToken.Cancel();
            await Task.WhenAll(_savingTask);
            _stopSavingToken.Dispose();

            //? save leftovers;
            this.SaveToDatabase();
        }
    }
}

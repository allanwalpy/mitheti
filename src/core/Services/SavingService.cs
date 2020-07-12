using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Mitheti.Core.Services
{
    public class SavingService : ISavingService, IDisposable
    {
        public const string RecordDelayConfigKey = "database:delay";
        public const int RecordDelayDefault = 1;
        public const int MillisecondsInMinute = 60 * 1000;

        private readonly object _lock = new object();

        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private readonly List<AppTimeModel> _records = new List<AppTimeModel>();
        private readonly Task _savingTask;

        public SavingService(IConfiguration config)
        {
            var delayMinutes = config.GetValue(RecordDelayConfigKey, RecordDelayDefault);

            _savingTask = SavingTask(_tokenSource.Token, delayMinutes * MillisecondsInMinute);
            _savingTask.ConfigureAwait(false);
            _savingTask.Start();
        }

        public void Add(AppTimeModel data)
        {
            lock (_lock)
            {
                var sameRecord = _records.Where(data.IsSameTimeSpan).FirstOrDefault();

                if (sameRecord == null)
                {
                    _records.Add(data);
                }
                else
                {
                    sameRecord.Duration += data.Duration;
                }

            }
        }

        private async Task SavingTask(CancellationToken stoppingToken, int delay)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(delay, stoppingToken).ThrowNoExceptionOnCancelled();

                SaveToDatabase();
            }
        }

        private void SaveToDatabase()
        {
            lock (_lock)
            {
                if (_records.Count == 0)
                {
                    return;
                }

                using var context = new DatabaseContext();
                context.AddRange(_records);
                context.SaveChanges();
                
                _records.Clear();
            }
        }

        public void Dispose()
        {
            //? stop saving to database task;
            _tokenSource.Cancel();
            Task.WhenAll(_savingTask).Wait();
            _tokenSource.Dispose();

            //? save leftovers;
            SaveToDatabase();
        }
    }
}
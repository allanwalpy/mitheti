using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

using Mitheti.Core.Database;

namespace Mitheti.Core.Services
{
    public class SavingService : ISavingService, IDisposable
    {
        public const string RecordDelayConfigKey = "database:delay";
        public const int MillisecondsInMinute = 1000 * 60;

        private readonly object _lock = new object();
        
        private CancellationTokenSource _stopSavingToken;
        private List<AppTimeModel> _records;
        private Task _savingTask;

        public SavingService(IConfiguration config)
        {
            _records = new List<AppTimeModel>();
            
            var delayMinutes = config.GetValue<int>(RecordDelayConfigKey);
            _stopSavingToken = new CancellationTokenSource();
            _savingTask = Task.Run(() => SavingTask(_stopSavingToken.Token, delayMinutes * MillisecondsInMinute));
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
                
                using (var context = new DatabaseContext())
                {
                    context.AddRange(_records);
                    context.SaveChanges();
                }

                _records.Clear();
            }
        }

        public void Dispose()
        {
            //? stop saving to database task;
            _stopSavingToken.Cancel();
            Task.WhenAll(_savingTask).Wait();
            _stopSavingToken.Dispose();

            //? save leftovers;
            SaveToDatabase();
        }
    }
}

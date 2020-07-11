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
        public const int MillisecondsInMinute = 1000 * 60;

        private readonly object _lock = new object();

        //эти три поля можно сделать реад онли
        private CancellationTokenSource _stopSavingToken;
        private List<AppTimeModel> _records;
        private Task _savingTask;

        public SavingService(IConfiguration config)
        {
            //лучше инитить при обьявлении такое
            _records = new List<AppTimeModel>();

            //в конфиге этого хначения может не быть, тогда вернется default(int), те 0
            var delayMinutes = config.GetValue<int>(RecordDelayConfigKey);
            _stopSavingToken = new CancellationTokenSource();

            //это можно сделать без лямбд
            // _savingTask = SavingTask(_stopSavingToken.Token, delayMinutes * MillisecondsInMinute);
            // _savingTask.ConfigureAwait(false);
            // _savingTask.Start();
            _savingTask = Task.Run(() => SavingTask(_stopSavingToken.Token, delayMinutes * MillisecondsInMinute));
        }

        public void Add(AppTimeModel data)
        {
            //можно лочить сам _records, область лока лучше уменьшать
            lock (_lock)
            {
                //lock тут
                var sameRecord = _records.Where(data.IsSameTimeSpan).FirstOrDefault();

                if (sameRecord == null)
                {
                    //lock тут
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
                //лок тут
                if (_records.Count == 0)
                {
                    return;
                }

                using (var context = new DatabaseContext())
                {
                    //лок тут
                    context.AddRange(_records);

                    //сохранение может происходить догло поэтому странно на это время лочить
                    context.SaveChanges();
                }
                //лок тут
                _records.Clear();
            }
        }

        public void Dispose()
        {
            //? stop saving to database task;
            _stopSavingToken.Cancel();

            //тут создается лишняя такска, почему тогда не _savingTask.GetAwaiter().GetResult();
            Task.WhenAll(_savingTask).Wait();
            _stopSavingToken.Dispose();

            //? save leftovers;
            SaveToDatabase();
        }
    }
}

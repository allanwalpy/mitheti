using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Mitheti.Core.Services
{
    public class SavingService : ISavingService, IDisposable
    {
        public const string DelayConfigKey = "database:delay";
        public const int DefaultDelay = 1;
        public const int MinDelay = 1;
        public const int MaxDelay = 2 *60; //? 2 hours;
        public const int MillisecondsInMinute = 60 * 1000;
        public const int StopWait = 500;

        private readonly IDatabaseService _database;
        private readonly IAddFilterService _filter;

        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private readonly List<AppTimeModel> _records = new List<AppTimeModel>();
        private readonly Task _savingTask;

        public SavingService(IConfiguration config, IAddFilterService filter, IDatabaseService database)
        {
            _database = database;
            _filter = filter;

            var delayMinutes = config.GetValue(DelayConfigKey, DefaultDelay).LimitTo(MinDelay, MaxDelay);
            _savingTask = SavingTask(_tokenSource.Token, delayMinutes * MillisecondsInMinute);
        }

        public void Save(string appName, int duration, DateTime timestamp)
        {
            if (!_filter.HavePassed(appName))
            {
                return;
            }

            lock (_records)
            {
                var sameRecord = _records.Find(
                    item => item.Equals(appName) && item.Equals(timestamp));

                if (sameRecord == null)
                {
                    _records.Add(new AppTimeModel { AppName = appName, Duration = duration, Time = timestamp});
                }
                else
                {
                    sameRecord.Duration += duration;
                }
            }
        }

        private async Task SavingTask(CancellationToken stoppingToken, int delay)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                SaveToDatabase();

                await Task.Delay(delay, stoppingToken);
            }
        }

        private void SaveToDatabase()
        {
            lock (_records)
            {
                if (_records.Count == 0)
                {
                    return;
                }

                _database.AddAsync(_records).Wait();
                _records.Clear();
            }
        }

        public void Dispose()
        {
            _tokenSource.Cancel();
            _savingTask.WaitCancelled(StopWait);
            _tokenSource.Dispose();

            //? save leftovers;
            SaveToDatabase();
        }
    }
}
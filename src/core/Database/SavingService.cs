using System;
using System.Linq;
using System.Threading;
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
        private Timer _timer;

        private readonly object _lock = new object();
        private List<AppTimeModel> _accumulated = new List<AppTimeModel>();

        public SavingService(ILogger<SavingService> logger, IConfiguration config, IConnectionService databaseService)
        {
            _logger = logger;
            _databaseService = databaseService;

            var delay = TimeSpan.FromMinutes(config.GetValue<int>(RecordDelayConfigKey)).Milliseconds;
            _logger.LogDebug($"setting timer for {nameof(SavingService)} with delay of {delay} milliseconds");
            _timer = new Timer(this.FlashingToDatabase, null, 0, delay );
        }

        public void AddRecordedTime(AppTimeModel data)
        {
            lock (_lock)
            {
                this._accumulated.AddAppTime(data);
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

            _timer.Dispose();
            this.FlashingToDatabase(null);
        }
    }
}

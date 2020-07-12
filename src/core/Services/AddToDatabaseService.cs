using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Mitheti.Core.Services
{
    public class AddToDatabaseService : IAddToDatabaseService
    {
        public const string AppListConfigKey = "applist";

        private readonly ILogger<AddToDatabaseService> _logger;
        private readonly ISavingService _database;
        private readonly List<string> _appList;

        public AddToDatabaseService(IConfiguration config, ILogger<AddToDatabaseService> logger, ISavingService database)
        {
            _logger = logger;
            _database = database;
            
            _appList = config.GetSection(AppListConfigKey)?.Get<string[]>()?.ToList();
        }

        public void Add(string app, int delay)
        {
            if (_appList.Count == 0 || _appList.Contains(app))
            {
                var info = new AppTimeModel
                {
                    AppName = app,
                    Duration = delay,
                    Time = DateTime.Now
                };
                
                _logger.LogTrace($"adding to database: {info}");
                
                _database.Add(info);
            }
        }
    }
}
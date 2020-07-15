using System;
using System.Collections.Generic;
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

        public AddToDatabaseService(IConfiguration config, ILogger<AddToDatabaseService> logger,
            ISavingService database)
        {
            _logger = logger;
            _database = database;

            _appList = config.GetList(AppListConfigKey, new List<string>());
        }

        public void Add(string app, int delay)
        {
            // меня не покидает чувство что это какое-то странное условие
            // выглядит как бизнесс логика не отделена от функционала
            // думаю это проверку нужно делать выше Add
            if (_appList.Count != 0 && !_appList.Contains(app))
            {
                _logger.LogTrace($"not adding to database: {app} with {delay}ms");
                return;
            }

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

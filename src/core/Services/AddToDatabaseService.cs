using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Mitheti.Core.Services
{
    public class AddToDatabaseService : IAddToDatabaseService
    {
        public const string AppListConfigKey = "applist";

        private readonly ISavingService _database;
        private readonly List<string> _appList;

        public AddToDatabaseService(IConfiguration config, ISavingService database)
        {
            _database = database;

            _appList = config.GetSection(AppListConfigKey)?.Get<string[]>()?.ToList();
        }

        public void Add(string app, int delay)
        {
            if (_appList.Count == 0 || _appList.Contains(app))
            {
                _database.Add(new AppTimeModel
                {
                    AppName = app,
                    Duration = delay,
                    Time = DateTime.Now
                });
            }
        }
    }
}
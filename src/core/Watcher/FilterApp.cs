using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

using Mitheti.Core.Database;

namespace Mitheti.Core.Watcher
{
    public class FilterApp : IFilterApp
    {
        public const string AppListConfigKey = "applist";

        private readonly ISavingService _database;
        private readonly List<string> _appList;

        public FilterApp(IConfiguration config, ISavingService database)
        {
            _database = database;

            _appList = GetList<string>(config, AppListConfigKey);
        }

        private List<T> GetList<T>(IConfiguration config, string key)
            => config.GetSection(key).Get<T[]>()?.ToList();

        private bool IsAddToDatabase(string app)
        {
            return (app != null)                //? skip null strings;
                || (_appList.Count == 0)        //? add all processes if no apps specified in list;
                || (_appList.Contains(app));    //? add app if it's in app list;
        }

        public void Add(string app, int delay)
        {
            bool IsAdd = this.IsAddToDatabase(app);
            if (!IsAdd)
            {
                return;
            }

            _database.Add(new AppTimeModel(app, delay, DateTime.Now));
        }
    }
}

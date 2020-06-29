using System;
using Microsoft.Extensions.Configuration;

using Mitheti.Core.Watcher;

namespace Mitheti.Core.Database
{
    //TODO: flash changes every n seconds;
    //TODO: add IDosposable;
    //TODO: add class or method with optimization of DB by same Hour, Day, Year, ProcessName;
    public class SavingService : ISavingService
    {
        private ConnectionService _databaseService;
        private int _watcherDelay;

        public SavingService(IConfiguration config, ConnectionService databaseService)
        {
            _databaseService = databaseService;
            _watcherDelay = config.GetValue<int>(Worker.DelayConfigKey);
        }

        //TODO: refactor;
        public void AddRecordedTime(ProcessInfo info)
        {
            if (info == null)
            {
                return;
            }

            //TODO: add methods to parse between AppTimeSpanModel & ProcessInfo;
            _databaseService.Context.AppTimeSpans.Add(
                new AppTimeSpanModel
                {
                    AppName = info.Name,
                    TimeSpan = _watcherDelay,
                    Time = DateTime.Now
                }
            );

            //TODO: do less often;
            _databaseService.Context.SaveChanges();
        }
    }
}

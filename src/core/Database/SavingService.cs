using System;
using Microsoft.Extensions.Configuration;

using Mitheti.Core.Watcher;

namespace Mitheti.Core.Database
{
    //TODO: flash changes every n seconds;
    //TODO: add IDosposable;
    public class SavingService : ISavingService
    {
        private ConnectionService _databaseService;
        private int _watcherDelay;

        public SavingService(IConfiguration config, ConnectionService databaseService)
        {
            this._databaseService = databaseService;
            this._watcherDelay = config.GetValue<int>(Worker.DelayConfigKey);
        }

        //TODO: refactor;
        public void AddRecordedTime(ProcessInfo info)
        {
            if (info == null)
            {
                return;
            }

            //TODO: add methods to parse between AppTimeSpanModel & ProcessInfo;
            this._databaseService.Context.AppTimeSpans.Add(
                new AppTimeSpanModel
                {
                    AppName = info.Name,
                    TimeSpan = _watcherDelay,
                    Time = DateTime.Now
                }
            );

            //TODO: do less often;
            this._databaseService.Context.SaveChanges();
        }
    }
}

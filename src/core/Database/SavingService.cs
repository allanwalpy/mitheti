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

        public SavingService(ConnectionService databaseService)
        {
            _databaseService = databaseService;
        }

        //TODO: refactor;
        public void AddRecordedTime(AppTimeSpanModel data)
        {
            if (data == null)
            {
                return;
            }

            //TODO: accumulate;
            _databaseService.Context.AppTimeSpans.Add(data);

            //TODO: do less often;
            _databaseService.Context.SaveChanges();
        }
    }
}

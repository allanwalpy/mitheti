using System.Linq;

namespace Mitheti.Core.Database
{
    //TODO: flash changes every n seconds;
    //TODO: add IDosposable;
    //TODO: add class or method with optimization of DB by same Hour, Day, Year, ProcessName;
    //TODO: add whitelist for appnames;
    public class SavingService : ISavingService
    {
        private IConnectionService _databaseService;

        public SavingService(IConnectionService databaseService)
        {
            _databaseService = databaseService;
        }

        //TODO: refactor;
        public void AddRecordedTime(AppTimeModel data)
        {
            if (data == null)
            {
                return;
            }

            this.SaveRecordToContext(_databaseService.Context, data);
        }

        private void SaveRecordToContext(DatabaseContext context, AppTimeModel data)
        {
            //TODO: accumulate;
            context.Add(data);

            //TODO: do less often;
            context.SaveChanges();
        }

        private AppTimeModel GetExactAppTime(DatabaseContext context, AppTimeModel data)
            => context.AppTimes
                .Where((item) =>
                    (item.Hour == data.Hour)
                    && (item.Day == data.Day)
                    && (item.Month == data.Month)
                    && (item.AppName == data.AppName)
                    && (item.Year == data.Year))
                    .First();
    }
}

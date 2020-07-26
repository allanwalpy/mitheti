using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Mitheti.Core.Services
{
    public class SizeLimitDatabaseService : ISizeLimitDatabaseService
    {
        public const string ConfigKey = "database:maxSize";
        public const int DefaultSize = 1;
        public const int MinSize = 1;
        public const int MaxSize = 128;
        public const string DatabaseFilename = DatabaseContext.DatabaseFilename;
        public const int StartWithLastYears = 2;
        public const int DaysStep = 7 * 14;

        private readonly IDatabaseService _database;
        private readonly int _maxSizeMb;

        public SizeLimitDatabaseService(IConfiguration config, IDatabaseService database)
        {
            _database = database;
            _maxSizeMb = config.GetValue(ConfigKey, DefaultSize).LimitTo(MinSize, MaxSize);
        }

        public async Task LimitDatabase()
        {
            var size = GetSize();
            if (size < _maxSizeMb)
            {
                return;
            }

            await ClearUntilSizeIs(_maxSizeMb);
        }

        public long GetSize() => new FileInfo(DatabaseFilename).Length;

        private long GetSizeMb() => GetSize() / (1024 * 1024);

        //TODO: check if works;
        private async Task ClearUntilSizeIs(int sizeMb)
        {
            var timePeriod = new TimePeriod
            {
                End = new DateTime(DateTime.Now.Year - StartWithLastYears)
            };

            while ((sizeMb < GetSizeMb()) && !timePeriod.Equals(DateTime.Now))
            {
                await _database.ClearAsync(timePeriod);
                timePeriod.Begin = timePeriod.Begin.Add(TimeSpan.FromDays(DaysStep));
            }
        }
    }
}
using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Mitheti.Core.Services
{
    public class SizeLimitDatabaseService : ISizeLimitDatabaseService
    {
        public const string ConfigKey = "database:maxSizeMb";
        public const int DefaultSize = 1;
        public const int MinSize = 1;
        public const int MaxSize = 128;
        public const string DatabaseFilename = DatabaseContext.DatabaseFilename;
        public const long BytesInMb = 1024 * 1024;
        public const int StartWithLastYears = 2;
        public const int DaysStep = 7 * 14;

        private readonly IDatabaseService _database;
        private readonly int _maxSizeMb;

        public SizeLimitDatabaseService(IConfiguration config, IDatabaseService database)
        {
            _database = database;
            _maxSizeMb = config.GetValue(ConfigKey, DefaultSize).LimitTo(MinSize, MaxSize);
        }

        public void LimitDatabase()
        {
            var size = GetSizeMb();
            if (size < _maxSizeMb)
            {
                return;
            }

            ClearUntilSizeIs(_maxSizeMb);
        }

        public long GetSizeMb() => new FileInfo(DatabaseFilename).Length / BytesInMb;

        //TODO: check if works;
        private void ClearUntilSizeIs(int sizeMb)
        {
            var timePeriod = new TimePeriod
            {
                End = new DateTime(DateTime.Now.Year - StartWithLastYears)
            };

            while ((sizeMb < GetSizeMb()) && !timePeriod.Equals(DateTime.Now))
            {
                _database.ClearAsync(timePeriod).Wait();
                timePeriod.Begin = timePeriod.Begin.Add(TimeSpan.FromDays(DaysStep));
            }
        }
    }
}
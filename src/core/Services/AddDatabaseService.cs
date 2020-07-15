using System;

namespace Mitheti.Core.Services
{
    public class AddDatabaseService : IAddDatabaseService
    {
        private readonly IAddFilterService _addFilter;
        private readonly ISavingService _saving;

        public AddDatabaseService(IAddFilterService addFilter, ISavingService saving)
        {
            _addFilter = addFilter;
            _saving = saving;
        }

        public void Add(string app, int duration)
        {
            if (_addFilter.Pass(app))
            {
                _saving.Add(new AppTimeModel
                {
                    AppName = app,
                    Duration = duration,
                    Time = DateTime.UtcNow
                });
            }
        }
    }
}
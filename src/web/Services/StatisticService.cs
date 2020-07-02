using System.Collections.Generic;

using Mitheti.Core.Database;
using Mitheti.Web.Models;
using Mitheti.Web.Extensions;

namespace Mitheti.Web.Services
{
    public class StatisticService : IStatisticService
    {
        public IConnectionService _connectionService;

        public StatisticService(IConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        public List<AppTimeViewModel> GetAppTimes()
        {
            using (var context = _connectionService.Context)
            {
                return Normalize(context.AppTimes).ItemsToView();
            }
        }

        private List<AppTimeModel> Normalize(IEnumerable<AppTimeModel> database)
        {
            List<AppTimeModel> result = new List<AppTimeModel>();
            foreach(var item in database)
            {
                AddAppTime(result, item);
            }
            return result;
        }

        private void AddAppTime(List<AppTimeModel> list, AppTimeModel newItem)
        {
            var founded = list.Find(item =>
                item.AppName == newItem.AppName
                && item.Day == newItem.Day
                && item.Month == newItem.Month
                && item.Year == newItem.Year);

            if (founded == null)
            {
                list.Add(newItem);
            } else {
                founded.Duration += newItem.Duration;
            }
        }
    }
}

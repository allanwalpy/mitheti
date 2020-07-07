using System.Collections.Generic;
using System.Linq;

using Mitheti.Core.Database;
using Mitheti.Web.Models;
using Mitheti.Web.Extensions;

namespace Mitheti.Web.Services
{
    public class SearchService : ISearchService
    {
        public IConnectionService _connectionService;

        public SearchService(IConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        public List<AppTimeViewModel> Get(SearchFilter filter)
        {
            using (var dbContext = _connectionService.Context)
            {
                var db = dbContext.AppTimes;

                var result = db.Where(filter.Apply);

                return Normalize(result).ItemsToView();
            }
        }

        private static List<AppTimeModel> Normalize(IEnumerable<AppTimeModel> database)
        {
            List<AppTimeModel> result = new List<AppTimeModel>();
            foreach (var item in database)
            {
                AddAppTime(result, item);
            }
            return result;
        }

        private static void AddAppTime(List<AppTimeModel> list, AppTimeModel newItem)
        {
            var founded = list.Find(item => newItem.IsSameTimeSpanDay(item));

            if (founded != null)
            {
                founded.Duration += newItem.Duration;
                return;
            }

            list.Add(newItem);
        }
    }
}

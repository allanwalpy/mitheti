using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Mitheti.Core.Services
{
    public class AddFilterService : IAddFilterService
    {
        public const string AppListConfigKey = "applist";

        private readonly List<string> _appList;

        public AddFilterService(IConfiguration config)
        {
            _appList = config.GetList(AppListConfigKey, new List<string>());
        }

        public bool HavePassed(string app) => _appList.Count == 0 || _appList.Contains(app);
    }
}
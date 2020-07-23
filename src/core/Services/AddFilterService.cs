using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Mitheti.Core.Services
{
    public class AddFilterService : IAddFilterService
    {
        public const string AppListConfigKey = "filter:list";
        public const string AppModeConfigKey = "filter:allowed";

        private readonly List<string> _list;
        private readonly bool _mode;

        public AddFilterService(IConfiguration config)
        {
            _list = config.GetList(AppListConfigKey, new List<string>());
            _mode = config.GetValue(AppModeConfigKey, false);
        }

        //? always true on empty list;
        //? on _mode `true` filter only apps in list;
        //? on _mode `false` filter only apps not in list;
        public bool HavePassed(string app)
            => _list.Count == 0 || (_mode == _list.Contains(app));
    }
}
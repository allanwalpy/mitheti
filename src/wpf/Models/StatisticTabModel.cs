using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Mitheti.Wpf.Models
{
    public sealed class StatisticTabModel
    {
        public Dictionary<string, string> Localization { get; set; }

        public List<string> DayOfWeekString { get; set; }
        public ObservableCollection<TopAppModel> TopAppsData { get; set; }
    }
}
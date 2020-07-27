using System.Collections.Generic;
using System.Collections.ObjectModel;
using Mitheti.Wpf.Models.Statistic;

namespace Mitheti.Wpf.Models
{
    public sealed class StatisticTabModel
    {
        public Dictionary<string, string> Localization { get; set; }

        public ObservableCollection<DayOfWeekItemModel> DayOfWeekData { get; set; }
        public ObservableCollection<TopAppItemModel> TopAppsData { get; set; }
    }
}
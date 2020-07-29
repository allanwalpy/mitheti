using System.Collections.ObjectModel;
using Mitheti.Wpf.Models.Statistic;

namespace Mitheti.Wpf.Models
{
    public sealed class StatisticTabModel
    {
        public ObservableCollection<DayOfWeekItemModel> DayOfWeekData { get; set; }
        public ObservableCollection<TopAppItemModel> TopAppsData { get; set; }
    }
}
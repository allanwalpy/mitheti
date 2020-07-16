using System.Collections.Generic;

namespace Mitheti.Wpf.Models
{
    public sealed class StatisticTabModel
    {
        public Dictionary<string, string> Localization { get; set; }

        public List<string> DayOfWeekString { get; set; }
        public List<string> TopAppsString { get; set; }
    }
}
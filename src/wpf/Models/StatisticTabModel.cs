using System.Collections.Generic;

namespace Mitheti.Wpf.Models
{
    public sealed class StatisticTabModel : BaseModel
    {
        public List<string> DayOfWeekString { get; set; }
        public List<string> TopAppsString { get; set; }
    }
}
using System.Collections.Generic;

namespace Mitheti.Wpf.Model
{
    public sealed class MainWindowModel : BaseModel
    {
        public string IsLaunchedString { get; set; }
        public List<string> DayOfWeekString { get; set; }
        public List<string> TopAppsString { get; set; }
    }
}
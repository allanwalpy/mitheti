using System.Collections.Generic;
using Mitheti.Core;

namespace Mitheti.Wpf.Models
{
    public sealed class SettingTabModel
    {
        public Dictionary<string, string> Localization { get; set; }

        public LimitedCountableSetting<int> TrackingDelaySetting { get; set; }
        public LimitedCountableSetting<int> SavingDelaySetting { get; set; }
        public LimitedCountableSetting<int> DatabaseSizeSetting { get; set; }

        public TimePeriod ClearSetting { get; set; }
    }
}
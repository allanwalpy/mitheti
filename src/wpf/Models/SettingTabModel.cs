using System.Collections.ObjectModel;
using Mitheti.Core;

namespace Mitheti.Wpf.Models
{
    public sealed class SettingTabModel
    {
        public NumberRangeSetting TrackingDelaySetting { get; set; }
        public NumberRangeSetting SavingDelaySetting { get; set; }
        public NumberRangeSetting DatabaseSizeSetting { get; set; }
        public bool FilterMode { get; set; }
        public ObservableCollection<string> FilterList { get; set; }

        public TimePeriod ClearSetting { get; set; }
        public string ClearButtonLabel { get; set; }
    }
}
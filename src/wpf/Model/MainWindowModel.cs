using System.Collections.Generic;

namespace Mitheti.Wpf.Model
{
    public sealed class MainWindowModel
    {
        public Dictionary<string, string> Localization { get; set; }
        public string IsLaunchedString { get; set; }
    }
}
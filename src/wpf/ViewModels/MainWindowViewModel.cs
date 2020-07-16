using System.Collections.Generic;
using Mitheti.Wpf.Services;

namespace Mitheti.Wpf.ViewModels
{
    public class MainWindowViewModel
    {
        public Dictionary<string, string> Localization { get; set; }

        public MainWindowViewModel(ILocalizationService localization)
        {
            Localization = localization.Data;
        }
    }
}
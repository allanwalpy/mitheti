using System.Collections.Generic;
using Mitheti.Wpf.Services;

namespace Mitheti.Wpf.ViewModel
{
    public abstract class BaseViewModel
    {
        public Dictionary<string, string> Localization { get; }

        public BaseViewModel(ILocalizationService localization)
        {
            Localization = localization.Data;
        }
    }
}

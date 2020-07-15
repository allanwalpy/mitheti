using System.Collections.Generic;
using Mitheti.Wpf.Services;

namespace Mitheti.Wpf.ViewModels
{
    // здесь тоже самое что и для BaseModel
    public abstract class BaseViewModel
    {
        public Dictionary<string, string> Localization { get; }

        public BaseViewModel(ILocalizationService localization)
        {
            Localization = localization.Data;
        }
    }
}

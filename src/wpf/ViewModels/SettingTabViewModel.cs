using System.Collections.Generic;
using System.ComponentModel;
using Mitheti.Wpf.Services;

namespace Mitheti.Wpf.ViewModels
{
    public class SettingTabViewModel
    {
        //TODO: complete;
        public event PropertyChangedEventHandler PropertyChanged;

        public Dictionary<string, string> Localization { get; }

        public SettingTabViewModel(ILocalizationService localization)
        {
            Localization = localization.Data;
        }

        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
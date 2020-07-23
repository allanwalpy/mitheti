using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Mitheti.Wpf.Models
{
    public class AboutTabModel
    {
        public Dictionary<string, string> Localization { get; set; }

        public string License { get; set; }
        public ObservableCollection<ContributorModel> Contributors { get; set; }
    }
}
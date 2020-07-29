using System.Collections.ObjectModel;

namespace Mitheti.Wpf.Models
{
    public class AboutTabModel
    {
        public string License { get; set; }
        public ObservableCollection<ContributorModel> Contributors { get; set; }
    }
}
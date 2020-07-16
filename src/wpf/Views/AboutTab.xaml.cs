using Mitheti.Wpf.Services;
using Mitheti.Wpf.ViewModels;

namespace Mitheti.Wpf.Views
{
    // забудь про существование UserControl это бажно существо которое
    // вместо UserControl сделай ResourceDictionary и туда положи DataTemplate для своих ViewModel
    // этот словарь можно будет просто запихнуть в ресурсы окна или какого нить элемента чтобы вьюшки
    // описанные в DataTemplate`ах работали
    public partial class AboutTab
    {
        public AboutTab(ILocalizationService localization)
        {
            DataContext = new AboutTabViewModel(localization);
            InitializeComponent();
        }
    }
}

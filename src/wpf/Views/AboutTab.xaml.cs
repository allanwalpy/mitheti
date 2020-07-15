using System.Collections.Generic;
using System.Windows.Controls;
using Mitheti.Core.Services;
using Mitheti.Wpf.Services;
using Mitheti.Wpf.ViewModels;

namespace Mitheti.Wpf.Views
{
    // забудь про существование UserControl это бажно существо которое
    // вместо UserControl сделай ResourceDictionary и туда положи DataTemplate для своих ViewModel
    // этот словарь можно будет просто запихнуть в ресурсы окна или какого нить элемента чтобы вьюшки
    // описанные в DataTemplate`ах работали

    // также тут во View присутствует кодэбихайнд логика этого быть не должно
    // вся логика работы с данными должна находиться во ViewModel
    // вся логика работы интерфейса должна быть только в xaml
    public partial class AboutTab
    {
        public AboutTab(ILocalizationService localization)
        {
            DataContext = new AboutTabViewModel(localization);
            InitializeComponent();

            SetCollaborators(localization.Config.GetList("Window:About:Collaborators", new List<string>()));
        }

        // TODO: add from github api?;
        private void SetCollaborators(List<string> collaborators)
        {
            foreach (var person in collaborators)
            {
                TabContent.Children.Add(new Label {Content = person});
            }
        }
    }
}

using System.Collections.Generic;
using System.Windows.Controls;
using Mitheti.Core.Services;
using Mitheti.Wpf.Services;
using Mitheti.Wpf.ViewModels;

namespace Mitheti.Wpf.Views
{
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
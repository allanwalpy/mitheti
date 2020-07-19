using System.Collections.Generic;
using Mitheti.Wpf.ViewModels;

namespace Mitheti.Wpf.Views
{
    public partial class ContributorAboutTab
    {
        public ContributorAboutTab(Dictionary<string, string> localization, string name, string githubUrl, string personalUrl, string avatarUrl)
        {
            DataContext = new ContributorViewModel(localization, name, githubUrl, personalUrl, avatarUrl);
            //InitializeComponent();
        }
    }
}
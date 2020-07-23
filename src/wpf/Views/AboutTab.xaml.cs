using System.Diagnostics;
using System.Windows.Navigation;
using Mitheti.Wpf.ViewModels;

namespace Mitheti.Wpf.Views
{
    // TODO: find replacement to UserControl (ResourceDictionary & DataTemplate?)?;
    public partial class AboutTab
    {
        public AboutTab(AboutTabViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
        private void OpenInBrowser(object sender, RequestNavigateEventArgs args)
        {
            Process.Start(new ProcessStartInfo(args.Uri.AbsolutePath));
            args.Handled = true;
        }
    }
}
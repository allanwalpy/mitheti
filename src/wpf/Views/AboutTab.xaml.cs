using System;
using System.Threading;
using System.Threading.Tasks;
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
            viewModel.Initialize(this);
        }
    }
}
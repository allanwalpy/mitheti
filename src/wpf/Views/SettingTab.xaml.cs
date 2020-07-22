using Mitheti.Wpf.ViewModels;

namespace Mitheti.Wpf.Views
{
    public partial class SettingTab
    {
        public SettingTab(SettingTabViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
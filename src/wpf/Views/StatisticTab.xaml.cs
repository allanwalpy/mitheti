using Mitheti.Wpf.ViewModels;

namespace Mitheti.Wpf.Views
{
    public partial class StatisticTab
    {
        public StatisticTab(StatisticTabViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
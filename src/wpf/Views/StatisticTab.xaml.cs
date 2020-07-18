using System.Windows.Controls;
using System.Windows.Input;
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

        //? see https://stackoverflow.com/a/16235785 ;
        private void OnMouseScroll(object sender, MouseWheelEventArgs args)
        {
            var scroll = sender as ScrollViewer;
            scroll?.ScrollToVerticalOffset(scroll.VerticalOffset - args.Delta);

            args.Handled = true;
        }
    }
}
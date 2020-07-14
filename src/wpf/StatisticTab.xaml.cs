using System.Windows;
using Mitheti.Core.Services;
using Mitheti.Wpf.Services;
using Mitheti.Wpf.ViewModel;

namespace Mitheti.Wpf
{
    public partial class StatisticTab
    {
        private MainWindow _window;

        public StatisticTab(ILocalizationService localization, IStatisticDayOfWeekService dayOfWeek,
            IStatisticTopAppService topApp)
        {
            DataContext = new StatisticTabViewModel(localization, dayOfWeek, topApp);
            InitializeComponent();
        }
        
        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            _window = (MainWindow) Window.GetWindow(this);
        }
    }
}
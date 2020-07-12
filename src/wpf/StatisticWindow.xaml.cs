using Mitheti.Core.Services;
using Mitheti.Wpf.Services;
using Mitheti.Wpf.ViewModel;

namespace Mitheti.Wpf
{
    public partial class StatisticWindow
    {
        public StatisticWindow(ILocalizationService localization, IStatisticDayOfWeekService dayOfWeekService)
        {
            DataContext = new StatisticWindowViewModel(localization, dayOfWeekService);
            InitializeComponent();

            // TODO: replace StatisticWindow and MainWindow with nameof;
            Title = localization["StatisticWindow:Title"];
        }
    }
}
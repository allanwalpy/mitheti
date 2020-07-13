using Mitheti.Core.Services;
using Mitheti.Wpf.Services;
using Mitheti.Wpf.ViewModel;

namespace Mitheti.Wpf
{
    public partial class StatisticWindow
    {
        public StatisticWindow(ILocalizationService localization, IStatisticDayOfWeekService dayOfWeek,
            IStatisticTopAppService topApp)
        {
            DataContext = new StatisticWindowViewModel(localization, dayOfWeek, topApp);
            InitializeComponent();

            Title = localization[$"{nameof(StatisticWindow)}:Title"];
        }
    }
}
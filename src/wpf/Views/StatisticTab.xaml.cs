using Mitheti.Core.Services;
using Mitheti.Wpf.Services;
using Mitheti.Wpf.ViewModels;

namespace Mitheti.Wpf.Views
{
    public partial class StatisticTab
    {
        public StatisticTab(ILocalizationService localization, IStatisticDayOfWeekService dayOfWeek,
            IStatisticTopAppService topApp)
        {
            DataContext = new StatisticTabViewModel(localization, dayOfWeek, topApp);
            InitializeComponent();
        }
    }
}
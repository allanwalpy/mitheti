using Mitheti.Core.Services;
using Mitheti.Wpf.Services;
using Mitheti.Wpf.ViewModels;

namespace Mitheti.Wpf.Views
{
    // тоже самое что и для AboutTab
    public partial class StatisticTab
    {
        public StatisticTab(ILocalizationService localization, IStatisticDatabaseService statisticDatabase)
        {
            DataContext = new StatisticTabViewModel(localization, statisticDatabase);
            InitializeComponent();
        }
    }
}
using System.Collections.Generic;
using System.Windows.Controls;
using Mitheti.Wpf.Services;
using Mitheti.Wpf.Views;

namespace Mitheti.Wpf.ViewModels
{
    public class MainWindowViewModel
    {
        private readonly MainTab _mainTab;
        private readonly StatisticTab _statisticTab;
        private readonly AboutTab _aboutTab;

        public Dictionary<string, string> Localization { get; }

        public MainWindowViewModel(ILocalizationService localization,
            MainTab mainTab, StatisticTab statisticTab, AboutTab aboutTab)
        {
            Localization = localization.Data;

            _mainTab = mainTab;
            _statisticTab = statisticTab;
            _aboutTab = aboutTab;
        }

        public void SetTabs(TabControl tabControl)
        {
            tabControl.Items.Insert(0, GetTabItem("Window:Main:Tab:Title", _mainTab));
            tabControl.Items.Insert(1, GetTabItem("Window:Statistic:Tab:Title", _statisticTab));
            tabControl.Items.Insert(2, GetTabItem("Window:About:Tab:Title", _aboutTab));
        }

        private TabItem GetTabItem(string header, UserControl content)
            => new TabItem {Header = Localization[header], Content = content};
    }
}
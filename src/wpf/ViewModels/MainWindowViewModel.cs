using System.Windows.Controls;
using Mitheti.Wpf.Views;

namespace Mitheti.Wpf.ViewModels
{
    public class MainWindowViewModel
    {
        private readonly MainTab _mainTab;
        private readonly StatisticTab _statisticTab;
        private readonly SettingTab _settingTab;
        private readonly AboutTab _aboutTab;

        public MainWindowViewModel(MainTab mainTab, StatisticTab statisticTab, SettingTab settingTab, AboutTab aboutTab)
        {
            _mainTab = mainTab;
            _statisticTab = statisticTab;
            _settingTab = settingTab;
            _aboutTab = aboutTab;
        }

        public void SetTabs(TabControl tabControl)
        {
            tabControl.Items.Insert(0, GetTabItem("Window:Main:Tab:Title", _mainTab));
            tabControl.Items.Insert(1, GetTabItem("Window:Statistic:Tab:Title", _statisticTab));
            tabControl.Items.Insert(2, GetTabItem("Window:Setting:Tab:Title", _settingTab));
            tabControl.Items.Insert(3, GetTabItem("Window:About:Tab:Title", _aboutTab));
        }

        private static TabItem GetTabItem(string header, UserControl content)
            => new TabItem {Header = header.Translate(), Content = content};
    }
}
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using Mitheti.Core;
using Mitheti.Core.Services;
using Mitheti.Wpf.ViewModels;

namespace Mitheti.Wpf.Views
{
    public partial class SettingTab
    {
        private readonly IDatabaseService _database;
        private readonly SettingTabViewModel _viewModel;

        public SettingTab(SettingTabViewModel viewModel, IDatabaseService database)
        {
            _database = database;

            _viewModel = viewModel;
            DataContext = viewModel;
            InitializeComponent();
        }

        private void OnSaveClick(object sender, RoutedEventArgs e) => _viewModel.OnSaveClick(sender, e);

        private void OnClearClick(object sender, RoutedEventArgs e)
        {
            var button = (sender as Button) ?? throw new ArgumentNullException(nameof(sender));
            button.IsEnabled = false;

            ClearAction(button).ConfigureAwait(false);
        }

        private async Task ClearAction(UIElement button)
        {
            var dateBegin = ClearSettingBeginDate.SelectedDate
                            ?? throw new ArgumentNullException(nameof(ClearSettingBeginDate.SelectedDate));
            var timeBegin = ClearSettingBeginTime.SelectedTime
                            ?? throw new ArgumentNullException(nameof(ClearSettingBeginTime.SelectedTime));
            var begin = new DateTime(dateBegin.Year, dateBegin.Month, dateBegin.Day,
                timeBegin.Hour, timeBegin.Minute,0);

            var dateEnd = ClearSettingEndDate.SelectedDate
                          ?? throw new ArgumentNullException(nameof(ClearSettingEndDate.SelectedDate));
            var timeEnd = ClearSettingEndTime.SelectedTime
                          ?? throw new ArgumentNullException(nameof(ClearSettingEndTime.SelectedTime));
            var end = new DateTime(dateEnd.Year, dateEnd.Month, dateEnd.Day, timeEnd.Hour, timeEnd.Minute, 0);

            await _database.ClearAsync(new TimePeriod
                {
                    Begin = begin,
                    End = end
                })
                .ContinueWith((task) => Dispatcher.Invoke(() => button.IsEnabled = true));

            _viewModel.UpdateClearButtonLabel();
        }

        private void Option_FilterList_DeleteItem(object sender, RoutedEventArgs e)
            => _viewModel.DeleteFilterListItem(sender, e);

        private void Option_FilterList_OnDialogClosing(object sender, DialogClosingEventArgs args)
            => _viewModel.AddFilterListItem(FilterListAddTextBox.Text, args);
    }
}
using System;
using System.Windows;
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

        private async void OnClearClick(object sender, RoutedEventArgs e)
        {
            var dateBegin = ClearSettingBeginDate.SelectedDate ?? throw new ArgumentNullException(nameof(ClearSettingBeginDate.SelectedDate));
            var timeBegin = ClearSettingBeginTime.SelectedTime ?? throw new ArgumentNullException(nameof(ClearSettingBeginTime.SelectedTime));
            var begin = new DateTime(dateBegin.Year, dateBegin.Month, dateBegin.Day, timeBegin.Hour, timeBegin.Minute, timeBegin.Second);

            var dateEnd = ClearSettingEndDate.SelectedDate ?? throw new ArgumentNullException(nameof(ClearSettingEndDate.SelectedDate));
            var timeEnd = ClearSettingEndTime.SelectedTime ?? throw new ArgumentNullException(nameof(ClearSettingEndTime.SelectedTime));
            var end = new DateTime(dateEnd.Year, dateEnd.Month, dateEnd.Day, timeEnd.Hour, timeEnd.Minute, timeEnd.Second);

            //TODO: make button disabled during clearing;
            await _database.ClearAsync(new TimePeriod
            {
                Begin = begin,
                End = end
            });
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Mitheti.Core.Services;
using Mitheti.Wpf.ViewModels;

namespace Mitheti.Wpf.Views
{
    public partial class StatisticTab : IDisposable
    {
        public const int UpdateDuration = 60 * 1000; //? one minute;
        public const int StopWait = 250;

        private readonly Task _updateInfoTask;
        private readonly StatisticTabViewModel _viewModel;
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();

        public StatisticTab(StatisticTabViewModel viewModel)
        {
            _viewModel = viewModel;
            DataContext = viewModel;
            InitializeComponent();
            viewModel.Initialize(this).ConfigureAwait(false);

            _updateInfoTask = UpdateInfo(_tokenSource.Token);
        }

        //? fix no scroll on content mouse scroll; see https://stackoverflow.com/a/16235785 ;
        private void OnMouseScroll(object sender, MouseWheelEventArgs args)
        {
            var scroll = sender as ScrollViewer;
            scroll?.ScrollToVerticalOffset(scroll.VerticalOffset - args.Delta);

            args.Handled = true;
        }

        private async Task UpdateInfo(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(UpdateDuration, token);
                await _viewModel.UpdateInfo();
            }
        }

        public void Dispose()
        {
            _tokenSource.Cancel();
            _updateInfoTask.WaitCancelled(StopWait);
            _tokenSource.Dispose();
        }
    }
}
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Mitheti.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HostLauncher _hostLauncher;
        private Label _statusLabel;

        public MainWindow()
        {
            InitializeComponent();

            _statusLabel = null;

            _hostLauncher = new HostLauncher();
            _hostLauncher.OnStatusChange += this.StatusChangeEvent;
        }

        protected override void OnClosed(EventArgs args)
        {
            _hostLauncher.Dispose();

            base.OnClosed(args);
        }

        public void StartClick(object sender, RoutedEventArgs args)
        {
            _hostLauncher.StartAsync().ConfigureAwait(false);
        }

        public void StopClick(object sender, RoutedEventArgs args)
        {
            _hostLauncher.StopAsync().ConfigureAwait(false);
        }

        //TODO:FIXME: use setting values, not just magic strings;
        //TODO: open in internal browser on new window;
        public void StatisticClick(object sender, RoutedEventArgs e)
        {
            WebWindow window = new WebWindow();
            window.Show();
        }

        private void OnStatusLabelLoaded(object sender, RoutedEventArgs args)
        {
            //? save status label;
            _statusLabel = sender as Label;
            if (_statusLabel == null)
            {
                throw new ArgumentNullException(nameof(_statusLabel), "label not founded");
            }

            //? update status on label;
            this.UpdateStatus(isLaunched: false);
        }

        private void StatusChangeEvent(object sender, EventArgs args)
        {
            bool? isLaunched = (args as StatusChangeEventArgs)?.IsLaunched;

            this.UpdateStatus(isLaunched ?? false);
        }

        private void UpdateStatus(bool isLaunched)
        {
            //TODO:add json settnig for this;
            _statusLabel.Content =    isLaunched ? "запущен"     : "остановлен";
            _statusLabel.Foreground = isLaunched ? Brushes.Green : Brushes.Red;
        }
    }
}

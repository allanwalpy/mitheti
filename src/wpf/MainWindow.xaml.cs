using System;
using System.Diagnostics;
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
            _hostLauncher.OnHostStatusChange += this.StatusChangeEvent;

            this.UpdateStatus(isLaunched: false);
        }

        protected override void OnClosed(EventArgs args)
        {
            _hostLauncher.Dispose();

            base.OnClosed(args);
        }

        public void StartClick(object sender, RoutedEventArgs args) => _hostLauncher.StartAsync();

        public void StopClick(object sender, RoutedEventArgs args) => _hostLauncher.StopAsync();

        //TODO:FIXME: use setting values, not just magic strings;
        //TODO: open in internal browser on new window;
        public void StatisticClick(object sender, RoutedEventArgs e)
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = "http://localhost:5004",
                UseShellExecute = true
            };
            Process.Start(processInfo);
        }

        private void StatusLabelLoaded(object sender, RoutedEventArgs args)
        {
            _statusLabel = sender as Label;
            if (_statusLabel == null)
            {
                throw new ArgumentNullException(nameof(_statusLabel), "label not founded");
            }
        }

        private void StatusChangeEvent(object sender, EventArgs args)
        {
            bool? isLaunched = (args as HostLauncher.HostStatusChangeEvent)?.IsLaunched;
            if (isLaunched == null)
            {
                return;
            }

            this.UpdateStatus(isLaunched ?? false);
        }

        private void UpdateStatus(bool isLaunched)
        {
            if (_statusLabel == null)
            {
                return;
            }

            //TODO:add json settnig for this;
            _statusLabel.Content =    isLaunched ? "запущен"     : "остановлен";
            _statusLabel.Foreground = isLaunched ? Brushes.Green : Brushes.Red;
        }
    }
}

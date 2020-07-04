using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

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
            _hostLauncher.OnHostStatusChange += this.UpdateStatusLabel;
            this.Closed += (sener, args) => _hostLauncher.Dispose();
        }

        public void StartClick(object sender, RoutedEventArgs args)
        {
            _hostLauncher.Start();
        }

        public void StopClick(object sender, RoutedEventArgs args)
        {
            _hostLauncher.Stop();
        }

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

        private void UpdateStatusLabel(object sender, EventArgs args)
        {
            if (_statusLabel == null)
            {
                return;
            }

            var isLaunched = (args as HostLauncher.HostStatusChangeEvent)?.IsLaunched ?? false;
            var message = isLaunched ? "запущен" : "остановлен";
            _statusLabel.Content = message;
        }
    }
}

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
            this.Closed += (sener, args) => _hostLauncher.Dispose();
        }

        public void StartClick(object sender, RoutedEventArgs args)
        {
            _hostLauncher.Start();
            this.UpdateStatusLabel();
        }

        public void StopClick(object sender, RoutedEventArgs args)
        {
            _hostLauncher.Stop();
            this.UpdateStatusLabel();
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

            this.UpdateStatusLabel();
        }

        private void UpdateStatusLabel()
        {
            if (_statusLabel == null)
            {
                return;
            }

            var message = _hostLauncher.IsLaunched ? "запущен" : "остановлен";
            _statusLabel.Content = message;
        }
    }
}

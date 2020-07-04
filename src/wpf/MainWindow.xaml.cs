using System;
using System.Diagnostics;
using System.Windows;

namespace Mitheti.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HostLauncher _hostLauncher;

        public MainWindow()
        {
            InitializeComponent();

            _hostLauncher = new HostLauncher();
            this.Closed += (sener, args) => _hostLauncher.Dispose();
        }

        public void StartClick(object sender, RoutedEventArgs args) => _hostLauncher.Start();

        public void StopClick(object sender, RoutedEventArgs args) => _hostLauncher.Stop();

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
    }
}

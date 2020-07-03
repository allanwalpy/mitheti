using System;
using System.Windows;

namespace Mitheti.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        private ServiceControll _serviceControll;

        public MainWindow()
        {
            InitializeComponent();

            _serviceControll = new ServiceControll();
            //this.Closed += (a, b) => this._serviceControll.Dispose();
        }

        public void StartClick(object sender, RoutedEventArgs args)
        {
            _serviceControll.Start();
        }

        public void StopClick(object sender, RoutedEventArgs args)
        {
            _serviceControll.Stop();
        }

        public void StatisticClick(object sender, RoutedEventArgs e)
        {
            //TODO:FIXME:;
        }

        public void Dispose() => _serviceControll.Dispose();
    }
}

using System;
using System.ComponentModel;
using System.Drawing;
using Forms = System.Windows.Forms;
using Mitheti.Core.Services;
using Mitheti.Wpf.Services;
using Mitheti.Wpf.ViewModels;

namespace Mitheti.Wpf.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>



    // здесь лучше избежать взаимодействия с интерфейсом, только логика работы самого Window
    // всяких табов тут в коде быть не должно
    public partial class MainWindow
    {
        private const string TrayIconBaseFile = "./Resources/tray";
        public const string TrayIconOnFile = TrayIconBaseFile + ".on.ico";
        public const string TrayIconOffFile = TrayIconBaseFile + ".off.ico";
        public const string TrayIconDefaultFile = TrayIconOffFile;

        private readonly IWatcherControlService _watcherControl;
        private readonly ILocalizationService _localization;
        private readonly Forms.NotifyIcon _tray;

        public MainWindow(ILocalizationService localization, IWatcherControlService watcherControl,
            IStatisticDayOfWeekService dayOfWeek, IStatisticTopAppService topApp)
        {
            _localization = localization;
            _watcherControl = watcherControl;

            DataContext = new MainWindowViewModel(localization, dayOfWeek, topApp);
            InitializeComponent();
            TabMain.Content = new MainTab(localization, watcherControl);
            TabStatistic.Content = new StatisticTab(localization, dayOfWeek, topApp);
            TabAbout.Content = new AboutTab(localization);

            _tray = new Forms.NotifyIcon();
            ConfigureTray();

            Title = _localization[$"Window:Title"];
            Closing += HideWindow;
        }

        private void ConfigureTray()
        {
            //TODO: make separate class configuration for tray?;
            _tray.MouseClick += OnTrayClick;
            _tray.Icon = new Icon(TrayIconDefaultFile);
            _tray.Visible = true;
            _watcherControl.StatusChanged += ChangeTray;

            _tray.ContextMenuStrip = new Forms.ContextMenuStrip();
            _tray.ContextMenuStrip.Items.Add(_localization["Tray:Options:Show"]).Click += OnTrayClickShow;
            _tray.ContextMenuStrip.Items.Add(_localization["Tray:Options:Close"]).Click += OnTrayClickExit;
        }

        private void OnTrayClick(object? sender, Forms.MouseEventArgs args)
        {
            var isLeftClick = ((args.Button & Forms.MouseButtons.Left) != 0);
            if (isLeftClick)
            {
                OnTrayClickShow(sender, args);
            }
        }

        private void OnTrayClickShow(object? sender, EventArgs args)
        {
            Activate();
            Show();
        }

        private void OnTrayClickExit(object sender, EventArgs args) => Exit();

        private void ChangeTray(object sender, WatcherStatusEventArgs args)
            => _tray.Icon = new Icon(args.IsLaunched ? TrayIconOnFile : TrayIconOffFile);

        private void HideWindow(object sender, CancelEventArgs args)
        {
            args.Cancel = true;
            Hide();
        }

        internal void Exit()
        {
            _watcherControl.StatusChanged -= ChangeTray;
            _tray.MouseClick -= OnTrayClick;
            _tray.ContextMenuStrip.Dispose();
            _tray.Dispose();

            Closing -= HideWindow;
            Close();
        }
    }
}

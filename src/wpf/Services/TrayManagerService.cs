using System;
using System.Drawing;
using Microsoft.Extensions.Logging;
using Forms = System.Windows.Forms;
using Mitheti.Core.Services;
using Mitheti.Wpf.Views;

namespace Mitheti.Wpf.Services
{
    public class TrayManagerService : ITrayManagerService
    {
        private const string TrayIconBaseFile = "./Resources/tray";
        public const string TrayIconOnFile = TrayIconBaseFile + ".on.ico";
        public const string TrayIconOffFile = TrayIconBaseFile + ".off.ico";

        private readonly  ILogger<TrayManagerService> _logger;
        private readonly IWatcherControlService _watcherControl;
        private readonly ILocalizationService _localization;

        private MainWindow _window;
        private Forms.NotifyIcon _tray;

        public TrayManagerService(ILogger<TrayManagerService> logger, ILocalizationService localization, IWatcherControlService watcherControl)
        {
            _watcherControl = watcherControl;
            _localization = localization;
            _logger = logger;
        }

        public void Initialize(MainWindow window)
        {
            _window = window;
            _tray = new Forms.NotifyIcon();

            _tray.MouseClick += OnTrayClick;
            ChangeTray(window, new WatcherStatusEventArgs(false));
            _tray.Visible = true;
            _watcherControl.WatcherStatusChanged += ChangeTray;

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
            _window.Activate();
            _window.Show();
        }

        private void OnTrayClickExit(object sender, EventArgs args) => _window.Exit();

        private void ChangeTray(object sender, WatcherStatusEventArgs args)
            => _tray.Icon = new Icon(args.IsLaunched ? TrayIconOnFile : TrayIconOffFile);

        public void Dispose()
        {
            try
            {
                //TODO:FIXME: crash on exit without try catch block;
                _watcherControl.WatcherStatusChanged -= ChangeTray;
                _tray.MouseClick -= OnTrayClick;
                _tray.ContextMenuStrip.Dispose();
                _tray.Dispose();
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Exit with exception on {nameof(Dispose)}: \n{e}");
            }

        }
    }
}
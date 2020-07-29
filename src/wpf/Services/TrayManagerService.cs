using System;
using System.Drawing;
using Microsoft.Extensions.Logging;
using Forms = System.Windows.Forms;
using Mitheti.Core.Services;

namespace Mitheti.Wpf.Services
{
    public class TrayManagerService : ITrayManagerService
    {
        private const string TrayIconBaseFile = "./Resources/tray";
        public const string TrayIconOnFile = TrayIconBaseFile + ".on.ico";
        public const string TrayIconOffFile = TrayIconBaseFile + ".off.ico";

        private readonly ILogger<TrayManagerService> _logger;
        private readonly IWatcherControlService _watcherControl;

        private Forms.NotifyIcon _tray;

        public event EventHandler WindowShowing;
        public event EventHandler WindowExiting;

        public TrayManagerService(ILogger<TrayManagerService> logger, IWatcherControlService watcherControl)
        {
            _watcherControl = watcherControl;
            _logger = logger;

            Initialize();
        }

        private void Initialize()
        {
            _tray = new Forms.NotifyIcon();

            _tray.MouseClick += OnTrayClick;

            _tray.Visible = true;
            _watcherControl.WatcherStatusChanged += ChangeTray;

            _tray.ContextMenuStrip = new Forms.ContextMenuStrip();
            _tray.ContextMenuStrip.Items.Add("Tray:Options:Show".Translate()).Click += OnTrayClickShow;
            _tray.ContextMenuStrip.Items.Add("Tray:Options:Close".Translate()).Click += OnTrayClickExit;

            ChangeTray(null, new WatcherStatusEventArgs(false));
        }

        private void OnTrayClick(object? sender, Forms.MouseEventArgs args)
        {
            var isLeftClick = ((args.Button & Forms.MouseButtons.Left) != 0);
            if (isLeftClick)
            {
                OnTrayClickShow(sender, args);
            }
        }

        private void OnTrayClickShow(object? sender, EventArgs args) => WindowShowing?.Invoke(sender, args);

        private void OnTrayClickExit(object sender, EventArgs args) => WindowExiting?.Invoke(sender, args);

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
                _logger.LogWarning($"Exit with exception on {nameof(TrayManagerService)}.{nameof(Dispose)}: \n{e}");
            }
        }
    }
}
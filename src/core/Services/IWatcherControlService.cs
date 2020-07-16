namespace Mitheti.Core.Services
{
    public delegate void WatcherStatusChangedHandler(object sender, WatcherStatusEventArgs args);

    public interface IWatcherControlService
    {
        event WatcherStatusChangedHandler WatcherStatusChanged;

        bool IsLaunched { get; }

        void Start();

        void Stop();
    }
}
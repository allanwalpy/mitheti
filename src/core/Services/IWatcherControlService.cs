using System.Threading.Tasks;

namespace Mitheti.Core.Services
{
    public delegate void StatusChangedHandler(object sender, WatcherStatusEventArgs args);

    public interface IWatcherControlService
    {
        event StatusChangedHandler StatusChanged;

        bool IsLaunched { get; }

        void Start();

        Task StopAsync();
    }
}
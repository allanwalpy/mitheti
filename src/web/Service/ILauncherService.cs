using System.Threading.Tasks;

namespace Mitheti.Web.Service
{
    public interface ILauncherService
    {
        bool IsRunning { get; }
        bool IsProcessing { get; }
        LauncherServiceState State { get; }
        void StartAsync();
        void StopAsync();
    }

    public enum LauncherServiceState
    {
        Stopped = 0,
        Running = 1,
        Launching = 2,
        Stopping = 3
    }
}

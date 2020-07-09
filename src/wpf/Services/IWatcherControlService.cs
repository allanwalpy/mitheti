using System.Threading.Tasks;

namespace Mitheti.Wpf.Services
{
    public interface IWatcherControlService
    {
        bool IsLaunched { get; }
        
        void Start();

        Task StopAsync();
    }
}
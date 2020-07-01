using System.Threading;
using System.Threading.Tasks;

namespace Mitheti.Core.Watcher
{
    public interface IWatcherService
    {
        Task Run(CancellationToken stoppingToken);
    }
}

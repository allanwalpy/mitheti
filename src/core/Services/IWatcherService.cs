using System.Threading;
using System.Threading.Tasks;

namespace Mitheti.Core.Services
{
    public interface IWatcherService
    {
        Task Run(CancellationToken stoppingToken);
    }
}

using System.Threading;
using System.Threading.Tasks;

namespace Mitheti.Core.Services
{
    public interface IWatcherService
    {
        //RunAsync
        Task Run(CancellationToken stoppingToken);
    }
}

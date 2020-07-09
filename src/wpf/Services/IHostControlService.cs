using System.Threading.Tasks;

namespace Mitheti.Wpf.Services
{
    public interface IHostControlService
    {
        Task StartAsync();

        Task StopAsync();
    }
}
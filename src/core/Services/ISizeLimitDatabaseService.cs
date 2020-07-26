using System.Threading.Tasks;

namespace Mitheti.Core.Services
{
    // TODO: add servicing service, with this and other (optimizing and clear records) services on tasks;
    public interface ISizeLimitDatabaseService
    {
        Task LimitDatabase();

        long GetSize();
    }
}
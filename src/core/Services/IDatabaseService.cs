using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mitheti.Core.Services
{
    public interface IDatabaseService
    {
        DatabaseContext GetContext();

        Task AddAsync(IEnumerable<AppTimeModel> list);

        Task ClearAsync(TimePeriod period);
    }
}
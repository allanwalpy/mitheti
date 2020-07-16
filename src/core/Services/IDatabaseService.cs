using System.Collections.Generic;

namespace Mitheti.Core.Services
{
    public interface IDatabaseService
    {
        DatabaseContext GetContext();

        void Add(IEnumerable<AppTimeModel> list);

        void Clear(TimePeriod period);
    }
}
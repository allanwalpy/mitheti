using System.Collections.Generic;

using Mitheti.Core.Database;

namespace Mitheti.Web.Services
{
    public interface IStatisticService
    {
        List<AppTimeModel> GetAppTimes();
    }
}

using System.Collections.Generic;

using Mitheti.Web.Models;

namespace Mitheti.Web.Services
{
    public interface IStatisticService
    {
        List<AppTimeViewModel> GetAppTimes();
    }
}

using System.Collections.Generic;

using Mitheti.Core.Database;
using Mitheti.Web.Models;

namespace Mitheti.Web.Services
{
    public interface ISearchService
    {
        List<AppTimeViewModel> Get(SearchFilter filter);
    }
}

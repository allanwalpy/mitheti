using System.Collections.Generic;

namespace Mitheti.Core.Services
{
    public interface IStatisticTopAppService
    {
        /// <summary>
        /// returns <paramref name="topSize"/> most used apps;
        /// </summary>
        List<TopAppInfo> Get(int topSize, TimeInterval interval);
    }
}
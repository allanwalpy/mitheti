using System.Collections.Generic;

namespace Mitheti.Core.Services
{
    // TODO: add percentage of total of all apps;
    public interface IStatisticTopAppService
    {
        /// <summary>
        /// returns <paramref name="topSize"/> most used apps;
        /// for specified time period;
        /// </summary>
        List<TopAppInfo> Get(int topSize, TimeInterval interval);
        
        /// <summary>
        /// returns <paramref name="topSize"/> most used apps;
        /// </summary>
        List<TopAppInfo> Get(int topSize);
    }
}
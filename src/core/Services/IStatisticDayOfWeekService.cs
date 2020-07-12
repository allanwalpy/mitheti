using System.Collections.Generic;

namespace Mitheti.Core.Services
{
    // TODO: add time periods, for which calculate;
    // TODO: return total seconds instead milliseconds;
    public interface IStatisticDayOfWeekService
    {
        /// <summary>
        /// returns list, where index is day of week (starting with Sunday = 0)
        /// and value is total milliseconds spend in that week;
        /// </summary>
        List<int> GetTotal();

        /// <summary>
        /// returns list, where index is day of week (starting with Sunday = 0)
        /// and value is percentage compare to maximum value (among day of weeks);
        /// </summary>
        List<double> GetPercentage();
    }
}
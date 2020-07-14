using System.Collections.Generic;

namespace Mitheti.Core.Services
{
    // TODO: unite GetTotal and GetPercentage for not calculating twice;
    public interface IStatisticDayOfWeekService
    {
        /// <summary>
        /// returns list, where index is day of week (starting with Sunday = 0)
        /// and value is total milliseconds spend in that week
        /// for specified time period;
        /// </summary>
        List<int> GetTotal(TimeInterval interval);

        /// <summary>
        /// returns list, where index is day of week (starting with Sunday = 0)
        /// and value is total milliseconds spend in that week;
        /// </summary>
        List<int> GetTotal();

        /// <summary>
        /// returns list, where index is day of week (starting with Sunday = 0)
        /// and value is percentage compare to maximum value (among day of weeks)
        /// for specified time period;
        /// </summary>
        List<double> GetPercentage(TimeInterval interval);
        
        /// <summary>
        /// returns list, where index is day of week (starting with Sunday = 0)
        /// and value is percentage compare to maximum value (among day of weeks);
        /// </summary>
        List<double> GetPercentage();
    }
}
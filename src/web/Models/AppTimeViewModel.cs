using System;
using System.Text;

namespace Mitheti.Web.Models
{
    public class AppTimeViewModel
    {
        public string AppName { get; set; }

        public int SecondsSpend { get; set; }

        public DateTime Timestamp { get; set; }

        public string ToStringTimestamp(
            string hoursFormat,
            string minutesFormat,
            string secondsFormat,
            string millisecondsFormat)
        {
            // TODO:;
            return null;
        }
    }
}

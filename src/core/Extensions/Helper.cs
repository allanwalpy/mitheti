using System;
using System.Diagnostics;

using Mitheti.Core.Database;

namespace Mitheti.Core.Extensions
{
    public static class Helper
    {
        public static AppTimeSpanModel ToDatabaseModel(this Process info, int duration)
            => info == null
                ? null
                : new AppTimeSpanModel
                {
                    AppName = info.ProcessName,
                    TimeSpan = duration,
                    Time = DateTime.Now
                };
    }
}

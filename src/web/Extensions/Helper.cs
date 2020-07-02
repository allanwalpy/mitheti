using System;
using System.Collections.Generic;

using Mitheti.Core.Database;
using Mitheti.Web.Models;

namespace Mitheti.Web.Extensions
{
    public static class Helper
    {
        public static AppTimeViewModel ToView(this AppTimeModel dbModel)
            => new AppTimeViewModel()
            {
                AppName = dbModel.AppName,
                SecondsSpend = dbModel.Duration / 1000,
                Timestamp = new DateTime(
                    year: dbModel.Year,
                    month: dbModel.Month,
                    day: dbModel.Day,
                    hour: dbModel.Hour,
                    minute: 0,
                    second: 0)
            };

        public static List<AppTimeViewModel> ItemsToView(this List<AppTimeModel> list)
            => list.ConvertAll<AppTimeViewModel>(item => item.ToView());
    }
}

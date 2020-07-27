using System;
using System.Globalization;
using Humanizer;
using Humanizer.Localisation;

namespace Mitheti.Wpf.ViewModels
{
    public static class Extensions
    {
        internal static string LanguageCodeConfigKey = "Language:Code";

        internal static string HumanizeForStatistic(this TimeSpan timeSpan, string languageCode)
            => timeSpan.Humanize(
                precision: 2,
                countEmptyUnits: true,
                culture: new CultureInfo(languageCode),
                minUnit: TimeUnit.Second,
                maxUnit: TimeUnit.Day,
                toWords: false);
    }
}
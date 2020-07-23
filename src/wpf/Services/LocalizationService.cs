using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Mitheti.Wpf.Services
{
    //TODO:FIXME: rethink the way app localized;
    public class LocalizationService : ILocalizationService
    {
        public const string SectionKey = "localization";

        public string this[string key, string defaultValue] => Config.GetValue(key, defaultValue);

        public Dictionary<string, string> Data { get; }
        public IConfigurationSection Config { get; }

        public LocalizationService(IConfiguration config, ISettingsManager settingsManager)
        {
            Data = new Dictionary<string, string>();
            Config = config.GetSection(SectionKey);
            Data = settingsManager.GetConfigAsDictionary(config, SectionKey, new List<string>());
        }
    }
}
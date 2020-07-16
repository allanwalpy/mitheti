using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Mitheti.Wpf.Services
{
    //TODO:FIXME: rethink the way app localized;
    public class LocalizationService : ILocalizationService
    {
        private const string SectionKey = "localization";

        public string this[string key, string defaultValue] => Config.GetValue(key, defaultValue);

        public Dictionary<string, string> Data { get; }
        public IConfigurationSection Config { get; }

        public LocalizationService(IConfiguration config)
        {
            Data = new Dictionary<string, string>();
            Config = config.GetSection(SectionKey);
            PopulateData(Config);
        }

        //TODO:FIXME: find better solution;
        private void PopulateData(IConfigurationSection section)
        {
            List<IConfigurationSection> records = section.GetChildren().ToList();
            while (records.Count > 0)
            {
                var record = records.First();
                var children = record.GetChildren();

                records.AddRange(children);
                Data.Add(record.Path.Substring(SectionKey.Length + 1), record.Value ?? string.Empty);

                records.RemoveAt(0);
            }
        }
    }
}
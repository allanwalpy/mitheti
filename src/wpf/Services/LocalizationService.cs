using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Mitheti.Wpf.Services
{
    //TODO:FIXME: rethink the way app localized;
    public class LocalizationService : ILocalizationService
    {
        private const string SectionKey = "localization";

        public Dictionary<string, string> Data { get; }

        public LocalizationService(IConfiguration config)
        {
            Data = new Dictionary<string, string>();
            PopulateData(config.GetSection(SectionKey));
        }

        //TODO:FIXME: find better solution;
        private void PopulateData(IConfigurationSection section)
        {
            List<IConfigurationSection> records = section.GetChildren().ToList();
            while (records.Count != 0)
            {
                var record = records.First();
                var children = record.GetChildren();
                if (children.Any())
                {
                    records.AddRange(children);
                }

                if (record.Value != null)
                {
                    Data.Add(record.Path.Substring(SectionKey.Length + 1), record.Value);
                }

                records.RemoveAt(0);
            }
        }
    }
}

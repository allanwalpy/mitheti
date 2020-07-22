using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Mitheti.Core.Services;
using Newtonsoft.Json;

namespace Mitheti.Wpf.Services
{
    public class SettingsManager : ISettingsManager
    {
        public const string DefaultFilename = Extensions.ConfigFile;
        public const string LocalizationSectionKey = LocalizationService.SectionKey;
        public static readonly string[] DefaultIgnoreSections = {LocalizationSectionKey};

        public Dictionary<string, string> GetConfigSectionAsDictionary(IConfigurationSection config,
            string[] ignoreSections = null)
            => GetConfigAsDictionary(config, config.Path, ignoreSections);

        public Dictionary<string, string> GetConfigAsDictionary(IConfiguration config, string configKey = null,
            string[] ignoreSections = null)
        {
            var ignore = ignoreSections ?? DefaultIgnoreSections;
            var records = config.GetChildren().ToList();
            var result = new Dictionary<string, string>();

            for (; records.Count > 0; records.RemoveAt(0))
            {
                var record = records.First();
                var children = record.GetChildren();

                if (ignore.Contains(record.Path)) continue;
                records.AddRange(children);

                if (record.Value == null) continue;
                result.Add(GetKey(record, configKey), record.Value);
            }

            return result;
        }

        private static string GetKey(IConfigurationSection record, string sectionKey)
            => string.IsNullOrEmpty(sectionKey) ? record.Path : record.Path.Substring(sectionKey.Length + 1);

        public async Task SaveConfiguration(Dictionary<string, string> data, string filename = null)
            => await File.WriteAllTextAsync(filename ?? DefaultFilename,JsonConvert.SerializeObject(data));
    }
}
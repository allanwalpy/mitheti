using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace Mitheti.Wpf.Localization
{
    //? see https://github.com/dradovic/Embedded.Json.Localization/blob/master/src/Embedded.Json.Localization/ServicesCollectionExtensions.cs ;
    public class JsonStringLocalizer : IStringLocalizer
    {
        public const string LocalizationConfigFile = "localization.json";
        
        private readonly Lazy<Dictionary<string, string>> _dictionary;
        
        public JsonStringLocalizer()
        {
            _dictionary = new Lazy<Dictionary<string, string>>(ReadLocalizeFile);
            
        }

        private static Dictionary<string, string> ReadLocalizeFile()
            => JsonConvert.DeserializeObject<Dictionary<string, string>>(
                File.ReadAllText(LocalizationConfigFile));
        
        public LocalizedString this[string key]
        {
            get
            {
                var hasKey = _dictionary.Value.Keys.Contains(key);
                return new LocalizedString(
                    key,
                    (hasKey ? _dictionary.Value[key] : String.Empty),
                    !hasKey);
            }
        }

        public LocalizedString this[string key, params object[] args] => this[key];
        
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCulture)
            => _dictionary.Value.Select(item => new LocalizedString(item.Key, item.Value));
        
        public IStringLocalizer WithCulture(CultureInfo culture)
            => throw new NotSupportedException("Obsolete API");
    }
}
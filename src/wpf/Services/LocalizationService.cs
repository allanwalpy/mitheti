using System.Collections.Generic;
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
            Data = config.GetValue<Dictionary<string, string>>(SectionKey);
        }
    }
}
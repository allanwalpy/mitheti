using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Mitheti.Wpf.Services
{
    public interface ILocalizationService
    {
        string this[string key, string defaultValue = ""] { get; }
        
        Dictionary<string, string> Data { get; }
        
        IConfigurationSection Config { get; }
    }
}
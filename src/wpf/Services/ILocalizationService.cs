using System.Collections.Generic;

namespace Mitheti.Wpf.Services
{
    public interface ILocalizationService
    {
        string this[string key, string defaultValue = ""] { get; }
        
        Dictionary<string, string> Data { get; }
    }
}
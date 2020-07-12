using System.Collections.Generic;
using System.Drawing;

namespace Mitheti.Wpf.Services
{
    public interface ILocalizationService
    {
        string this[string key, string defaultValue = ""] { get; }
        
        Dictionary<string, string> Data { get; }
    }
}
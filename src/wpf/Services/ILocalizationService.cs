using System.Collections.Generic;

namespace Mitheti.Wpf.Services
{
    public interface ILocalizationService
    {
        Dictionary<string, string> Data { get; }
    }
}
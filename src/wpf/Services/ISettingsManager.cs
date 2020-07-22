using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Mitheti.Wpf.Services
{
    public interface ISettingsManager
    {
        Dictionary<string, string> GetConfigSectionAsDictionary(IConfigurationSection config,
            string[] ignoreSections = null);

        Dictionary<string, string> GetConfigAsDictionary(IConfiguration config,
            string configKey = null, string[] ignoreSections = null);

        Task SaveConfiguration(Dictionary<string, string> data, string filename = null);
    }
}
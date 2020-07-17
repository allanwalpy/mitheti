using System.Collections.Generic;
using Mitheti.Core.Services;
using Mitheti.Wpf.Services;

namespace Mitheti.Wpf.ViewModels
{
    public class AboutTabViewModel
    {
        public Dictionary<string, string> Localization { get; }
        public string Contributors { get; }

        public AboutTabViewModel(ILocalizationService localization)
        {
            Localization = localization.Data;

            // TODO: use github api?
            Contributors = string.Join(";\n",
                localization.Config.GetList("Window:About:Collaborators", new List<string>())) + ";";
        }
    }
}
using System.Collections.Generic;

namespace Mitheti.Wpf.Models
{
    public class ContributorModel
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string AvatarUrl { get; set; }
        public string GithubUrl { get; set; }
        public string PersonalUrl { get; set; }

        //TODO:FIXME: get to normal translation method, not these >_>;
        public Dictionary<string, string> Localization { get; set; }
    }
}
using System.Collections.Generic;

namespace Mitheti.Wpf.Models
{
    public class ContributorMode
    {
        public Dictionary<string, string> Localization { get; set; }

        public string Name { get; set; }
        public string GithubUrl { get; set; }
        public string PersonalUrl { get; set; }
        public string AvatarUrl { get; set; }
    }
}
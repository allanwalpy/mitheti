using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MaterialDesignThemes.Wpf;
using Mitheti.Wpf.Services;
using Octokit;

namespace Mitheti.Wpf.ViewModels
{
    public class ContributorViewModel
    {
        public Dictionary<string, string> Localization { get; set; }

        public string Name { get; set; }
        public string GithubUrl { get; set; }
        public string PersonalUrl { get; set; }
        public string AvatarUrl { get; set; }

        public ContributorViewModel(Dictionary<string, string> localization, string name, string githubUrl, string personalUrl, string avatarUrl)
        {
            Localization = localization;

            Name = name;
            GithubUrl = githubUrl;
            PersonalUrl = personalUrl;
            AvatarUrl = avatarUrl;
        }

    }
}
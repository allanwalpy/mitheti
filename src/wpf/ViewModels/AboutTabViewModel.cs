using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Mitheti.Core.Services;
using Mitheti.Wpf.Models;
using Octokit;
using Mitheti.Wpf.Services;

namespace Mitheti.Wpf.ViewModels
{
    public class AboutTabViewModel
    {
        private const string Token = "MjU4MjkxNmVkNDc1YzZmZWU4MTYxZTZmOTYwODMxNzY3NDdkM2I4Ng==";

        public Dictionary<string, string> Localization { get; }
        public string License { get; set; }

        public ObservableCollection<ContributorModel> Contributors { get; set; }

        public AboutTabViewModel(ILocalizationService localization)
        {
            Localization = localization.Data;

            License = string.Format(Localization["Window:About:License:Template"],
                Localization["Window:About:License:Main"],
                Localization["Window:About:License:Uses"],
                string.Join(Localization["Window:About:License:Attributes:Separator"], localization.Config.GetList<string>("Window:About:License:Attributes")));
            Contributors = new ObservableCollection<ContributorModel>();

            PopulateContributors().ConfigureAwait(false);
        }

        private async Task PopulateContributors()
        {
            var client = new GitHubClient(new ProductHeaderValue("mitheti"))
            {
                Credentials = new Credentials(Encoding.UTF8.GetString(Convert.FromBase64String(Token)))
            };

            var contributors = await client.Repository.GetAllContributors("allan-walpy", "mitheti");
            foreach (var item in contributors)
            {
                var user = await client.User.Get(item.Login);
                var profile = $"https://github.com/{user.Login}/";

                Contributors.Add(new ContributorModel
                {
                    Name = user.Name,
                    Login = user.Login,
                    AvatarUrl = user.AvatarUrl,
                    GithubUrl = profile,
                    PersonalUrl = user.Blog ?? profile,
                    //TODO:FIXME: >_> V_V <_<;
                    Localization = Localization
                });
            }
        }
    }
}
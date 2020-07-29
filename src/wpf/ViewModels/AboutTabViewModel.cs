using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Mitheti.Wpf.Models;
using Octokit;

namespace Mitheti.Wpf.ViewModels
{
    public class AboutTabViewModel
    {
        private const string Token = "MjU4MjkxNmVkNDc1YzZmZWU4MTYxZTZmOTYwODMxNzY3NDdkM2I4Ng==";
        public string License { get; set; }

        public ObservableCollection<ContributorModel> Contributors { get; set; }
        public ICommand HyperlinkClick => new HyperlinkCommand();

        public AboutTabViewModel()
        {

            License = string.Format("Window:About:License:Template".Translate(),
                "Window:About:License:Main".Translate(),
                "Window:About:License:Uses".Translate(),
                "Window:About:License:Attributes".Translate());
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
                    PersonalUrl = user.Blog ?? profile
                });
            }
        }
    }
}
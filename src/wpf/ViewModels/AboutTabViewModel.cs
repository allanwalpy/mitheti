using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MaterialDesignThemes.Wpf;
using Mitheti.Wpf.Services;
using Mitheti.Wpf.Views;
using Octokit;

namespace Mitheti.Wpf.ViewModels
{
    public class AboutTabViewModel
    {
        private const string Token = "MjU4MjkxNmVkNDc1YzZmZWU4MTYxZTZmOTYwODMxNzY3NDdkM2I4Ng==";

        private AboutTab _aboutTab;
        public Dictionary<string, string> Localization { get; }

        public AboutTabViewModel(ILocalizationService localization)
        {
            Localization = localization.Data;
        }

        public void Initialize(AboutTab aboutTab)
        {
            _aboutTab = aboutTab;

            PopulateContributors().ConfigureAwait(false);
        }
        private async Task PopulateContributors()
        {
            var client = new GitHubClient(new ProductHeaderValue("mitheti"))
            {
                Credentials = new Credentials(Encoding.UTF8.GetString(Convert.FromBase64String(Token)))
            };

            var contributors = await client.Repository.Collaborator.GetAll("allan-walpy", "mitheti");
            foreach (var item in contributors)
            {
                var user = await client.User.Get(item.Login);

                _aboutTab.Contributors.Children.Add(GetContributorCard(user));
            }
        }

        // TODO: use xaml instead of programatically created;
        private Card GetContributorCard(User user)
        {
            var card = new Card {Padding = new Thickness(32), Margin = new Thickness(16)};

            var mainPanel = new StackPanel {Orientation = Orientation.Horizontal};
            var image = new Image
            {
                Source = new BitmapImage(new Uri(user.AvatarUrl)),
                Width = 52,
                Height = 52,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            mainPanel.Children.Add(image);

            var infoPanel = new StackPanel {Orientation = Orientation.Vertical, Margin = new Thickness(8)};
            var nameText = new TextBlock
            {
                Style = _aboutTab.FindResource("MaterialDesignHeadline6TextBlock") as Style,
                Text = user.Name
            };
            var linkText = new TextBlock {Style = _aboutTab.FindResource("MaterialDesignTextBlock") as Style};
            var linkGithub = new Hyperlink {NavigateUri = new Uri($"https://github.com/{user.Login}")};
            var linkGithubText = new TextBlock
            {
                Style = _aboutTab.FindResource("MaterialDesignBody1Hyperlink") as Style,
                Text = Localization["Window:About:GithubLink"]
            };
            linkGithub.Inlines.Add(linkGithubText);
            var linkPersonal = new Hyperlink {NavigateUri = new Uri(user.Blog)};
            var linkPersonalText = new TextBlock
            {
                Style = _aboutTab.FindResource("MaterialDesignBody2Hyperlink") as Style,
                Text = Localization["Window:About:PersonalLink"]
            };
            linkPersonal.Inlines.Add(linkPersonalText);
            linkText.Inlines.Add(linkGithub);
            linkText.Inlines.Add(linkPersonal);
            infoPanel.Children.Add(nameText);
            infoPanel.Children.Add(linkText);
            mainPanel.Children.Add(infoPanel);

            card.Content = mainPanel;

            return card;
        }
    }
}
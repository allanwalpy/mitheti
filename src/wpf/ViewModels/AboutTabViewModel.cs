using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using MaterialDesignThemes.Wpf;
using Mitheti.Core.Services;
using Octokit;
using Mitheti.Wpf.Services;
using Mitheti.Wpf.Views;

namespace Mitheti.Wpf.ViewModels
{
    public class AboutTabViewModel
    {
        private const string Token = "MjU4MjkxNmVkNDc1YzZmZWU4MTYxZTZmOTYwODMxNzY3NDdkM2I4Ng==";

        private AboutTab _aboutTab;
        public Dictionary<string, string> Localization { get; }
        public string License { get; set; }

        public AboutTabViewModel(ILocalizationService localization)
        {
            Localization = localization.Data;

            var attributes = localization.Config.GetList<string>("Window:About:License:Attributes");
            License = string.Format(Localization["Window:About:License:Template"],
                Localization["Window:About:License:Main"],
                Localization["Window:About:License:Uses"],
                string.Join(Localization["Window:About:License:Attributes:Separator"], attributes));
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

            var contributors = await client.Repository.GetAllContributors("allan-walpy", "mitheti");
            foreach (var item in contributors)
            {
                var user = await client.User.Get(item.Login);

                _aboutTab.Contributors.Children.Add(GetContributorCard(user));
            }
        }

        // TODO: use xaml instead of programatically created;
        private Card GetContributorCard(Account user)
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
                Text = string.Format(Localization["Window:About:Contributor:Name"], user.Name, user.Login)
            };
            var linkText = new TextBlock {Style = _aboutTab.FindResource("MaterialDesignTextBlock") as Style};
            var linkGithub = new Hyperlink {NavigateUri = new Uri($"https://github.com/{user.Login}")};
            linkGithub.RequestNavigate += OpenInBrowser;
            var linkGithubText = new TextBlock
            {
                // TODO:FIXME: Style = _aboutTab.FindResource("MaterialDesignHeadline6Hyperlink") as Style,
                Text = Localization["Window:About:Contributor:GithubLink"]
            };
            linkGithub.Inlines.Add(linkGithubText);
            linkText.Inlines.Add(linkGithub);
            linkText.Inlines.Add(Localization["Window:About:Contributor:SeparatorLink"]);

            if (!string.IsNullOrEmpty(user.Blog))
            {
                var linkPersonal = new Hyperlink {NavigateUri = new Uri(user.Blog)};
                linkPersonal.RequestNavigate += OpenInBrowser;
                var linkPersonalText = new TextBlock
                {
                    //TODO:FIXME: Style = _aboutTab.FindResource("MaterialDesignHeadline6Hyperlink") as Style,
                    Text = Localization["Window:About:Contributor:PersonalLink"]
                };
                linkPersonal.Inlines.Add(linkPersonalText);
                linkText.Inlines.Add(linkPersonal);
            }

            infoPanel.Children.Add(nameText);
            infoPanel.Children.Add(linkText);
            mainPanel.Children.Add(infoPanel);

            card.Content = mainPanel;

            return card;
        }

        private static void OpenInBrowser(object sender, RequestNavigateEventArgs args)
        {
            Process.Start(new ProcessStartInfo(args.Uri.AbsolutePath));
            args.Handled = true;
        }
    }
}
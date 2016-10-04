using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Windows;

using MapRSS_LogicEngine;

namespace SFTD_project
{
    /// <summary>
    /// Interaction logic for CreateNewSubscribeWindow.xaml
    /// </summary>
    public partial class SubscriptionWindow : Window
    {
        private MapRSS main;
        private RSSFeed originalFeed;

        public SubscriptionWindow(MapRSS program, RSSFeed feed = null)
        {
            InitializeComponent();

            main = program;
            originalFeed = feed;

            List<MapRSSItem> chSource = new List<MapRSSItem>(program.Channels);
            chSource.Insert(0, new Channel("No Group Selected"));

            Groups.ItemsSource = chSource;
            Groups.SelectedIndex = 0;

            if (feed != null)
            {
                Title = "Modify a subscription";
                Confirm.Content = "Save Changes";
                inputAlias.Text = feed.Name;
                inputURL.Text = feed.URL;
                UpdatePeriod.Text = feed.UpdatePeriod.ToString();

                if (feed.Parent != null) { Groups.SelectedItem = feed.Parent; }
            }
        }

        //Confirm button
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (!InternetAvailability.IsAvailable())
            {
                MessageBox.Show("Error! No internet connection");
                return;
            }

            SyndicationFeed feed = null;
            if (!RSSFeed.TryParseFeed(inputURL.Text, out feed))
            {
                MessageBox.Show("Error! Invalid feed URL");
                return;
            }

            int update = 0;
            int.TryParse(UpdatePeriod.Text, out update);
            if (update <= 0)
            {
                MessageBox.Show("Error! Invalid update period");
                return;
            }

            // determine channel
            Channel parent = null;
            if (Groups.SelectedIndex > 0)
            {
                parent = Groups.SelectedItem as Channel;
            }

            // begin creation/modification
            if (originalFeed == null)
            {
                main.CreateFeed(inputAlias.Text, feed.Title.Text, inputURL.Text, update, parent);
            }
            else
            {
                main.ModifyFeed(originalFeed, inputAlias.Text, feed.Title.Text, inputURL.Text, update, parent);
            }
            Close();
        }
    }
}

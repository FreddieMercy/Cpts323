/// <summary>
/// This is a Team Project contributed by the Team S.F.T.D for the class Cpts 323 in Washignton State University
/// The contributers are:
///     Stephen Goeppele-Parrish
///     Junhao Zhang "Freddie"
///     Ching-Yen "Tim" Lin
///     Dustin Crossman
///
/// Copyrights since 2015, all rights reserved.
/// </summary>
using System;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ServiceModel.Syndication;
using System.Xml;

namespace MapRSS_LogicEngine
{

    public partial class MapRSS
    {
        // EVENT HANDLERS

        private void OnFeedPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged(e.PropertyName);
        }

        private void UpdateFeed(object sender, EventArgs e)
        {
            if (!InternetAvailability.IsAvailable()) { return; }

            ParseFeed(sender as RSSFeed);
            NotifyPropertyChanged("Updated");
        }

        // CORE METHODS

        /// <summary>
        /// 
        /// </summary>
        public void CreateFeed(string alias, string alt, 
                string address, int update, Channel channel = null)
        {
            string name = CreateFeedName(alias, alt);
            RSSFeed feed = new RSSFeed(name, address, update);

            m_feeds.Add(feed);
            m_root.Add(feed);
            MoveItem(feed, channel);

            ParseFeed(feed);

            feed.PropertyChanged += OnFeedPropertyChanged;
            feed.FeedUpdateTick += UpdateFeed;
            NotifyPropertyChanged("Feed");
        }

        /// <summary>
        /// 
        /// </summary>
        public void ModifyFeed(RSSFeed feed, string alias, string alt, 
                string address, int update, Channel channel = null)
        {
            if (alias != feed.Name) { feed.Name = CreateFeedName(alias, alt); }
            if (address != feed.URL) { feed.URL = address; }
            if (update != feed.UpdatePeriod) { feed.UpdatePeriod = update; }
            if (channel != feed.Parent) { MoveItem(feed, channel); }
        }

        /// <summary>
        /// Completely removes the parameter feed from the MapRSS instance.
        /// </summary>
        public void DeleteFeed(RSSFeed feed)
        {
            MoveItem(feed, null);
            m_root.Remove(feed);
            m_feeds.Remove(feed);

            feed.PropertyChanged -= OnFeedPropertyChanged;
            feed.FeedUpdateTick -= UpdateFeed;
            NotifyPropertyChanged("Feed");

            if(feed.Hasloc)
            {
                feed.raiseEventWhenDeleted();
            }
        }

        /// <summary>
        /// Returns an RSSFeed with the given alias from the list of all
        /// RSSFeeds. If no such RSSFeed exists, null is returned.
        /// </summary>
        public RSSFeed GetFeed(string alias)
        {
            return GetItem(alias, m_feeds) as RSSFeed;
        }

        // UTILITY METHODS

        /// <summary>
        /// Returns a name that does not conflict with other feed names based on the
        /// parameter input. If the input is an empty string or whitespace, then the
        /// returned name will based on the parameter basename.
        /// </summary>
        private string CreateFeedName(string input, string basename)
        {
            return CreateItemName(input, basename, m_feeds);
        }

        private void ParseFeed(RSSFeed feed)
        {
            XmlReader reader = XmlReader.Create(feed.URL);
            SyndicationFeed synFeed = SyndicationFeed.Load(reader);
            reader.Close();

            ObservableCollection<Article> list = new ObservableCollection<Article>();
            foreach (SyndicationItem item in synFeed.Items)
            {
                Article feedItem = new Article()
                {
                    Title = item.Title.Text.Trim(),
                    PublishDate = item.PublishDate.ToString("f").Trim(),
                    Link = item.Links[0].Uri.ToString(),
                    Summary = item.Summary != null ? Regex.Replace(item.Summary.Text, "<.+?>", string.Empty).Trim() : string.Empty,
                    HasRead = false,
                    IsSelected = false,
                    target = this.target
                };


                findTopics += (sender, e) => feedItem.addMySelf(sender, e, DateA, DateB, Topic_HideRead);

                feedItem.setArticle(all_state, uslocation, onMapArticles, feed);
                feedItem.PropertyChanged += onMapArticlesChanged;
                list.Add(feedItem);
            }
            feed.Articles = list;
        }
    }
}

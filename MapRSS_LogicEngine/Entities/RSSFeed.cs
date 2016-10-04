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
using System.ComponentModel;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.ServiceModel.Syndication;
using System.Windows.Threading;
using System.Xml;

namespace MapRSS_LogicEngine
{
    public class RSSFeed : MapRSSItem, INotifyPropertyChanged
    {
        // FIELDS

        private int m_update; // update period in minutes
        private string m_address;
        private ObservableCollection<Article> m_items;
        private DispatcherTimer m_timer;
        private bool hasLoc; //For map
        // EVENTS

        public event EventHandler FeedUpdateTick;
        public event PropertyChangedEventHandler PropertyChanged; //for map, delete its article from Pushpins

        // CONSTRUCTORS

        internal RSSFeed(string alias, string address, int update)
        {
            m_name = alias;
            m_address = address;
            m_parent = null;
            m_update = update;
            m_items = null;

            m_timer = new DispatcherTimer();
            m_timer.Tick += Timer_Tick;
            UpdateTimer();
            m_timer.Start();

            hasLoc = false;
        }        

        // METHODS

        /// <summary>
        /// Attempts to parse a SyndicationFeed from the parameter url.
        /// A return value indicates whether the parsing suceeded.
        /// </summary>
        public static bool TryParseFeed(string url, out SyndicationFeed result)
        {
            result = null;
            try
            {
                SyndicationFeed feed = SyndicationFeed.Load(XmlReader.Create(url));

                foreach (SyndicationItem item in feed.Items)
                {
                    Debug.Print(item.Title.Text);
                }

                feed.BaseUri = new Uri(url);
                result = feed;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // // EVENT HANDLERS

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (FeedUpdateTick != null)
            {
                FeedUpdateTick(this, e);
            }
        }

        // // UTILITY METHODS

        private void UpdateTimer()
        {
            m_timer.Interval = new TimeSpan(0, m_update, 0);
        }

        // PROPERTIES

        public int UpdatePeriod
        {
            get { return m_update; }
            set
            {
                m_update = value;
                UpdateTimer();
            }
        }

        public string URL
        {
            get { return m_address; }
            set { m_address = value; }
        }

        public ObservableCollection<Article> Articles
        {
            get { return m_items; }
            set { m_items = value; }
        }

        public ObservableCollection<Article> ArticlesHasLoc
        {
            get
            {
                //var temp = m_items.Select(pts => pts.HasLoc == true);
                //return (ObservableCollection<Article>)temp;
                //return (ObservableCollection<Article>)m_items.Select(pts => pts.HasLoc == true);
                ObservableCollection<Article> tmp = new ObservableCollection<Article>();
                foreach(Article x in m_items)
                {
                    if(x.HasLoc)
                    {
                        tmp.Add(x);
                    }
                }

                return tmp;
            }
        }


        //For map

        public bool Hasloc
        {
            set
            {
                hasLoc = value;
            }

            get
            {
                return hasLoc;
            }
        }

        public void raiseEventWhenDeleted()
        {
            PropertyChanged(this, new PropertyChangedEventArgs("Delted"));
        }
    }

    public class FakeRSSFeed
    {
        public string Name { get; set; }
        public ObservableCollection<Article> Articles { get; set; }
    }

}

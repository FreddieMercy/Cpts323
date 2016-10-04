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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;

namespace MapRSS_LogicEngine
{
    public partial class MapRSS
    {
        // FIELDS

        private Favorites favs;

        private ObservableCollection<MapRSSItem> m_feeds;
        private ObservableCollection<MapRSSItem> m_channels;
        private ObservableCollection<MapRSSItem> m_root;

        private DateTime firstDate;
        private DateTime secondDate;

            //It has those in TopicMethods:
                //private Topic_Folder keyWord = new Topic_Folder();
                //private Topic_Keyword txtBxNull = null;
                //private CheckBox Topic_HideRead;
        // EVENTS

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangedEventHandler findTopics;
            //For Database

                //It has those in MapMethods:
                //private Dictionary<string, string> all_state = new Dictionary<string, string> { };
                //private Dictionary<string, List<Tuple<string, Double, Double>>> uslocation = new Dictionary<string, List<Tuple<string, Double, Double>>>();
                //private ObservableCollection<Pushpin> onMapArticles = new ObservableCollection<Pushpin>();
                //private List<Tuple<string, Double, Double>> _list_city = new List<Tuple<string, double, double>>();
        // CONSTRUCTORS

        public MapRSS(CheckBox topic_HideRead)
        {
            favs = new Favorites();
            Topic_HideRead = topic_HideRead;
            m_feeds = new ObservableCollection<MapRSSItem>();
            m_channels = new ObservableCollection<MapRSSItem>();
            m_root = new ObservableCollection<MapRSSItem>();
            m_root.Add(favs);
            firstDate = DateTime.MinValue;
            secondDate = DateTime.MaxValue;
         
            //In MapMethods:
            readData();
            //foreach (string x in uslocation.Keys)
            //{
            //    foreach (Tuple<string, Double, Double> y in uslocation[x])
            //    {
            //        _list_city.Add(y);
            //    }

            //}

        }

        // METHODS

        // // EVENT HANDLERS

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        // PROPERTIES

        public ObservableCollection<MapRSSItem> Feeds
        {
            get { return m_feeds; }
        }

        public ObservableCollection<MapRSSItem> Channels
        {
            get { return m_channels; }
        }

        public ObservableCollection<MapRSSItem> Root
        {
            get { return m_root; }
        }

        public Favorites Favorites
        {
            get { return m_root[0] as Favorites; }
        }

        public DateTime DateA
        {
            get { return firstDate; }
            set { firstDate = value; }
        }

        public DateTime DateB
        {
            get { return secondDate; }
            set { secondDate = value; }
        }

    }
}

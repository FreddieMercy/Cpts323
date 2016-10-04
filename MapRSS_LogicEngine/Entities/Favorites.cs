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
using System.Xml.Serialization;
using System.IO;

namespace MapRSS_LogicEngine
{
    public class Favorites : MapRSSItem
    {
        private ObservableCollection<Article> m_items;

        public Favorites() 
        {
            m_items = OpenFav();
           
            m_name = "Favorites";
            m_parent = null;
        }

        public void Add(Article _article)
        {
            if(!Contains(_article))
            {
                m_items.Add(_article);
            }
        }

        public void Remove(Article _article)
        {
            foreach(Article a in m_items)
            {
                if (a == _article)
                {
                    m_items.Remove(_article);
                    break;
                }
            }
        }

        public bool Contains(Article _article)
        {
            foreach(Article a in m_items)
            {
                if (a == _article)
                    return true;
            }

            return false;
        }

        public ObservableCollection<Article> Items
        {
            get
            {
                return m_items;
            }
        }

        public ObservableCollection<Article> ItemsSource
        {
            set
            {
                m_items = value;
            }
        }

        public ObservableCollection<Article> OpenFav()
        {
            ObservableCollection<Article> keys = new ObservableCollection<Article>();

            try
            {
                using (StreamReader reader = new StreamReader("../../XML/Favorite.xml"))
                {
                    XmlSerializer seri = new XmlSerializer(typeof(ObservableCollection<Article>));
                    keys = (seri.Deserialize(reader) as ObservableCollection<Article>);
                }

            }

            catch (Exception)
            {

            }

            return keys;
        }
    }
}

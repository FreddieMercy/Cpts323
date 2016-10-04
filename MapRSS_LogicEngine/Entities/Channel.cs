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
using System.Collections.ObjectModel;

namespace MapRSS_LogicEngine
{
    public class Channel : MapRSSItem
    {
        // FIELDS

        private ObservableCollection<MapRSSItem> m_items;

        // CONSTRUCTORS

        public Channel(string name)
        {
            m_name = name;
            m_parent = null;
            m_items = new ObservableCollection<MapRSSItem>();
        }

        // PROPERTIES

        public ObservableCollection<MapRSSItem> Items
        {
            get { return m_items; }
        }
    }
}

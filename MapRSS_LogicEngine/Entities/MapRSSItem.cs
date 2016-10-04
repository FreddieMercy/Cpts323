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
using System.ComponentModel;

namespace MapRSS_LogicEngine
{
    abstract public class MapRSSItem : INotifyPropertyChanged
    {
        // FIELDS

        protected string m_name;
        protected Channel m_parent;

        // EVENTS

        public event PropertyChangedEventHandler PropertyChanged;

        //METHODS

        protected void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        // PROPERTIES

        public string Name
        {
            get { return m_name; }
            set
            {
                m_name = value;
                NotifyPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Gets or sets the parent of the item. 
        /// Automatically moves itself in and out of the parent's collections.
        /// </summary>
        public Channel Parent
        {
            get { return m_parent; }
            set { m_parent = value; }
        }
    }
}

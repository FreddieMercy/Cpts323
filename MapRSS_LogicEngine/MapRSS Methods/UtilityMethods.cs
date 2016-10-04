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
    public partial class MapRSS
    {
        private void MoveItem(MapRSSItem item, Channel channel)
        {
            if (item.Parent == channel) { return; }

            if (item.Parent == null) { m_root.Remove(item); }
            else { item.Parent.Items.Remove(item); }

            if (channel == null) { m_root.Add(item); }
            else { channel.Items.Add(item); }

            item.Parent = channel;
        }

        private MapRSSItem GetItem(string name, ObservableCollection<MapRSSItem> list)
        {
            foreach (MapRSSItem item in list)
            {
                if (item.Name == name) { return item; }
            }
            return null;
        }

        /// <summary>
        /// Returns a name that does not conflict with those contained in the parameter list.
        /// If the parameter input is and empty string or whitespace, then the returned name 
        /// will be based on the parameter emptyname.
        /// </summary>
        private string CreateItemName(string input, string emptyname, ObservableCollection<MapRSSItem> list)
        {
            string basename = input;
            if (string.IsNullOrWhiteSpace(input))
            {
                basename = emptyname.Trim();
            }

            string name = basename;
            int counter = 1;
            while (ContainsItemName(name, list))
            {
                name = basename + " (" + counter++ + ")";
            }
            return name;
        }

        private bool ContainsItemName(string name, ObservableCollection<MapRSSItem> list)
        {
            foreach (MapRSSItem item in list)
            {
                if (item.Name == name) { return true; }
            }
            return false;
        }
    }
}

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
namespace MapRSS_LogicEngine
{

    public partial class MapRSS
    {
        // CORE METHODS

        public void CreateChannel(string name, Channel group = null)
        {
            string newName = CreateChannelName(name, "Untitled Group");

            Channel channel = new Channel(newName);

            m_channels.Add(channel);
            m_root.Add(channel);
            MoveItem(channel, group);
        }

        public void ModifyChannel(Channel channel, string name, Channel group = null)
        {
            if (name != channel.Name)
            {
                channel.Name = CreateChannelName(name, "Untitled Group");
            }
            if (group != channel.Parent) { MoveItem(channel, group); }
        }

        public void DeleteChannel(Channel channel)
        {
            while (channel.Items.Count > 0)
            {
                MoveItem(channel.Items[0], channel.Parent);
            }

            MoveItem(channel, null);
            m_root.Remove(channel);
            m_channels.Remove(channel);
        }

        public Channel GetChannel(string name)
        {
            return GetItem(name, m_channels) as Channel;
        }

        // UTILITY METHODS

        /// <summary>
        /// Returns a name that does not conflict with other channel names based on the
        /// parameter input. If the input is an empty string or whitespace, then the
        /// returned name will based on the parameter basename.
        /// </summary>
        private string CreateChannelName(string input, string basename)
        {
            return CreateItemName(input, basename, m_channels);
        }
    }
}

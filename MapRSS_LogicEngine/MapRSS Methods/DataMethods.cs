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
using System.Text;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Windows.Forms;

namespace MapRSS_LogicEngine
{
    public partial class MapRSS
    {
        public void LoadData()
        {
            XmlReader Reader;
            try
            {
                Reader = XmlReader.Create("../../XML/MapRSSdata.xml");
            }
            catch
            {
                //file is no exist
                return;
            }
            //load
            while (Reader.Read())
            {
                //look for root
                if (Reader.MoveToContent() == XmlNodeType.Element && Reader.Name == "root")
                {
                    while (Reader.Read() && Reader.Name != "channels")
                    {
                        if (Reader.Name == "name")
                        {
                            string name = Reader.ReadElementContentAsString();
                            CreateChannel(name);
                        }
                    }
                }
                // look for channel
                else if (Reader.MoveToContent() == XmlNodeType.Element && Reader.Name == "channel")
                {

                    while (Reader.Read() && Reader.Name != "feeds")
                    {
                        if (Reader.Name == "root_name")
                        {
                            string root = Reader.ReadElementContentAsString();
                            var channel = GetChannel(root);
                            Reader.Read();
                            string name = Reader.ReadElementContentAsString();
                            CreateChannel(name, channel);
                        }
                    }
                }
                //look for feed
                else if (Reader.MoveToContent() == XmlNodeType.Element && Reader.Name == "feed")
                {
                    string name = Reader.GetAttribute("name");
                    Reader.Read();
                    Reader.Read();
                    string channel_name = Reader.ReadElementContentAsString();
                    var channel = GetChannel(channel_name);
                    Reader.Read();
                    string URL = Reader.ReadElementContentAsString();
                    Reader.Read();
                    int update = Convert.ToInt32(Reader.ReadElementContentAsString());
                    AutoCreateFeed(name, URL, update, channel);
                    Reader.Read();

                }
            }
            Reader.Dispose();
        }

        //save 
        public void SaveData()
        {
            XmlWriter writer = null;
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;
            settings.IndentChars = ("\t");
            settings.NewLineOnAttributes = false;
            // make the XmlWriter
            writer = XmlWriter.Create("../../XML/MapRSSdata.xml", settings);
            writer.WriteStartElement("MapRSS");
            writer.WriteStartElement("root");
            //save roots
            foreach (var item in m_root)
            {
                //only save channel not the feed
                if (item is Channel)
                {
                    writer.WriteElementString("name", item.Name);
                }

            }
            writer.WriteEndElement();

            //save channels
            writer.WriteStartElement("channels");
            foreach (var item in m_channels)
            {
                //only save channel which have root
                if (item.Parent != null)
                {
                    writer.WriteStartElement("channel");
                    writer.WriteElementString("root_name", item.Parent.Name);
                    writer.WriteElementString("name", item.Name);
                    writer.WriteEndElement();
                }
            }
            writer.WriteEndElement();

            //save feeds
            writer.WriteStartElement("feeds");
            foreach (RSSFeed item in m_feeds)
            {
                writer.WriteStartElement("feed");
                writer.WriteAttributeString("name", item.Name);

                //check the feed's channel because it may be null
                if (item.Parent != null)
                {
                    writer.WriteElementString("channel_name", item.Parent.Name);
                }
                else
                {
                    writer.WriteElementString("channel_name", "null");
                }
                writer.WriteElementString("URL", item.URL);
                writer.WriteElementString("update", item.UpdatePeriod.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.Dispose();
        }

        //create the feed
        private void AutoCreateFeed(string title, string url, int update, Channel parent = null)
        {
            try
            {
                SyndicationFeed feed = SyndicationFeed.Load(XmlReader.Create(url));
                feed.BaseUri = new Uri(url);
                CreateFeed(title, feed.Title.Text, url, update, parent);
            }
            catch (Exception)
            {
                MessageBox.Show("The feed \"" + title + "\" cannot be parsed, ignored.\n[error code: ]");
            }
        }

    }
}


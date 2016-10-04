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
    public partial class MapRSS
    {
        public void AddFavorite(Article _article)
        {
            favs.Add(_article);
        }

        public void RemoveFavorite(Article _article)
        {
            favs.Remove(_article);
        }

        public void SaveFavHandler(object sender, EventArgs e)
        {
            using (TextWriter _writer = new StreamWriter("../../XML/Favorite.xml"))
            {
                XmlSerializer seri = new XmlSerializer(typeof(ObservableCollection<Article>));
                seri.Serialize(_writer, favs.Items);
            }
        }
    }
}
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Maps.MapControl.WPF;

namespace MapRSS_LogicEngine
{
    public partial class MapRSS
    {

        private Dictionary<string, string> all_state = new Dictionary<string, string> { };
        private Dictionary<string, List<Tuple<string, Double, Double>>> uslocation = new Dictionary<string, List<Tuple<string, Double, Double>>>();
        private ObservableCollection<Pushpin> onMapArticles = new ObservableCollection<Pushpin>();
        //private List<Tuple<string, Double, Double>> _list_city = new List<Tuple<string, double, double>>();
        public ObservableCollection<FakeRSSFeed> getonMapFeeds
        {
            get
            {
                ObservableCollection<FakeRSSFeed> tmp = new ObservableCollection<FakeRSSFeed>();
                foreach (RSSFeed x in m_feeds)
                {
                    if (x.Hasloc)
                    {
                        tmp.Add(new FakeRSSFeed(){Articles = x.ArticlesHasLoc, Name = x.Name});
                    }
                }

                return tmp;
            }
            
        }
        public ObservableCollection<Pushpin> getOnMapArticles
        {   get
            {
                return onMapArticles;
            }
        }

        private void readData()
        {
            //readData()
            using (TextFieldParser data = new TextFieldParser("../../XML/US_Location/uslocations.csv"))
            {
                data.TextFieldType = FieldType.Delimited;
                data.SetDelimiters(",");
                data.ReadFields();
                //read databass and make dictionary with state for key and tuple of city, latitude and longitude for value
                while (!data.EndOfData)
                {
                    var array = data.ReadFields();
                    var state = array[2];
                    var city = array[3];
                    var latitude = Convert.ToDouble(array[5]);
                    var longitude = Convert.ToDouble(array[6]);
                    if (!uslocation.ContainsKey(state))
                    {
                        uslocation.Add(state, new List<Tuple<string, Double, Double>> { new Tuple<string, Double, Double>(city, latitude, longitude) });
                    }
                    else
                    {
                        uslocation[state].Add(new Tuple<string, Double, Double>(city, latitude, longitude));
                    }
                }
                //data.Dispose();
            }
            //also make the state to the dictionary for easily finding abbreviation
            using (TextFieldParser data = new TextFieldParser("../../XML/US_Location/usstate.txt"))
            {
                data.TextFieldType = FieldType.Delimited;
                data.SetDelimiters(" ");
                while (!data.EndOfData)
                {
                    var array = data.ReadFields();

                    string abbreviation = array[0];
                    string state_name = null;
                    if (array.Length > 2)
                    {
                        for (int i = 1; i < array.Length; i++)
                        {
                            if (i != array.Length - 1)
                            {
                                state_name += array[i] + " ";
                            }
                            else
                            {
                                state_name += array[i];
                            }
                        }
                    }
                    else
                    {
                        state_name = array[1];
                    }
                    all_state.Add(state_name, abbreviation);
                    all_state.Add(abbreviation, abbreviation);

                }
                //data.Dispose();
            }
        }

        public void onMapArticlesChanged(object sender, PropertyChangedEventArgs e)
        {
            Article tmp = sender as Article;
            ObservableCollection<Article> parent = (tmp.Location).DataContext as ObservableCollection<Article>;
            if(parent.Count == 0)
            {
                onMapArticles.Remove(tmp.Location);

                //return;
            }

            //PropertyChanged(this, new PropertyChangedEventArgs("updMap"));
        }
    }
}

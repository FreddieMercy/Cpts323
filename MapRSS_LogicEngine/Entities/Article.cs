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
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maps.MapControl.WPF;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.ComponentModel;

namespace MapRSS_LogicEngine
{

    public class Article : IEquatable<Article>, INotifyPropertyChanged
    {
        public string Title { get; set; }
        public string PublishDate { get; set; }
        public string Link { get; set; }
        public string Summary { get; set; }
        public bool HasRead { get; set; }
        public bool IsSelected { get; set; }
        public bool IsFavorite { get; set; }

        private bool hasLoc;

        private Pushpin location;
        public int Hit { get; set; }
        public ObservableCollection<Article> target {get; set;}

        public event PropertyChangedEventHandler PropertyChanged;

        public Article()
        {
            hasLoc = false;
            location = null;
            Hit = 0;
        }

        public void deletedFromPin(object sender, EventArgs e)
        {
            ObservableCollection<Article> tmp = location.DataContext as ObservableCollection<Article>;
            tmp.Remove(this);
            if (tmp.Count == 0)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("ArticleDeleted"));
            }
        }

        public void addToPin(ObservableCollection<Pushpin> onMapArticles)
        {
            PushpinCompare cmpr = new PushpinCompare();
            if (onMapArticles.Contains(location, cmpr))
            {
                try
                {
                    Pushpin tmp = onMapArticles.Single(pts => pts.Location == location.Location);
                    (tmp.DataContext as ObservableCollection<Article>).Add(this);
                    location = tmp;
                }
                catch (Exception)
                {
                    //MessageBox.Show("Catch Exception at \"onMapArticles.Single(pts => pts.Location == location.Location);\" in Article.cs");
                }

                return;
            }


            location.DataContext = new ObservableCollection<Article>();
            (location.DataContext as ObservableCollection<Article>).Add(this);
            location.Content = new Button() { Opacity = 0, DataContext = location, Width = location.Width, Height = location.Height };

            onMapArticles.Add(location);
        }

        public void setArticle(Dictionary<string, string> _all_state, Dictionary<string, List<Tuple<string, Double, Double>>> _uslocation, ObservableCollection<Pushpin> onMapArticles, RSSFeed parent)
        {

            Tuple<Double, Double> loc = searchString(Title, _all_state, _uslocation);

            if (loc == null)
            {
                loc = searchString(Summary, _all_state, _uslocation);
            }

            if (loc == null)
            {
                return;
            }
            hasLoc = true;
            parent.Hasloc = true;
            parent.PropertyChanged += deletedFromPin;
            location = new Pushpin() { Location = new Location() { Latitude = loc.Item1, Longitude = loc.Item2 } };

            addToPin(onMapArticles);

        }


        public bool HasLoc
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

        public Pushpin Location
        {
            get
            {
                return location;
            }
        }

        // Location methods
        private Tuple<double, double> searchString(string text, Dictionary<string, string> all_state, Dictionary<string, List<Tuple<string, double, double>>> uslocation)
        {
            var temp1 = text;//.ToUpper();
            string pattern = @"[\p{P}]";
            Regex rgx = new Regex(pattern);
            string tokenize = rgx.Replace(temp1, " ");
            string[] stringSeparators = new string[] { " " };
            //skip all punctuation
            var parse = tokenize.Split(stringSeparators, StringSplitOptions.None);
            //reverse the string array for loop from the end
            Array.Reverse(parse);
            string state = null;
            var city_list = new List<Tuple<string, double, double>>();
            var target_list = new List<String>();
            
            //loop all the word and fin the word is Uppercase
            foreach (string word in parse)
            {
                var target = word;
                if (target != "" && Char.IsUpper(word[0]))
                {
                    if (target.Length > 2)
                    {
                        string temp = target.ToLower();
                        target = target[0].ToString() + temp.Substring(1);


                    }
                    target_list.Add(target);
                    //find the state name
                    if (state == null && all_state.ContainsKey(target))
                    {
                        state = all_state[target];
                        city_list = uslocation[state];
                    }

                }
            }
            //find the city 
            foreach (var word in target_list)
            {
                //if (state != null)
                //{
                    var list_city = from citys in city_list
                                    where (word == citys.Item1)
                                    select new { citys.Item2, citys.Item3 };
                    foreach (var city1 in list_city)
                    {
                        return new Tuple<double, double>(city1.Item2, city1.Item3);
                    }
                //}
                //else
                //{
                //    var list_city = from citys in _list_city
                //                    where (word == citys.Item1)
                //                    select new { citys.Item2, citys.Item3 };
                //    foreach (var city1 in list_city)
                //    {
                //        return new Tuple<double, double>(city1.Item2, city1.Item3);
                //    }
                //}
            }


            if (state != null)
            {
                return (new Tuple<double, double>(city_list[0].Item2, city_list[0].Item3));
            }
            return null;


        }


        // METHODS (for IEquatable)

        public override string ToString()
        {
            return Title + ", " + PublishDate;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }
            Article objAsItem = obj as Article;
            return (objAsItem == null) ? false : Equals(objAsItem);
        }

        public override int GetHashCode()
        {
            return Link.GetHashCode();
        }

        public bool Equals(Article other)
        {
            if (other == null) { return false; }
            return (this.Link.Equals(other.Link));
        }

        public static bool operator ==(Article x, Article y)
        {
            if (ReferenceEquals(x, y)) { return true; }

            if ((x as object == null) || (y as object == null)) { return false; }

            return x.Link == y.Link;
        }

        public static bool operator !=(Article x, Article y)
        {
            return !(x.Link == y.Link);
        }

        public void addMySelf(object sender, PropertyChangedEventArgs e, DateTime dateA, DateTime dateB, CheckBox Topic_HideRead)
        {
            if((Title.Contains(e.PropertyName) | Summary.Contains(e.PropertyName)) && !target.Contains(this))
            {
                //target.Add(this);
                DateTime itemDate = DateTime.MinValue;
                DateTime.TryParse(PublishDate, out itemDate);
                bool withinRange = (dateA <= itemDate) && (dateB >= itemDate);
                if ((!(bool)Topic_HideRead.IsChecked || !HasRead) && withinRange)
                {
                    target.Add(this);
                }
                else // otherwise do not display and uncheck checkbox
                {
                    IsSelected = false;
                }
            }

            Hit++;
        }
    }

    public class RSSFeedItemEqualityComparer : IEqualityComparer<Article>
    {
        public bool Equals(Article x, Article y)
        {
            return (x == y);
        }

        public int GetHashCode(Article item)
        {
            return item.GetHashCode();
        }
    }

    internal class PushpinCompare : IEqualityComparer<Pushpin>
    {
        public bool Equals(Pushpin x, Pushpin y)
        {

            if (ReferenceEquals(x, y)) return true;

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                return false;

            return x.Location == y.Location;
        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(Pushpin p)
        {
            return p.GetHashCode();
        }

    }
}

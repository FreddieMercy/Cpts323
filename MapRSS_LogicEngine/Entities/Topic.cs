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
using System.Xml;
using System.Xml.Serialization;

namespace MapRSS_LogicEngine
{
    public class Keyword
    {
        public string Text { get; set; }
        public Topic_Keyword Parent { get; set; }

    }

    public class Topic_Keyword : INotifyPropertyChanged
    {
        private ObservableCollection<Keyword> keyWords;
        private string date;
        public event PropertyChangedEventHandler PropertyChanged;

        private void emptyHandler(object sender, EventArgs e)
        {
            if ((sender as ObservableCollection<Keyword>).Count == 0)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Empty"));
            }
        }

        public Topic_Keyword()
        {
            keyWords = new ObservableCollection<Keyword>();
            keyWords.CollectionChanged += emptyHandler;
            date = DateTime.Today.ToShortDateString();
        }

        public Topic_Keyword(string _date, ObservableCollection<string> _key)
        {
            keyWords = new ObservableCollection<Keyword>();
            foreach (string x in _key)
            {
                keyWords.Add(new Keyword() { Text = x, Parent = this });
            }
            keyWords.CollectionChanged += emptyHandler;
            date = _date;
        }

        public Topic_Keyword(string _date)
        {
            keyWords = new ObservableCollection<Keyword>();
            keyWords.CollectionChanged += emptyHandler;
            date = _date;
        }

        public void Add(string s)
        {
            keyWords.Add(new Keyword() { Text = s, Parent = this });
        }

        public string Date
        {
            get { return date; }
        }

        public ObservableCollection<Keyword> Items
        {
            get { return keyWords; }
        }

        public ObservableCollection<string> ItemsToString()
        {
            ObservableCollection<string> Items = new ObservableCollection<string>();
            foreach (Keyword x in keyWords)
            {
                Items.Add(x.Text);
            }
            return Items;
        }

        public bool Contains(string s)
        {

            foreach (Keyword x in keyWords)
            {
                if (x.Text == s)
                {
                    return true;
                }
            }

            return false;

        }
    }

    public class Topic_Folder : INotifyPropertyChanged
    {
        private ObservableCollection<Topic_Keyword> keyWords;
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<string> topic = new ObservableCollection<string>();
        private void changedKeyword(object sender, EventArgs e)
        {
            PropertyChanged(this, new PropertyChangedEventArgs("Changed"));
        }
        public Topic_Folder()
        {
            keyWords = new ObservableCollection<Topic_Keyword>();
        }

        public Topic_Folder(ObservableCollection<Topic_Keyword> _key)
        {
            keyWords = _key;

            foreach (Topic_Keyword x in _key)
            {
                x.PropertyChanged += DeleteEmpty;
                x.Items.CollectionChanged += changedKeyword;
            }
        }

        private void DeleteEmpty(object sender, EventArgs e)
        {
            Topic_Keyword tmp = sender as Topic_Keyword;
            tmp.Items.CollectionChanged -= changedKeyword;
            keyWords.Remove(tmp);
            topic.Remove(tmp.Date);
        }

        public ObservableCollection<string> Topic
        {
            get
            {
                return topic;
            }
        }

        //public void Add(string s, ComboBoxItem _date = null)
        //{
        //    string date = DateTime.Today.ToShortDateString();
        //    if (_date != null)
        //    {
        //        date = _date.Content.ToString();
        //    }

        //    //if (!Contains(s))
        //    //{

        //        if (!String.IsNullOrEmpty(date) && !String.IsNullOrWhiteSpace(date))
        //        {
        //            foreach (Topic_Keyword x in keyWords)
        //            {
        //                if (x.Date == date)
        //                {
        //                    x.Add(s);
        //                    if(_date != null && _date.DataContext != x)
        //                    {
        //                        _date.DataContext = x;
        //                    }
        //                    return;
        //                }
        //            }

        //        //}

        //        Topic_Keyword it = new Topic_Keyword(date);
        //        it.Add(s);
        //        it.Items.CollectionChanged += changedKeyword;
        //        keyWords.Add(it);

        //    }
        //}

        public void AddK(Topic_Keyword x)
        {
            x.PropertyChanged += DeleteEmpty;
            x.Items.CollectionChanged += changedKeyword;
            keyWords.Add(x);
        }

        public void Remove(Keyword remv)
        {
            remv.Parent.Items.Remove(remv);
        }

        public bool Contains(string s)
        {
            foreach (Topic_Keyword x in keyWords)
            {
                foreach (Keyword y in x.Items)
                {
                    if (y.Text == s)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public ObservableCollection<Topic_Keyword> Item
        {
            get { return keyWords; }
        }

        public XmlTree GetXmlSeri()
        {

            ObservableCollection<XmlNode> tmp = new ObservableCollection<XmlNode>();

            foreach (Topic_Keyword x in Item)
            {
                XmlNode sth = new XmlNode() { date = x.Date, keyword = x.ItemsToString() };
                tmp.Add(sth);
            }

            return new XmlTree() { list = tmp };
        }

        public bool containsDate(string s)
        {
            foreach (Topic_Keyword x in keyWords)
            {
                if (x.Date == s)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public class XmlTree
    {
        [XmlArray(ElementName = "Topic_Folder")]
        public ObservableCollection<XmlNode> list { get; set; }

        public Topic_Folder _list()
        {
            ObservableCollection<Topic_Keyword> tmp = new ObservableCollection<Topic_Keyword>();

            foreach (XmlNode x in list)
            {
                Topic_Keyword sth = new Topic_Keyword(x.date, x.keyword);
                tmp.Add(sth);
            }

            return new Topic_Folder(tmp);
        }
    }

    public class XmlNode
    {
        [XmlAttribute(AttributeName = "Date")]
        public string date { get; set; }

        [XmlArray(ElementName = "Topic_Keyword")]

        public ObservableCollection<string> keyword { get; set; }
    }
}

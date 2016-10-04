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
using System.Windows.Controls;
using System.IO;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Xml.Serialization;


namespace MapRSS_LogicEngine
{
    public partial class MapRSS
    {
        private ObservableCollection<Article> target = new ObservableCollection<Article>();
        private Topic_Folder keyWord = new Topic_Folder();
        private Topic_Keyword txtBxNull = null;
        private CheckBox Topic_HideRead;

        public ObservableCollection<Article>getTarget
        {
            get
            {
                //var tmp = from t in target orderby t.Hit select t;
                //return tmp as ObservableCollection<Article>;
                return target;
            }
        }

        public void findAll(ObservableCollection <string> sth)
        {
            foreach(string s in sth)
            {
                findTopics(this, new PropertyChangedEventArgs(s));
            }
        }


        public void SaveTopicHandler(object sender, EventArgs e)
        {
            try
            {
                using (TextWriter _writer = new StreamWriter("../../XML/TopicKeywords.xml"))
                {
                    XmlSerializer seri = new XmlSerializer(typeof(XmlTree));
                    seri.Serialize(_writer, keyWord.GetXmlSeri());

                }
            }
            catch (Exception)
            {
                //Something ??????????
            }
        }


        public ObservableCollection<Topic_Keyword> OpenKeyWords(ComboBox Topic_Combo)
        {
            ObservableCollection<Topic_Keyword> keys = new ObservableCollection<Topic_Keyword>();

            try
            {
                using (StreamReader reader = new StreamReader("../../XML/TopicKeywords.xml"))
                {
                    XmlSerializer seri = new XmlSerializer(typeof(XmlTree));
                    keyWord = (seri.Deserialize(reader) as XmlTree)._list();
                    keys = keyWord.Item;
                    foreach (Topic_Keyword x in keys)
                    {
                        if (x.Date == DateTime.Today.ToShortDateString())
                        {
                            txtBxNull = x;
                        }


                        Topic_Combo.Items.Add(new ComboBoxItem() { Content = x.Date, DataContext = x });
                        keyWord.Topic.Add(x.Date);
                    }

                    Topic_Combo.SelectedItem = null;
                }

            }

            catch (Exception)
            {

            }

            if(txtBxNull == null)
            {
                txtBxNull = new Topic_Keyword();
            }

            return keys;
        }

        public Topic_Folder KeyWord
        {
            get { return keyWord; }
        }

        public ObservableCollection<string> Topic
        {
            get { return keyWord.Topic; }
        }

        public Topic_Keyword txtBoxIsNull
        { get { return txtBxNull; } }

    }
}

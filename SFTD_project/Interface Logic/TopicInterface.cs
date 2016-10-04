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
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using MapRSS_LogicEngine;

namespace SFTD_project
{
    public partial class MainWindow : Window
    {

        //DO what topic interface suppose to do
        //the topic interface will not affect the main interface since I copied the main interface xaml code and renamed the variables names, except the read/unread mark.

        private ObservableCollection<Keyword> list = new ObservableCollection<Keyword>();

        public void topicTabSelected(object sender, EventArgs e)
        {
            SearchTopic(sender, new RoutedEventArgs ());

            if (Topic_Text.Text != null && Topic_Text.Text != "")
            {
                if (Topic_Combo.SelectedItem != null)
                {
                    Topic_Keyword tmp = (Topic_Combo.SelectedItem as ComboBoxItem).DataContext as Topic_Keyword;
                    if(!tmp.Contains(Topic_Text.Text))
                    {
                        Topic_Bookmark.IsEnabled = true;
                    }
                    else
                    {
                        Topic_Bookmark.IsEnabled = false;
                    }
                }
                else if (!program.txtBoxIsNull.Contains(Topic_Text.Text))
                {
                    Topic_Bookmark.IsEnabled = true;
                }
                else
                {
                    Topic_Bookmark.IsEnabled = false;
                }
                
            }
            else
            {
                Topic_Bookmark.IsEnabled = false;
            }

        }


        private void Topic_SetFeedData()
        {
            SearchTopic(null, new RoutedEventArgs());

        }

        private void Topic_OnTopicSelected(object sender, RoutedEventArgs e)
        {
            try
            {
                Topic_Text.Text = ((sender as TreeView).SelectedItem as Keyword).Text;
            }
            catch (Exception)
            {
                
                
            }
        }

        private void SearchTopic(object sender, RoutedEventArgs e)
        {
            program.getTarget.Clear();

            ObservableCollection<string> sth = new ObservableCollection<string>();

            string s = "";

            foreach(char x in Topic_Text.Text)
            {
                if(x!='+')
                {
                    s += x;
                }
                else
                {
                    sth.Add(s);
                    s = "";
                }
            }

            sth.Add(s);
            program.findAll(sth);
            Topic_FeedData.ItemsSource=program.getTarget;

            string option = (Topic_Display.SelectedItem as ComboBoxItem).Content as string;
            if (option == "All")
            {
                Topic_FeedData.ItemsSource = program.getTarget;
            }
            else
            {
                Topic_FeedData.ItemsSource = new ObservableCollection<Article>(program.getTarget.Take(int.Parse(option)));
            }
        }

        private void SetTopic(object sender, RoutedEventArgs e)
        {
            string x = "";
            foreach (string s in ((sender as Button).DataContext as Topic_Keyword).ItemsToString())
            {
                x += s + "+";
            }
            Topic_Text.Text = x.Substring(0, x.Length - 1);
            Topic_Bookmark.IsEnabled = false;
        }

        public void Topic_Bookmark_Click(object sender, RoutedEventArgs e)
        {
            if(Topic_Combo.SelectedItem!=null)
            {
                ComboBoxItem tmp = Topic_Combo.SelectedItem as ComboBoxItem;
                Topic_Keyword t = tmp.DataContext as Topic_Keyword;
                t.Add(Topic_Text.Text);
                if(!program.KeyWord.Item.Contains(t))
                {
                    program.KeyWord.AddK(t);
                }
            }
            else
            {
                if (!program.Topic.Contains(DateTime.Today.ToShortDateString()))
                {
                    program.Topic.Add(DateTime.Today.ToShortDateString());
                    ComboBoxItem t = new ComboBoxItem() { Content = DateTime.Today.ToShortDateString(), DataContext = program.txtBoxIsNull };
                    Topic_Combo.Items.Add(t);
                    program.KeyWord.AddK(program.txtBoxIsNull);
                }
                program.txtBoxIsNull.Add(Topic_Text.Text);
            }


        }


        private void Topic_ComboNewTopic_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tmp = sender as TextBox;

            if (tmp.Text == "Create New Topic ..." | String.IsNullOrEmpty(tmp.Text) | String.IsNullOrWhiteSpace(tmp.Text))
            {
                tmp.Text = "";
            }
        }


        private void Topic_ComboNewTopic_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tmp = (sender as TextBox);
            if ((String.IsNullOrEmpty(tmp.Text) | String.IsNullOrWhiteSpace(tmp.Text) | tmp.Text == "Create New Topic ..." | tmp.Text == DateTime.Today.ToShortDateString() | program.Topic.Contains(tmp.Text)))
            {
                tmp.Text = "Create New Topic ...";
                Topic_Combo.SelectedItem = null;

                return;
            }


            Topic_Combo.Items.Add(new ComboBoxItem() { Content = tmp.Text, IsSelected = true, DataContext = new Topic_Keyword(tmp.Text) });
            program.Topic.Add(tmp.Text);
            return;

        }

        private void changedTopic(object sender, EventArgs e)
        {
            Topic_RSSTreeView.ItemsSource = program.KeyWord.Item;
            Topic_Bookmark.IsEnabled = false;
            Topic_RSSTreeView.ItemTemplate = null;
            topicTabSelected(sender, e);
            
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button del = sender as Button;

            Button temp = (Button)xamlMaker("../../img/xaml/delete.xaml");

            temp.Click += (s,a)=>DeleteButton_Del(s, a, del);
            Global_ToolBar.Items[1] = temp;

            Topic_RSSTreeView.ItemTemplate = setUpTheHierarchy();

        }

        private HierarchicalDataTemplate setUpTheHierarchy()
        {

            //Class Topic_Keywords
            FrameworkElementFactory ft = new FrameworkElementFactory(typeof(StackPanel));
            ft.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            FrameworkElementFactory im = new FrameworkElementFactory(typeof(Image));
            im.SetValue(Image.SourceProperty, findImageSource("../../img/icons/folder-icon-16x16.png"));
            im.SetValue(Image.WidthProperty, Double.Parse("14"));
            im.SetValue(Image.HeightProperty, Double.Parse("14"));
            im.SetValue(Image.MarginProperty, new Thickness(4, 0, 0, 0));
            ft.AppendChild(im);

            FrameworkElementFactory tb = new FrameworkElementFactory(typeof(TextBlock));
            tb.SetBinding(TextBlock.TextProperty, new Binding("Date"));
            tb.SetValue(TextBlock.MarginProperty, new Thickness(4, 0, 0, 0));

            ft.AppendChild(tb);

            HierarchicalDataTemplate t = new HierarchicalDataTemplate(typeof(Topic_Keyword));
            t.ItemsSource = new Binding("Items");
            t.VisualTree = ft;


            //Class Keywords

            FrameworkElementFactory ff = new FrameworkElementFactory(typeof(StackPanel));
            ff.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            FrameworkElementFactory ima = new FrameworkElementFactory(typeof(Image));
            ima.SetValue(Image.SourceProperty, findImageSource("../../img/icons/feed-icon-14x14.png"));
            ima.SetValue(Image.WidthProperty, Double.Parse("14"));
            ima.SetValue(Image.HeightProperty, Double.Parse("14"));
            ima.SetValue(Image.MarginProperty, new Thickness(4, 0, 0, 0));
            ff.AppendChild(ima);

            FrameworkElementFactory tbt = new FrameworkElementFactory(typeof(TextBlock));
            tbt.SetBinding(TextBlock.TextProperty, new Binding("Text"));
            //tbt.SetBinding(TextBlock, new Binding() { RelativeSource= RelativeSource.Self });
            tbt.SetValue(TextBlock.MarginProperty, new Thickness(4, 0, 0, 0));
            ff.AppendChild(tbt);



            FrameworkElementFactory f = new FrameworkElementFactory(typeof(CheckBox));
            f.SetBinding(CheckBox.ClickModeProperty, new Binding("ThreeWay"));
            f.SetBinding(CheckBox.DataContextProperty, new Binding());
            f.AppendChild(ff);
            f.AddHandler(CheckBox.CheckedEvent, new RoutedEventHandler(isChecked));
            f.AddHandler(CheckBox.UncheckedEvent, new RoutedEventHandler(isUnchecked));


            HierarchicalDataTemplate tmp = new HierarchicalDataTemplate(typeof(Keyword));

            tmp.ItemsSource = new Binding("Items");

            tmp.VisualTree = f;

            t.ItemTemplate = tmp;

            return t;
        }

        private void DeleteButton_Del(object sender, RoutedEventArgs e, Button bn)
        {
            foreach (Keyword x in list)
            {
                program.KeyWord.Remove(x);
            }

            Topic_RSSTreeView.ItemTemplate = null;
            Global_ToolBar.Items[1] = bn;
        }

        private BitmapImage findImageSource(string s)
        {
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(s, UriKind.Relative);
            bi3.EndInit();

            return bi3;
        }

        private void isChecked(object sender, EventArgs e)
        {
            list.Add((sender as CheckBox).DataContext as Keyword);
            
        }

        private void isUnchecked(object sender, EventArgs e)
        {
            list.Remove((sender as CheckBox).DataContext as Keyword);
        }
    }
}

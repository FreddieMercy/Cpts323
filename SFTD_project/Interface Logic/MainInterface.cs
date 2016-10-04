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
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.IO;
using System.Windows.Markup;
using MapRSS_LogicEngine;

namespace SFTD_project
{
    public partial class MainWindow : Window
    {   
        // UTILITY METHODS

        private void SetFeedDataItemSource()
        {
            TreeView tree = RSSTreeView;

            if (tree.SelectedItem is RSSFeed)
            {
                ObservableCollection<Article> items = new ObservableCollection<Article>();
                DateTime dateA = program.DateA, dateB = program.DateB;
                RSSFeed rss = tree.SelectedItem as RSSFeed;
                foreach (Article item in rss.Articles)
                {
                    // if hide toggle is not check or item has not
                    // been read, will display item
                    DateTime itemDate = DateTime.MinValue;
                    DateTime.TryParse(item.PublishDate, out itemDate);
                    bool withinRange = (dateA <= itemDate) && (dateB >= itemDate);
                    if ((!(bool)HideRead.IsChecked || !item.HasRead) && withinRange)
                    {
                        items.Add(item);
                    }
                    else // otherwise do not display and uncheck checkbox
                    {
                        item.IsSelected = false;
                    }
                }

                string option = (Display.SelectedItem as ComboBoxItem).Content as string;
                if (option == "All")
                {
                    FeedData.ItemsSource = items;
                }
                else
                {
                    FeedData.ItemsSource = new ObservableCollection<Article>(items.Take(int.Parse(option)));
                }
            }
            else if (tree.SelectedItem is Favorites)
            {
                ObservableCollection<Article> items = new ObservableCollection<Article>();
                Favorites favs = tree.SelectedItem as Favorites;
                foreach (Article item in favs.Items)
                {
                    // if hide toggle is not check or item has not
                    // been read, will display item
                    if (!(bool)HideRead.IsChecked || !item.HasRead)
                    {
                        items.Add(item);
                    }
                    else // otherwise do not display and uncheck checkbox
                    {
                        item.IsSelected = false;
                    }
                    //items.Add(item);
                    //item.IsSelected = false;
                }

                FeedData.ItemsSource = items;
            }
            else if (tree.SelectedItem is Article)
            {
                ObservableCollection<Article> items = new ObservableCollection<Article>();
                items.Add(tree.SelectedItem as Article);
                FeedData.ItemsSource = items;
            }
        }

        // GENERATED EVENT HANDLERS
        private void Link_Click2(object sender, RoutedEventArgs e)
        {
            Hyperlink link = sender as Hyperlink;
            // Enable at your own risk; (script errors)
            Topic_Browser.Source = (sender as Hyperlink).NavigateUri;
            Article selected = Topic_FeedData.SelectedItem as Article;
            selected.HasRead = true;
            SetFeedDataItemSource();
            Topic_SetFeedData();
            
        }

        private void Link_Click(object sender, RoutedEventArgs e)
        {

            Hyperlink link = sender as Hyperlink;
            // Enable at your own risk; (script errors)
            Browser.Source = (sender as Hyperlink).NavigateUri;
            Article selected = FeedData.SelectedItem as Article;
            selected.HasRead = true;
            SetFeedDataItemSource();
            Topic_SetFeedData();

        }

        private void Select_SelectionChanged2(object sender, SelectionChangedEventArgs e)
        {
            ComboBox menu = sender as ComboBox;

            if (menu.SelectedIndex != -1)
            {
                if (Topic_FeedData.ItemsSource != null)
                {
                    ComboBoxItem selected = menu.SelectedItem as ComboBoxItem;
                    string option = selected.Content as string;

                    foreach (Article item in Topic_FeedData.ItemsSource)
                    {
                        if (option == "All") { item.IsSelected = true; }
                        else if (option == "None") { item.IsSelected = false; }
                        else if (option == "Read") { item.IsSelected = item.HasRead; }
                        else if (option == "Unread") { item.IsSelected = !item.HasRead; }
                    }

                    Topic_FeedData.Items.Refresh();
                }
                menu.SelectedIndex = -1;
            }
        }

        private void Select_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox menu = sender as ComboBox;

            if (menu.SelectedIndex != -1)
            {
                if (FeedData.ItemsSource != null)
                {
                    ComboBoxItem selected = menu.SelectedItem as ComboBoxItem;
                    string option = selected.Content as string;

                    foreach (Article item in FeedData.ItemsSource)
                    {
                        if (option == "All") { item.IsSelected = true; }
                        else if (option == "None") { item.IsSelected = false; }
                        else if (option == "Read") { item.IsSelected = item.HasRead; }
                        else if (option == "Unread") { item.IsSelected = !item.HasRead; }
                    }

                    FeedData.Items.Refresh();
                }
                menu.SelectedIndex = -1;
            }
        }

        private void Quantity_SelectionChanged2(object sender, SelectionChangedEventArgs e)
        {
            Topic_SetFeedData();
        }

        private void Quantity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetFeedDataItemSource();
        }

        private void MarkAsRead_Click2(object sender, RoutedEventArgs e)
        {
            if (Topic_FeedData.ItemsSource == null) { return; }

            foreach (Article item in Topic_FeedData.ItemsSource)
            {
                if (item.IsSelected) { item.HasRead = true; }
            }
            SetFeedDataItemSource();
            Topic_SetFeedData();
        }

        private void MarkAsRead_Click(object sender, RoutedEventArgs e)
        {
            if (FeedData.ItemsSource == null) { return; }

            foreach (Article item in FeedData.ItemsSource)
            {
                if (item.IsSelected) { item.HasRead = true; }
            }
            SetFeedDataItemSource();
            Topic_SetFeedData();
        }

        private void MarkAsUnread_Click2(object sender, RoutedEventArgs e)
        {
            if (Topic_FeedData.ItemsSource == null) { return; }

            foreach (Article item in Topic_FeedData.ItemsSource)
            {
                if (item.IsSelected) { item.HasRead = false; }
            }
            SetFeedDataItemSource();
            Topic_SetFeedData();
        }

        private void MarkAsUnread_Click(object sender, RoutedEventArgs e)
        {
            if (FeedData.ItemsSource == null) { return; }

            foreach (Article item in FeedData.ItemsSource)
            {
                if (item.IsSelected) { item.HasRead = false; }
            }
            SetFeedDataItemSource();
            Topic_SetFeedData();
        }

        private void FavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            if (FeedData.ItemsSource == null) { return; }

            foreach (Article item in FeedData.ItemsSource)
            {
                if (item.IsSelected) 
                { 
                    item.IsFavorite = true;
                    program.AddFavorite(item);
                }
            }
            SetFeedDataItemSource();
        }


        private void UnFavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            if (FeedData.ItemsSource == null) { return; }

            foreach (Article item in FeedData.ItemsSource)
            {
                if (item.IsSelected) 
                { 
                    item.IsFavorite = true; 
                    program.RemoveFavorite(item);
                }
            }
            SetFeedDataItemSource();
        }


        //Load obj from local Xaml
        private object xamlMaker(string dir)
        {
            StreamReader reader = new StreamReader(dir);

            object temp = (object)XamlReader.Parse(reader.ReadToEnd());

            reader.Dispose();

            return temp;
        }
    }
}

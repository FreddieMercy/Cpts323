using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Collections.ObjectModel;
using MapRSS_LogicEngine;

namespace SFTD_project
{
    /// <summary>
    /// Interaction logic for MapSimpleWindowOnArticle.xaml
    /// </summary>
    public partial class MapSimpleWindowOnArticle : Window
    {
        private WebBrowser _browser;
        public MapSimpleWindowOnArticle(ObservableCollection<Article> onMapArticles, WebBrowser browser)
        {
            InitializeComponent();
            _browser = browser;
            
            SimpleGrid.ItemsSource = onMapArticles;
        }

        private void Link_Click(object sender, RoutedEventArgs e)
        {

            _browser.Source = new Uri((sender as Hyperlink).NavigateUri.ToString());
            this.Close();
        }
    }
}

using System.Windows;
using MapRSS_LogicEngine;

namespace SFTD_project
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ManageFeedsWindow : Window
    {
        MapRSS mainProgram;

        public ManageFeedsWindow(MapRSS main)
        {
            InitializeComponent();

            mainProgram = main;

            treeView.ItemsSource = main.Root;
        }

        private void CreateSub_Click(object sender, RoutedEventArgs e)
        {
            new SubscriptionWindow(mainProgram).ShowDialog();
        }

        private void CreateChannel_Click(object sender, RoutedEventArgs e)
        {
            new ChannelWindow(mainProgram).ShowDialog();
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            Window modWindow = null;

            object item = treeView.SelectedItem;
            if (item is RSSFeed)
            {
                modWindow = new SubscriptionWindow(mainProgram, item as RSSFeed);
            }
            else if (item is Channel)
            {
                modWindow = new ChannelWindow(mainProgram, item as Channel);
            }
            
            if (modWindow != null)
            {
                modWindow.ShowDialog();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            object item = treeView.SelectedItem;
            if (item is RSSFeed)
            {
                mainProgram.DeleteFeed(item as RSSFeed);
            }
            else if (item is Channel)
            {
                mainProgram.DeleteChannel(item as Channel);
            }
        }
    }
}

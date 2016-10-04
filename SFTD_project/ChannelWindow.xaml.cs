using System.Collections.Generic;
using System.Windows;
using MapRSS_LogicEngine;

namespace SFTD_project
{
    /// <summary>
    /// Interaction logic for CreateNewGroupsWin.xaml
    /// </summary>
    public partial class ChannelWindow : Window
    {
        private MapRSS main;
        private Channel originalChannel;

        public ChannelWindow(MapRSS program, Channel channel = null)
        {
            InitializeComponent();

            // INITIALIZE FIELDS

            main = program;
            originalChannel = channel;

            List<MapRSSItem> chSource = new List<MapRSSItem>(program.Channels);
            chSource.Insert(0, new Channel("No Group Selected"));

            Groups.ItemsSource = chSource;
            Groups.SelectedIndex = 0;

            if (channel != null)
            {
                Title = "Modify a channel";
                Confirm.Content = "Save Changes";
                Input.Text = channel.Name;

                chSource.Remove(channel);
                Groups.Items.Refresh();
                if (channel.Parent != null) { Groups.SelectedItem = channel.Parent; }
            }
        }

        //Comfirm Button
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            // determine channel
            Channel parent = null;
            if (Groups.SelectedIndex > 0)
            {
                parent = Groups.SelectedItem as Channel;
            }

            // begin creation/modification
            if (originalChannel == null)
            {
                main.CreateChannel(Input.Text, parent);
            }
            else
            {
                main.ModifyChannel(originalChannel, Input.Text, parent);
            }
            Close();
        }
    }
}

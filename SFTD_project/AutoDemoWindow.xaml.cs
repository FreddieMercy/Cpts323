using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Reflection;
using MapRSS_LogicEngine;

namespace SFTD_project
{
    /// <summary>
    /// Interaction logic for AutoDemoWindow.xaml
    /// </summary>
    public partial class AutoDemoWindow : Window
    {
        MapRSS program;
        Window main_window;

        bool InstructionsLock;

        string SubscribeToFeedInfo;
        string ViewFavoritesInfo;
        string ShowMapInterfaceInfo;
        string ManageFeedsInfo;
        string FullDemoInfo;

        public AutoDemoWindow(MapRSS program, Window main_window)
        {
            InitializeComponent();
            this.program = program;
            this.main_window = main_window;

            SetInfoTexts();
        }

        private void SubscribeToFeedButtonClicked(object sender, RoutedEventArgs e)
        {
            SubscribeToFeed(sender, e);
        }

        private bool SubscribeToFeed(object sender, RoutedEventArgs e)
        {
            InstructionsLock = true;
            this.InstructionsBox.Text = SubscribeToFeedInfo;

            SubscriptionWindow s = new SubscriptionWindow(program);

            s.Title = "Add a subscription";
            s.inputAlias.Text = "New Subsription Name Here";
            s.inputURL.Text = "RSS URL Here. Need a suggestion? http://rss.nytimes.com/services/xml/rss/nyt/World.xml";

            s.ShowDialog();

            InstructionsLock = false;
            return true;
        }

        private void ViewFavoritesButtonClicked(object sender, RoutedEventArgs e)
        {
            ViewFavorites(sender, e);
        }

        private bool ViewFavorites(object sender, RoutedEventArgs e)
        {
            InstructionsLock = true;
            this.InstructionsBox.Text = ViewFavoritesInfo;

            SubscriptionWindow s = new SubscriptionWindow(program);
            MainWindow m = (main_window as MainWindow);
            m.tabControl.SelectedIndex = 0;

            object a = m.RSSTreeView.Items[0];

            DependencyObject dObject = m.RSSTreeView.ItemContainerGenerator.ContainerFromItem(a);
            MethodInfo selectMethod = typeof(TreeViewItem).GetMethod("Select", BindingFlags.NonPublic | BindingFlags.Instance);
            selectMethod.Invoke(dObject, new object[] { true });

            InstructionsLock = false;
            return true;
        }

        private void ShowMapInterfaceButtonClicked(object sender, RoutedEventArgs e)
        {
            ShowMapInterface(sender, e);
        }

        private bool ShowMapInterface(object sender, RoutedEventArgs e)
        {
            InstructionsLock = true;
            this.InstructionsBox.Text = ShowMapInterfaceInfo;

            (main_window as MainWindow).tabControl.SelectedIndex = 1;

            InstructionsLock = false;
            return true;
        }

        private void ManageFeedsButtonClicked(object sender, RoutedEventArgs e)
        {
            ManageFeeds(sender, e);
        }

        private bool ManageFeeds(object sender, RoutedEventArgs e)
        {
            InstructionsLock = true;
            InstructionsBox.Text = ManageFeedsInfo;

            new ManageFeedsWindow(program).ShowDialog();

            InstructionsLock = false;
            return true;
        }

        private void FullDemoButtonClicked(object sender, RoutedEventArgs e)
        {
            // Call each demo action in succession but bail on exceptions
            // (is it cool to throw exeptions in event handlers?)
            // and then explain the issue in the demo text box
            InstructionsLock = true;

            if (!RunLesson(SubscribeToFeed, this, null)) { ClearLock(); return; }
            if (!RunLesson(ViewFavorites, this, null)) { ClearLock(); return; }
            if (!RunLesson(ShowMapInterface, this, null)) { ClearLock(); return; }
            if (!RunLesson(ManageFeeds, this, null)) { ClearLock(); return; }

            InstructionsLock = false;
        }

        private void SetLock()
        {
            InstructionsLock = true;
        }

        private void ClearLock()
        {
            InstructionsLock = false;
        }

        private bool RunLesson(Func<object, RoutedEventArgs, bool> f, object sender, RoutedEventArgs e)
        {
            while (true)
            {
                f(sender, e);
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Would you like to repeat that lesson?", "Confirm", System.Windows.MessageBoxButton.YesNoCancel);
                if (messageBoxResult == MessageBoxResult.No)
                    return true; // Go on
                else if (messageBoxResult == MessageBoxResult.Cancel)
                    return false; // Cancel full demo
                // else: Must be Yes. Loop.
            }
        }

        private void ButtonMouseOver(object sender, MouseEventArgs e)
        {
            switch ((sender as Button).Name)
            {
                case "FullDemoButton": SetInstructionText(FullDemoInfo); break;
                case "SubscribeToFeedButton": SetInstructionText(SubscribeToFeedInfo); break;
                case "ShowMapInterfaceButton": SetInstructionText(ShowMapInterfaceInfo); break;
                case "ManageFeedsButton": SetInstructionText(ManageFeedsInfo); break;
                case "ViewFavoritesButton": SetInstructionText(ViewFavoritesInfo); break;
            }
        }

        private void SetInstructionText(string text)
        {
            if (!InstructionsLock)
                InstructionsBox.Text = text;
        }

        private void AppendInstructionText(string text)
        {
            InstructionsBox.Text += text;
        }

        private void SetInfoTexts()
        {
            SubscribeToFeedInfo = "The subscription window allows you to create new subscriptions or modify existing ones.";
            ViewFavoritesInfo = "On the main interface you can view your favorites articles. Here: I've selected your favorites for you.";
            ShowMapInterfaceInfo = "This is the map interface. Here you can see where articles are coming from.";
            ManageFeedsInfo = "Here you can add, delete, and modify feeds and channels. Handy!";
            FullDemoInfo = "The full demo performs every action show above.";
        }
    }
}

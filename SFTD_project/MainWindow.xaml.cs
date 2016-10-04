using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Reflection;
using MapRSS_LogicEngine;
using Microsoft.Maps.MapControl.WPF;

namespace SFTD_project
{
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
    public partial class MainWindow : Window
    {
        MapRSS program;

        public MainWindow()
        {
            InitializeComponent();

            // INITIALIZE FIELDS

            program = new MapRSS(Topic_HideRead);
            program.LoadData();
            Display.SelectedIndex = 0;

                //In MapInterface
                    //private LocationConverter locConverter = new LocationConverter();


                //In TopicInterface
                    //private ObservableCollection<Keyword> list = new ObservableCollection<Keyword>();

            // EVENT HANDLERS MAIN

            program.PropertyChanged += UpdateFeedDataDisplay;
            RSSTreeView.ItemsSource = program.Root;            // Link TreeView to MapRSS hierachy list
            RSSTreeView.SelectedItemChanged += OnItemSelected; // Event handler that links DataGrid & TreeView
            Browser.Navigated += Browser_Navigated;            // Event handler that stops JS errors
            Closing += SaveBeforeCloseHandler;

            // EVENT HANDLERS MAP
            MapTreeView.SelectedItemChanged += (sender, e) => {

                Article tmp = (sender as TreeView).SelectedItem as Article;

                //if "selectedItem" is Feed, or deleting selected feed
                //I was going to use the if-statement but the operator "!" (not) had been custom defined
                try
                {
                    tmp.HasRead = true;

                    myMap.SetView(tmp.Location.Location, 12);

                    MapBrowser.Source = new Uri(tmp.Link);
                }
                catch(Exception)
                {

                }
            
            };
            MapTreeView.ItemsSource = program.getonMapFeeds;
            MapBrowser.Navigated += Browser_Navigated;
            myMap.Focus();
            changeMapMode.SelectionChanged += changeMapModeHander;
            myMap.ViewChangeOnFrame += new EventHandler<MapEventArgs>(viewMap_ViewChangeOnFrame);
            program.getOnMapArticles.CollectionChanged += (sender, e) =>
            {
                firstRunningAddPinPoints(false);
                ArticlePinPoints.Children.Clear();
                firstRunningAddPinPoints();
            };
            firstRunningAddPinPoints();

            // EVENT HANDLERS TOPIC
            Topic_Display.SelectedIndex = 0;
            Topic_Text.TextChanged += topicTabSelected;
            Topic_RSSTreeView.SelectedItemChanged += Topic_OnTopicSelected;
            Topic_Combo.SelectionChanged += topicTabSelected;
            Topic_Browser.Navigated += Browser_Navigated;
            Closing += program.SaveTopicHandler;
            Topic_RSSTreeView.ItemsSource = program.OpenKeyWords(Topic_Combo);
            program.KeyWord.Item.CollectionChanged += changedTopic;
            program.KeyWord.PropertyChanged += changedTopic;

            // EVENT HANDLER FAVORITE
            Closing += program.SaveFavHandler;

        }     


        // CUSTOM EVENT HANDLERS

        private void OnItemSelected(object sender, RoutedEventArgs e)
        {
            SetFeedDataItemSource();
            Topic_SetFeedData();
        }

        private void UpdateFeedDataDisplay(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Updated")
            {
                upDateMapLayer();
                SetFeedDataItemSource();
            }
            else if (e.PropertyName == "Feed")
            {
                upDateMapLayer();
                FeedData.ItemsSource = null;

            }
        }

        private void Browser_Navigated(object sender, NavigationEventArgs e)
        {
            HideJsScriptErrors(sender as WebBrowser);
        }

        private void SaveBeforeCloseHandler(object sender, EventArgs e)
        {
            program.SaveData();
        }

        private void OpenManageWindow_Click(object sender, EventArgs e)
        {
            new ManageFeedsWindow(program).ShowDialog();
        }

        private void OpenTimeFilterWindow_Click(object sender, RoutedEventArgs e)
        {
            new TimeFilterWindow(program).ShowDialog();
            SetFeedDataItemSource();
        }

        private void DemoButton_Click(object sender, RoutedEventArgs e)
        {
            new AutoDemoWindow(program, this).ShowDialog();
        }

        // WEB BROWSER ERROR HANDLING

        public void HideJsScriptErrors(WebBrowser wb)
        {
            // IWebBrowser2 interface
            // Exposes methods that are implemented by the WebBrowser control  
            // Searches for the specified field, using the specified binding constraints.
            FieldInfo fld = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fld == null) { return; }
            object obj = fld.GetValue(wb);
            if (obj == null) { return; }
            // Silent: Sets or gets a value that indicates whether the object can display dialog boxes.
            // HRESULT IWebBrowser2::get_Silent(VARIANT_BOOL *pbSilent);HRESULT IWebBrowser2::put_Silent(VARIANT_BOOL bSilent);
            obj.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, obj, new object[] { true });
        }

    }
}

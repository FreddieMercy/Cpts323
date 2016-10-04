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
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using MapRSS_LogicEngine;
using System.Globalization;
using Microsoft.Maps.MapControl.WPF;
using Microsoft.Maps.MapControl.WPF.Design;

namespace SFTD_project
{
    public partial class MainWindow : Window
    {
        private LocationConverter locConverter = new LocationConverter();
        
        private void ChangeMapView_Click(object sender, RoutedEventArgs e)
        {
            // Parse the information of the button's Tag property
            string[] tagInfo = ((Button)sender).Tag.ToString().Split(' ');
            Location center = (Location)locConverter.ConvertFrom(tagInfo[0]);
            double zoom = System.Convert.ToDouble(tagInfo[1]);

            // Set the map view
            myMap.SetView(center, zoom);

        }


        private void changeMapModeHander(object sender, EventArgs e)
        {

            switch ((sender as ComboBox).SelectedIndex)
            {
                case 1:
                    myMap.Mode = new RoadMode();
                    break;
                default:
                    myMap.Mode = new AerialMode(true);
                    break;
            }

        }

        private void AnimationLevel_SelectionChanged(object sender, RoutedEventArgs e)
        {

            if ((sender as CheckBox).IsChecked == true)
            {
                myMap.AnimationLevel = AnimationLevel.Full;
                return;
            }


            myMap.AnimationLevel = AnimationLevel.None;
        }

        private void viewMap_ViewChangeOnFrame(object sender, MapEventArgs e)
        {
            // Gets the map object that raised this event.
            Map map = sender as Map;
            // Determine if we have a valid map object.
            if (map != null)
            {
                // Gets the center of the current map view for this particular frame.
                Location mapCenter = map.Center;

                // Updates the latitude and longitude values, in real time,
                // as the map animates to the new location.
                txtLatitude.Text = string.Format(CultureInfo.InvariantCulture,
                  "{0:F5}", mapCenter.Latitude);
                txtLongitude.Text = string.Format(CultureInfo.InvariantCulture,
                    "{0:F5}", mapCenter.Longitude);
            }
        }


        private void CheckedMapArticlePinPoints(object sender, RoutedEventArgs e)
        {
            Article tmp = (sender as CheckBox).DataContext as Article;
            (tmp.Location.Content as Button).Click += MouseButtonDownHandler;
            tmp.addToPin(program.getOnMapArticles);
            
        }

        private void UnCheckedMapArticlePinPoints(object sender, RoutedEventArgs e)
        {
            Article tmp = (sender as CheckBox).DataContext as Article;
            (tmp.Location.Content as Button).Click -= MouseButtonDownHandler;
            tmp.deletedFromPin(sender, e);
        }



        private void upDateMapLayer()
        {
            firstRunningAddPinPoints(false);
            MapTreeView.ItemsSource = program.getonMapFeeds;
            firstRunningAddPinPoints();
        }

        private void firstRunningAddPinPoints(bool sub = true)
        {
            foreach (Pushpin x in program.getOnMapArticles)
            {
                if(sub)
                {
                    (x.Content as Button).Click += MouseButtonDownHandler;
                    ArticlePinPoints.Children.Add(x);
                }
                else
                {
                    (x.Content as Button).Click -= MouseButtonDownHandler;
                    ArticlePinPoints.Children.Remove(x);
                }
            }
        }

        private void MouseButtonDownHandler(object sender, RoutedEventArgs e)
        {
            Pushpin p = (sender as Button).DataContext as Pushpin;

            myMap.SetView(p.Location, 12);

            new MapSimpleWindowOnArticle((p.DataContext as ObservableCollection<Article>), MapBrowser).ShowDialog();
        }
    }
}

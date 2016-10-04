using MapRSS_LogicEngine;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SFTD_project
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class TimeFilterWindow : Window
    {
        private MapRSS program;

        public TimeFilterWindow(MapRSS program)
        {
            InitializeComponent();

            this.program = program;
            DateA.SelectedDate = program.DateA;
            DateB.SelectedDate = program.DateB;
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = comboBox.SelectedIndex;

            if (index == 4)
            {
                DateA.IsEnabled = true;
                DateB.IsEnabled = true;
            }
            else if (index == 3)
            {
                DateA.IsEnabled = false;
                DateB.IsEnabled = true;
            }
            else if (index > 0)
            {
                DateA.IsEnabled = true;
                DateB.IsEnabled = false;
            }
            else // index == 0, probably
            {
                DateA.IsEnabled = false;
                DateB.IsEnabled = false;
            }
        }

        private void CloseTime_Click(object sender, RoutedEventArgs e)
        {
            int index = comboBox.SelectedIndex;

            if (index == 1 && (DateA.SelectedDate != null)) // on date
            {
                DateTime time = (DateTime)DateA.SelectedDate;
                program.DateA = time;
                program.DateB = time.AddMinutes(1439.0); // 1440 minutes in a day minus 1 minute
            }
            else if (index == 2 && (DateA.SelectedDate != null)) // newer than date
            {
                program.DateA = (DateTime)DateA.SelectedDate;
                program.DateB = DateTime.MaxValue;
            }
            else if (index == 3 && (DateA.SelectedDate != null)) // older than date
            {
                program.DateA = DateTime.MinValue;
                program.DateB = (DateTime)DateB.SelectedDate;
            }
            else if (index == 4) // between dates
            {
                program.DateA = (DateA.SelectedDate != null) ? (DateTime)DateA.SelectedDate : DateTime.MinValue;
                program.DateB = (DateB.SelectedDate != null) ? (DateTime)DateB.SelectedDate : DateTime.MaxValue;
            }
            else // from all time
            {
                program.DateA = DateTime.MinValue;
                program.DateB = DateTime.MaxValue;
            }
            
            Close();
        }
    }
}

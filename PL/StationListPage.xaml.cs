using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Input;
using PO;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationListPage.xaml
    /// </summary>
    public partial class StationListPage : Page
    {
        private BlApi.IBL bl = BlApi.BlFactory.GetBl();
        private ObservableCollection<StationToList> stations;

        public StationListPage(ObservableCollection<StationToList> stations)
        {
            InitializeComponent();
            this.stations = stations;
            StationsListView.ItemsSource = this.stations;
            StationsData();
        }

        private void StationsData()
        {
            stations.Clear();
            foreach (var station in bl.GetStations())
            {
                StationToList newStation = new StationToList();
                CopyPropertiesTo(station, newStation);
                stations.Add(newStation);
            }
        }

        private void GroupByAvailableStationsButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            StationsListView.ItemsSource = bl.GetStationsByGroupAvailableStations();
        }

        private void RefreshGroupButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            StationsListView.ItemsSource = stations;
            StationsData();
        }

        //private void AddStationButton_Click(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    //listWindow.ShowData.Content = new StationPage(listWindow, stations);
        //}

        //private void StationsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    StationToList stationToList = (StationToList)StationsListView.SelectedItem;
        //    BO.Station station = bl.GetStation(stationToList.Id);
        //    listWindow.ShowData.Content = new StationPage(listWindow, station, stations);
        //}

        public void CopyPropertiesTo<T, S>(S from, T to)
        {
            foreach (PropertyInfo propTo in to.GetType().GetProperties())
            {
                PropertyInfo propFrom = typeof(S).GetProperty(propTo.Name);
                if (propFrom == null)
                    continue;
                var value = propFrom.GetValue(from, null);
                if (value is ValueType || value is string)
                    propTo.SetValue(to, value);
            }
        }
    }
}

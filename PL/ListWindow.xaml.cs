using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PO;

namespace PL
{
    /// <summary>
    /// Interaction logic for ListWindow.xaml
    /// </summary>
    public partial class ListWindow : Window
    {
        private BlApi.IBL bl;
        private DroneListPage droneListPage;
        private DronePage dronePage;
        private BO.Drone drone;

        private StationListPage stationListPage;
        private StationPage stationPage;
        private BO.Station station;
        public ListWindow()
        {
            InitializeComponent();
            bl = BlApi.BlFactory.GetBl();
            droneListPage = new DroneListPage();
            stationListPage = new StationListPage();
        }
        private void ListTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowData.Content = "";
            if (ListDrones.IsSelected)
            {
                droneListPage.AddDroneButton.Click += DroneListPage_Add;
                droneListPage.DronesListView.MouseDoubleClick += DroneListPage_Actions;
                ShowList.Content = droneListPage;
            }

            if (ListStations.IsSelected)
            {
                stationListPage.AddStationButton.Click += StationListPage_Add;
                stationListPage.StationsListView.MouseDoubleClick += StationListPage_Actions;
                ShowList.Content = stationListPage;
            }
            if (ListCustomers.IsSelected)
                ShowList.Content = new CustomerListPage(this);
            if (ListParcels.IsSelected)
                ShowList.Content = new ParcelListPage(this);
            if (CloseWindow.IsSelected)
            {
                CloseWindow.Visibility = Visibility.Hidden;
                this.Close();
            }
        }

        private void DroneListPage_Add(object sender, RoutedEventArgs e)
        {
            ObservableCollection<PO.DroneToList> drones = new ObservableCollection<DroneToList>();
            dronePage = new DronePage(drones);
            ShowData.Content = dronePage;//go to the window that can add a drone
        }
        private void DroneListPage_Actions(object sender, MouseButtonEventArgs e)
        {
            ObservableCollection<PO.DroneToList> drones = new ObservableCollection<DroneToList>();
            DroneToList droneToList = (DroneToList)droneListPage.DronesListView.SelectedItem;
            drone = bl.GetDrone(droneToList.Id);
            dronePage = new DronePage(drone, drones);
            dronePage.ParcelDataButton.Click += DronePage_DataParcel;
            ShowData.Content = dronePage;
        }
        private void DronePage_DataParcel(object sender, RoutedEventArgs e)
        {
            ShowData.Content = new ParcelInDronePage(drone);
        }

        private void StationListPage_Add(object sender, RoutedEventArgs e)
        {
            ObservableCollection<PO.StationToList> stations = new ObservableCollection<StationToList>();
            stationPage = new StationPage(stations);
            ShowData.Content = stationPage; //go to the window that can add a station        
        }
        private void StationListPage_Actions(object sender, MouseButtonEventArgs e)
        {
            ObservableCollection<PO.StationToList> stations = new ObservableCollection<StationToList>();
            StationToList stationToList = (StationToList)stationListPage.StationsListView.SelectedItem;
            station = bl.GetStation(stationToList.Id);
            stationPage = new StationPage(station, stations);
            ShowData.Content = stationPage;
        }

        private void CloseWithSpecialButton(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (CloseWindow.Visibility != Visibility.Hidden)
                e.Cancel = true;
        }

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

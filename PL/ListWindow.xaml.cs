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
        private BlApi.IBL bl = BlApi.BlFactory.GetBl();

        // Drone:
        private DroneListPage droneListPage;
        private DronePage dronePage;
        private ObservableCollection<DroneToList> drones = new ObservableCollection<DroneToList>();
        private Drone drone;

        // Station:
        private StationListPage stationListPage
        private StationPage stationPage;
        private ObservableCollection<PO.StationToList> stations = new ObservableCollection<StationToList>();
        private BO.Station station;

        private DroneInChargePage droneInChargePage;

        private CustomerListPage customerListPage = new CustomerListPage();
        private CustomerPage customerPage;
        private BO.Customer customer;
        public ListWindow()
        {
            InitializeComponent();
        }
        private void ListTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowData.Content = "";
            if (ListDrones.IsSelected)
            {
                droneListPage = new DroneListPage(drones);
                droneListPage.AddDroneButton.Click += DroneListPage_Add;
                droneListPage.DronesListView.MouseDoubleClick += DroneListPage_Actions;
                ShowList.Content = droneListPage;
            }

            if (ListStations.IsSelected)
            {
                stationListPage = new StationListPage(stations);
                stationListPage.AddStationButton.Click += StationListPage_Add;
                stationListPage.StationsListView.MouseDoubleClick += StationListPage_Actions;
                ShowList.Content = stationListPage;
            }

            if (ListCustomers.IsSelected)
            {
                customerListPage.AddCustomerButton.Click += CustomerListPage_Add;
                customerListPage.CustomersListView.MouseDoubleClick += CustomerListPage_Actions;
                ShowList.Content = customerListPage;
            }
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
            dronePage = new DronePage(drones);
            ShowData.Content = dronePage;//go to the window that can add a drone
        }
        private void DroneListPage_Actions(object sender, MouseButtonEventArgs e)
        {
            DroneToList droneToList = (DroneToList)droneListPage.DronesListView.SelectedItem;
            BO.Drone boDrone = new BO.Drone();
            boDrone = bl.GetDrone(droneToList.Id);
            drone = CopyBoDroneToPoDrone(boDrone, drone);
            dronePage = new DronePage(drone, drones);
            dronePage.ParcelDataButton.Click += DronePage_DataParcel;
            ShowData.Content = dronePage;
        }
        private void DronePage_DataParcel(object sender, RoutedEventArgs e)
        {
            ShowData.Content = new ParcelInDronePage(drone);
        }
        private Drone CopyBoDroneToPoDrone(BO.Drone boDrone, Drone poDrone)
        {
            poDrone = new Drone();
            CopyPropertiesTo(boDrone, poDrone);
            poDrone.Location = new Location();
            CopyPropertiesTo(boDrone.Location, poDrone.Location);
            poDrone.ParcelByTransfer = new ParcelInTransfer();
            if (poDrone.Status == DroneStatuses.Delivery)
            {
                CopyPropertiesTo(boDrone.ParcelByTransfer, poDrone.ParcelByTransfer);
                poDrone.ParcelByTransfer.Sender = new CustomerInParcel();
                CopyPropertiesTo(boDrone.ParcelByTransfer.Sender, poDrone.ParcelByTransfer.Sender);
                poDrone.ParcelByTransfer.Target = new CustomerInParcel();
                CopyPropertiesTo(boDrone.ParcelByTransfer.Target, poDrone.ParcelByTransfer.Target);
                poDrone.ParcelByTransfer.PickUpLocation = new Location();
                CopyPropertiesTo(boDrone.ParcelByTransfer.PickUpLocation, poDrone.ParcelByTransfer.PickUpLocation);
                poDrone.ParcelByTransfer.TargetLocation = new Location();
                CopyPropertiesTo(boDrone.ParcelByTransfer.TargetLocation, poDrone.ParcelByTransfer.TargetLocation);
            }

            return poDrone;
        }
        private void StationListPage_Add(object sender, RoutedEventArgs e)
        {
            stationPage = new StationPage(stations);
            ShowData.Content = stationPage; //go to the window that can add a station        
        }
        private void StationListPage_Actions(object sender, MouseButtonEventArgs e)
        {
            ObservableCollection<PO.StationToList> stations = new ObservableCollection<StationToList>();
            StationToList stationToList = (StationToList)stationListPage.StationsListView.SelectedItem;
            station = bl.GetStation(stationToList.Id);
            stationPage = new StationPage(station, stations);
            stationPage.DroneDataButton.Click += StationPage_DroneData;
            ShowData.Content = stationPage;
        }
        private void StationPage_DroneData(object sender, RoutedEventArgs e)
        {
            droneInChargePage = new DroneInChargePage(station);
            droneInChargePage.DronesListView.MouseDoubleClick += DroneInCharge_Actions;
            ShowData.Content = droneInChargePage;
        }
        private void DroneInCharge_Actions(object sender, MouseButtonEventArgs e)
        {
            BO.DroneInCharge droneInCharge = (BO.DroneInCharge)droneInChargePage.DronesListView.SelectedItem;
            BO.Drone boDrone = new BO.Drone();
            boDrone = bl.GetDrone(droneInCharge.Id);
            CopyBoDroneToPoDrone(boDrone, drone);
            dronePage = new DronePage(drone, drones);
            dronePage.ParcelDataButton.Click += DronePage_DataParcel;
            ShowData.Content = dronePage;
        }
        private void CustomerListPage_Add(object sender, RoutedEventArgs e)
        {
            ObservableCollection<PO.CustomerToList> customers = new ObservableCollection<CustomerToList>();
            customerPage = new CustomerPage(customers);
            ShowData.Content = customerPage; //go to the window that can add a station   
        }
        private void CustomerListPage_Actions(object sender, MouseButtonEventArgs e)
        {
            ObservableCollection<PO.CustomerToList> customers = new ObservableCollection<CustomerToList>();
            CustomerToList selectedCustomer = (CustomerToList)customerListPage.CustomersListView.SelectedItem;
            customer = bl.GetCustomer(selectedCustomer.Id);
            customerPage = new CustomerPage(customer, customers);
            customerPage.ParcelFromTheCustomerButton.Click += CustomerPage_DataSendParcel;
            ShowData.Content = customerPage;
        }
        private void CustomerPage_DataSendParcel(object sender, RoutedEventArgs routedEventArgs)
        {
            ShowData.Content = new ParcelInCustomerPage(customer);
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

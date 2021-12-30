using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
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
        private StationListPage stationListPage;
        private StationPage stationPage;
        private ObservableCollection<StationToList> stations = new ObservableCollection<StationToList>();
        private Station station;

        private DroneInChargePage droneInChargePage;

        //Customer:
        private CustomerListPage customerListPage;
        private CustomerPage customerPage;
        ObservableCollection<CustomerToList> customers = new ObservableCollection<CustomerToList>();
        private Customer customer;

        private ParcelInCustomerPage parcelInCustomerPage;

        //Parcel:
        private ParcelListPage parcelListPage;
        private ParcelPage parcelPage;
        ObservableCollection<ParcelToList> parcels = new ObservableCollection<ParcelToList>();
        private Parcel parcel;

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
                customerListPage = new CustomerListPage(customers);
                customerListPage.AddCustomerButton.Click += CustomerListPage_Add;
                customerListPage.CustomersListView.MouseDoubleClick += CustomerListPage_Actions;
                ShowList.Content = customerListPage;
            }

            if (ListParcels.IsSelected)
            {
                parcelListPage = new ParcelListPage(parcels);
                parcelListPage.AddParcelButton.Click += ParcelListPage_Add;
                parcelListPage.ParcelsListView.MouseDoubleClick += ParcelListPage_Actions;
                ShowList.Content = parcelListPage;
            }
                
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
            if (poDrone.Status == DroneStatuses.Delivery)
            {
                poDrone.ParcelByTransfer = new ParcelInTransfer();
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
            StationToList stationToList = (StationToList)stationListPage.StationsListView.SelectedItem;
            BO.Station boStation = new BO.Station();
            boStation = bl.GetStation(stationToList.Id);
            station = CopyBoStationToPoStation(boStation, station);
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
            DroneInCharge droneInCharge = (DroneInCharge)droneInChargePage.DronesListView.SelectedItem;
            BO.Drone boDrone = new BO.Drone();
            boDrone = bl.GetDrone(droneInCharge.Id);
            drone = CopyBoDroneToPoDrone(boDrone, drone);
            dronePage = new DronePage(drone, drones);
            dronePage.ParcelDataButton.Click += DronePage_DataParcel;
            ShowData.Content = dronePage;
        }
        private Station CopyBoStationToPoStation(BO.Station boStation, Station poStation)
        {
            poStation = new Station();
            CopyPropertiesTo(boStation, poStation);
            poStation.Location = new Location();
            CopyPropertiesTo(boStation.Location, poStation.Location);

            List<DroneInCharge> dronesInCharge = new List<DroneInCharge>();
            foreach (var droneInCharge in boStation.DronesInCharges)
            {
                DroneInCharge newDroneInCharge = new DroneInCharge();
                CopyPropertiesTo(droneInCharge, newDroneInCharge);
                dronesInCharge.Add(newDroneInCharge);
            }
            poStation.DronesInCharges = dronesInCharge;

            return poStation;
        }
        private void CustomerListPage_Add(object sender, RoutedEventArgs e)
        {
            customerPage = new CustomerPage(customers);
            ShowData.Content = customerPage; //go to the window that can add a customer   
        }
        private void CustomerListPage_Actions(object sender, MouseButtonEventArgs e)
        {
            CustomerToList customerToList = (CustomerToList)customerListPage.CustomersListView.SelectedItem;
            BO.Customer boCustomer = new BO.Customer();
            boCustomer = bl.GetCustomer(customerToList.Id);
            customer = CopyBoCustomerToPoCustomer(boCustomer, customer);
            customerPage = new CustomerPage(customer, customers);
            customerPage.ParcelFromTheCustomerButton.Click += CustomerPage_DataParcelFromCustomer;
            customerPage.ParcelToTheCustomerButton.Click += CustomerPage_DataParcelToCustomer;
            ShowData.Content = customerPage;
        }
        private void CustomerPage_DataParcelFromCustomer(object sender, RoutedEventArgs routedEventArgs)
        {
            parcelInCustomerPage = new ParcelInCustomerPage(customer.FromTheCustomerList);
            parcelInCustomerPage.ParcelListView.MouseDoubleClick += CustomerPage_DataParcel;
            ShowData.Content = parcelInCustomerPage;
        }
        private void CustomerPage_DataParcelToCustomer(object sender, RoutedEventArgs routedEventArgs)
        {
            parcelInCustomerPage = new ParcelInCustomerPage(customer.ToTheCustomerList);
            parcelInCustomerPage.ParcelListView.MouseDoubleClick += CustomerPage_DataParcel;
            ShowData.Content = parcelInCustomerPage;
        }
        private void CustomerPage_DataParcel(object sender, MouseButtonEventArgs e)
        {
            ParcelInCustomer parcelInCustomer = (ParcelInCustomer) parcelInCustomerPage.ParcelListView.SelectedItem;
            BO.Parcel boParcel = bl.GetParcel(parcelInCustomer.Id);
            parcel = CopyBoParcelToPoParcel(boParcel, parcel);
            parcelPage = new ParcelPage(parcel, parcels);
            parcelPage.SenderButton.Click += CustomerPage_DataSender;
            parcelPage.TargetButton.Click += CustomerPage_DataTarget;
            parcelPage.DroneDataButton.Click += DronePage_DataDroneInParcel;
            ShowData.Content = parcelPage;
        }
        private Customer CopyBoCustomerToPoCustomer(BO.Customer boCustomer, Customer poCustomer)
        {
            poCustomer = new Customer();
            CopyPropertiesTo(boCustomer, poCustomer);
            poCustomer.Location = new Location();
            CopyPropertiesTo(boCustomer.Location, poCustomer.Location);

            List<ParcelInCustomer> parcelInCustomers = new List<ParcelInCustomer>();
            foreach (var parcel in boCustomer.FromTheCustomerList)
            {
                ParcelInCustomer newParcel = new ParcelInCustomer();
                CopyPropertiesTo(parcel, newParcel);
                newParcel.CustomerInDelivery = new CustomerInParcel();
                CopyPropertiesTo(parcel.CustomerInDelivery, newParcel.CustomerInDelivery);
                parcelInCustomers.Add(newParcel);
            }
            poCustomer.FromTheCustomerList = parcelInCustomers;

            parcelInCustomers.Clear();

            foreach (var parcel in boCustomer.ToTheCustomerList)
            {
                ParcelInCustomer newParcel = new ParcelInCustomer();
                CopyPropertiesTo(parcel, newParcel);
                newParcel.CustomerInDelivery = new CustomerInParcel();
                CopyPropertiesTo(parcel.CustomerInDelivery, newParcel.CustomerInDelivery);
                parcelInCustomers.Add(newParcel);
            }
            poCustomer.ToTheCustomerList = parcelInCustomers;

            return poCustomer;
        }
        private void ParcelListPage_Add(object sender, RoutedEventArgs e)
        {
            parcelPage = new ParcelPage(parcels);
            ShowData.Content = parcelPage; // go to the window that can add a parcel
        }
        private void ParcelListPage_Actions(object sender, MouseButtonEventArgs e)
        {
            ParcelToList parcelToList = (ParcelToList)parcelListPage.ParcelsListView.SelectedItem;
            BO.Parcel boParcel = new BO.Parcel();
            boParcel = bl.GetParcel(parcelToList.Id);
            parcel = CopyBoParcelToPoParcel(boParcel, parcel);
            parcelPage = new ParcelPage(parcel, parcels);
            parcelPage.SenderButton.Click += CustomerPage_DataSender;
            parcelPage.TargetButton.Click += CustomerPage_DataTarget;
            parcelPage.DroneDataButton.Click += DronePage_DataDroneInParcel;
            ShowData.Content = parcelPage;
        }
        private void CustomerPage_DataSender(object sender, RoutedEventArgs e)
        {
            ShowData.Content = new CustomerInParcelPage(parcel.Sender);
        }
        private void CustomerPage_DataTarget(object sender, RoutedEventArgs e)
        {
            ShowData.Content = new CustomerInParcelPage(parcel.Target);
        }
        private void DronePage_DataDroneInParcel(object sender, RoutedEventArgs e)
        {
            ShowData.Content = new DroneInParcelPage(parcel.DroneInParcel);
        }
        private Parcel CopyBoParcelToPoParcel(BO.Parcel boParcel, Parcel poParcel)
        {
            poParcel = new Parcel();
            CopyPropertiesTo(boParcel, poParcel);
            poParcel.Sender = new CustomerInParcel();
            CopyPropertiesTo(boParcel.Sender, poParcel.Sender);
            poParcel.Target = new CustomerInParcel();
            CopyPropertiesTo(boParcel.Target, poParcel.Target);
            if (boParcel.Scheduled != null)
            {
                poParcel.DroneInParcel = new DroneInParcel();
                CopyPropertiesTo(boParcel.DroneInParcel, poParcel.DroneInParcel);
                poParcel.DroneInParcel.Location = new Location();
                CopyPropertiesTo(boParcel.DroneInParcel.Location, poParcel.DroneInParcel.Location);
            }

            return poParcel;
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

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
        DroneListPage droneListPage;

        public ListWindow()
        {
            InitializeComponent();
            bl = BlApi.BlFactory.GetBl();
            droneListPage = new DroneListPage(this);
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
                ShowList.Content = new StationListPage(this);
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
            ShowData.Content = new DronePage(this, drones);//go to the window that can add a drone
        }
        private void DroneListPage_Actions(object sender, MouseButtonEventArgs e)
        {
            ObservableCollection<PO.DroneToList> drones = new ObservableCollection<DroneToList>();
            DroneToList droneToList = (DroneToList)droneListPage.DronesListView.SelectedItem;
            BO.Drone drone = bl.GetDrone(droneToList.Id);
            ShowData.Content = new DronePage(this, drone, drones);
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

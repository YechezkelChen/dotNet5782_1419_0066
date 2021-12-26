using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListPage : Page
    {
        ListWindow listWindow;
        private BlApi.IBL bl;
        private ObservableCollection<DroneToList> drones;

        public DroneListPage(ListWindow window)
        {
            InitializeComponent();
            listWindow = window;
            bl = BlApi.BlFactory.GetBl();
            drones = new ObservableCollection<DroneToList>();
            DronesListView.ItemsSource = drones;
            DronesData();

            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        private void DronesData()
        {
            drones.Clear();
            IEnumerable<BO.DroneToList> dronesData = new List<BO.DroneToList>();
            IEnumerable<BO.DroneToList> dronesFiltering = new List<BO.DroneToList>();

            dronesData = bl.GetDrones();

            // Filtering of status.
            if (StatusSelector.SelectedItem == null)
                dronesFiltering = bl.GetDrones();
            else
                dronesFiltering = bl.GetDronesByStatus((BO.DroneStatuses)StatusSelector.SelectedItem);

            dronesData = dronesFiltering.ToList().FindAll(drone => dronesData.ToList().Find(d => d.Id == drone.Id) != null);

            // Filtering of max weight
            if (WeightSelector.SelectedItem == null)
                dronesFiltering = bl.GetDrones();
            else
                dronesFiltering = bl.GetDronesByMaxWeight((BO.WeightCategories)WeightSelector.SelectedItem);

            dronesData = dronesFiltering.ToList().FindAll(drone => dronesData.ToList().Find(d => d.Id == drone.Id) != null);

            // Show the list after the filtering
            foreach (var drone in dronesData)
            {
                DroneToList newDrone = new DroneToList();
                listWindow.CopyPropertiesTo(drone, newDrone);
                newDrone.Location = new Location();
                listWindow.CopyPropertiesTo(drone.Location, newDrone.Location);
                drones.Add(newDrone);
            }
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DronesData();
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DronesData();
        }

        private void GroupByStatusButton_Click(object sender, RoutedEventArgs e)
        {
            DronesListView.ItemsSource = bl.GetDronesByGroupStatus();
        }

        private void RefreshStatusButton_Click(object sender, RoutedEventArgs e)
        {
            StatusSelector.SelectedItem = null;
            DronesData();
        }

        private void RefreshWeightButton_Click(object sender, RoutedEventArgs e)
        {
            WeightSelector.SelectedItem = null;
            DronesData();
        }

        private void RefreshGroupButton_Click(object sender, RoutedEventArgs e)
        {
            DronesListView.ItemsSource = drones;
            DronesData();
        }

        //private void AddDrone_Click(object sender, RoutedEventArgs e)
        //{
        //    listWindow.ShowData.Content = new DronePage(listWindow, drones);//go to the window that can add a drone
        //}

        private void ClosePageButton_Click(object sender, RoutedEventArgs e)
        {
            this.Content = "";
        }

        //private void DronesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    DroneToList droneToList = (DroneToList)DronesListView.SelectedItem;
        //    BO.Drone drone = bl.GetDrone(droneToList.Id);
        //    listWindow.ShowData.Content = new DronePage(listWindow, drone, drones);
        //}
    }
}

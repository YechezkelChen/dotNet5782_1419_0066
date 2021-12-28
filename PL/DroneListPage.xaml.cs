using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
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
        private BlApi.IBL bl = BlApi.BlFactory.GetBl();
        private ObservableCollection<DroneToList> drones;

        public DroneListPage(ObservableCollection<DroneToList> drones)
        {
            InitializeComponent();
            this.drones = drones;
            DronesListView.DataContext = this.drones;
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
                CopyPropertiesTo(drone, newDrone);
                newDrone.Location = new Location();
                CopyPropertiesTo(drone.Location, newDrone.Location);
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
            drones.Clear();
            var groups = bl.GetDronesByGroupStatus();
            foreach (var group in groups)
                foreach (var drone in group)
                {
                    DroneToList newDrone = new DroneToList();
                    CopyPropertiesTo(drone, newDrone);
                    newDrone.Location = new Location();
                    CopyPropertiesTo(drone.Location, newDrone.Location);
                    drones.Add(newDrone);
                }
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
            DronesData();
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

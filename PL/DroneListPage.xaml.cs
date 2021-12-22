﻿using System;
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
            foreach (var drone in bl.GetDrones())
            {
                DroneToList newDrone = new DroneToList();
                listWindow.CopyPropertiesTo(drone, newDrone);
                drones.Add(newDrone);
            }

            DronesListView.DataContext = drones;
            //ShowDronesAfterFiltering();
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }



        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowDronesAfterFiltering();
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowDronesAfterFiltering();
        }

        private void RefreshStatusButton_Click(object sender, RoutedEventArgs e)
        {
            StatusSelector.SelectedItem = null;
            ShowDronesAfterFiltering();
        }

        private void RefreshWeightButton_Click(object sender, RoutedEventArgs e)
        {
            WeightSelector.SelectedItem = null;
            ShowDronesAfterFiltering();
        }

        private void AddDrone_Click(object sender, RoutedEventArgs e)
        {
            listWindow.ShowData.Content = new DronePage(listWindow); //go to the window that can add a drone
            ShowDronesAfterFiltering();
        }

        private void ClosePageButton_Click(object sender, RoutedEventArgs e)
        {
            this.Content = "";
        }

        private void DronesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneToList droneToList = (DroneToList)DronesListView.SelectedItem;
            BO.Drone drone = bl.GetDrone(droneToList.Id);
            listWindow.ShowData.Content = new DronePage(listWindow, drone);
            ShowDronesAfterFiltering();
        }

        private void ShowDronesAfterFiltering()
        {
            IEnumerable<BO.DroneToList> drones = new List<BO.DroneToList>();
            IEnumerable<BO.DroneToList> dronesFiltering = new List<BO.DroneToList>();

            drones = bl.GetDrones();

            // Filtering of status.
            if (StatusSelector.SelectedItem == null)
                dronesFiltering = bl.GetDrones();
            else
                dronesFiltering = bl.GetDronesByStatus((BO.DroneStatuses)StatusSelector.SelectedItem);

            drones = dronesFiltering.ToList().FindAll(drone => drones.ToList().Find(d => d.Id == drone.Id) != null);

            // Filterig of max weight
            if (WeightSelector.SelectedItem == null)
                dronesFiltering = bl.GetDrones();
            else
                dronesFiltering = bl.GetDronesByMaxWeight((BO.WeightCategories)WeightSelector.SelectedItem);

            drones = dronesFiltering.ToList().FindAll(drone => drones.ToList().Find(d => d.Id == drone.Id) != null);
            
            // Show the list after the filtering
            DronesListView.ItemsSource = drones;
        }

    }
}

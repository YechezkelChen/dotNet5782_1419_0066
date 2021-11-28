using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IBL;
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        private IBL.IBL bl;

        IEnumerable<DroneToList> drones = new List<DroneToList>();
        public DroneListWindow(IBL.IBL ibl)
        {
            InitializeComponent();
            bl = ibl;
            drones = bl.GetDrones(drone => true);
            DronesListView.ItemsSource = drones;
            StatusSelctor.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            WeightSelctor.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        private void StatusSelctor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusSelctor.SelectedItem == null)
                drones = bl.GetDrones(drone => true);
            else
                drones = bl.GetDrones(drone => drone.Status == (DroneStatuses)Enum.Parse(typeof(DroneStatuses), StatusSelctor.SelectedItem.ToString()) && drones.ToList().Find(d => d.Id == drone.Id) != null);
            DronesListView.ItemsSource = drones;
        }

        private void WeightSelctor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WeightSelctor.SelectedItem == null)
                drones = bl.GetDrones(drone => true);
            else
            {
                WeightCategories maxWeightToShow = (WeightCategories)Enum.Parse(typeof(WeightCategories), WeightSelctor.SelectedItem.ToString());

                if (maxWeightToShow == WeightCategories.Heavy)
                    drones = bl.GetDrones(drone => true && drones.ToList().Find(d => d.Id == drone.Id) != null);//(etgar 2)
                else if (maxWeightToShow == WeightCategories.Medium)
                    drones = bl.GetDrones(drone => (drone.Weight == maxWeightToShow || drone.Weight == WeightCategories.Light) &&
                            drones.ToList().Find(d => d.Id == drone.Id) != null);//(etgar 2)
                else
                    drones = bl.GetDrones(drone => drone.Weight == maxWeightToShow &&
                            drones.ToList().Find(d => d.Id == drone.Id) != null);//(atgar 2)
            }
            DronesListView.ItemsSource = drones;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            StatusSelctor.SelectedIndex = -1;
            WeightSelctor.SelectedIndex = -1;
        }

        private void AddDrone_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(bl).ShowDialog(); //go to the window that can add a drone
            ShowDronesAfterActivitys();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DronesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneToList droneToList = (DroneToList)DronesListView.SelectedItem;
            Drone drone = bl.GetDrone(droneToList.Id);
            new DroneWindow(bl, drone).ShowDialog();
            ShowDronesAfterActivitys();

        }

        private void ShowDronesAfterActivitys()
        {
            drones = bl.GetDrones(drone => true);

            if (StatusSelctor.SelectedItem == null)
                drones = bl.GetDrones(drone => true);
            else
                drones = bl.GetDrones(drone => drone.Status == (DroneStatuses)Enum.Parse(typeof(DroneStatuses), StatusSelctor.SelectedItem.ToString()) && drones.ToList().Find(d => d.Id == drone.Id) != null);
            DronesListView.ItemsSource = drones;

            if (WeightSelctor.SelectedItem == null)
                drones = bl.GetDrones(drone => true);
            else
            {
                WeightCategories maxWeightToShow = (WeightCategories)Enum.Parse(typeof(WeightCategories), WeightSelctor.SelectedItem.ToString());

                if (maxWeightToShow == WeightCategories.Heavy)
                    drones = bl.GetDrones(drone => true && drones.ToList().Find(d => d.Id == drone.Id) != null);//(etgar 2)
                else if (maxWeightToShow == WeightCategories.Medium)
                    drones = bl.GetDrones(drone => (drone.Weight == maxWeightToShow || drone.Weight == WeightCategories.Light) &&
                            drones.ToList().Find(d => d.Id == drone.Id) != null);//(etgar 2)
                else
                    drones = bl.GetDrones(drone => drone.Weight == maxWeightToShow &&
                            drones.ToList().Find(d => d.Id == drone.Id) != null);//(atgar 2)
            }
            DronesListView.ItemsSource = drones;
        }
    }
}

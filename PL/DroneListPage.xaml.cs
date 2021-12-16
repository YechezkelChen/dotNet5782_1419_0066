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
using BO;


namespace PL
{
    /// <summary>
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListPage : Page
    {
        ListWindow listWindow;
        private BlApi.IBL bl;

        public DroneListPage(ListWindow Window, BlApi.IBL ibl)
        {
            InitializeComponent();
            listWindow = Window;
            bl = ibl;
            ShowDronesAfterFiltering();
            StatusSelctor.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            WeightSelctor.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        private void StatusSelctor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowDronesAfterFiltering();
        }

        private void WeightSelctor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowDronesAfterFiltering();
        }

        private void RefreshStatusButton_Click(object sender, RoutedEventArgs e)
        {
            StatusSelctor.SelectedItem = null;
            ShowDronesAfterFiltering();
        }

        private void RefreshWeightButton_Click(object sender, RoutedEventArgs e)
        {
            WeightSelctor.SelectedItem = null;
            ShowDronesAfterFiltering();
        }

        private void AddDrone_Click(object sender, RoutedEventArgs e)
        {
            listWindow.ShowData.Content = new DronePage(bl); //go to the window that can add a drone
            ShowDronesAfterFiltering();
        }

        private void ClosePageButton_Click(object sender, RoutedEventArgs e)
        {
            this.Content = "";
        }

        private void DronesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneToList droneToList = (DroneToList)DronesListView.SelectedItem;
            Drone drone = bl.GetDrone(droneToList.Id);
            listWindow.ShowData.Content = new DronePage(bl, drone);
            ShowDronesAfterFiltering();
        }

        private void ShowDronesAfterFiltering()
        {
            IEnumerable<DroneToList> drones = new List<DroneToList>();
            IEnumerable<DroneToList> dronesFiltering = new List<DroneToList>();

            drones = bl.GetDrones();

            // Filtering of status.
            if (StatusSelctor.SelectedItem == null)
                dronesFiltering = bl.GetDrones();
            else
                dronesFiltering = bl.GetDronesByStatus((DroneStatuses)StatusSelctor.SelectedItem);

            drones = dronesFiltering.ToList().FindAll(drone => drones.ToList().Find(d => d.Id == drone.Id) != null);

            // Filterig of max weight
            if (WeightSelctor.SelectedItem == null)
                dronesFiltering = bl.GetDrones();
            else
                dronesFiltering = bl.GetDronesByMaxWeight((WeightCategories)WeightSelctor.SelectedItem);

            drones = dronesFiltering.ToList().FindAll(drone => drones.ToList().Find(d => d.Id == drone.Id) != null);
            
            // Show the list after the filtering
            DronesListView.ItemsSource = drones;
        }

    }
}

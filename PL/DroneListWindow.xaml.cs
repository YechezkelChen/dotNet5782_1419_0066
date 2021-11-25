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
        public DroneListWindow(IBL.IBL ibl)
        {
            InitializeComponent();
            bl = ibl;
            DronesListView.ItemsSource = bl.GetDrones(drone => true);
            DroneSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            //MessageBox.Show(DroneSelector.ToString());
            //DroneSelector.ItemsSource = 
            //Status.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            //Weight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        private void ViewStatusDrone(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem statusDrone = new ComboBoxItem();
            //if (DroneSelector.SelectedItem == DroneStatus)
            //{
            //    if (Status.SelectedItem == Aviailable)
            //        DronesListView.ItemsSource = bl.GetDrones(drone => drone.Status == DroneStatuses.Available);
            //    if (Status.SelectedItem == Maintenance)
            //        DronesListView.ItemsSource = bl.GetDrones(drone => drone.Status == DroneStatuses.Maintenance);
            //    if (Status.SelectedItem == Delivery)
            //        DronesListView.ItemsSource = bl.GetDrones(drone => drone.Status == DroneStatuses.Delivery);
            //}

            //if (DroneSelector.SelectedItem == DroneWeight)
            //{
            //    if (Weight.SelectedItem == Light)
            //        DronesListView.ItemsSource = bl.GetDrones(drone => drone.Weight == WeightCategories.Light);
            //    if (Weight.SelectedItem == Medium)
            //        DronesListView.ItemsSource = bl.GetDrones(drone => drone.Weight == WeightCategories.Medium);
            //    if (Weight.SelectedItem == Heavy)
            //        DronesListView.ItemsSource = bl.GetDrones(drone => drone.Weight == WeightCategories.Heavy);
            //}
        }
    }
}

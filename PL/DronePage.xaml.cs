using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using PO;


namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DronePage : Page
    {
        private BlApi.IBL bl = BlApi.BlFactory.GetBl();
        private ListWindow listWindow;
        private ObservableCollection<DroneToList> drones;
        private BO.Drone drone;

        public DronePage(ListWindow window, ObservableCollection<DroneToList> drones)
        {
            InitializeComponent();
            listWindow = window;
            this.drones = drones;
            WeightComboBox.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            StationComboBox.ItemsSource = bl.GetStationsWithAvailableCharge();
            drone = new BO.Drone();

            // hidden irrelevant buttons 
            UpdateModelButton.Visibility = Visibility.Hidden;
            //SendToChargeButton.Visibility = Visibility.Hidden;
            ReleaseFromChargeButton.Visibility = Visibility.Hidden;
            ConnectParcelButton.Visibility = Visibility.Hidden;
            CollectParcelButton.Visibility = Visibility.Hidden;
            SupplyParcelButton.Visibility = Visibility.Hidden;
        }
        public DronePage(ListWindow window, BO.Drone drone, ObservableCollection<DroneToList> drones)
        {
            InitializeComponent();
            listWindow = window;
            this.drone = drone;
            this.drones = drones;
            DataDroneGrid.DataContext = drone;
            ActionsDroneGrid.DataContext = drone;
            AddButton.Visibility = Visibility.Hidden;
            //BlockingControls();
            ShowDronesAfterActions();
        }

        private void ClosePageButton_Click(object sender, RoutedEventArgs e)
        {
            this.Content = "";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            drone.Id = int.Parse(IdTextBox.Text);
            drone.Model = ModelTextBox.Text;
            drone.Weight = (BO.WeightCategories)WeightComboBox.SelectedItem;

            if (StationComboBox.ItemsSource == null)
                MessageBox.Show("There is no station with a free standing to put the drone for charging", "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);

            BO.StationToList stationCharge = new BO.StationToList();
            stationCharge = (BO.StationToList)StationComboBox.SelectedItem;

            try
            {
                bl.AddDrone(drone, stationCharge.Id);
            }
            catch (BO.IdException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (BO.ModelException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (BO.ChargeSlotsException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (BO.StatusDroneException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (BO.BatteryDroneException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("The add is success!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            DroneToList newDrone = new DroneToList();
            newDrone.Location = new Location();
            listWindow.CopyPropertiesTo(bl.GetDrone(drone.Id), newDrone);
            drones.Add(newDrone);
            this.Content = "";
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Content = "";
        }

        private void UpdateModelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.UpdateDroneModel(drone.Id, drone.Model);
            }
            catch (BO.IdException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (BO.ModelException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("The update is success!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            drone = bl.GetDrone(drone.Id);

            ShowDronesAfterActions();
        }

        private void SendToChargeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.SendDroneToDroneCharge(drone.Id);
            }
            catch (BO.IdException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (BO.StatusDroneException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (BO.BatteryDroneException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("The send success!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            drone = bl.GetDrone(drone.Id);

            foreach (var elementStation in bl.GetStations())
            {
                BO.Station station = bl.GetStation(elementStation.Id);
                if (drone.Location.Longitude == station.Location.Longitude && drone.Location.Latitude == station.Location.Latitude)
                    PresentStationLabel.Text = station.Id.ToString();
            }

            SendToChargeButton.Visibility = Visibility.Hidden;
            ReleaseFromChargeButton.Visibility = Visibility.Visible;
            ShowDronesAfterActions();
        }

        private void ReleaseFromChargeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.ReleaseDroneFromDroneCharge(drone.Id);
            }
            catch (BO.IdException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (BO.StatusDroneException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("The release success!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            drone = bl.GetDrone(drone.Id);

            ReleaseFromChargeButton.Visibility = Visibility.Hidden;
            SendToChargeButton.Visibility = Visibility.Visible;
            ShowDronesAfterActions();
        }

        private void ConnectParcelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.ConnectParcelToDrone(drone.Id);
            }
            catch (BO.IdException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (BO.StatusDroneException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (BO.NoPackagesToDroneException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("The connection success!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            drone = bl.GetDrone(drone.Id);

            ConnectParcelButton.Visibility = Visibility.Hidden;
            CollectParcelButton.Visibility = Visibility.Visible;
            ShowDronesAfterActions();
        }

        private void CollectParcelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.CollectionParcelByDrone(drone.Id);
            }
            catch (BO.IdException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (BO.StatusDroneException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("The collection success!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            drone = bl.GetDrone(drone.Id);

            CollectParcelButton.Visibility = Visibility.Hidden;
            SupplyParcelButton.Visibility = Visibility.Visible;
            ShowDronesAfterActions();
        }

        private void SupplyParcelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.SupplyParcelByDrone(drone.Id);
            }
            catch (BO.IdException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (BO.StatusDroneException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("The supply success!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            drone = bl.GetDrone(drone.Id);

            SupplyParcelButton.Visibility = Visibility.Hidden;
            ConnectParcelButton.Visibility = Visibility.Visible;
            ShowDronesAfterActions();
        }

        private void BlockingControls()
        {
            //IdTextBox.IsEnabled = false;
            WeightComboBox.IsEnabled = false;
            PresentWeightLabel.IsEnabled = false;
            StationComboBox.IsEnabled = false;
            PresentStationLabel.IsEnabled = false;
            BatteryTextBox.IsEnabled = false;
            StatusTextBox.IsEnabled = false;
            LocationTextBox.IsEnabled = false;

            // hidden irrelevant bottuns
            AddButton.Visibility = Visibility.Hidden;
            CancelButton.Visibility = Visibility.Hidden;
        }

        private void ShowDronesAfterActions()
        {
            //IdTextBox.Text = drone.Id.ToString();
            //ModelTextBox.Text = drone.Model;
           // PresentWeightLabel.Text = drone.Weight.ToString();

            if (drone.Status == BO.DroneStatuses.Maintenance)
                foreach (var elementStation in bl.GetStations())
                {
                    BO.Station station = bl.GetStation(elementStation.Id);
                    if (drone.Location.Longitude == station.Location.Longitude && drone.Location.Latitude == station.Location.Latitude)
                        PresentStationLabel.Text = station.Id.ToString();
                }

            //BatteryTextBox.Text = drone.Battery.ToString();
            //StatusTextBox.Text = drone.Status.ToString();
            //LocationTextBox.Text = drone.Location.ToString();

            //if (drone.Status == BO.DroneStatuses.Maintenance)
            //    SendToChargeButton.Visibility = Visibility.Hidden;
            //else
                ReleaseFromChargeButton.Visibility = Visibility.Hidden;

            if (drone.Status != BO.DroneStatuses.Delivery)
            {
                CollectParcelButton.Visibility = Visibility.Hidden;
                SupplyParcelButton.Visibility = Visibility.Hidden;
            }
            else
            {
                if (drone.ParcelByTransfer.OnTheWay != true)
                {
                    ConnectParcelButton.Visibility = Visibility.Hidden;
                    SupplyParcelButton.Visibility = Visibility.Hidden;
                }
                else
                {
                    ConnectParcelButton.Visibility = Visibility.Hidden;
                    CollectParcelButton.Visibility = Visibility.Hidden;
                }
            }
        }

        private void ParcelDataButton_Click(object sender, RoutedEventArgs e)
        {
            listWindow.ShowData.Content = new ParcelInDronePage(listWindow, drone);
        }
    }
}

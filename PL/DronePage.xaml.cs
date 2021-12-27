using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
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
        private ObservableCollection<DroneToList> drones;
        private Drone drone = new Drone();

        public DronePage(ObservableCollection<DroneToList> drones)
        {
            InitializeComponent();
            this.drones = drones;

            WeightComboBox.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            StationComboBox.ItemsSource = bl.GetStationsWithAvailableCharge();

            UpdateModelButton.Visibility = Visibility.Hidden; // help for all the things in xmal
        }

        public DronePage(Drone drone, ObservableCollection<DroneToList> drones)
        {
            InitializeComponent();
            this.drones = drones;
            this.drone = drone;

            DataDroneGrid.DataContext = drone;
            ActionsDroneGrid.DataContext = drone;

            AddButton.Visibility = Visibility.Hidden; // help for all the things in xmal

            FixButtonsAfterActions();
        }

        private void ClosePageButton_Click(object sender, RoutedEventArgs e)
        {
            this.Content = "";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            BO.Drone boDrone = new BO.Drone();
            boDrone.Id = int.Parse(IdTextBox.Text);
            boDrone.Model = ModelTextBox.Text;
            boDrone.Weight = (BO.WeightCategories)WeightComboBox.SelectedItem;

            if (StationComboBox.ItemsSource == null)
                MessageBox.Show("There is no station with a free standing to put the drone for charging", "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);

            BO.StationToList stationCharge = new BO.StationToList();
            stationCharge = (BO.StationToList)StationComboBox.SelectedItem;

            try
            {
                bl.AddDrone(boDrone, stationCharge.Id);
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

            MessageBox.Show("You have a new drone!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Update the view
            DroneToList newDrone = new DroneToList();
            newDrone.Location = new Location();
            boDrone = bl.GetDrone(boDrone.Id);
            CopyPropertiesTo(boDrone, newDrone);
            CopyPropertiesTo(boDrone.Location, newDrone.Location);
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

            FixButtonsAfterActions();
            UpdateListDrones(drone);
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
            
            // Update the view
            BO.Drone boDrone = bl.GetDrone(drone.Id);
            drone.Battery = boDrone.Battery;
            drone.Status = DroneStatuses.Maintenance;
            CopyPropertiesTo(boDrone.Location, drone.Location);

            FixButtonsAfterActions();
            UpdateListDrones(drone);
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

            // Update the view
            BO.Drone boDrone = bl.GetDrone(drone.Id);
            drone.Battery = boDrone.Battery;
            drone.Status = DroneStatuses.Available;

            FixButtonsAfterActions();
            UpdateListDrones(drone);
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

            // Update the view
            BO.Drone boDrone = bl.GetDrone(drone.Id);
            drone.Status = DroneStatuses.Delivery;
            drone.ParcelByTransfer = new ParcelInTransfer();
            CopyPropertiesTo(boDrone.ParcelByTransfer, drone.ParcelByTransfer);

            FixButtonsAfterActions();
            UpdateListDrones(drone);
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
           
            // Update the view
            BO.Drone boDrone = bl.GetDrone(drone.Id);
            drone.Battery = boDrone.Battery;
            drone.Location = new Location();
            CopyPropertiesTo(boDrone.Location, drone.Location);
            drone.ParcelByTransfer.OnTheWay = true;

            FixButtonsAfterActions();
            UpdateListDrones(drone);
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

            // Update the view
            BO.Drone boDrone = bl.GetDrone(drone.Id);
            drone.Battery = boDrone.Battery;
            drone.Status = DroneStatuses.Available;
            drone.Location = new Location();
            CopyPropertiesTo(boDrone.Location, drone.Location);
            drone.ParcelByTransfer = null;

            FixButtonsAfterActions();
            UpdateListDrones(drone);
        }

        private void FixButtonsAfterActions()
        {
            if (drone.Status == DroneStatuses.Maintenance)
                foreach (var elementStation in bl.GetStations())
                {
                    BO.Station station = bl.GetStation(elementStation.Id);
                    BO.DroneInCharge droneInCharge = station.DronesInCharges.FirstOrDefault(elementDrone => elementDrone.Id == drone.Id);
                    if (!(droneInCharge is null))
                        PresentStationLabel.Text = station.Id.ToString();
                }
            else
                PresentStationLabel.Text = "";

            //BatteryTextBox.Text = drone.Battery.ToString();
            //StatusTextBox.Text = drone.Status.ToString();
            //LocationTextBox.Text = drone.Location.ToString();

            if (drone.Status != DroneStatuses.Available)
                SendToChargeButton.Visibility = Visibility.Hidden;
            if (drone.Status != DroneStatuses.Maintenance)
                ReleaseFromChargeButton.Visibility = Visibility.Hidden;

            // Hidden all
            ConnectParcelButton.Visibility = Visibility.Hidden;
            CollectParcelButton.Visibility = Visibility.Hidden;
            SupplyParcelButton.Visibility = Visibility.Hidden;
            // then:

            if (drone.Status == DroneStatuses.Available)
                ConnectParcelButton.Visibility = Visibility.Visible;
            if (drone.Status == DroneStatuses.Delivery)
            {
                if (drone.ParcelByTransfer.OnTheWay == false)
                    CollectParcelButton.Visibility = Visibility.Visible;
                else
                    SupplyParcelButton.Visibility = Visibility.Visible;
            }
        }

        private void UpdateListDrones(Drone updateDrone)
        {
            for (int i = 0; i < drones.Count(); i++)
                if (drones[i].Id == updateDrone.Id)
                {
                    DroneToList newDrone = drones[i];
                    CopyPropertiesTo(updateDrone, newDrone);
                    newDrone.Location = new Location();
                    CopyPropertiesTo(updateDrone.Location, newDrone.Location);
                    drones[i] = newDrone;
                }
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

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
using BL;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DronePage : Page
    {
        private BlApi.IBL bl;
        private Drone drone;

        public DronePage(BlApi.IBL ibl)
        {
            InitializeComponent();
            bl = ibl;
            WeightComboBox.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            StationComboBox.ItemsSource = bl.GetStationsWithAvailableCharge();
            drone = new Drone();

            // hidden irrelevant drone data
            PresentWeightLabel.Visibility = Visibility.Hidden;
            PresentStationLabel.Visibility = Visibility.Hidden;
            BatteryLabel.Visibility = Visibility.Hidden;
            BatteryTextBox.Visibility = Visibility.Hidden;
            StatusLabel.Visibility = Visibility.Hidden;
            StatusTextBox.Visibility = Visibility.Hidden;
            LocationLabel.Visibility = Visibility.Hidden;
            LocationTextBox.Visibility = Visibility.Hidden;

            // hidden irrelevant bottuns 
            UpdateModelButton.Visibility = Visibility.Hidden;
            SendToChargeButton.Visibility = Visibility.Hidden;
            RealeseFromChargeButton.Visibility = Visibility.Hidden;
            SendToDeliveryButton.Visibility = Visibility.Hidden;
            CollectParcelButton.Visibility = Visibility.Hidden;
            SupplyParcelButton.Visibility = Visibility.Hidden;
        }
        public DronePage(BlApi.IBL ibl, Drone droneHelp)
        {
            InitializeComponent();
            bl = ibl;
            drone = droneHelp;
            DataDroneGrid.DataContext = drone;
            BlockingControls();
            ShowDronesAfterActions();
        }

        private void ClosePageButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void DataDroneGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            int id;
            if (IdTextBox.Text == "" || !IdTextBox.Text.All(char.IsDigit))
                id = 0;
            else
                id = int.Parse(IdTextBox.Text);

            if (id < 100000 || id > 999999) // Check that it's 6 digits.
            {
                IdTextBox.Foreground = Brushes.Red;
                drone.Id = 0;
            }
            else
            {
                drone.Id = id;
                IdTextBox.Foreground = Brushes.SlateGray;
            }
        }

        private void ModelTextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            string model = ModelTextBox.Text;
            if (model == "")
                ModelTextBox.Foreground = Brushes.Red;
            else
            {
                drone.Model = model;
                ModelTextBox.Foreground = Brushes.SlateGray;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (drone.Id == 0)
            {
                MessageBoxResult result = MessageBox.Show("Id is illegal!, please enter legal id with 6 digits to continue, or Cancel to stop the adding", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        return;
                    //case MessageBoxResult.Cancel:
                    //    this.Close();
                    //    return;
                    //default:
                    //    this.Close();
                    //    return;
                }
            }

            if (drone.Model == "")
            {
                MessageBoxResult result = MessageBox.Show("Model must have value, please enter legal model to continue, or Cancel to stop the adding", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        return;
                    //case MessageBoxResult.Cancel:
                    //    this.Close();
                    //    return;
                    //default:
                    //    this.Close();
                    //    return;
                }
            }

            if (WeightComboBox.SelectedItem == null)
            {
                MessageBoxResult result = MessageBox.Show("Weight must have value, please choose weight to continue, or Cancel to stop the adding", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        return;
                    //case MessageBoxResult.Cancel:
                    //    this.Close();
                    //    return;
                    //default:
                    //    this.Close();
                    //    return;
                }
            }
            else
                drone.Weight = (WeightCategories)WeightComboBox.SelectedItem;

            if (StationComboBox.ItemsSource == null)
                MessageBox.Show("There is no station with a free standing to put the drone for charging", "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);

            StationToList stationCharge = new StationToList();
            if (StationComboBox.SelectedItem == null)
            {
                MessageBoxResult result = MessageBox.Show("Drone must have station to charge, please choose station to continue, or Cancel to stop the adding", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        return;
                    //case MessageBoxResult.Cancel:
                    //    this.Close();
                    //    return;
                    //default:
                    //    this.Close();
                    //    return;
                }
            }
            else
                stationCharge = (StationToList)StationComboBox.SelectedItem;

            try
            {
                bl.AddDrone(drone, stationCharge.Id);
            }
            catch (IdException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (ModelException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (ChargeSlotsException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (StatusDroneException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (BatteryDroneException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("The add is succesid!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            //CloseWindowButton.Visibility = Visibility.Hidden;
            //this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            //CloseWindowButton.Visibility = Visibility.Hidden;
            //this.Close();
        }

        private void UpdateModelButton_Click(object sender, RoutedEventArgs e)
        {
            if (ModelTextBox.Foreground == Brushes.Red)
            {
                MessageBoxResult result = MessageBox.Show("Model must have value, please enter legal model to continue, or Cancel to stop the update", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        return;
                    case MessageBoxResult.Cancel:
                        ModelTextBox.Clear();
                        return;
                    default:
                        ModelTextBox.Clear();
                        return;
                }
            }

            try
            {
                bl.UpdateDroneModel(drone.Id, drone.Model);
            }
            catch (IdException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (ModelException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            MessageBox.Show("The update is succesid!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            drone = bl.GetDrone(drone.Id);

            ShowDronesAfterActions();
        }

        private void SendToChargeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.SendDroneToDroneCharge(drone.Id);
            }
            catch (IdException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (StatusDroneException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (BatteryDroneException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("The send succesid!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            drone = bl.GetDrone(drone.Id);

            foreach (var elementStation in bl.GetStations())
            {
                Station station = bl.GetStation(elementStation.Id);
                if (drone.Location.Longitude == station.Location.Longitude && drone.Location.Latitude == station.Location.Latitude)
                    PresentStationLabel.Text = station.Id.ToString();
            }

            SendToChargeButton.Visibility = Visibility.Hidden;
            RealeseFromChargeButton.Visibility = Visibility.Visible;
            ShowDronesAfterActions();
        }

        private void RealeseFromChargeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.ReleaseDroneFromDroneCharge(drone.Id);
            }
            catch (IdException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (StatusDroneException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("The realese succesid!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            drone = bl.GetDrone(drone.Id);

            RealeseFromChargeButton.Visibility = Visibility.Hidden;
            SendToChargeButton.Visibility = Visibility.Visible;
            ShowDronesAfterActions();
        }

        private void SendToDeliveryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.ConnectParcelToDrone(drone.Id);
            }
            catch (IdException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (StatusDroneException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (NoPackagesToDroneException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("The connection succesid!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            drone = bl.GetDrone(drone.Id);

            SendToDeliveryButton.Visibility = Visibility.Hidden;
            CollectParcelButton.Visibility = Visibility.Visible;
            ShowDronesAfterActions();
        }

        private void CollectParcelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.CollectionParcelByDrone(drone.Id);
            }
            catch (IdException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (StatusDroneException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("The collection succesid!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
            catch (IdException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (StatusDroneException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("The supply succesid!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            drone = bl.GetDrone(drone.Id);

            SupplyParcelButton.Visibility = Visibility.Hidden;
            SendToDeliveryButton.Visibility = Visibility.Visible;
            ShowDronesAfterActions();
        }

        private void BlockingControls()
        {
            IdTextBox.IsEnabled = false;
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
            ModelTextBox.Text = drone.Model;
            PresentWeightLabel.Text = drone.Weight.ToString();

            if (drone.Status == DroneStatuses.Maintenance)
                foreach (var elementStation in bl.GetStations())
                {
                    Station station = bl.GetStation(elementStation.Id);
                    if (drone.Location.Longitude == station.Location.Longitude && drone.Location.Latitude == station.Location.Latitude)
                        PresentStationLabel.Text = station.Id.ToString();
                }

            BatteryTextBox.Text = drone.Battery.ToString();
            StatusTextBox.Text = drone.Status.ToString();
            LocationTextBox.Text = drone.Location.ToString();

            if (drone.Status == DroneStatuses.Maintenance)
                SendToChargeButton.Visibility = Visibility.Hidden;
            else
                RealeseFromChargeButton.Visibility = Visibility.Hidden;

            if (drone.Status != DroneStatuses.Delivery)
            {
                CollectParcelButton.Visibility = Visibility.Hidden;
                SupplyParcelButton.Visibility = Visibility.Hidden;
            }
            else
            {
                if (drone.ParcelByTransfer.OnTheWay != true)
                {
                    SendToDeliveryButton.Visibility = Visibility.Hidden;
                    SupplyParcelButton.Visibility = Visibility.Hidden;
                }
                else
                {
                    SendToDeliveryButton.Visibility = Visibility.Hidden;
                    CollectParcelButton.Visibility = Visibility.Hidden;
                }
            }
        }

        private void ShowParcelInDrone_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new ParcelInDronePage(drone);
        }
    }
}

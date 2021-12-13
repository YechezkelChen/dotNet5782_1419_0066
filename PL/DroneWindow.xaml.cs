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
    public partial class DroneWindow : Window
    {
        private BlApi.IBL bl;
        private Drone drone;

        public DroneWindow(BlApi.IBL ibl)
        {
            InitializeComponent();
            bl = ibl;
            WeightComboBox.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            StationComboBox.ItemsSource = bl.GetStationsCharge();
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

            // hidden data parcel
            DataParcelGrid.Visibility = Visibility.Hidden;

            // hidden irrelevant bottuns 
            UpdateModelButton.Visibility = Visibility.Hidden;
            SendToChargeButton.Visibility = Visibility.Hidden;
            RealeseFromChargeButton.Visibility = Visibility.Hidden;
            SendToDeliveryButton.Visibility = Visibility.Hidden;
            CollectParcelButton.Visibility = Visibility.Hidden;
            SupplyParcelButton.Visibility = Visibility.Hidden;
        }

        public DroneWindow(BlApi.IBL ibl, Drone droneHelp)
        {
            InitializeComponent();
            bl = ibl;
            drone = droneHelp;

            // print data drone
            IdTextBox.Text = drone.Id.ToString();
            IdTextBox.IsEnabled = false;
            ModelTextBox.Text = drone.Model;
            PresentWeightLabel.Text = drone.Weight.ToString();
            WeightComboBox.IsEnabled = false;
            PresentWeightLabel.IsEnabled = false;

            if (drone.Status == DroneStatuses.Maintenance)
                foreach (var elementStation in bl.GetStations())
                {
                    Station station = bl.GetStation(elementStation.Id);
                    if (drone.Location.Longitude == station.Location.Longitude && drone.Location.Latitude == station.Location.Latitude)
                        PresentStationLabel.Text = station.Id.ToString();
                }

            StationComboBox.IsEnabled = false;
            PresentStationLabel.IsEnabled = false;
            BatteryTextBox.Text = drone.Battery.ToString();
            BatteryTextBox.IsEnabled = false;
            StatusTextBox.Text = drone.Status.ToString();
            StatusTextBox.IsEnabled = false;
            LocationTextBox.Text = drone.Location.ToString();
            LocationTextBox.IsEnabled = false;

            // print data parcel in drone
            IdParcelTextBox.Text = drone.ParcelByTransfer.Id.ToString();
            IdParcelTextBox.IsEnabled = false;
            StatusParcelTextBox.Text = drone.ParcelByTransfer.Status.ToString();
            StatusParcelTextBox.IsEnabled = false;
            PriorityParcelTextBox.Text = drone.ParcelByTransfer.Priority.ToString();
            PriorityParcelTextBox.IsEnabled = false;
            WeightParcelTextBox.Text = drone.ParcelByTransfer.Weight.ToString();
            WeightParcelTextBox.IsEnabled = false;
            if (drone.ParcelByTransfer.Id != 0)
            {
                SenderInParcelTextBox.Text = drone.ParcelByTransfer.SenderInParcel.ToString();
                PickUpLocationParcelTextBox.Text = drone.ParcelByTransfer.PickUpLocation.ToString();
                ReceiverInParcelTextBox.Text = drone.ParcelByTransfer.ReceiverInParcel.ToString();
                TargetLocationParcelTextBox.Text = drone.ParcelByTransfer.TargetLocation.ToString();
                DistanceOfTransferTextBox.Text = drone.ParcelByTransfer.DistanceOfTransfer.ToString();
            }
            SenderInParcelTextBox.IsEnabled = false;
            PickUpLocationParcelTextBox.IsEnabled = false;
            ReceiverInParcelTextBox.IsEnabled = false;
            TargetLocationParcelTextBox.IsEnabled = false;
            DistanceOfTransferTextBox.IsEnabled = false;

            // hidden irrelevant bottuns
            AddButton.Visibility = Visibility.Hidden;
            CancelButton.Visibility = Visibility.Hidden;

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
                if (drone.ParcelByTransfer.Status != true)
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

        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            CloseWindowButton.Visibility = Visibility.Hidden;
            this.Close();
        }

        private void CloseWithSpecialButton(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (CloseWindowButton.Visibility != Visibility.Hidden)
                e.Cancel = true;
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
                    case MessageBoxResult.Cancel:
                        this.Close();
                        return;
                    default:
                        this.Close();
                        return;
                }
            }

            if (drone.Model == "")
            {
                MessageBoxResult result = MessageBox.Show("Model must have value, please enter legal model to continue, or Cancel to stop the adding", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        return;
                    case MessageBoxResult.Cancel:
                        this.Close();
                        return;
                    default:
                        this.Close();
                        return;
                }
            }

            if (WeightComboBox.SelectedItem == null)
            {
                MessageBoxResult result = MessageBox.Show("Weight must have value, please choose weight to continue, or Cancel to stop the adding", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        return;
                    case MessageBoxResult.Cancel:
                        this.Close();
                        return;
                    default:
                        this.Close();
                        return;
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
                    case MessageBoxResult.Cancel:
                        this.Close();
                        return;
                    default:
                        this.Close();
                        return;
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
            CloseWindowButton.Visibility = Visibility.Hidden;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CloseWindowButton.Visibility = Visibility.Hidden;
            this.Close();
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

            ModelTextBox.Text = drone.Model;
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

            BatteryTextBox.Text = drone.Battery.ToString();
            StatusTextBox.Text = drone.Status.ToString();
            LocationTextBox.Text = drone.Location.ToString();

            SendToChargeButton.Visibility = Visibility.Hidden;
            RealeseFromChargeButton.Visibility = Visibility.Visible;
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

            PresentStationLabel.Text = "";
            BatteryTextBox.Text = drone.Battery.ToString();
            StatusTextBox.Text = drone.Status.ToString();
            LocationTextBox.Text = drone.Location.ToString();

            RealeseFromChargeButton.Visibility = Visibility.Hidden;
            SendToChargeButton.Visibility = Visibility.Visible;
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

            BatteryTextBox.Text = drone.Battery.ToString();
            StatusTextBox.Text = drone.Status.ToString();
            LocationTextBox.Text = drone.Location.ToString();

            IdParcelTextBox.Text = drone.ParcelByTransfer.Id.ToString();
            StatusParcelTextBox.Text = drone.ParcelByTransfer.Status.ToString();
            PriorityParcelTextBox.Text = drone.ParcelByTransfer.Priority.ToString();
            WeightParcelTextBox.Text = drone.ParcelByTransfer.Weight.ToString();
            SenderInParcelTextBox.Text = drone.ParcelByTransfer.SenderInParcel.ToString();
            PickUpLocationParcelTextBox.Text = drone.ParcelByTransfer.PickUpLocation.ToString();
            ReceiverInParcelTextBox.Text = drone.ParcelByTransfer.ReceiverInParcel.ToString();
            TargetLocationParcelTextBox.Text = drone.ParcelByTransfer.TargetLocation.ToString();
            DistanceOfTransferTextBox.Text = drone.ParcelByTransfer.DistanceOfTransfer.ToString();

            SendToDeliveryButton.Visibility = Visibility.Hidden;
            CollectParcelButton.Visibility = Visibility.Visible;
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

            BatteryTextBox.Text = drone.Battery.ToString();
            StatusTextBox.Text = drone.Status.ToString();
            LocationTextBox.Text = drone.Location.ToString();

            StatusParcelTextBox.Text = drone.ParcelByTransfer.Status.ToString();

            CollectParcelButton.Visibility = Visibility.Hidden;
            SupplyParcelButton.Visibility = Visibility.Visible;
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

            BatteryTextBox.Text = drone.Battery.ToString();
            StatusTextBox.Text = drone.Status.ToString();
            LocationTextBox.Text = drone.Location.ToString();

            IdParcelTextBox.Text = "";
            StatusParcelTextBox.Text = "";
            PriorityParcelTextBox.Text = "";
            WeightParcelTextBox.Text = "";
            SenderInParcelTextBox.Text = "";
            PickUpLocationParcelTextBox.Text = "";
            ReceiverInParcelTextBox.Text = "";
            TargetLocationParcelTextBox.Text = "";
            DistanceOfTransferTextBox.Text = "";

            SupplyParcelButton.Visibility = Visibility.Hidden;
            SendToDeliveryButton.Visibility = Visibility.Visible;
        }
    }
}

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
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private IBL.IBL bl;
        private Drone drone;

        public DroneWindow(IBL.IBL ibl)
        {
            InitializeComponent();
            bl = ibl;
            WeightComboBox.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            StationComboBox.ItemsSource = bl.GetStationsCharge();
            drone = new Drone();

            // hidden irrelevant drone data
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
            //TextUpdateModelLabel.Visibility = Visibility.Hidden;
            //GetUpdateModelTextBox.Visibility = Visibility.Hidden;
            SendToChargeButton.Visibility = Visibility.Hidden;
            RealeseFromChargeButton.Visibility = Visibility.Hidden;
            // HoursOfChargeLabel.Visibility = Visibility.Hidden;
            //HoursOfChargeTextBox.Visibility = Visibility.Hidden;
            SendToDeliveryButton.Visibility = Visibility.Hidden;
            CollectParcelButton.Visibility = Visibility.Hidden;
            SupplyParcelButton.Visibility = Visibility.Hidden;

           // textToDisplay.IsReadOnly = true;
        }

        public DroneWindow(IBL.IBL ibl, Drone droneHelp)
        {
            InitializeComponent();
            bl = ibl;
            drone = droneHelp;

            // print data drone
            IdTextBox.Text = drone.Id.ToString();
            IdTextBox.IsReadOnly = true;
            ModelTextBox.Text = drone.Model;
            WeightComboBox.SelectedItem = drone.Weight.ToString();
            WeightComboBox.IsReadOnly = true;
            if(drone.Status == DroneStatuses.Maintenance) // שגיאה??
                StationComboBox.SelectedItem = bl.GetStations().ToList().Find(station => drone.Location == bl.GetStation(station.Id).Location).Id;
            StationComboBox.IsReadOnly = true;
            BatteryTextBox.Text = drone.Battery.ToString();
            BatteryTextBox.IsReadOnly = true;
            StatusTextBox.Text = drone.Status.ToString();
            StatusTextBox.IsReadOnly = true;
            LocationTextBox.Text = drone.Location.ToString();
            LocationTextBox.IsReadOnly = true;

            // print data parcel in drone
            IdParcelTextBox.Text = drone.ParcelByTransfer.Id.ToString();
            IdParcelTextBox.IsReadOnly = true;
            StatusParcelTextBox.Text = drone.ParcelByTransfer.Status.ToString();
            StatusParcelTextBox.IsReadOnly = true;
            PriorityParcelTextBox.Text = drone.ParcelByTransfer.Priority.ToString();
            PriorityParcelTextBox.IsReadOnly = true;
            WeightParcelTextBox.Text = drone.ParcelByTransfer.Weight.ToString();
            WeightParcelTextBox.IsReadOnly = true;
            if (drone.ParcelByTransfer != null)
            {
                SenderInParcelTextBox.Text = drone.ParcelByTransfer.SenderInParcel.ToString();
                SenderInParcelTextBox.IsReadOnly = true;
                PickUpLocationParcelTextBox.Text = drone.ParcelByTransfer.PickUpLocation.ToString();
                PickUpLocationParcelTextBox.IsReadOnly = true;
                ReceiverInParcelTextBox.Text = drone.ParcelByTransfer.ReceiverInParcel.ToString();
                ReceiverInParcelTextBox.IsReadOnly = true;
                TargetLocationParcelTextBox.Text = drone.ParcelByTransfer.TargetLocation.ToString();
                TargetLocationParcelTextBox.IsReadOnly = true;
                DistanceOfTransferTextBox.Text = drone.ParcelByTransfer.DistanceOfTransfer.ToString();
                DistanceOfTransferTextBox.IsReadOnly = true;
            }

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
                if (drone.ParcelByTransfer.Status == true)
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
            catch (DroneException ex)
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
            catch (DroneException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("The update is succesid!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            drone = bl.GetDrone(drone.Id);
            DataDroneLabel.Content = drone;
        }

        private void SendToChargeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.SendDroneToDroneCharge(drone.Id);
            }
            catch (DroneException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("The send succesid!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            drone = bl.GetDrone(drone.Id);
            DataDroneLabel.Content = drone;
        }

        private void RealeseFromChargeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.ReleaseDroneFromDroneCharge(drone.Id);
            }
            catch (DroneException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("The realese succesid!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            drone = bl.GetDrone(drone.Id);
            DataDroneLabel.Content = drone;
        }

        private void SendToDeliveryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.ConnectParcelToDrone(drone.Id);
            }
            catch (DroneException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (ParcelException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("The connection succesid!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            drone = bl.GetDrone(drone.Id);
            DataDroneLabel.Content = drone;

            SendToDeliveryButton.Visibility = Visibility.Hidden;
            CollectParcelButton.Visibility = Visibility.Visible;
        }

        private void CollectParcelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.CollectionParcelByDrone(drone.Id);
            }
            catch (DroneException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("The collection succesid!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            drone = bl.GetDrone(drone.Id);
            DataDroneLabel.Content = drone;

            CollectParcelButton.Visibility = Visibility.Hidden;
            SupplyParcelButton.Visibility = Visibility.Visible;
        }

        private void SupplyParcelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.SupplyParcelByDrone(drone.Id);
            }
            catch (DroneException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (ParcelException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("The supply succesid!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            drone = bl.GetDrone(drone.Id);
            DataDroneLabel.Content = drone;

            SupplyParcelButton.Visibility = Visibility.Hidden;
            SendToDeliveryButton.Visibility = Visibility.Visible;
        }
    }
}

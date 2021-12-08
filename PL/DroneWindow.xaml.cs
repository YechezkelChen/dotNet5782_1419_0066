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
            UpdateDroneGrid.Visibility = Visibility.Hidden;
            GetWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            GetStation.ItemsSource = bl.GetStationsCharge();
            drone = new Drone();
        }

        public DroneWindow(IBL.IBL ibl, Drone droneHelp)
        {
            InitializeComponent();
            bl = ibl;
            drone = droneHelp;
            AddDroneGrid.Visibility = Visibility.Hidden;
            DataDroneLabel.Content = "\n" + drone;
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

        private void GetId_MouseLeave(object sender, MouseEventArgs e)
        {
            int id;
            if (GetId.Text == "" || !GetId.Text.All(char.IsDigit))
                id = 0;
            else
                id = int.Parse(GetId.Text);

            if (id < 100000 || id > 999999) // Check that it's 6 digits.
            {
                GetId.Background = Brushes.DarkRed;
                drone.Id = 0;
            }
            else
            {
                drone.Id = id;
                GetId.Background = Brushes.White;
            }
        }

        private void GetModel_MouseLeave(object sender, MouseEventArgs e)
        {
            string model = GetModel.Text;
            if (model == "")
                GetModel.Background = Brushes.DarkRed;
            else
            {
                drone.Model = model;
                GetModel.Background = Brushes.White;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (drone.Id == 0)
            {
                MessageBoxResult result = MessageBox.Show("Id is illegal!, please enter legal id to continue, or Cancel to stop the adding", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
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

            if (GetWeight.SelectedItem == null)
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
                drone.Weight = (WeightCategories)GetWeight.SelectedItem;

            if (GetStation.ItemsSource == null)
                MessageBox.Show("There is no station with a free standing to put the drone for charging", "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);

            StationToList stationCharge = new StationToList();
            if (GetStation.SelectedItem == null)
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
                stationCharge = (StationToList)GetStation.SelectedItem;

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
            if (GetUpdateModelTextBox.Background == Brushes.DarkRed)
            {
                MessageBoxResult result = MessageBox.Show("Model must have value, please enter legal model to continue, or Cancel to stop the update", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        return;
                    case MessageBoxResult.Cancel:
                        GetUpdateModelTextBox.Clear();
                        return;
                    default:
                        GetUpdateModelTextBox.Clear();
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

        private void GetUpdateModelTextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            string model = GetUpdateModelTextBox.Text;
            if (model == "")
                GetUpdateModelTextBox.Background = Brushes.DarkRed;
            else
            {
                drone.Model = model;
                GetUpdateModelTextBox.Background = Brushes.White;
            }
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
            int Hours;
            if (HoursOfChargeTextBox.Background == Brushes.DarkRed || HoursOfChargeTextBox.Text == "")
            {
                MessageBox.Show("Enter amount of charge!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
                Hours = int.Parse(HoursOfChargeTextBox.Text);

            try
            {
                bl.ReleaseDroneFromDroneCharge(drone.Id, Hours);
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

        private void HoursOfChargeTextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            int Hours;
            if (HoursOfChargeTextBox.Text == "" || !HoursOfChargeTextBox.Text.All(char.IsDigit))
                Hours = -1;
            else
                Hours = int.Parse(HoursOfChargeTextBox.Text);

            if (Hours < 0)
                HoursOfChargeTextBox.Background = Brushes.DarkRed;
            else
                HoursOfChargeTextBox.Background = Brushes.White;
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
        }
    }
}

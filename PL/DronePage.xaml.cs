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
        private BO.Drone boDrone;
        private Drone drone = new Drone();
        private ListWindow listWindow;
        private ObservableCollection<DroneToList> drones;
        public DronePage(ListWindow window, ObservableCollection<DroneToList> drones)
        {
            InitializeComponent();
            listWindow = window;
            this.drones = drones;
            WeightComboBox.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            StationComboBox.ItemsSource = bl.GetStationsWithAvailableCharge();
            boDrone = new BO.Drone();

            // hidden irrelevant boDrone data
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
            ReleaseFromChargeButton.Visibility = Visibility.Hidden;
            ConnectParcelButton.Visibility = Visibility.Hidden;
            CollectParcelButton.Visibility = Visibility.Hidden;
            SupplyParcelButton.Visibility = Visibility.Hidden;
        }
        public DronePage(ListWindow window, BO.Drone drone, ObservableCollection<DroneToList> drones)
        {
            InitializeComponent();
            listWindow = window;

            DataDroneGrid.DataContext = drone;
            boDrone = drone;
            listWindow.CopyPropertiesTo(boDrone, this.drone);
            this.drone.Location = new Location();
            listWindow.CopyPropertiesTo(boDrone.Location, this.drone.Location);

            this.drones = drones;
            BlockingControls();
            ShowDronesAfterActions();
        }

        private void ClosePageButton_Click(object sender, RoutedEventArgs e)
        {
            this.Content = "";
        }

        private void IdTextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            int id;
            if (IdTextBox.Text == "" || !IdTextBox.Text.All(char.IsDigit))
                id = 0;
            else
                id = int.Parse(IdTextBox.Text);

            if (id < 100000 || id > 999999) // Check that it's 6 digits.
            {
                IdTextBox.Foreground = Brushes.Red;
                boDrone.Id = 0;
            }
            else
            {
                boDrone.Id = id;
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
                boDrone.Model = model;
                ModelTextBox.Foreground = Brushes.SlateGray;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            boDrone.Id = int.Parse(IdTextBox.Text);
            boDrone.Model = ModelTextBox.Text;


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
                boDrone.Weight = (BO.WeightCategories)WeightComboBox.SelectedItem;

            if (StationComboBox.ItemsSource == null)
                MessageBox.Show("There is no station with a free standing to put the boDrone for charging", "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);

            BO.StationToList stationCharge = new BO.StationToList();
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

            MessageBox.Show("The add is succesid!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            DroneToList newDrone = new DroneToList();
            newDrone.Location = new Location();
            listWindow.CopyPropertiesTo(bl.GetDrone(boDrone.Id), newDrone);
            drones.Add(newDrone);
            //CloseWindowButton.Visibility = Visibility.Hidden;
            //this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Content = "";
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
                bl.UpdateDroneModel(boDrone.Id, boDrone.Model);
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


            MessageBox.Show("The update is succesid!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            boDrone = bl.GetDrone(boDrone.Id);

            ShowDronesAfterActions();
        }

        private void SendToChargeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.SendDroneToDroneCharge(boDrone.Id);
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

            MessageBox.Show("The send succesid!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            boDrone = bl.GetDrone(boDrone.Id);

            foreach (var elementStation in bl.GetStations())
            {
                BO.Station station = bl.GetStation(elementStation.Id);
                if (boDrone.Location.Longitude == station.Location.Longitude && boDrone.Location.Latitude == station.Location.Latitude)
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
                bl.ReleaseDroneFromDroneCharge(boDrone.Id);
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

            MessageBox.Show("The realese succesid!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            boDrone = bl.GetDrone(boDrone.Id);

            ReleaseFromChargeButton.Visibility = Visibility.Hidden;
            SendToChargeButton.Visibility = Visibility.Visible;
            ShowDronesAfterActions();
        }

        private void ConnectParcelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.ConnectParcelToDrone(boDrone.Id);
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

            MessageBox.Show("The connection succesid!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            boDrone = bl.GetDrone(boDrone.Id);

            ConnectParcelButton.Visibility = Visibility.Hidden;
            CollectParcelButton.Visibility = Visibility.Visible;
            ShowDronesAfterActions();
        }

        private void CollectParcelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.CollectionParcelByDrone(boDrone.Id);
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

            MessageBox.Show("The collection succesid!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            boDrone = bl.GetDrone(boDrone.Id);

            CollectParcelButton.Visibility = Visibility.Hidden;
            SupplyParcelButton.Visibility = Visibility.Visible;
            ShowDronesAfterActions();
        }

        private void SupplyParcelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.SupplyParcelByDrone(boDrone.Id);
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

            MessageBox.Show("The supply succesid!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            boDrone = bl.GetDrone(boDrone.Id);

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
            //IdTextBox.Text = boDrone.Id.ToString();
            //ModelTextBox.Text = boDrone.Model;
           // PresentWeightLabel.Text = boDrone.Weight.ToString();

            if (boDrone.Status == BO.DroneStatuses.Maintenance)
                foreach (var elementStation in bl.GetStations())
                {
                    BO.Station station = bl.GetStation(elementStation.Id);
                    if (boDrone.Location.Longitude == station.Location.Longitude && boDrone.Location.Latitude == station.Location.Latitude)
                        PresentStationLabel.Text = station.Id.ToString();
                }

            //BatteryTextBox.Text = boDrone.Battery.ToString();
            //StatusTextBox.Text = boDrone.Status.ToString();
            //LocationTextBox.Text = boDrone.Location.ToString();

            if (boDrone.Status == BO.DroneStatuses.Maintenance)
                SendToChargeButton.Visibility = Visibility.Hidden;
            else
                ReleaseFromChargeButton.Visibility = Visibility.Hidden;

            if (boDrone.Status != BO.DroneStatuses.Delivery)
            {
                CollectParcelButton.Visibility = Visibility.Hidden;
                SupplyParcelButton.Visibility = Visibility.Hidden;
            }
            else
            {
                if (boDrone.ParcelByTransfer.OnTheWay != true)
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
            listWindow.ShowData.Content = new ParcelInDronePage(listWindow, boDrone);
        }
    }
}

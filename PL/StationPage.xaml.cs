using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationPage.xaml
    /// </summary>
    public partial class StationPage : Page
    {
        private BlApi.IBL bl = BlApi.BlFactory.GetBl();
        private Station station;
        private ListWindow listWindow;
        public StationPage(ListWindow window)
        {
            InitializeComponent();

        }

        public StationPage(ListWindow window, Station station)
        {
            InitializeComponent();

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
                station.Id = 0;
            }
            else
            {
                station.Id = id;
                IdTextBox.Foreground = Brushes.SlateGray;
            }
        }

        private void NameTextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            string name = NameTextBox.Text;
            if (name == "")
                NameTextBox.Foreground = Brushes.Red;
            else
            {
                station.Name = name;
                NameTextBox.Foreground = Brushes.SlateGray;
            }
        }

        private void LongitudeTextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            double Longitude;
            if (LongitudeTextBox.Text == "" || !LongitudeTextBox.Text.All(char.IsDigit))
                Longitude = 0;
            else
                Longitude = int.Parse(LongitudeTextBox.Text);

            if (Longitude < -1 || Longitude > 1) // Check that it's 6 digits.
            {
                LongitudeTextBox.Foreground = Brushes.Red;
                station.Location.Longitude = 0;
            }
            else
            {
                station.Location.Longitude = Longitude;
                LongitudeTextBox.Foreground = Brushes.SlateGray;
            }
        }

        private void LatitudeTextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            double Latitude;
            if (LatitudeTextBox.Text == "" || !LatitudeTextBox.Text.All(char.IsDigit))
                Latitude = 0;
            else
                Latitude = int.Parse(LatitudeTextBox.Text);

            if (Latitude < -1 || Latitude > 1) // Check that it's 6 digits.
            {
                LatitudeTextBox.Foreground = Brushes.Red;
                station.Location.Latitude = 0;
            }
            else
            {
                station.Location.Latitude = Latitude;
                LatitudeTextBox.Foreground = Brushes.SlateGray;
            }
        }

        private void AvailableChargeSlotsTextBox_Mouseleave(object sender, MouseEventArgs e)
        {
            int AvailableChargeSlots;
            if (AvailableChargeSlotsTextBox.Text == "" || !AvailableChargeSlotsTextBox.Text.All(char.IsDigit))
                AvailableChargeSlots = 0;
            else
                AvailableChargeSlots = int.Parse(AvailableChargeSlotsTextBox.Text);

            if (AvailableChargeSlots < 0 ) // Check that it's 6 digits.
            {
                AvailableChargeSlotsTextBox.Foreground = Brushes.Red;
                station.AvalibleChargeSlots = 0;
            }
            else
            {
                station.AvalibleChargeSlots = AvailableChargeSlots;
                AvailableChargeSlotsTextBox.Foreground = Brushes.SlateGray;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (station.Id < 100000 || station.Id > 999999)
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

            if (station.Name == "")
            {
                MessageBoxResult result = MessageBox.Show("Name must have value, please enter legal name to continue, or Cancel to stop the adding", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
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

            if (station.Location.Latitude < -1 || station.Location.Latitude > 1) 
            {
                MessageBoxResult result = MessageBox.Show("Enter a longitude between -1 to 1 to your station, please enter legal longitude to continue, or Cancel to stop the adding", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
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
           

            if (station.Location.Latitude < -1 || station.Location.Latitude >1)
            {
                MessageBoxResult result = MessageBox.Show("Enter a Latitude between -1 to 1 to your station, please enter legal Latitude to continue, or Cancel to stop the adding", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
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

            if (station.AvalibleChargeSlots < 0)
            {
                MessageBoxResult result = MessageBox.Show("Enter a number of charge slots with positive or 0 value your station, please enter legal value to continue, or Cancel to stop the adding", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
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

            try
            {
                bl.AddStation(station);
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

        private void DroneInChargeButtom_Click(object sender, RoutedEventArgs e)
        {
            listWindow.ShowData.Content = new DroneInChargePage(station);
        }

        private void UpdateStationlButton_Click(object sender, RoutedEventArgs e)
        {
            if (station.Name == " " )
            {
                MessageBoxResult result = MessageBox.Show("Name is empty, please enter legal name with some constant, or Cancel to stop the adding", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
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


            if (station.AvalibleChargeSlots < 0)
            {
                MessageBoxResult result = MessageBox.Show("Enter a number of charge slots with positive or 0 value your station, please enter legal value to continue, or Cancel to stop the adding", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
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

            try
            {
                bl.UpdateDataStation(station.Id,station.Name,station.AvalibleChargeSlots);
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

       
    }
}

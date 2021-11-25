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

        public DroneWindow(IBL.IBL ibl)
        {
            InitializeComponent();
            bl = ibl;
            GetWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            GetStation.ItemsSource = bl.GetStations(station => station.ChargeSlots > 0);

        }

        public DroneWindow(Drone drone)
        {
            InitializeComponent();

            // hidden the add controls
            TextIdLabel.Content = "";
            GetId.Visibility = Visibility.Hidden;
            TextModelLabel.Content = "";
            GetModel.Visibility = Visibility.Hidden;
            TextWeightLabel.Content = "";
            GetWeight.Visibility = Visibility.Hidden;
            TextStationLabel.Content = "";
            GetStation.Visibility = Visibility.Hidden;
            AddButton.Visibility = Visibility.Hidden;
            CancelButton.Visibility = Visibility.Hidden;


        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Drone drone = new Drone();
            int id;
            if (GetId.Text.ToString() == "")
                id = 0;
            else
                id = int.Parse(GetId.Text.ToString());

            if (id <= 0)
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
            else
                drone.Id = id;

            string model = GetModel.Text;
            if (model == "")
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
            else
                drone.Model = model;

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
                drone.Weight = (WeightCategories)Enum.Parse(typeof(WeightCategories), GetWeight.SelectedItem.ToString());

            if(GetStation.ItemsSource == null)
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
                MessageBox.Show(ex.Message);
            }

            MessageBox.Show("The add is success!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();

            //צריך להציג את הרחפן שנוסף עם הרשימה האחרונה !!!! חלק ב סעיף 14!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // בונוס!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    }
}

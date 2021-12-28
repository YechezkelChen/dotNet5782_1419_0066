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
    /// Interaction logic for StationPage.xaml
    /// </summary>
    public partial class StationPage : Page
    {
        private BlApi.IBL bl = BlApi.BlFactory.GetBl();
        private ObservableCollection<StationToList> stations;
        private Station station = new Station();

        public StationPage(ObservableCollection<StationToList> stations)
        {
            InitializeComponent();
            this.stations = stations;
            UpdateStationButton.Visibility = Visibility.Hidden;
        }

        public StationPage(Station station, ObservableCollection<StationToList> stations)
        {
            InitializeComponent();
            this.stations = stations;
            this.station = station;
            AddButton.Visibility = Visibility.Hidden;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            BO.Station boStation = new BO.Station();
            boStation.Id = int.Parse(IdTextBox.Text);
            boStation.Name = NameTextBox.Text;
            boStation.Location.Longitude = Convert.ToDouble(LongitudeTextBox.Text);
            boStation.Location.Latitude = Convert.ToDouble(LatitudeTextBox.Text);
            boStation.AvalibleChargeSlots = int.Parse(AvailableChargeSlotsTextBox.Text);

            try
            {
                bl.AddStation(boStation);
            }
            catch (BO.IdException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (BO.ChargeSlotsException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (BO.NameException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (BO.LocationException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("You have a new station!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Update the view
            StationToList newStation = new StationToList();
            boStation = bl.GetStation(boStation.Id);
            CopyPropertiesTo(boStation, newStation);
            stations.Add(newStation);
            this.Content = "";
        }

        private void UpdateModelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.UpdateDataStation(station.Id, station.Name, station.AvailableChargeSlots);
            }
            catch (BO.IdException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (BO.NameException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("The update is success!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            UpdateListDrones(drone);
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

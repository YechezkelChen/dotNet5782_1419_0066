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
    /// Interaction logic for ParcelPage.xaml
    /// </summary>
    public partial class ParcelPage : Page
    {
        private BlApi.IBL bl = BlApi.BlFactory.GetBl();
        private ObservableCollection<ParcelToList> parcels;
        private Parcel parcel = new Parcel();
        public ParcelPage(ObservableCollection<ParcelToList> parcels)
        {
            InitializeComponent();
            this.parcels = parcels;
        }
        public ParcelPage(Parcel parcel, ObservableCollection<ParcelToList> parcels)
        {
            InitializeComponent();
            this.parcels = parcels;
            this.parcel = parcel;
            DataParcelGrid.DataContext = parcel;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            BO.Parcel boParcel = new BO.Parcel();
            boParcel.Sender.Id = int.Parse(SenderIdTextBox.Text);
            boParcel.Target.Id = int.Parse(TargetIdTextBox.Text);
            boParcel.Weight = (BO.WeightCategories)WeightComboBox.SelectedItem;
            boParcel.Priority = (BO.Priorities) PriorityComboBox.SelectedItem;

            try
            {
                bl.AddParcel(boParcel);
            }
            catch (BO.IdException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("You have a new parcel!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Update the view
            ParcelToList newParcel = new ParcelToList();
            boParcel=bl.GetParcel()
            newDrone.Location = new Location();
            boDrone = bl.GetDrone(boDrone.Id);
            CopyPropertiesTo(boDrone, newDrone);
            CopyPropertiesTo(boDrone.Location, newDrone.Location);
            drones.Add(newDrone);
            this.Content = "";
        }

    }
}

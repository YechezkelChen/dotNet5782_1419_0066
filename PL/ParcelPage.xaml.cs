using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BO;
using Parcel = PO.Parcel;
using ParcelToList = PO.ParcelToList;
using Priorities = PO.Priorities;
using WeightCategories = PO.WeightCategories;

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

            SenderComboBox.ItemsSource = bl.GetCustomers();
            TargetComboBox.ItemsSource = bl.GetCustomers();
            WeightComboBox.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            PriorityComboBox.ItemsSource = Enum.GetValues(typeof(Priorities));

            DroneDataButton.Visibility = Visibility.Hidden;
        }
        public ParcelPage(Parcel parcel, ObservableCollection<ParcelToList> parcels)
        {
            InitializeComponent();
            this.parcels = parcels;
            this.parcel = parcel;

            DataParcelGrid.DataContext = parcel;

            AddButton.Visibility = Visibility.Hidden;
        }
        private void ClosePageButton_Click(object sender, RoutedEventArgs e)
        {
            this.Content = "";
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            BO.Parcel boParcel = new BO.Parcel();
            BO.CustomerToList customer = new BO.CustomerToList();
            customer = (BO.CustomerToList)SenderComboBox.SelectedItem;
            boParcel.Sender = new CustomerInParcel();
            boParcel.Sender.Id = customer.Id;
            customer = (BO.CustomerToList)TargetComboBox.SelectedItem;
            boParcel.Target = new CustomerInParcel();
            boParcel.Target.Id = customer.Id;
            boParcel.Weight = (BO.WeightCategories)WeightComboBox.SelectedItem;
            boParcel.Priority = (BO.Priorities) PriorityComboBox.SelectedItem;

            int idParcel;
            try
            {
                idParcel = bl.AddParcel(boParcel);
            }
            catch (BO.IdException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("You have a new parcel!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Update the view
            ParcelToList newParcel = new ParcelToList();
            BO.ParcelToList boParcelToList = new BO.ParcelToList();
            boParcelToList = bl.GetParcels().First(p => p.Id == idParcel);
            bl.CopyPropertiesTo(boParcelToList, newParcel);
            parcels.Add(newParcel);
            this.Content = "";
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Content = "";

        }
        private void RemoveParcelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.RemoveParcel(parcel.Id);
            }
            catch (BO.ScheduledException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Update the view
            parcels.Remove(parcels.Where(p => p.Id == parcel.Id).Single());
            this.Content = "";
        }
    }
}

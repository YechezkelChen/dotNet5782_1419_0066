using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using PO;

namespace PL
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        private BlApi.IBL bl = BlApi.BlFactory.GetBl();
        private Customer userCustomer;
        private ObservableCollection<CustomerToList> customers = new ObservableCollection<CustomerToList>();
        private CustomerPage customerPage;
        private ParcelInCustomerPage parcelInCustomerPage;
        private ParcelPage parcelPage;
        private ObservableCollection<ParcelToList> parcels;
        private Parcel parcel;

        // 2 constructors 1 for add, 1 for exist
        public UserWindow() // for add
        {
            InitializeComponent();
            this.Content = new CustomerPage(customers);
        }
        public UserWindow(int x) // for exist
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            int userId = Convert.ToInt32(IdTextBox.Text);
            userId = userId * 10 + bl.LastDigitId(userId); // Add check digit to Id
            BO.Customer userBoCustomer = new BO.Customer();
            try
            {
                userBoCustomer = bl.GetCustomer(userId);
            }
            catch (BO.IdException ex)
            {
                MessageBox.Show("You are one step away from being part of the family.\nPlease create an account.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            userCustomer = CopyBoCustomerToPoCustomer(userBoCustomer, userCustomer);
            customerPage = new CustomerPage(userCustomer);
            customerPage.ParcelFromTheCustomerButton.Click += CustomerPage_DataParcelFromCustomer;
            customerPage.ParcelToTheCustomerButton.Click += CustomerPage_DataParcelToCustomer;
            this.Content = customerPage;
        }

        private void CustomerPage_DataParcelFromCustomer(object sender, RoutedEventArgs routedEventArgs)
        {
            parcelInCustomerPage = new ParcelInCustomerPage(userCustomer.FromTheCustomerList);
            parcelInCustomerPage.ParcelListView.MouseDoubleClick += CustomerPage_DataParcel;
            this.Content = parcelInCustomerPage;
        }
        private void CustomerPage_DataParcelToCustomer(object sender, RoutedEventArgs routedEventArgs)
        {
            parcelInCustomerPage = new ParcelInCustomerPage(userCustomer.ToTheCustomerList);
            parcelInCustomerPage.ParcelListView.MouseDoubleClick += CustomerPage_DataParcel;
            this.Content = parcelInCustomerPage;
        }
        private void CustomerPage_DataParcel(object sender, MouseButtonEventArgs e)
        {
            ParcelInCustomer parcelInCustomer = (ParcelInCustomer)parcelInCustomerPage.ParcelListView.SelectedItem;
            BO.Parcel boParcel = bl.GetParcel(parcelInCustomer.Id);
            parcel = CopyBoParcelToPoParcel(boParcel, parcel);
            parcelPage = new ParcelPage(parcel, parcels);
            parcelPage.SenderButton.Click += CustomerPage_DataSender;
            parcelPage.TargetButton.Click += CustomerPage_DataTarget;
            parcelPage.DroneDataButton.Click += DronePage_DataDroneInParcel;
            this.Content = parcelPage;
        }
        private void CustomerPage_DataSender(object sender, RoutedEventArgs e)
        {
            this.Content = new CustomerInParcelPage(parcel.Sender);
        }
        private void CustomerPage_DataTarget(object sender, RoutedEventArgs e)
        {
            this.Content = new CustomerInParcelPage(parcel.Target);
        }
        private void DronePage_DataDroneInParcel(object sender, RoutedEventArgs e)
        {
            this.Content = new DroneInParcelPage(parcel.DroneInParcel);
        }
        private Customer CopyBoCustomerToPoCustomer(BO.Customer boCustomer, Customer poCustomer)
        {
            poCustomer = new Customer();
            bl.CopyPropertiesTo(boCustomer, poCustomer);
            poCustomer.Location = new Location();
            bl.CopyPropertiesTo(boCustomer.Location, poCustomer.Location);

            List<ParcelInCustomer> parcelInCustomers = new List<ParcelInCustomer>();
            foreach (var parcel in boCustomer.FromTheCustomerList)
            {
                ParcelInCustomer newParcel = new ParcelInCustomer();
                bl.CopyPropertiesTo(parcel, newParcel);
                newParcel.CustomerInDelivery = new CustomerInParcel();
                bl.CopyPropertiesTo(parcel.CustomerInDelivery, newParcel.CustomerInDelivery);
                parcelInCustomers.Add(newParcel);
            }
            poCustomer.FromTheCustomerList = parcelInCustomers;

            parcelInCustomers.Clear();

            foreach (var parcel in boCustomer.ToTheCustomerList)
            {
                ParcelInCustomer newParcel = new ParcelInCustomer();
                bl.CopyPropertiesTo(parcel, newParcel);
                newParcel.CustomerInDelivery = new CustomerInParcel();
                bl.CopyPropertiesTo(parcel.CustomerInDelivery, newParcel.CustomerInDelivery);
                parcelInCustomers.Add(newParcel);
            }
            poCustomer.ToTheCustomerList = parcelInCustomers;

            return poCustomer;
        }

        private Parcel CopyBoParcelToPoParcel(BO.Parcel boParcel, Parcel poParcel)
        {
            poParcel = new Parcel();
            bl.CopyPropertiesTo(boParcel, poParcel);
            poParcel.Sender = new CustomerInParcel();
            bl.CopyPropertiesTo(boParcel.Sender, poParcel.Sender);
            poParcel.Target = new CustomerInParcel();
            bl.CopyPropertiesTo(boParcel.Target, poParcel.Target);
            if (boParcel.Scheduled != null)
            {
                poParcel.DroneInParcel = new DroneInParcel();
                bl.CopyPropertiesTo(boParcel.DroneInParcel, poParcel.DroneInParcel);
                poParcel.DroneInParcel.Location = new Location();
                bl.CopyPropertiesTo(boParcel.DroneInParcel.Location, poParcel.DroneInParcel.Location);
            }

            return poParcel;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using PO;


namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerPage.xaml
    /// </summary>
    public partial class CustomerPage : Page
    {
        private BlApi.IBL bl = BlApi.BlFactory.GetBl();
        private ObservableCollection<CustomerToList> customers;
        private Customer customer = new Customer();
        
        public CustomerPage(ObservableCollection<CustomerToList> customers)
        {
            InitializeComponent();
            this.customers = customers;

            UpdateCustomerDataButton.Visibility = Visibility.Hidden;
        }
        public CustomerPage(Customer customer, ObservableCollection<CustomerToList> customers)
        {
            InitializeComponent();
            this.customers = customers;
            this.customer = customer;

            DataCustomerGrid.DataContext = customer;
            AddButton.Visibility = Visibility.Hidden;
        }
        private void ClosePageButton_Click(object sender, RoutedEventArgs e)
        {
            this.Content = "";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            BO.Customer boCustomer = new BO.Customer();
            boCustomer.Id = int.Parse(IdTextBox.Text);
            boCustomer.Name = NameTextBox.Text;
            boCustomer.Phone = PhoneTextBox.Text;
            boCustomer.Location = new BO.Location();
            boCustomer.Location.Latitude = double.Parse(LatitudeTextBox.Text);
            boCustomer.Location.Longitude = double.Parse(LongitudeTextBox.Text);

            try
            {
                bl.AddCustomer(boCustomer);
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
            catch (BO.PhoneException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (BO.LocationException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            MessageBox.Show("You have a new customer!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Update the view
            CustomerToList newCustomer = new CustomerToList();
            BO.CustomerToList boCustomerToList = new BO.CustomerToList();
            boCustomerToList.Id = boCustomer.Id * 10 + bl.LastDigitId(boCustomer.Id);
            boCustomerToList = bl.GetCustomers().First(customer => customer.Id == boCustomerToList.Id);
            bl.CopyPropertiesTo(boCustomerToList, newCustomer);
            customers.Add(newCustomer);
            this.Content = "";
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Content = "";
        }
        private void RemoveCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.RemoveCustomer(customer.Id);
            }
            catch (BO.ScheduledException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Update the view
            customers.Remove(customers.Where(c => c.Id == customer.Id).Single());
            this.Content = "";
        }
        private void UpdateCustomerDataButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.UpdateDataCustomer(customer.Id, customer.Name, customer.Phone);
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

            UpdateListCustomers(customer);
        }

        private void UpdateListCustomers(Customer updateCustomer)
        {
            for (int i = 0; i < customers.Count; i++)
                if (customers[i].Id == updateCustomer.Id)
                {
                    CustomerToList newCustomer = customers[i];
                    bl.CopyPropertiesTo(updateCustomer, newCustomer);
                    customers[i] = newCustomer;
                }
        }
    }
}

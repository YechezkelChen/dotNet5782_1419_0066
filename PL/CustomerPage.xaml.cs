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
            boCustomerToList.Id = boCustomer.Id * 10 + lastDigitID(boCustomer.Id);
            boCustomerToList = bl.GetCustomers().First(customer => customer.Id == boCustomerToList.Id);
            CopyPropertiesTo(boCustomerToList, newCustomer);
            customers.Add(newCustomer);
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
                    CopyPropertiesTo(updateCustomer, newCustomer);
                    customers[i] = newCustomer;
                }
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

        /// <summary>
        /// get last digit of the id
        /// </summary>
        /// <returns></return the last digit of the id>
        private int lastDigitID(int lessID)
        {
            int digit1, digit2, sumResultDigits = 0, digitID;
            for (int i = 1; i <= lessID; i++)
            {
                digit1 = lessID % 10;
                digit1 *= 2;//Calculating the digits double their weight.
                sumResultDigits += sumDigits(digit1);//The sum of the result digits.
                lessID /= 10;
                digit2 = lessID % 10;
                digit2 *= 1;//Calculating the digits double their weight.
                sumResultDigits += sumDigits(digit2);//The sum of the result digits.
                lessID /= 10;
            }
            sumResultDigits %= 10;//The unity digit of the result.

            digitID = 10 - sumResultDigits;
            return digitID;//Returning the missing digit.v
        }

        /// <summary>
        ///Entering a number by the computer.
        /// <returns></return the sum of digit >
        private int sumDigits(int num)
        {
            int sum_digits = 0;
            while (num > 0)
            {
                sum_digits += num % 10;
                num = num / 10;
            }
            return sum_digits;//Return of the sum of his digits.
        }

    }
}

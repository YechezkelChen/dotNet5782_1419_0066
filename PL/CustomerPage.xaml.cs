using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using BO;
using CustomerToList = PO.CustomerToList;

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerPage.xaml
    /// </summary>
    public partial class CustomerPage : Page
    {
        private BlApi.IBL bl = BlApi.BlFactory.GetBl();
        private ObservableCollection<CustomerToList> customers;
        private BO.Customer customer;
        
        public CustomerPage(ObservableCollection<CustomerToList> customers)
        {
            InitializeComponent();
            this.customers = customers;
            customer = new BO.Customer();

            ParcelFromTheCustomerButton.Visibility = Visibility.Hidden;
            ParcelToTheCustomerButton.Visibility = Visibility.Hidden;
            UpdateCustomerDataButton.Visibility = Visibility.Hidden;
        }
        public CustomerPage(BO.Customer customer, ObservableCollection<CustomerToList> customers)
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

            customer.Id = int.Parse(IdTextBox.Text);
            customer.Name = NameTextBox.Text;
            customer.Phone = PhoneTextBox.Text;
            customer.Location = new Location();
            customer.Location.Latitude = Convert.ToDouble(LatitudeTextBox.Text);
            customer.Location.Longitude = Convert.ToDouble(LongitudeTextBox.Text);
            try
            {
                bl.AddCustomer(customer);
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
           

            MessageBox.Show("The add is success!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            CustomerToList newCustomerToList = new CustomerToList();
            customer.Id = customer.Id *10 + lastDigitID(customer.Id);
            CopyPropertiesTo(bl.GetCustomer(customer.Id), newCustomerToList);
            customers.Add(newCustomerToList);
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
            customer = bl.GetCustomer(customer.Id);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Content = "";
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

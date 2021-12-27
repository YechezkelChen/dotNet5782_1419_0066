using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PO;

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerListPage.xaml
    /// </summary>
    public partial class CustomerListPage : Page
    {
        private BlApi.IBL bl;
        //private BO.Customer customer;
        //private ListWindow listWindow;
        private ObservableCollection<CustomerToList> customers;

        public CustomerListPage()
        {
            InitializeComponent();
            bl = BlApi.BlFactory.GetBl();
            customers = new ObservableCollection<CustomerToList>();
            CustomersListView.ItemsSource = customers;
            CustomersData();
        }

        private void CustomersData()
        {
            customers.Clear();
            IEnumerable<BO.CustomerToList> customersData = new List<BO.CustomerToList>();
            customersData = bl.GetCustomers();
            // Show the list after the filtering
            foreach (var customer in customersData)
            {
                CustomerToList newCustomerToList = new CustomerToList();
                CopyPropertiesTo(customer, newCustomerToList);
                customers.Add(newCustomerToList);
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

        //public CustomerListPage(ListWindow window)
        //{
        //    InitializeComponent();
        //    listWindow = window;
        //    bl = BlApi.BlFactory.GetBl();
        //    customers = new ObservableCollection<CustomerToList>();
        //    CustomersListView.ItemsSource = customers;
        //    foreach (var customer in bl.GetCustomers())
        //    {
        //        CustomerToList newCustomer = new CustomerToList();
        //        listWindow.CopyPropertiesTo(customer, newCustomer);
        //        customers.Add(newCustomer);
        //    }
        //}

    }
}

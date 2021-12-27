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
        private BlApi.IBL bl = BlApi.BlFactory.GetBl();
        private ObservableCollection<CustomerToList> customers;

        public CustomerListPage(ObservableCollection<CustomerToList> customers)
        {
            InitializeComponent();
            this.customers = customers;
            CustomersListView.DataContext = customers;
            CustomersData();
        }

        private void CustomersData()
        {
            customers.Clear();
            foreach (var customer in bl.GetCustomers())
            {
                CustomerToList newCustomer = new CustomerToList();
                CopyPropertiesTo(customer, newCustomer);
                customers.Add(newCustomer);
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
    }
}

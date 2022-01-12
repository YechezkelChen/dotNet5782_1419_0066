using System.Collections.ObjectModel;
using System.Windows.Controls;
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
                bl.CopyPropertiesTo(customer, newCustomer);
                customers.Add(newCustomer);
            }
        }
    }
}

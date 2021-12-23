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
        private BO.Customer customer;
        private ListWindow listWindow;
        private ObservableCollection<CustomerToList> customers;
        public CustomerListPage(ListWindow window)
        {
            InitializeComponent();
            listWindow = window;
            bl = BlApi.BlFactory.GetBl();
            customers = new ObservableCollection<CustomerToList>();
            CustomersListView.ItemsSource = customers;
            foreach (var customer in bl.GetCustomers())
            {
                CustomerToList newCustomer = new CustomerToList();
                listWindow.CopyPropertiesTo(customer, newCustomer);
                customers.Add(newCustomer);
            }
        }
    }
}

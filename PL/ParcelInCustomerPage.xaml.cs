using System.Windows;
using System.Windows.Controls;
using PO;

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelInCustomerPage.xaml
    /// </summary>
    public partial class ParcelInCustomerPage : Page
    {
        private Customer customer;
        public ParcelInCustomerPage(Customer customer)
        {
            InitializeComponent();
            this.customer = customer;
            DataParcelGrid.DataContext = customer.FromTheCustomerList;
        }
    }
}

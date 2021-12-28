using System.Windows.Controls;
using PO;
namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerInParcelPage.xaml
    /// </summary>
    public partial class CustomerInParcelPage : Page
    {
        public CustomerInParcelPage(CustomerInParcel customer)
        {
            InitializeComponent();
            DataCustomerGrid.DataContext = customer;
        }
    }
}

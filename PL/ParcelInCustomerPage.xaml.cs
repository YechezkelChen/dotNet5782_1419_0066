using System.Collections.Generic;
using System.Windows.Controls;
using PO;

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelInCustomerPage.xaml
    /// </summary>
    public partial class ParcelInCustomerPage : Page
    {
        private IEnumerable<ParcelInCustomer> parcelsInCustomer;

        public ParcelInCustomerPage(IEnumerable<ParcelInCustomer> parcelsInCustomer)
        {
            InitializeComponent();
            this.parcelsInCustomer = parcelsInCustomer;
            ParcelListView.DataContext = parcelsInCustomer;
        }
    }
}

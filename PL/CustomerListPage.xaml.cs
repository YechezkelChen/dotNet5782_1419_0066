using System.Windows.Controls;

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerListPage.xaml
    /// </summary>
    public partial class CustomerListPage : Page
    {
        ListWindow listWindow;
        private BlApi.IBL bl;
        public CustomerListPage(ListWindow Window)
        {
            InitializeComponent();
        }
    }
}

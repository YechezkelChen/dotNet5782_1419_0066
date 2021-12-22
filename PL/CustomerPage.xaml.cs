using System.Windows;
using System.Windows.Controls;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerPage.xaml
    /// </summary>
    public partial class CustomerPage : Page
    {
        private BlApi.IBL bl = BlApi.BlFactory.GetBl();
        private Customer customer;
        private ListWindow listWindow;
        public CustomerPage(ListWindow window)
        {
            InitializeComponent();
            listWindow = window;
            customer = new Customer();
        }
        public CustomerPage(ListWindow window, Customer customerHelp)
        {
            InitializeComponent();
            listWindow = window;
            customer = customerHelp;
        }
        private void ClosePageButton_Click(object sender, RoutedEventArgs e)
        {
            this.Content = "";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Content = "";
        }
    }
}

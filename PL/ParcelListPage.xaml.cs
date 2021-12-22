using System.Windows.Controls;

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelListPage.xaml
    /// </summary>
    public partial class ParcelListPage : Page
    {
        ListWindow listWindow;
        private BlApi.IBL bl;
        public ParcelListPage(ListWindow Window)
        {
            InitializeComponent();
        }
    }
}

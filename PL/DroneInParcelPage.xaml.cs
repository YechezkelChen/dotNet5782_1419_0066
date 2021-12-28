using System.Windows;
using System.Windows.Controls;
using PO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneInParcelPage.xaml
    /// </summary>
    public partial class DroneInParcelPage : Page
    {
        public DroneInParcelPage(DroneInParcel drone)
        {
            InitializeComponent();
            DataDroneGrid.DataContext = drone;
        }

        private void ClosePageButton_Click(object sender, RoutedEventArgs e)
        {
            this.Content = "";
        }

    }
}

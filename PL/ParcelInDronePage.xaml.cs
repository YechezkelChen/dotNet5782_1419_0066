using System.Windows;
using System.Windows.Controls;
using PO;

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelInDrone.xaml
    /// </summary>
    public partial class ParcelInDronePage : Page
    {
        private Drone drone;
        public ParcelInDronePage(Drone drone)
        {
            InitializeComponent();
            this.drone = drone;
            DataParcelGrid.DataContext = drone.ParcelByTransfer;
        }

        private void ClosePageButton_Click(object sender, RoutedEventArgs e)
        {
            //listWindow.ShowData.Content = new DronePage(listWindow, drone);
        }
    }
}

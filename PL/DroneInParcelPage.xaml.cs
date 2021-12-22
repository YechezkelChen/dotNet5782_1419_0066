using System.Windows;
using System.Windows.Controls;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneInParcelPage.xaml
    /// </summary>
    public partial class DroneInParcelPage : Page
    {
        private BlApi.IBL bl = BlApi.BlFactory.GetBl();
        private Drone drone;
        public DroneInParcelPage()
        {
            InitializeComponent();
        }

        private void ClosePageButton_Click(object sender, RoutedEventArgs e)
        {
            this.Content = "";
        }

    }
}

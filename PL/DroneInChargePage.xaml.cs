using System.Windows.Controls;
using System.Windows.Input;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneInChargePage.xaml
    /// </summary>
    public partial class DroneInChargePage : Page
    {
        private BlApi.IBL bl;
        private Station station;
        public DroneInChargePage(Station station)
        {
            InitializeComponent();
            this.station = station;
            DronesListView.ItemsSource = station.DronesInCharges;

        }
    }
}

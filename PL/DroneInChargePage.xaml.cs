using System.Windows.Controls;
using PO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneInChargePage.xaml
    /// </summary>
    public partial class DroneInChargePage : Page
    {
        private BlApi.IBL bl = BlApi.BlFactory.GetBl();
        private Station station = new Station();

        public DroneInChargePage(Station station)
        {
            InitializeComponent();
            this.station = station;
            DronesListView.DataContext = station.DronesInCharges;
        }
    }
}

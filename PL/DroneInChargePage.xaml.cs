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
        ListWindow listWindow;
        private BlApi.IBL bl;
        public DroneInChargePage(Station station)
        {
            InitializeComponent();
            Station shoeStation = station;

        }

        private void DronesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneToList droneToList = (DroneToList)DronesListView.SelectedItem;
            Drone drone = bl.GetDrone(droneToList.Id);
            listWindow.ShowData.Content = new DronePage(listWindow, drone);
            //ShowDronesAfterFiltering();
        }
    }
}

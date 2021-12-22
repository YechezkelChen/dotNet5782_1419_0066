using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
            ShowDronesAfterFiltering();
        }
    }
}

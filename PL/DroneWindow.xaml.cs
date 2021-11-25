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
using System.Windows.Shapes;
using IBL;
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private IBL.IBL bl;

        public DroneWindow(IBL.IBL ibl)
        {
            InitializeComponent();
            bl = ibl;
            GetWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            GetStation.ItemsSource = bl.GetStations(station => station.ChargeSlots > 0);
            Continue.Click += Continue_Click;
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            Drone drone = new Drone();
            int id = int.Parse(GetId.Text.ToString());
            if (id <= 0)
            {
                MessageBoxResult result = MessageBox.Show("Id is illegal!, please enter legal id to continue, or Cancel to stop the adding", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        GetId.Clear();
                        break;
                    case MessageBoxResult.Cancel:
                        Close();
                        break;
                    default:
                        Close();
                        break;
                }
            }
            else
                drone.Id = id;
        }
    }
}

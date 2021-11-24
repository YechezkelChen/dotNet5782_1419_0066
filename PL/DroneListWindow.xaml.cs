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
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        private IBL.IBL bl;
        public DroneListWindow(IBL.IBL ibl)
        {
            InitializeComponent();
            bl = ibl;
            DronesListView.ItemsSource = bl.GetDrones(drone => true);
        }

        private void StatusSelectorClick(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show(DroneSelector.SelectedItem.ToString());
        }


    }
}

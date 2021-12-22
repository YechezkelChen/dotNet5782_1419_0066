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

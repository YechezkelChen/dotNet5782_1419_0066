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

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BlApi.IBL bl;
        public MainWindow()
        {
            InitializeComponent();
            bl = BlApi.BlFactory.GetBl();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            CloseButton.Visibility = Visibility.Hidden;
            this.Close();
        }
        private void CloseWithSpecialButton(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (CloseButton.Visibility != Visibility.Hidden)
                e.Cancel = true;
        }
        private void ShowDroneButton_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new DroneListPage(bl);
        }
    }
}

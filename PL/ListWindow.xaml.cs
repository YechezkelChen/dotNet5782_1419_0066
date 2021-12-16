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

namespace PL
{
    /// <summary>
    /// Interaction logic for ListWindow.xaml
    /// </summary>
    public partial class ListWindow : Window
    {
        BlApi.IBL bl;
        public ListWindow(BlApi.IBL ibl)
        {
            InitializeComponent();
            bl = ibl;
        }
        //private void Close_Click(object sender, RoutedEventArgs e)
        //{
        //    CloseButton.Visibility = Visibility.Hidden;
        //    this.Close();
        //}
        //private void CloseWithSpecialButton(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    if (CloseButton.Visibility != Visibility.Hidden)
        //        e.Cancel = true;
        //}

        private void ListTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListDrones.IsSelected)
                ShowList.Content = new DroneListPage(bl);
            if (ListStations.IsSelected)
                ShowList.Content = new StationListPage(bl);
            if (ListCustomers.IsSelected)
                ShowList.Content = new CustomerListPage(bl);
            if (ListParcels.IsSelected)
                ShowList.Content = new ParcelListPage(bl);
        }
    }
}

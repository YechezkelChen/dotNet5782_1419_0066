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
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelListPage.xaml
    /// </summary>
    public partial class ParcelListPage : Page
    {
        ListWindow listWindow;
        private BlApi.IBL bl;
        public ParcelListPage(ListWindow Window)
        {
            InitializeComponent();
            listWindow = Window;
            bl = BlApi.BlFactory.GetBl();
            ShowDronesAfterFiltering();
            StatusSelctor.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            WeightSelctor.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        private void ParcelListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelToList parcelToList = (ParcelToList)ParcelsListView.SelectedItem;
            Parcel parcel = bl.GetParcel(parcelToList.Id);
            listWindow.ShowData.Content = new ParcelPage(bl, parcel);
            ShowDronesAfterFiltering();
        }

        private void ShowDronesAfterFiltering()
        {
            IEnumerable<ParcelToList> parcels = new List<ParcelToList>();
            IEnumerable<ParcelToList> parcelsFiltering = new List<ParcelToList>();

            parcels = bl.GetParcels();

            // Filtering of status.
            if (StatusSelctor.SelectedItem == null)
                parcelsFiltering = bl.GetParcels();
            else
                parcelsFiltering = bl.GetParcelsByStatus((DroneStatuses)StatusSelctor.SelectedItem);

            drones = dronesFiltering.ToList().FindAll(drone => drones.ToList().Find(d => d.Id == drone.Id) != null);

            // Filterig of max weight
            if (WeightSelctor.SelectedItem == null)
                dronesFiltering = bl.GetDrones();
            else
                dronesFiltering = bl.GetDronesByMaxWeight((WeightCategories)WeightSelctor.SelectedItem);

            drones = dronesFiltering.ToList().FindAll(drone => drones.ToList().Find(d => d.Id == drone.Id) != null);

            // Show the list after the filtering
            DronesListView.ItemsSource = drones;
        }
    }
}

using System.Collections.ObjectModel;
using System.Windows.Controls;
using PO;

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelListPage.xaml
    /// </summary>
    public partial class ParcelListPage : Page
    {
        private BlApi.IBL bl = BlApi.BlFactory.GetBl();
        private BO.Parcel parcel;
        private ListWindow listWindow;
        private ObservableCollection<ParcelToList> parcels;
        public ParcelListPage(ListWindow window)
        {
            InitializeComponent();
            listWindow = window;
            bl = BlApi.BlFactory.GetBl();
            parcels = new ObservableCollection<ParcelToList>();
            ParcelsListView.ItemsSource = parcels;
            foreach (var parcel in bl.GetParcels())
            {
                ParcelToList newParcel = new ParcelToList();
                listWindow.CopyPropertiesTo(parcel, newParcel);
                parcels.Add(newParcel);
            }
        }
    }
}

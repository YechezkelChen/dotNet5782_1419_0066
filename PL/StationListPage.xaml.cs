using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using PO;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationListPage.xaml
    /// </summary>
    public partial class StationListPage : Page
    {
        private BlApi.IBL bl = BlApi.BlFactory.GetBl();
        private BO.Station station;
        private ListWindow listWindow;
        private ObservableCollection<StationToList> stations;
        public StationListPage(ListWindow window)
        {
            InitializeComponent();
            listWindow = window;
            bl = BlApi.BlFactory.GetBl();
            stations = new ObservableCollection<StationToList>();
            StationsListView.ItemsSource = stations;
            foreach (var station in bl.GetStations())
            {
                StationToList newStation = new StationToList();
                listWindow.CopyPropertiesTo(station, newStation);
                stations.Add(newStation);
            }
        }
    }
}

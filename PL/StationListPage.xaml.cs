using System.Collections.ObjectModel;
using System.Windows.Controls;
using PO;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationListPage.xaml
    /// </summary>
    public partial class StationListPage : Page
    {
        private BlApi.IBL bl = BlApi.BlFactory.GetBl();
        private ObservableCollection<StationToList> stations;

        public StationListPage(ObservableCollection<StationToList> stations)
        {
            InitializeComponent();
            this.stations = stations;
            StationsListView.DataContext = this.stations;
            StationsData();
        }

        private void StationsData()
        {
            stations.Clear();
            foreach (var station in bl.GetStations())
            {
                StationToList newStation = new StationToList();
                bl.CopyPropertiesTo(station, newStation);
                stations.Add(newStation);
            }
        }

        private void GroupByAvailableStationsButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            stations.Clear();
            var groups = bl.GetStationsByGroupAvailableStations();
            foreach (var group in groups)
                foreach (var station in group)
                {
                    StationToList newsStation = new StationToList();
                    bl.CopyPropertiesTo(station, newsStation);
                    stations.Add(newsStation);
                }
        }

        private void RefreshGroupButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            StationsListView.ItemsSource = stations;
            StationsData();
        }
    }
}

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
        private ListWindow listWindow;
        private BlApi.IBL bl;
        private ObservableCollection<StationToList> stations;

        public StationListPage(ListWindow window)
        {
            InitializeComponent();
            listWindow = window;
            bl = BlApi.BlFactory.GetBl();
            stations = new ObservableCollection<StationToList>();
            StationsListView.ItemsSource = stations;
            StationsData();
        }

        private void StationsData()
        {
            foreach (var station in bl.GetStations())
            {
                StationToList newStation = new StationToList();
                listWindow.CopyPropertiesTo(station, newStation);
                stations.Add(newStation);
            }
        }

        private void GroupByAvailableStationsButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            StationsListView.ItemsSource = bl.GetStationsByGroupAvailableStations();
        }

        private void RefreshGroupButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            StationsListView.ItemsSource = stations;
            StationsData();
        }

        private void AddStationButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            listWindow.ShowData.Content = new StationPage(listWindow, stations);
        }

        private void StationsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            StationToList stationToList = (StationToList)StationsListView.SelectedItem;
            BO.Station station = bl.GetStation(stationToList.Id);
            listWindow.ShowData.Content = new StationPage(listWindow, station, stations);
        }
    }
}

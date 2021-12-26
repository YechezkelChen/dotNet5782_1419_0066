using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationPage.xaml
    /// </summary>
    public partial class StationPage : Page
    {
        private BlApi.IBL bl = BlApi.BlFactory.GetBl();
        private ListWindow listWindow;
        private ObservableCollection<StationToList> stations;
        private Station station;

        public StationPage(ListWindow window, ObservableCollection<StationToList> stations)
        {
            InitializeComponent();
            listWindow = window;
            this.stations = stations;
            station = new BO.Station();
        }

        public StationPage(ListWindow window, Station station, ObservableCollection<StationToList> stations)
        {
            InitializeComponent();
            listWindow = window;
            this.stations = stations;
            this.station = station;
        }
    }
}

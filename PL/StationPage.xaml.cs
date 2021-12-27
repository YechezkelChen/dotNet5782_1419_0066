using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using PO;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationPage.xaml
    /// </summary>
    public partial class StationPage : Page
    {
        private BlApi.IBL bl = BlApi.BlFactory.GetBl();
        private ObservableCollection<StationToList> stations;
        private Station station = new Station();

        public StationPage(ObservableCollection<StationToList> stations)
        {
            InitializeComponent();
            this.stations = stations;
        }

        public StationPage(Station station, ObservableCollection<StationToList> stations)
        {
            InitializeComponent();
            this.stations = stations;
            this.station = station;
        }
    }
}

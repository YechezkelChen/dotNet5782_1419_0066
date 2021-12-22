using System.Windows.Controls;
using System.Windows.Input;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationListPage.xaml
    /// </summary>
    public partial class StationListPage : Page
    {
        ListWindow listWindow;
        private BlApi.IBL bl;
        public StationListPage(ListWindow Window)
        {
            InitializeComponent();
        }

        private void StationsListView_MouseEnter(object sender, MouseEventArgs e)
        {
            StationToList stationToList = (StationToList)StationsListView.SelectedItem;
            Station station = bl.GetStation(stationToList.Id);
            listWindow.ShowData.Content = new StationPage(listWindow, station);
            //ShowDronesAfterFiltering();
        }
    }
}

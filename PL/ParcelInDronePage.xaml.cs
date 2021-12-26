using System.Windows;
using System.Windows.Controls;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelInDrone.xaml
    /// </summary>
    public partial class ParcelInDronePage : Page
    {
        private ListWindow listWindow;
        private Drone drone;
        public ParcelInDronePage(ListWindow window, Drone drone)
        {
            InitializeComponent();
            listWindow = window;
            this.drone = drone;
            DataParcelGrid.DataContext = drone.ParcelByTransfer;
            //ShowParcelInDrone();
        }

        private void ClosePageButton_Click(object sender, RoutedEventArgs e)
        {
            //listWindow.ShowData.Content = new DronePage(listWindow, drone);
        }

        //void ShowParcelInDrone()
        //{
        //    //IdParcelTextBox.Text = drone.ParcelByTransfer.Id.ToString();
        //    //StatusParcelTextBox.Text = drone.ParcelByTransfer.OnTheWay.ToString();
        //    //PriorityParcelTextBox.Text = drone.ParcelByTransfer.Priority.ToString();
        //    //WeightParcelTextBox.Text = drone.ParcelByTransfer.Weight.ToString();
        //    //SenderInParcelTextBox.Text = drone.ParcelByTransfer.Sender.ToString();
        //    //PickUpLocationParcelTextBox.Text = drone.ParcelByTransfer.PickUpLocation.ToString();
        //    //ReceiverInParcelTextBox.Text = drone.ParcelByTransfer.Target.ToString();
        //    //TargetLocationParcelTextBox.Text = drone.ParcelByTransfer.TargetLocation.ToString();
        //    //DistanceOfTransferTextBox.Text = drone.ParcelByTransfer.DistanceOfTransfer.ToString();
        //}
    }
}

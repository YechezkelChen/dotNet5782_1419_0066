using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            CloseButton.Visibility = Visibility.Hidden;
            this.Close();
        }
        private void CloseWithSpecialButton(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (CloseButton.Visibility != Visibility.Hidden)
                e.Cancel = true;
        }
        
        private void ManagerLoginButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new ListWindow().ShowDialog();
            this.Show();
        }

        private void UserLoginButton_Click(object sender, RoutedEventArgs e)
        {
            // הוספת חבילה, הצגת חבילה של הלקוח,אישור איסוף קולקט ואישור סופליי קבלה
            this.Hide();
            new UserWindow().ShowDialog();
            this.Show();
        }

        private void NewUserButton_Click(object sender, RoutedEventArgs e)
        {
            // הוספת לקוח

        }
    }
}

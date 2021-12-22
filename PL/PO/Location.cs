using System.ComponentModel;

namespace PO
{
    public class Location : INotifyPropertyChanged
    {
        private double longitude;
        public double Longitude
        {
            get => longitude;
            set { longitude = value; OnPropertyChanged("longitude"); }
        }

        private double latitude;
        public double Latitude
        {
            get => latitude;
            set { latitude = value; OnPropertyChanged("latitude"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

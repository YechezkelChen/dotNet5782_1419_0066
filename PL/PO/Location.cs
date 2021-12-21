using System.ComponentModel;

namespace PO
{
    public class Location : INotifyPropertyChanged
    {
        public double Longitude
        {
            get => Longitude;
            set { Longitude = value; OnPropertyChanged("Longitude"); }
        }
        public double Latitude
        {
            get => Latitude;
            set { Latitude = value; OnPropertyChanged("Latitude"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

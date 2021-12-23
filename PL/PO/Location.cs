using System;
using System.ComponentModel;

namespace PO
{
    public class Location : INotifyPropertyChanged
    {
        private double longitude;
        public double Longitude
        {
            get => longitude;
            set { longitude = Math.Round(value, 2); OnPropertyChanged("longitude"); }
        }

        private double latitude;
        public double Latitude
        {
            get => latitude;
            set { latitude = Math.Round(value, 2); OnPropertyChanged("latitude"); }
        }

        public override string ToString()
        {
            return $"Longitude = " + String.Format("{0:0.000}", longitude) + "\n" +
                   $"Latitude = " + String.Format("{0:0.000}", latitude) + "\n";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

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
            return "(" + ConvertLatitude(Latitude) + ", " + ConvertLongitude(Longitude) + ")";
        }
        public static string ConvertLatitude(double coord)   /// Funcs to Convert lattitudes and longtitudes from decimal to Degrees
        {
            char direction;
            double sec = (double)Math.Round(coord * 3600);
            double deg = Math.Abs(sec / 3600);
            sec = Math.Abs(sec % 3600);
            double min = sec / 60;
            sec %= 60;
            if (coord >= 0)
                direction = 'N';
            else
                direction = 'S';
            return $"{(int)deg}° {(int)min}' {sec}'' { direction}";
        }
        public static string ConvertLongitude(double coord)
        {
            char direction;
            double sec = (double)Math.Round(coord * 3600);
            double deg = Math.Abs(sec / 3600);
            sec = Math.Abs(sec % 3600);
            double min = sec / 60;
            sec %= 60;
            if (coord >= 0)
                direction = 'E';
            else
                direction = 'W';
            return $"{(int)deg}° {(int)min}' {sec}'' { direction}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

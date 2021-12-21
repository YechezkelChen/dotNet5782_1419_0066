using System.ComponentModel;

namespace PO
{
    public class DroneInParcel : INotifyPropertyChanged
    {
        public int Id
        {
            get => Id;
            set { Id = value; OnPropertyChanged("Id"); }
        }

        
        public double Battery
        {
            get => Battery;
            set { Battery = value; OnPropertyChanged("Battery"); }
        }

        public Location Location
        {
            get => Location;
            set { Location = value; OnPropertyChanged("Status"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


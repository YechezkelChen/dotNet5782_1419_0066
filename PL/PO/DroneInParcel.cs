using System.ComponentModel;

namespace PO
{
    public class DroneInParcel : INotifyPropertyChanged
    {
        private int id;
        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged("id"); }
        }

        private double battery;
        public double Battery
        {
            get => battery;
            set { battery = value; OnPropertyChanged("battery"); }
        }

        private Location location;
        public Location Location
        {
            get => location;
            set { location = value; OnPropertyChanged("location"); }
        }

        public override string ToString()
        {
            return
                $"Id #{id}: " +
                $"Battery = {battery}, " +
                $"Location ={location}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


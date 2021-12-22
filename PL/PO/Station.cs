using System.Collections.Generic;
using System.ComponentModel;

namespace PO
{
    public class Station : INotifyPropertyChanged
    {
        private int id;
        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged("id"); }
        }

        private string name;
        public string Name
        {
            get => name;
            set { name = value; OnPropertyChanged("name"); }
        }

        private Location location;
        public Location Location
        {
            get => location;
            set { location = value; OnPropertyChanged("location"); }
        }

        private int availableChargeSlots;
        public int AvailableChargeSlots
        {
            get => availableChargeSlots;
            set { availableChargeSlots = value; OnPropertyChanged("availableChargeSlots"); }
        }

        private IEnumerable<DroneInCharge> dronesInCharges;
        public IEnumerable<DroneInCharge> DronesInCharges
        {
            get => dronesInCharges;
            set { dronesInCharges = value; OnPropertyChanged("dronesInCharges"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

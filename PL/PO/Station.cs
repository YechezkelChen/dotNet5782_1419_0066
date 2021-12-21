using System.Collections.Generic;
using System.ComponentModel;

namespace PO
{
    public class Station : INotifyPropertyChanged
    {
        public int Id
        {
            get => Id;
            set { Id = value; OnPropertyChanged("Id"); }
        }

        public string Name
        {
            get => Name;
            set { Name = value; OnPropertyChanged("Name"); }
        }
        public Location Location
        {
            get => Location;
            set { Location = value; OnPropertyChanged("Location"); }
        }

        
        public int AvailableChargeSlots
        {
            get => AvailableChargeSlots;
            set { AvailableChargeSlots = value; OnPropertyChanged("AvailableChargeSlots"); }
        }

        public IEnumerable<DroneInCharge> DronesInCharges
        {
            get => DronesInCharges;
            set { DronesInCharges = value; OnPropertyChanged("DronesInCharges"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

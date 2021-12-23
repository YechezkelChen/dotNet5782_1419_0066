using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

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

        public override string ToString()
        {
            StringBuilder builderDroneChargeListPrint = new StringBuilder();
            foreach (var elementInCharge in DronesInCharges)
                builderDroneChargeListPrint.Append(elementInCharge).Append(", ");
            return $"Id #{id}: Name = {name},Location = {location}, " +
                   $"Avalible charge slots = {availableChargeSlots}, Drones in charges = {builderDroneChargeListPrint}.";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

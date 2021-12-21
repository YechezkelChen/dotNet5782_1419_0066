using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PO
{
    public class Station : INotifyPropertyChanged
    {
        public int Id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("Id"); }
        }

        public string Name
        {
            get { return Name; }
            set { Name = value; OnPropertyChanged("Name"); }
        }
        public Location Location
        {
            get { return Location; }
            set { Location = value; OnPropertyChanged("Location"); }
        }


        public int AvalibleChargeSlots
        {
            get { return AvalibleChargeSlots; }
            set { AvalibleChargeSlots = value; OnPropertyChanged("AvalibleChargeSlots"); }
        }

        public IEnumerable<DroneInCharge> DronesInCharges
        {
            get { return DronesInCharges; }
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

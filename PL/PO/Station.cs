using System;
using System.Collections.Generic;
using System.Linq;
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


        public int ChargeSlots
        {
            get { return ChargeSlots; }
            set { ChargeSlots = value; OnPropertyChanged("ChargeSlots"); }
        }

        public IEnumerable<DroneInCharge> InCharges
        {
            get { return InCharges; }
            set { InCharges = value; OnPropertyChanged("InCharges"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

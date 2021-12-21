using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
    public class StationToList : INotifyPropertyChanged
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
        public int AvalibleChargeSlots
        {
            get { return AvalibleChargeSlots; }
            set { AvalibleChargeSlots = value; OnPropertyChanged("AvalibleChargeSlots"); }
        }
        public int NotAvailableChargeSlots
        {
            get { return NotAvailableChargeSlots; }
            set { NotAvailableChargeSlots = value; OnPropertyChanged("NotAvailableChargeSlots"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

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
        public int ChargeSlotsAvailable
        {
            get { return ChargeSlotsAvailable; }
            set { ChargeSlotsAvailable = value; OnPropertyChanged("ChargeSlotsAvailable"); }
        }
        public int ChargeSlotsNotAvailable
        {
            get { return ChargeSlotsNotAvailable; }
            set { ChargeSlotsNotAvailable = value; OnPropertyChanged("ChargeSlotsNotAvailable"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

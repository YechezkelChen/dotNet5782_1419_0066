using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
    public class CustomerToList : INotifyPropertyChanged
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
        public string Phone
        {
            get { return Phone; }
            set { Phone = value; OnPropertyChanged("Phone"); }
        }
        public int SenderParcelDelivered
        {
            get { return SenderParcelDelivered; }
            set { SenderParcelDelivered = value; OnPropertyChanged("SenderParcelDelivered"); }
        }
        public int SenderParcelPickedUp
        {
            get { return SenderParcelPickedUp; }
            set { SenderParcelPickedUp = value; OnPropertyChanged("SenderParcelPickedUp"); }
        }
        public int TargetParcelDelivered
        {
            get { return TargetParcelDelivered; }
            set { TargetParcelDelivered = value; OnPropertyChanged("TargetParcelDelivered"); }
        }
        public int TargetParcelPickedUp
        {
            get { return TargetParcelPickedUp; }
            set { TargetParcelPickedUp = value; OnPropertyChanged("TargetParcelPickedUp"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

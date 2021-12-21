using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PO
{
    public class Parcel : INotifyPropertyChanged
    {
        public int Id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("Id"); }
        }

        public CustomerInParcel Sender
        {
            get { return Sender; }
            set { Sender = value; OnPropertyChanged("Sender"); }
        }
        public CustomerInParcel Target
        {
            get { return Target; }
            set { Target = value; OnPropertyChanged("Target"); }
        }

        public WeightCategories Weight
        {
            get { return Weight; }
            set { Weight = value; OnPropertyChanged("Weight"); }
        }

        public Priorities Priority
        {
            get { return Priority; }
            set { Priority = value; OnPropertyChanged("Priority"); }
        }

        public DroneInParcel DroneInParcel
        {
            get { return DroneInParcel; }
            set { DroneInParcel = value; OnPropertyChanged("DroneInParcel"); }
        }

        public DateTime? Requested
        {
            get { return Requested; }
            set { Requested = value; OnPropertyChanged("Requested"); }
        }
        public DateTime? Scheduled
        {
            get { return Scheduled; }
            set { Scheduled = value; OnPropertyChanged("Scheduled"); }
        }
        public DateTime? PickedUp
        {
            get { return PickedUp; }
            set { PickedUp = value; OnPropertyChanged("PickedUp"); }
        }
        public DateTime? Delivered
        {
            get { return Delivered; }
            set { Delivered = value; OnPropertyChanged("Delivered"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
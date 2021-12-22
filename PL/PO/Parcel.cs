using System;
using System.ComponentModel;

namespace PO
{
    public class Parcel : INotifyPropertyChanged
    {
        private int id;
        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged("id"); }
        }

        private CustomerInParcel sender;
        public CustomerInParcel Sender
        {
            get => sender;
            set { sender = value; OnPropertyChanged("sender"); }
        }

        private CustomerInParcel target;
        public CustomerInParcel Target
        {
            get => target;
            set { target = value; OnPropertyChanged("target"); }
        }

        private WeightCategories weight;
        public WeightCategories Weight
        {
            get => weight;
            set { weight = value; OnPropertyChanged("weight"); }
        }

        private Priorities priority;
        public Priorities Priority
        {
            get => priority;
            set { priority = value; OnPropertyChanged("priority"); }
        }

        private DroneInParcel droneInParcel;
        public DroneInParcel DroneInParcel
        {
            get => droneInParcel;
            set { droneInParcel = value; OnPropertyChanged("droneInParcel"); }
        }

        private DateTime? requested;
        public DateTime? Requested
        {
            get => requested;
            set { requested = value; OnPropertyChanged("requested"); }
        }

        private DateTime? scheduled;
        public DateTime? Scheduled
        {
            get => scheduled;
            set { scheduled = value; OnPropertyChanged("scheduled"); }
        }

        private DateTime? pickedUp;
        public DateTime? PickedUp
        {
            get => pickedUp;
            set { pickedUp = value; OnPropertyChanged("pickedUp"); }
        }

        private DateTime? delivered;
        public DateTime? Delivered
        {
            get => delivered;
            set { delivered = value; OnPropertyChanged("delivered"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
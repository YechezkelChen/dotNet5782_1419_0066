using System;
using System.ComponentModel;

namespace PO
{
    public class ParcelInTransfer : INotifyPropertyChanged
    {
        private int id;
        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged("id"); }
        }

        private bool onTheWay;
        public bool OnTheWay
        {
            get => onTheWay;
            set { onTheWay = value; OnPropertyChanged("onTheWay"); }
        }

        private Priorities priority;
        public Priorities Priority
        {
            get => priority;
            set { priority = value; OnPropertyChanged("priority"); }
        }

        private WeightCategories weight;
        public WeightCategories Weight
        {
            get => weight;
            set { weight = value; OnPropertyChanged("weight"); }
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

        private Location pickUpLocation;
        public Location PickUpLocation
        {
            get => pickUpLocation;
            set { pickUpLocation = value; OnPropertyChanged("pickUpLocation"); }
        }

        private Location targetLocation;
        public Location TargetLocation
        {
            get => targetLocation;
            set { targetLocation = value; OnPropertyChanged("targetLocation"); }
        }

        private double distanceOfTransfer;
        public double DistanceOfTransfer
        {
            get => distanceOfTransfer;
            set { distanceOfTransfer = value; OnPropertyChanged("distanceOfTransfer"); }
        }

        public override string ToString()
        {
            return $"Id #{id}:\n" +
                   $"On the way = {onTheWay}\n" +
                   $"Priority = {priority}\n" +
                   $"Weight = {weight}\n" +
                   $"Sender = {sender}\n" +
                   $"Target = {target}\n" +
                   $"Pick up location = {pickUpLocation}" +
                   $"Target location = {targetLocation}" +
                   $"Distance of transfer = " + String.Format("{0:0.00}", distanceOfTransfer) + "\n";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

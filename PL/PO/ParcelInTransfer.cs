using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
    public class ParcelInTransfer : INotifyPropertyChanged
    {
        public int Id
        {
            get => Id;
            set { Id = value; OnPropertyChanged("Id"); }
        }
        public bool OnTheWay
        {
            get => OnTheWay;
            set { OnTheWay = value; OnPropertyChanged("OnTheWay"); }
        }
        public Priorities Priority
        {
            get => Priority;
            set { Priority = value; OnPropertyChanged("Priority"); }
        }
        public WeightCategories Weight
        {
            get => Weight;
            set { Weight = value; OnPropertyChanged("Weight"); }
        }
        public CustomerInParcel Sender
        {
            get => Sender;
            set { Sender = value; OnPropertyChanged("Sender"); }
        }
        public CustomerInParcel Target
        {
            get => Target;
            set { Target = value; OnPropertyChanged("Target"); }
        }
        public Location PickUpLocation
        {
            get => PickUpLocation;
            set { PickUpLocation = value; OnPropertyChanged("PickUpLocation"); }
        }
        public Location TargetLocation
        {
            get => TargetLocation;
            set { TargetLocation = value; OnPropertyChanged("TargetLocation"); }
        }
        public double DistanceOfTransfer
        {
            get => DistanceOfTransfer;
            set { DistanceOfTransfer = value; OnPropertyChanged("DistanceOfTransfer"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

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
            get { return Id; }
            set { Id = value; OnPropertyChanged("Id"); }
        }
        public bool OnTheWay
        {
            get { return OnTheWay; }
            set { OnTheWay = value; OnPropertyChanged("OnTheWay"); }
        }
        public Priorities Priority
        {
            get { return Priority; }
            set { Priority = value; OnPropertyChanged("Priority"); }
        }
        public WeightCategories Weight
        {
            get { return Weight; }
            set { Weight = value; OnPropertyChanged("Weight"); }
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
        public Location PickUpLocation
        {
            get { return PickUpLocation; }
            set { PickUpLocation = value; OnPropertyChanged("PickUpLocation"); }
        }
        public Location TargetLocation
        {
            get { return TargetLocation; }
            set { TargetLocation = value; OnPropertyChanged("TargetLocation"); }
        }
        public double DistanceOfTransfer
        {
            get { return DistanceOfTransfer; }
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

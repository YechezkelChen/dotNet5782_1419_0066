using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
    public class ParcelByTransfer : INotifyPropertyChanged
    {
        public int Id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("Id"); }
        }
        public bool Status
        {
            get { return Status; }
            set { Status = value; OnPropertyChanged("Status"); }
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
        public CustomerInParcel SenderInParcel
        {
            get { return SenderInParcel; }
            set { SenderInParcel = value; OnPropertyChanged("SenderInParcel"); }
        }
        public Location PickUpLocation
        {
            get { return PickUpLocation; }
            set { PickUpLocation = value; OnPropertyChanged("PickUpLocation"); }
        }
        public CustomerInParcel ReceiverInParcel
        {
            get { return ReceiverInParcel; }
            set { ReceiverInParcel = value; OnPropertyChanged("ReceiverInParcel"); }
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

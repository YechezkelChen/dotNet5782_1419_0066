using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
    public class ParcelInCustomer : INotifyPropertyChanged
    {
        public int Id
        {
            get => Id;
            set { Id = value; OnPropertyChanged("Id"); }
        }
        public WeightCategories Weight
        {
            get => Weight;
            set { Weight = value; OnPropertyChanged("Weight"); }
        }
        public Priorities Priority
        {
            get => Priority;
            set { Priority = value; OnPropertyChanged("Priority"); }
        }
        public ParcelStatuses Status
        {
            get => Status;
            set { Status = value; OnPropertyChanged("Status"); }
        }
        public CustomerInParcel CustomerInDelivery
        {
            get => CustomerInDelivery;
            set { CustomerInDelivery = value; OnPropertyChanged("CustomerInDelivery"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

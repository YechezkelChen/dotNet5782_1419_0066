using System.ComponentModel;

namespace PO
{
    public class ParcelInCustomer : INotifyPropertyChanged
    {
        private int id;
        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged("id"); }
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

        private ParcelStatuses status;
        public ParcelStatuses Status
        {
            get => status;
            set { status = value; OnPropertyChanged("status"); }
        }

        private CustomerInParcel customerInDelivery;
        public CustomerInParcel CustomerInDelivery
        {
            get => customerInDelivery;
            set { customerInDelivery = value; OnPropertyChanged("customerInDelivery"); }
        }

        public override string ToString()
        {
            return $"Id #{id}: Weight = {weight}, Priority = {priority}, " +
                   $" Status = {status}, Customer in delivery = {customerInDelivery}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

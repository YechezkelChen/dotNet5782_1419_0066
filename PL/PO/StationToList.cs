using System.ComponentModel;

namespace PO
{
    public class StationToList : INotifyPropertyChanged
    {
        public int Id
        {
            get => Id;
            set { Id = value; OnPropertyChanged("Id"); }
        }
        public string Name
        {
            get => Name;
            set { Name = value; OnPropertyChanged("Name"); }
        }
        public int AvailableChargeSlots
        {
            get => AvailableChargeSlots;
            set { AvailableChargeSlots = value; OnPropertyChanged("AvailableChargeSlots"); }
        }
        public int NotAvailableChargeSlots
        {
            get => NotAvailableChargeSlots;
            set { NotAvailableChargeSlots = value; OnPropertyChanged("NotAvailableChargeSlots"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

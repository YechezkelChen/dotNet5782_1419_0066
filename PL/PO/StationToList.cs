using System.ComponentModel;

namespace PO
{
    public class StationToList : INotifyPropertyChanged
    {
        private int id;
        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged("id"); }
        }

        private string name;
        public string Name
        {
            get => name;
            set { name = value; OnPropertyChanged("name"); }
        }

        private int availableChargeSlots;
        public int AvailableChargeSlots
        {
            get => availableChargeSlots;
            set { availableChargeSlots = value; OnPropertyChanged("availableChargeSlots"); }
        }

        private int notAvailableChargeSlots;
        public int NotAvailableChargeSlots
        {
            get => notAvailableChargeSlots;
            set { notAvailableChargeSlots = value; OnPropertyChanged("notAvailableChargeSlots"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

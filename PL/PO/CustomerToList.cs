using System.ComponentModel;

namespace PO
{
    public class CustomerToList : INotifyPropertyChanged
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
        public string Phone
        {
            get => Phone;
            set { Phone = value; OnPropertyChanged("Phone"); }
        }
        public int SenderParcelDelivered
        {
            get => SenderParcelDelivered;
            set { SenderParcelDelivered = value; OnPropertyChanged("SenderParcelDelivered"); }
        }
        public int SenderParcelPickedUp
        {
            get => SenderParcelPickedUp;
            set { SenderParcelPickedUp = value; OnPropertyChanged("SenderParcelPickedUp"); }
        }
        public int TargetParcelDelivered
        {
            get => TargetParcelDelivered;
            set { TargetParcelDelivered = value; OnPropertyChanged("TargetParcelDelivered"); }
        }
        public int TargetParcelPickedUp
        {
            get => TargetParcelPickedUp;
            set { TargetParcelPickedUp = value; OnPropertyChanged("TargetParcelPickedUp"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

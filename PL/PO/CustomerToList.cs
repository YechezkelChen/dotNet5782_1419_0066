using System.ComponentModel;

namespace PO
{
    public class CustomerToList : INotifyPropertyChanged
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

        private string phone;
        public string Phone
        {
            get => phone;
            set { phone = value; OnPropertyChanged("phone"); }
        }

        private int senderParcelDelivered;
        public int SenderParcelDelivered
        {
            get => senderParcelDelivered;
            set { senderParcelDelivered = value; OnPropertyChanged("senderParcelDelivered"); }
        }

        private int senderParcelPickedUp;
        public int SenderParcelPickedUp
        {
            get => senderParcelPickedUp;
            set { senderParcelPickedUp = value; OnPropertyChanged("senderParcelPickedUp"); }
        }

        private int targetParcelDelivered;
        public int TargetParcelDelivered
        {
            get => targetParcelDelivered;
            set { targetParcelDelivered = value; OnPropertyChanged("targetParcelDelivered"); }
        }

        private int targetParcelPickedUp;
        public int TargetParcelPickedUp
        {
            get => targetParcelPickedUp;
            set { targetParcelPickedUp = value; OnPropertyChanged("targetParcelPickedUp"); }
        }

        public override string ToString()
        {
            return
                $"Id #{id}: Name = {name}, Phone = {phone}," +
                $"Sender parcels that was picked up = {senderParcelDelivered}," +
                $"Sender parcels that was scheduled = {senderParcelPickedUp}," +
                $"Target parcels that was delivered = {targetParcelDelivered}," +
                $"Target parcels that was picked up = {targetParcelPickedUp}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System.ComponentModel;

namespace PO
{
    public class ParcelToList : INotifyPropertyChanged
    {
        private int id;
        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged("id"); }
        }

        private string senderName;
        public string SenderName
        {
            get => senderName;
            set { senderName = value; OnPropertyChanged("senderName"); }
        }

        private string targetName;
        public string TargetName
        {
            get => targetName;
            set { targetName = value; OnPropertyChanged("targetName"); }
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

        public override string ToString()
        {
            return $"Id #{id}: Sender name = {senderName},Target name = {targetName}," +
                   $"Weight = {weight},  Priority = {priority}, Status = {status}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

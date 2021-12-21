using System.ComponentModel;

namespace PO
{
    public class DroneToList : INotifyPropertyChanged
    {
        public int Id
        {
            get => Id;
            set { Id = value; OnPropertyChanged("Id"); }
        }
        public string Model
        {
            get => Model;
            set { Model = value; OnPropertyChanged("Model"); }
        }
        public WeightCategories Weight
        {
            get => Weight;
            set { Weight = value; OnPropertyChanged("Weight"); }
        }
        public double Battery
        {
            get => Battery;
            set { Battery = value; OnPropertyChanged("Battery"); }
        }
        public DroneStatuses Status
        {
            get => Status;
            set { Status = value; OnPropertyChanged("Status"); }
        }
        public Location Location
        {
            get => Location;
            set { Location = value; OnPropertyChanged("Location"); }
        }
        public int IdParcel
        {
            get => IdParcel;
            set { IdParcel = value; OnPropertyChanged("IdParcel"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

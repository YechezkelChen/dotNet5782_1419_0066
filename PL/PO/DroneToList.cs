using System.ComponentModel;

namespace PO
{
    public class DroneToList : INotifyPropertyChanged
    {
        private int id;
        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged("id"); }
        }

        private string model;
        public string Model
        {
            get => model;
            set { model = value; OnPropertyChanged("model"); }
        }

        private WeightCategories weight;
        public WeightCategories Weight
        {
            get => weight;
            set { weight = value; OnPropertyChanged("weight"); }
        }

        private double battery;
        public double Battery
        {
            get => battery;
            set { battery = value; OnPropertyChanged("battery"); }
        }

        private DroneStatuses status;
        public DroneStatuses Status
        {
            get => status;
            set { status = value; OnPropertyChanged("status"); }
        }

        private Location location;
        public Location Location
        {
            get => location;
            set { location = value; OnPropertyChanged("location"); }
        }

        private int idParcel;
        public int IdParcel
        {
            get => idParcel;
            set { idParcel = value; OnPropertyChanged("idParcel"); }
        }

        public override string ToString()
        {
            return
                $"Id #{id}: Model = {model}, Weight = {weight}, " +
                $"Battery = {battery}, " +
                $"Parcel in delivery = { idParcel}, Drone Statues = {status}, Location = {location}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

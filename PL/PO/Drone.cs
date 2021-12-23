using System;
using System.ComponentModel;

namespace PO 
{
    public class Drone : INotifyPropertyChanged
    {
        private int id;
        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged("id"); }
        }

        private string model;
        public String Model
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

        private ParcelInTransfer parcelByTransfer;
        public ParcelInTransfer ParcelByTransfer
        {
            get => parcelByTransfer;
            set { parcelByTransfer = value; OnPropertyChanged("parcelByTransfer"); }
        }

        private Location location;
        public Location Location
        {
            get => location;
            set { location = value; OnPropertyChanged("location"); }
        }

        public override string ToString()
        {
            return
                $"Id #{id}:\n" +
                $"Model = {model}\n" +
                $"Weight = {weight}\n" +
                $"Battery = {battery}\n" +
                $"Status = {status}\n" +
                $"DeliveryByTransfer = {parcelByTransfer}" +
                $"Location = {location}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

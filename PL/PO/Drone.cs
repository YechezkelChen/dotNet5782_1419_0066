using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PO 
{
    public class Drone : INotifyPropertyChanged
    {
        public int Id
        {
            get => Id;
            set { Id = value; OnPropertyChanged("Id"); }
        }

        public String Model
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

        public ParcelInTransfer ParcelByTransfer
        {
            get => ParcelByTransfer;
            set { ParcelByTransfer = value; OnPropertyChanged("Status"); }
        }

        public Location Location
        {
            get => Location;
            set { Location = value; OnPropertyChanged("Status"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

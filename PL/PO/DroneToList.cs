using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
    public class DroneToList : INotifyPropertyChanged
    {
        public int Id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("Id"); }
        }
        public string Model
        {
            get { return Model; }
            set { Model = value; OnPropertyChanged("Model"); }
        }
        public WeightCategories Weight
        {
            get { return Weight; }
            set { Weight = value; OnPropertyChanged("Weight"); }
        }
        public double Battery
        {
            get { return Battery; }
            set { Battery = value; OnPropertyChanged("Battery"); }
        }
        public DroneStatuses Status
        {
            get { return Status; }
            set { Status = value; OnPropertyChanged("Status"); }
        }
        public Location Location
        {
            get { return Location; }
            set { Location = value; OnPropertyChanged("Location"); }
        }
        public int IdParcel
        {
            get { return IdParcel; }
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

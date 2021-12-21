using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
    public class ParcelToList : INotifyPropertyChanged
    {
        public int Id
        {
            get => Id;
            set { Id = value; OnPropertyChanged("Id"); }
        }
        public string SenderName
        {
            get => SenderName;
            set { SenderName = value; OnPropertyChanged("SenderName"); }
        }
        public string TargetName
        {
            get => TargetName;
            set { TargetName = value; OnPropertyChanged("TargetName"); }
        }
        public WeightCategories Weight
        {
            get => Weight;
            set { Weight = value; OnPropertyChanged("Weight"); }
        }
        public Priorities Priority
        {
            get => Priority;
            set { Priority = value; OnPropertyChanged("Priority"); }
        }
        public ParcelStatuses Status
        {
            get => Status;
            set { Status = value; OnPropertyChanged("Status"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

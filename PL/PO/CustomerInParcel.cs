using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
    public class CustomerInParcel : INotifyPropertyChanged
    {
        public int Id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("Id"); }
        }
        public string NameCustomer
        {
            get { return NameCustomer; }
            set
            {
                NameCustomer = value; OnPropertyChanged("NameCustomer");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

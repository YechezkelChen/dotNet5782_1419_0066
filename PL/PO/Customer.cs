using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PO
{
    public class Customer : INotifyPropertyChanged
    {
        public int Id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("Id"); }
        }
        public string Name
        {
            get { return Name; }
            set
            {
                Name = value; OnPropertyChanged("Name");
            }
        }
        public string Phone
        {
            get { return Phone; }
            set
            {
                Phone = value; OnPropertyChanged("Phone");
            }
        }
        public Location Location
        {
            get { return Location; }
            set
            {
                Location = value; OnPropertyChanged("Location");
            }
        }
        public IEnumerable<ParcelInCustomer> FromTheCustomerList
        {
            get { return FromTheCustomerList; }
            set
            {
                FromTheCustomerList = value; OnPropertyChanged("FromTheCustomerList");
            }
        }
        public IEnumerable<ParcelInCustomer> ToTheCustomerList
        {
            get { return ToTheCustomerList; }
            set
            {
                ToTheCustomerList = value; OnPropertyChanged("ToTheCustomerList");
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

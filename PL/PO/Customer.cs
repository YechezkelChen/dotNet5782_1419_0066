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
            get => Id;
            set { Id = value; OnPropertyChanged("Id"); }
        }
        public string Name
        {
            get => Name;
            set
            {
                Name = value; OnPropertyChanged("Name");
            }
        }
        public string Phone
        {
            get => Phone;
            set
            {
                Phone = value; OnPropertyChanged("Phone");
            }
        }
        public Location Location
        {
            get => Location;
            set
            {
                Location = value; OnPropertyChanged("Location");
            }
        }
        public IEnumerable<ParcelInCustomer> FromTheCustomerList
        {
            get => FromTheCustomerList;
            set
            {
                FromTheCustomerList = value; OnPropertyChanged("FromTheCustomerList");
            }
        }
        public IEnumerable<ParcelInCustomer> ToTheCustomerList
        {
            get => ToTheCustomerList;
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

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
        private int id;
        private string Name;
        public int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged("Id"); }
        }
        public string Name
        {
            get { return id; }
            set
            {
                id = value; OnPropertyChanged("Id");
            }
        }
        public string Phone { get; set; }
        public Location Location { get; set; }
        public IEnumerable<ParcelInCustomer> FromTheCustomerList { get; set; }
        public IEnumerable<ParcelInCustomer> ToTheCustomerList { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

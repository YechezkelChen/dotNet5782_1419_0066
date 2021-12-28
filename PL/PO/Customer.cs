using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PO
{
    public class Customer : INotifyPropertyChanged
    {
        private int id;
        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged("id"); }
        }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value; OnPropertyChanged("name");
            }
        }

        private string phone;
        public string Phone
        {
            get => phone;
            set
            {
                phone = value; OnPropertyChanged("phone");
            }
        }

        private Location location;
        public Location Location
        {
            get => location;
            set
            {
                location = value; OnPropertyChanged("location");
            }
        }

        private IEnumerable<ParcelInCustomer> fromTheCustomerList;
        public IEnumerable<ParcelInCustomer> FromTheCustomerList
        {
            get => fromTheCustomerList;
            set
            {
                fromTheCustomerList = value; OnPropertyChanged("fromTheCustomerList");
            }
        }

        private IEnumerable<ParcelInCustomer> toTheCustomerList;
        public IEnumerable<ParcelInCustomer> ToTheCustomerList
        {
            get => toTheCustomerList;
            set
            {
                toTheCustomerList = value; OnPropertyChanged("toTheCustomerList");
            }
        }

        public override string ToString()
        {
            StringBuilder builderFromTheCustomerList = new StringBuilder();
            StringBuilder builderToTheCustomerList = new StringBuilder();
            foreach (var parcelInCustomer in fromTheCustomerList)
                builderFromTheCustomerList.Append(parcelInCustomer).Append(", ");
            foreach (var parcelToCustomer in toTheCustomerList)
                builderToTheCustomerList.Append(parcelToCustomer).Append(", ");

            return
                $"Id #{id}: Name = {name}, Phone = {phone}, Location = {location}" +
                $"Parcels the customer sent = {builderFromTheCustomerList}\n" +
                $"Parcels the customer need to receive = {builderToTheCustomerList}.";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

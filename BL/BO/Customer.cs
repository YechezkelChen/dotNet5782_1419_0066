using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Location Location { get; set; }
        public IEnumerable<ParcelInCustomer> FromTheCustomerList { get; set; }
        public IEnumerable<ParcelInCustomer> ToTheCustomerList { get; set; }

        public override string ToString()
        {
            StringBuilder builderFromTheCustomerList = new StringBuilder();
            StringBuilder builderToTheCustomerList = new StringBuilder();
            foreach (var parcelInCustomer in FromTheCustomerList)
                builderFromTheCustomerList.Append(parcelInCustomer).Append(", ");
            foreach (var parcelToCustomer in ToTheCustomerList)
                builderToTheCustomerList.Append(parcelToCustomer).Append(", ");

            return
                $"Id #{Id}: Name = {Name}, Phone = {Phone}, Location = {Location}" +
                $"Parcels the customer sent = {builderFromTheCustomerList}\n" +
                $"Parcels the customer need to receive = {builderToTheCustomerList}.";
        }
    }
}

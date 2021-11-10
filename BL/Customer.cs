using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public Location Location { get; set; }
            public List<ParcelInCustomer> FromTheCustomerList { get; set; }
            public List<ParcelInCustomer> ToTheCustomerList { get; set; }

            public override string ToString()
            {
                return
                    $"Id #{Id}: Name = {Name}, Phone = {Phone},Location = {Location}," +
                    $"Parcels the customer sent = {FromTheCustomerList.ToString()}, " +
                    $"Parcels the customer need to receive = {ToTheCustomerList.ToString()}";
            }
        }
    }
}

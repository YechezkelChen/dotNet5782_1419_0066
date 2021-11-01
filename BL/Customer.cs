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
            public int id { get; set; }
            public string name { get; set; }
            public string phone { get; set; }
            public Location location { get; set; }
            public List<Parcel> fromTheCustomerList { get; set; }
            public List<Parcel> toTheCustomerList { get; set; }
        }
    }
}

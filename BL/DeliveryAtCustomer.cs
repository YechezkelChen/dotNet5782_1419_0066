using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    { 
        public class DeliveryAtCustomer
        {
            public int id { get; set; }
            public WeightCategories weight { get; set; }
            public Priorities priority { get; set; }
            public ParcelStatuses statuse { get; set; }
            public CustomerInDelivery customerInDelivery { get; set; }//how is the customer
        }
    }
}

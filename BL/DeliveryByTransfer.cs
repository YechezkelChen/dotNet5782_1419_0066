using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    { 
        public class DeliveryByTransfer
        {
            public int id { get; set; }
            public WeightCategories weight { get; set; }
            public Priorities priority { get; set; }
            public bool deliveryStatus { get; set; }
            public Location collectionLocation { get; set; }
            public Location deliveryDestinationLocation { get; set; }
            public double transportDistance { get; set; }

        }
    }
}

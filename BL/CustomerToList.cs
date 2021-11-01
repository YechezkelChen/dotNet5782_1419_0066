using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class CustomerToList
        {
            public int id { get; set; }
            public string name { get; set; }
            public string phone { get; set; }
            public int ParcelScheduledPickedUp { get; set; }
            public int ParcelScheduledNotPickedUp { get; set; }
            public int ParcelScheduledAndDelivered { get; set; }

        }
    }
    
}

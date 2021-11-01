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
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public TYPE type { get; set; }
            public TYPE type1 { get; set; }
            public TYPE type2 { get; set; }
            public TYPE type3 { get; set; }
            //public int ParcelScheduledPickedUp { get; set; }
            //public int ParcelScheduledNotPickedUp { get; set; }
            //public int ParcelScheduledAndDelivered { get; set; }
        }
    }
}

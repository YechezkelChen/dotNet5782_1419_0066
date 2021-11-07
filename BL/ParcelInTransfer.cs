using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class ParcelInTransfer
        {
            public int Id { get; set; }
            public bool ParcelStatus { get; set; }
            public Priorities Priority { get; set; }
            public WeightCategories Weight { get; set; }
            public CustomerInParcel SenderInParcel { get; set; }//the sender
            public CustomerInParcel ReceiverInParcel { get; set; }//the receiver
            public Location PickUpLocation { get; set; }
            public Location TargetLocation { get; set; }
            public double DistanceOfTransfer { get; set; }
        }
    }
}

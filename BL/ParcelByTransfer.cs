using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    { 
        public class ParcelByTransfer
        {
            public int Id { get; set; }
            public bool Status { get; set; }
            public Priorities Priority { get; set; }
            public WeightCategories Weight { get; set; }
            public CustomerInParcel SenderInParcel { get; set; }//the sender
            public Location PickUpLocation { get; set; }
            public CustomerInParcel ReceiverInParcel { get; set; }//the receiver}
            public Location TargetLocation { get; set; }
            public double DistanceOfTransfer { get; set; }

            public override string ToString()
            {
                return $"Id #{Id}: Parcel status = {Status},Priority = {Priority}," +
                       $"Weight = {Weight},  Sender in parcel = {SenderInParcel}, Receiver in parcel = {ReceiverInParcel}," +
                       $"Pick up location = {PickUpLocation},Target location = {TargetLocation}, " +
                       $"Distance of transfer = {DistanceOfTransfer}";
            }
        }
    }
}

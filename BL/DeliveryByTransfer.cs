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
            public int Id { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public bool DeliveryStatus { get; set; }
            public Location CollectionLocation { get; set; }
            public Location DeliveryDestinationLocation { get; set; }
            public double TransportDistance { get; set; }

            public override string ToString()
            {
                return
                    $"Id #{Id}: Weight = {Weight}, Priority = {Priority}," +
                    $"Delivery status = {DeliveryStatus}," +
                    $"Collection location = {CollectionLocation}," +
                    $"Delivery destination location = {DeliveryDestinationLocation}," +
                    $"Transport distance = {TransportDistance}";
            }
        }
    }
}

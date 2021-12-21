using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class ParcelInTransfer
    {
        public int Id { get; set; }
        public bool OnTheWay { get; set; }
        public Priorities Priority { get; set; }
        public WeightCategories Weight { get; set; }
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Target { get; set; }
        public Location PickUpLocation { get; set; }
        public Location TargetLocation { get; set; }
        public double DistanceOfTransfer { get; set; }

        public override string ToString()
        {
            return $"Id #{Id}:\n" +
                   $"On the way = {OnTheWay}\n" +
                   $"Priority = {Priority}\n" +
                   $"Weight = {Weight}\n" +
                   $"Sender = {Sender}\n" +
                   $"Target = {Target}\n" +
                   $"Pick up location = {PickUpLocation}" +
                   $"Target location = {TargetLocation}" +
                   $"Distance of transfer =" + String.Format("{0:0.00}", DistanceOfTransfer) + "\n";
        }
    }
}

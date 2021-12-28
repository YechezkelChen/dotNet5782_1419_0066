using System.Collections.Generic;
using System.Text;


namespace BO
{
    public class Station
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public int AvailableChargeSlots { get; set; }
        public IEnumerable<DroneInCharge> DronesInCharges { get; set; }
        public override string ToString()
        {
            StringBuilder builderDroneChargeListPrint = new StringBuilder();
            foreach (var elementInCharge in DronesInCharges)
                builderDroneChargeListPrint.Append(elementInCharge).Append(", ");
            return $"Id #{Id}: Name = {Name},Location = {Location}, " +
                   $"Available charge slots = {AvailableChargeSlots}, Drones in charges = {builderDroneChargeListPrint}.";
        }
    }
}
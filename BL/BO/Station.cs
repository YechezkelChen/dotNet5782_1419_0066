using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace BO
{
    public class Station
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public int ChargeSlots { get; set; }
        public List<DO.DroneCharge> InCharges { get; set; }
        public override string ToString()
        {
            StringBuilder builderDroneChargeListPrint = new StringBuilder();
            foreach (var elementInCharge in InCharges)
                builderDroneChargeListPrint.Append(elementInCharge).Append(", ");
            return $"Id #{Id}: Name = {Name},Location = {Location}, " +
                   $"Charge slots = {ChargeSlots}, Drone in charges = {builderDroneChargeListPrint}.";
        }
    }
}
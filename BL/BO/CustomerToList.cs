using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class CustomerToList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int SenderParcelDelivered { get; set; }
        public int SenderParcelPickedUp { get; set; }
        public int TargetParcelDelivered { get; set; }
        public int TargetParcelPickedUp { get; set; }

        public override string ToString()
        {
            return
                $"Id #{Id}: Name = {Name}, Phone = {Phone}," +
                $"Sender parcels that was picked up = {SenderParcelDelivered}," +
                $"Sender parcels that was scheduled = {SenderParcelPickedUp}," +
                $"Target parcels that was delivered = {TargetParcelDelivered}," +
                $"Target parcels that was picked up = {TargetParcelPickedUp}";
        }
    }
}